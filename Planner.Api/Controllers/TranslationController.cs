using Microsoft.AspNetCore.Mvc;
using Planner.Application.Interfaces.Services;
using Planner.Api.DTOs.Requests.Translation;

namespace Planner.Api.Controllers;

/// <summary>
/// Resolves localized UI messages in English or Persian.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TranslationController : ControllerBase
{
    private readonly ITranslationService translationService;

    public TranslationController(ITranslationService translationService)
    {
        this.translationService = translationService;    
    }

    /// <summary>Translates one message key into the requested language.</summary>
    [HttpPost("Translate")]
    public async Task<IActionResult> Translate([FromBody] TranslationRequestModel model)
    {
        string result = await translationService.Translate(model.MessageKey, model.Language);
        return Ok(result);
    }
}
