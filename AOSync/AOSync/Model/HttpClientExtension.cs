using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public static class HttpClientExtension
{
    public static async Task<TResult> SendRequestAsync<TRequest, TResult>(string url, TRequest request)
    {
        using (HttpClient client = new HttpClient())
        {
            // Serialize the request object to JSON
            var json = JsonSerializer.Serialize(request);
            Console.WriteLine(json);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send the POST request
            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TResult>(responseContent)!;
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code: {response.StatusCode}");
            }
        }
    }
}
