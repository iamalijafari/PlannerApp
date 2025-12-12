using Planner.Application.Enumerations;

namespace Planner.Application.Interfaces.Services;

public interface ITranslationService
{
    Task<string> Translate(MessageKey key, Language language);
}