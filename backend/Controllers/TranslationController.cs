using Google.Cloud.Translation.V2;
using management.Models;
using Microsoft.AspNetCore.Mvc;

namespace management.Controllers
{
    [Route("api/translate")]
    [ApiController]
    public class TranslationController : ControllerBase
    {
        private readonly TranslationClient _translationClient;

        public TranslationController()
        {
            _translationClient = TranslationClient.Create();
        }

        [HttpPost("translate")]
        public async Task<IActionResult> Translate([FromBody] TranslationRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Text) ||
                string.IsNullOrWhiteSpace(request.TargetLanguage) ||
                string.IsNullOrWhiteSpace(request.SourceLanguage))
            {
                return BadRequest("Text, source language, and target language are required.");
            }

            var response = await _translationClient.TranslateTextAsync(
                request.Text,
                request.TargetLanguage,
                request.SourceLanguage);

            return Ok(new { response.TranslatedText });
        }
    }
}
