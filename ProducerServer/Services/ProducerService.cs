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
        

        // for (int i = 0; i < 2; i++)
        // {
        //     Task.Run(SendLetters);
        // }
        _logger.LogInformation($"ZDAROV {_queue.Count}");
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
            // _queue.Enqueue(value);
            var client = new HttpClient();
            var serializedLetter = JsonConvert.SerializeObject(value);
            _logger.LogInformation(serializedLetter.ToString());
            var result = 
                await client.PostAsync("http://localhost:5168/api/send/to/consumer",
                    new StringContent(serializedLetter, Encoding.UTF8, "applications/json"));
            var content =  result.StatusCode.ToString();
            _logger.LogInformation(content);
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
                var client = new RestClient();
                var serializedLetter = JsonConvert.SerializeObject(letter);
                _logger.LogInformation(serializedLetter.ToString());
                var payload = new StringContent(serializedLetter, Encoding.UTF32, "applications/json");

                payload.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                _logger.LogInformation(payload.ToString());
                var request =new RestRequest("http://localhost:5168/api/send/to/consumer", Method.Post);
                request.AddParameter("application/json; charset=utf-8", letter, ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;
                var result =
                    await client.PostAsync(request);
                var content =  result.StatusCode.ToString();
                _logger.LogInformation(content);
            }
        }
    }
    
    
}