using System.Text.Json;
using PirnBackend.Models;
using PirnBackend.Models.Estonian;
using PirnBackend.Models.EstonianCaseCollection;

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
        };
    }

    private List<EstonianDefinitionGroup> GetDefinitions(JsonDocument json)
    {
        var searchResultArray = json.RootElement.GetProperty("searchResult").EnumerateArray();
        return searchResultArray.Select(GetEstonianDefinitionGroup).ToList();
    }

    private EstonianDefinitionGroup GetEstonianDefinitionGroup(JsonElement definitionJson)
    {
        var meaningsArray = definitionJson.GetProperty("meanings").EnumerateArray();
        var wordClassesArray = definitionJson.GetProperty("wordClasses").EnumerateArray();
        var wordClassString = !wordClassesArray.Any() ? "" : wordClassesArray.First().GetString();
        var wordClass = EstonianWordClassExtensions.GetEstonianWordClass(wordClassString);
            
        var subDefinitions = meaningsArray.Select(GetEstonianDefinition).ToList();
        var caseCollectionArray = definitionJson.GetProperty("wordForms").EnumerateArray();

        switch (wordClass)
        {
            case EstonianWordClass.Noun:
            {
                var nounCases = GetNounCases(caseCollectionArray);
                return new EstonianDefinitionGroup
                {
                    Definitions = subDefinitions,
                    WordClass = wordClass,
                    NounCases = nounCases
                };
            }
            case EstonianWordClass.Verb:
            case EstonianWordClass.Constant:
            case EstonianWordClass.Unknown:
            default:
                return new EstonianDefinitionGroup
                {
                    Definitions = subDefinitions,
                    WordClass = wordClass
                };
        }
    }

    private static EstonianDefinition GetEstonianDefinition(JsonElement meaningsJson)
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
            Examples = examples
        };
    }

    private static List<EstonianTranslation> GetTranslations(JsonDocument json)
    {
        var translationArray = json.RootElement.GetProperty("translations").EnumerateArray();
        return translationArray.Select(translationJson =>
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
    
    private static List<EstonianNounCase> GetNounCases(
        JsonElement.ArrayEnumerator caseCollectionArray)
    {
        var cases = new List<EstonianNounCase>();
        var caseCodeMap = new Dictionary<string, (EstonianNounCase Case, bool IsSingular)>();

        AddCase("Nimetav", "SgN", "PlN");
        AddCase("Omastav", "SgG", "PlG");
        AddCase("Osastav", "SgP", "PlP");
        AddCase("Lühike Sisseütlev", "SgAdt", null);
        AddCase("Sisseütlev", "SgIll", "PlIll");
        AddCase("Seesütlev", "SgIn", "PlIn");
        AddCase("Seestütlev", "SgEl", "PlEl");
        AddCase("Alaleütlev", "SgAll", "PlAll");
        AddCase("Alalütlev", "SgAd", "PlAd");
        AddCase("Alaltütlev", "SgAbl", "PlAbl");
        AddCase("Saav", "SgTr", "PlTr");
        AddCase("Rajav", "SgTer", "PlTer");
        AddCase("Olev", "SgEs", "PlEs");
        AddCase("Ilmaütlev", "SgAb", "PlAb");
        AddCase("Kassaütlev", "SgKom", "PlKom");

        foreach (var caseElement in caseCollectionArray)
        {
            var caseCode = caseElement.GetProperty("code").GetString() ?? "";
            var value = caseElement.GetProperty("value").GetString();

            if (!caseCodeMap.TryGetValue(caseCode, out var caseInfo)) continue;
            if (caseInfo.IsSingular)
                caseInfo.Case.Singular = value;
            else
                caseInfo.Case.Plural = value;
        }

        return cases;

        void AddCase(string name, string singularCode, string? pluralCode)
        {
            var nounCase = new EstonianNounCase { Name = name };
            cases.Add(nounCase);
            caseCodeMap[singularCode] = (nounCase, true);
            if (pluralCode != null)
                caseCodeMap[pluralCode] = (nounCase, false);
        }
    }
}