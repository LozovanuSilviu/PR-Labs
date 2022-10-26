using Newtonsoft.Json;
using ProducerServer.Models;
using RestSharp;

namespace ProducerServer.Services;

public class ProducerService
{
    private readonly ILogger<ProducerService> _logger;
    private readonly  Queue<News> _queue;
    public Mutex mutex { get; set; }
    public Mutex mutex1 { get; set; }
    public ProducerService(ILogger<ProducerService> logger)
    {
        _queue = new Queue<News>();
        _logger = logger;
        mutex = new Mutex();
        mutex1 = new Mutex();
        Run();
    }

    private  void Run()
    {
    
        for (int i = 0; i < 3; i++) 
        {
            Thread.Sleep(3000);
            Task.Run(ExtractData);
        }

        
        for (int i = 0; i < 5; i++)
        {
            Thread.Sleep(3000);
            Task.Run(SendLetters);
        }
    }

    private async Task ExtractData()
    {
        while (true)
        {
            mutex.WaitOne();
                var messages = File.ReadLines(@"C:\producerFiles\catFacts.txt").ToArray();
                var message = messages[new Random().Next(0,9)];
                var news = new News()
                {
                    Message = message
                };
                _queue.Enqueue(news);
            mutex.ReleaseMutex();
        }

    }

    private async Task SendLetters()
    {
        while (true)
        {
            mutex1.WaitOne();
                if (_queue.Count !=0)
                {
                    _queue.TryDequeue(out News letter);
                    var client = new RestClient("http://localhost:5168");
                    var serializedLetter = JsonConvert.SerializeObject(letter);
                    var request = new RestRequest("/api/send/to/consumer",Method.Post);
                    request.AddJsonBody(serializedLetter);
                    var result =client.ExecuteAsync(request);
                }

                mutex1.ReleaseMutex();
        }
    }
}