using Microsoft.AspNetCore.Mvc;
using Planner.Application.Interfaces.Services;
using Planner.Api.DTOs.Requests.Translation;

namespace Planner.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TranslationController : ControllerBase
{
    private readonly ITranslationService translationService;

    public TranslationController(ITranslationService translationService)
    {
        this.translationService = translationService;    
    }

    [HttpPost]
    public async Task<IActionResult> Translate(TranslationRequestModel model)
    {
        string result = await translationService.Translate(model.MessageKey, model.Language);
        return Ok(result);
    }
}