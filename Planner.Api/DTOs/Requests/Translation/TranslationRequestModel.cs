using Planner.Application.Enumerations;

namespace Planner.Api.DTOs.Requests.Translation;

public class TranslationRequestModel
{
    public MessageKey MessageKey { get; set; }
    public Language Language { get; set; }
}