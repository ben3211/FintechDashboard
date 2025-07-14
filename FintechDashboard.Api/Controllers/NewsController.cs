using FintechDashboard.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FintechDashboard.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly NewsService _newsService;
        private readonly OpenAiService _openAiService;

        public NewsController(NewsService newsService, OpenAiService openAiService)
        {
            _newsService = newsService;
            _openAiService = openAiService;
        }

        [HttpPost("summarize")]
        public async Task<IActionResult> Summarize([FromBody] SummarizeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Asset))
                return BadRequest("Asset is required.");

            var newsList = await _newsService.GetRecentNewsAsync(request.Asset);
            if (newsList == null || newsList.Count == 0)
                return NotFound("No news found for the specified asset.");

            var summary = await _openAiService.SummarizeNewsAsync(newsList);
            return Ok(new { asset = request.Asset, summary });
        }

        public class SummarizeRequest
        {
            public string Asset { get; set; }
        }
    }
} 