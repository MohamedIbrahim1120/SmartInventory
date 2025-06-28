using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuggestionController : ControllerBase
    {
        private readonly IInventorySuggestionService _suggestionService;

        public SuggestionController(IInventorySuggestionService suggestionService)
        {
            _suggestionService = suggestionService;
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStock()
        {
            var suggestions = await _suggestionService.GetLowStockSuggestionsAsync();
            return Ok(suggestions);
        }

        [HttpGet("stagnant")]
        public async Task<IActionResult> GetStagnant()
        {
            var suggestions = await _suggestionService.GetStagnantProductSuggestionsAsync();
            return Ok(suggestions);
        }

        [HttpGet("fast-moving")]
        public async Task<IActionResult> GetFastMoving()
        {
            var suggestions = await _suggestionService.GetFastMovingSuggestionsAsync();
            return Ok(suggestions);
        }
    }
}
