//using Microsoft.AspNetCore.Mvc;
//using Google.Cloud.Translation.V2;
//using management.Models;
//namespace management.Controllers
//{
//    [Route("api/translate")]
//    [ApiController]
//    public class TranslationController : ControllerBase
//    {
//        private readonly TranslationClient _translationClient;

//        public TranslationController()
//        {
//            _translationClient = TranslationClient.Create();
//        }

//        [HttpPost("translate")]
//        public async Task<IActionResult> Translate([FromBody] TranslationRequest request)
//        {
//            try
//            {
//                var response = await _translationClient.TranslateTextAsync(
//                    request.Text,
//                    request.TargetLanguage,
//                    request.SourceLanguage);

//                return Ok(new { TranslatedText = response.TranslatedText });
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { Message = ex.Message });
//            }
//        }
//    }

  
//}
