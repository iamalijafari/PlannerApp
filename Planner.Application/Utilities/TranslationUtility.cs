using Planner.Application.Enumerations;
using Planner.Application.Interfaces.Utilities;
using System.Xml.Linq;

namespace Planner.Application.Utilities;

public class TranslationUtility : ITranslationUtility
{
    private readonly Dictionary<string, Dictionary<string, string>> languages = new();

    public TranslationUtility()
    {
        foreach (Language language in Enum.GetValues<Language>())
        {
            string languageName = language.ToString();
            string filePath = Path.Combine(
                AppContext.BaseDirectory,
                "Dictionaries",
                $"Messages.{languageName}.xml");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Dictionary file not found: {filePath}");
            }

            var xml = XDocument.Load(filePath);

            var dict = xml.Descendants("Message")
                .ToDictionary(
                    m => m.Attribute("key")!.Value,
                    m => m.Value
                );

            languages[languageName] = dict;
        }
    }

    public Task<string> Translate(MessageKey key, Language lang)
    {
        string stringKey = key.ToString();

        if (languages[lang.ToString()].TryGetValue(stringKey, out string? value))
        {
            return Task.FromResult(value);
        }

        return Task.FromResult(stringKey);
    }
}
