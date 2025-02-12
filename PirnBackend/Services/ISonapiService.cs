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

        return new EstonianWord
        {
            OriginalLanguageText = originalLanguageText,
            DefinitionGroups = GetDefinitions(json),
            Translations = GetTranslations(json),
            SimilarWords = []
        };
    }

    private List<EstonianDefinitionGroup> GetDefinitions(JsonDocument json)
    {
        var searchResultArray = json.RootElement.GetProperty("searchResult").EnumerateArray();
        return searchResultArray.Select(resultJson =>
        {
            var meaningsArray = resultJson.GetProperty("meanings").EnumerateArray();
            var wordClassesArray = resultJson.GetProperty("wordClasses").EnumerateArray();
            var wordClassString = !wordClassesArray.Any() ? "" : wordClassesArray.First().GetString();
            var wordClass = EstonianWordClassExtensions.GetEstonianWordClass(wordClassString);
            
            var subDefinitions = meaningsArray.Select(meaningsJson =>
            {
                var partOfSpeechArray = meaningsJson.GetProperty("partOfSpeech").EnumerateArray();
                var partOfSpeechString = !partOfSpeechArray.Any() ? 
                    "" : 
                    partOfSpeechArray.First().GetProperty("code").GetString();
                var partOfSpeech = PartOfSpeechExtensions.GetEstonianPartOfSpeech(partOfSpeechString);
                
                var examplesArray = meaningsJson.GetProperty("examples").EnumerateArray();
                var examples = examplesArray
                    .Select(example => example.GetString())
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Cast<string>()
                    .ToList();
                
                return new EstonianDefinition
                {
                    Definition = meaningsJson.GetProperty("definition").GetString(),
                    PartOfSpeech = partOfSpeech,
                    Examples = examples,
                    CaseCollection = null
                };
            }).ToList();

            return new EstonianDefinitionGroup
            {
                Definitions = subDefinitions,
                WordClass = wordClass
            };
        }).ToList();
    }
    
    private List<EstonianTranslation> GetTranslations(JsonDocument json)
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
            
            return new EstonianTranslation
            {
                OriginalLanguageCode = originalLanguageCode,
                TranslatedLanguageCode = translatedLanguageCode,
                Translations = translationsTexts
            };
        }).ToList();
    }
}