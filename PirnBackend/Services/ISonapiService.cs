using System.Text.Json;
using PirnBackend.Models;
using PirnBackend.Models.Estonian;

namespace PirnBackend.Services;

public interface ISonapiService
{
    public EstonianWord GetWord(JsonDocument json, string typedWord);
}

public class SonapiService: ISonapiService
{
    public EstonianWord GetWord(JsonDocument json, string typedWord)
    {
        var originalLanguageText = json.RootElement.GetProperty("estonianWord").GetString() ?? typedWord;

        var searchResult = json.RootElement.GetProperty("searchResult").EnumerateArray();

        var translations = GetTranslations(json);

        return new EstonianWord
        {
            OriginalLanguageText = originalLanguageText,
            Meanings = null,
            Translations = translations,
            SimilarWords = null,
            WordClass = EstonianWordClass.Noomen,
            CaseCollection = null
        };
    }

    public List<Translation> GetTranslations(JsonDocument json)
    {
        var translationArray = json.RootElement.GetProperty("translations").EnumerateArray();
        return translationArray
            .Select(translationJson =>
        {
            var originalLanguageCode = translationJson.GetProperty("from").GetString();
            var translatedLanguageCode = translationJson.GetProperty("to").GetString();

            var translationsTexts = translationJson
                .GetProperty("translations")
                .EnumerateArray()
                .Select(x => x.GetString())
                .Where(x => !string.IsNullOrEmpty(x))
                .Cast<string>()
                .ToList();
            
            return new Translation
            {
                OriginalLanguageCode = originalLanguageCode,
                TranslatedLanguageCode = translatedLanguageCode,
                Translations = translationsTexts
            };
        }).ToList();
    }
}