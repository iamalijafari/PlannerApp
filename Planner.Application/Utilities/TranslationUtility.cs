using Planner.Application.Enumerations;
using Planner.Application.Interfaces.Utilities;
using System.Xml.Linq;

namespace Planner.Application.Utilities;

public class TranslationUtility : ITranslationUtility
{
    private readonly Dictionary<string, Dictionary<string, string>> languages = new();

    public TranslationUtility()
    {
        foreach(int i in Enum.GetValues(typeof(Language)))
        {
            String lang = Enum.GetName(typeof(Language), i);

            string filePath = Path.Combine(Path.Combine(AppContext.BaseDirectory, "Dictionaries"), $"Messages.{lang}.xml");

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

            languages[lang] = dict;
        }
    }

    public async Task<string> Translate(MessageKey key, Language lang)
    {
        string stringKey = key.ToString();

        if (languages[lang.ToString()].TryGetValue(stringKey, out string? value))
        {
            return value;
        }

        return $"{stringKey}";
    }
}