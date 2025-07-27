using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FintechDashboard.Api.Services
{
    public class OpenAiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenAiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
                ?? throw new InvalidOperationException("OPENAI_API_KEY environment variable is not set.");
        }

        public async Task<string> SummarizeNewsAsync(List<string> newsList)
        {
            var prompt = $"Summarize the following financial news and provide the overall sentiment (positive, negative, neutral):\n{string.Join("\n", newsList)}";
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[] {
                    new { role = "system", content = "You are a financial news summarizer." },
                    new { role = "user", content = prompt }
                }
            };
            var requestJson = JsonSerializer.Serialize(requestBody);
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
            {
                Content = new StringContent(requestJson, Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseJson);
            var summary = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
            return summary;
        }
    }
} 