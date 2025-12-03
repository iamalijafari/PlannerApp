using Planner.Application.Enumerations;

namespace Planner.Application.Interfaces.Utilities;

public interface ITranslationUtility
{
    public string Translate(MessageKey key, Language lang);
}