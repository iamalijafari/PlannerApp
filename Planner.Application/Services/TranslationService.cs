using Planner.Application.Enumerations;
using Planner.Application.Interfaces.Services;
using Planner.Application.Interfaces.Utilities;

namespace Planner.Application.Services;

public class TranslationService : ITranslationService
{
    private readonly ITranslationUtility translationUtility;

    public TranslationService(ITranslationUtility translationUtility)
    {
        this.translationUtility = translationUtility;
    }

    public async Task<string> Translate(MessageKey key, Language language)
    {
        return await translationUtility.Translate(key, language);
    }
}