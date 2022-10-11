using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using ProducerServer.Models;
using RestSharp;

namespace ProducerServer.Services;

public class ProducerService
{
    private readonly ILogger<ProducerService> _logger;
    private readonly  ConcurrentQueue<Letter> _queue;
    public ProducerService(ILogger<ProducerService> logger)
    {
        _queue = new ConcurrentQueue<Letter>();
        _logger = logger;
        Run();
    }

    private  Task Run()
    {
        for (int i = 0; i < 5; i++)
        {
            Task.Run(GenerateLetters);
        }
        
        for (int i = 0; i < 2; i++)
        {
            Task.Run(SendLetters);
        }
        return Task.CompletedTask;
    }

    private async Task GenerateLetters()
    {
        
        while (true)
        {
            var value = new Letter()
            {
                SenderName = "Sender"+new Random().Next(1,6),
                Message = "this is the message i want to share"
            };
            _queue.Enqueue(value);
            await Task.Delay(new Random().Next(1000,3000));
        }
    }

    private async Task SendLetters()
    {
        while (true)
        {

            if (_queue.Count !=0)
            {
               await Task.Delay(1000);

                _queue.TryDequeue(out Letter letter);
                var client = new RestClient("http://localhost:5168");
                var serializedLetter = JsonConvert.SerializeObject(letter);
                var request = new RestRequest("/api/send/to/consumer",Method.Post);
                request.AddJsonBody(serializedLetter);
                var result =await client.ExecuteAsync(request);
            }
        }
    }
    
    
}