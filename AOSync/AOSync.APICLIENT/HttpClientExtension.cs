using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace AOSync.APICLIENT;

public class HttpClientExtension
{
    private readonly IConfiguration _configuration;

    public HttpClientExtension(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static void LogMessage(string message)
    {
        // Define the relative path to the log file
        var relativePath = Path.Combine("..", "Log", $"log-{DateTime.UtcNow:yyyy-MM-dd}.txt");

        // Ensure the directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(relativePath)!);

        // Create a log entry with the current date and time
        var logEntry = $"{DateTime.Now}: {message}";

        // Append the log entry to the file
        File.AppendAllText(relativePath, logEntry + Environment.NewLine);
    }

    public async Task<TResult> SendRequestAsync<TRequest, TResult>(string url, TRequest request)
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("apikey", _configuration["APIKey"]);
            Debug.WriteLine($"Request: {request}");

            // Serialize the request object to JSON
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send the POST request
            var response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Response: {responseContent}");
                return JsonConvert.DeserializeObject<TResult>(responseContent)!;
            }

            throw new HttpRequestException($"Request failed with status code: {response.StatusCode}");
        }
    }
}