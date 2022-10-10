using System.Net;

namespace Lab1;

public class ProducerServer
{
    public bool Running;
    private HttpListener Listener;
    private Thread thread;

    public ProducerServer(int port)
    {
        Listener = new HttpListener();
        Listener.Prefixes.Add($"http://localhost:{port}");
    }

    public void Start()
    {
        Thread thread = new Thread(Run);
        thread.Start();
    }
    
    public void DisplayRequestInfo(HttpListenerRequest req)
    {
        Console.WriteLine("Endpoint:" + req.LocalEndPoint);
        Console.WriteLine("Method: " + req.HttpMethod);
        Console.WriteLine("Payload: ");
    }

    private void Run()
    {
        Running = true;
        Listener.Start();

        while (Running)
        {
            HttpListenerContext context = Listener.GetContext();
            HandleRequest(context);
        }
        Listener.Stop();
    }
    
    public void Stop()
    {
        Running = false;
        thread.Abort();
    }

    private void HandleRequest(HttpListenerContext context)
    {
        
    }
}