using Planner.Application.Enumerations;

namespace Planner.Application.Interfaces.Utilities;

public interface ITranslationUtility
{
    Task<string> Translate(MessageKey key, Language lang);
}