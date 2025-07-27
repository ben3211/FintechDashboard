namespace FintechDashboard.Api.Services
{
    public class NewsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public NewsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = Environment.GetEnvironmentVariable("NEWSAPI_API_KEY")
                ?? throw new InvalidOperationException("NEWSAPI_API_KEY environment variable is not set.");
        }

        public async Task<List<string>> GetRecentNewsAsync(string asset)
        {
            var url = $"https://newsapi.org/v2/everything?q={asset}&sortBy=publishedAt&apiKey={_apiKey}&language=en";
            var response = await _httpClient.GetFromJsonAsync<NewsApiResponse>(url);
            var articles = response?.Articles ?? new List<Article>();
            var newsList = new List<string>();
            foreach (var article in articles)
            {
                newsList.Add($"{article.Title} - {article.Description}");
            }
            return newsList;
        }

        private class NewsApiResponse
        {
            public List<Article> Articles { get; set; }
        }

        private class Article
        {
            public string Title { get; set; }
            public string Description { get; set; }
        }
    }
} 