using System.Net;
using System.Text;
namespace Lab1;

   public  class SendRequest
    {
        public static void SendPostRequest(string url, string data)
        {
            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Method = "POST";

            byte[] encodedData = Encoding.UTF8.GetBytes(data);
            webRequest.ContentType = "application/json";
            webRequest.ContentLength = encodedData.Length;

            Stream dataStream = webRequest.GetRequestStream();
            dataStream.Write(encodedData, 0, encodedData.Length);
            dataStream.Close();

            WebResponse response = webRequest.GetResponse();
            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new(dataStream);
                string responseFromServer = reader.ReadToEnd();
                Console.WriteLine(responseFromServer);
            }
            response.Close();
        }
    }
