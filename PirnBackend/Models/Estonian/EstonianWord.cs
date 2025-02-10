using PirnBackend.Extensions;
using PirnBackend.Models.Estonian;
using PirnBackend.Models.EstonianCaseCollection;

namespace PirnBackend.Models;

public class EstonianWord: IWord
{
    public required string OriginalLanguageText { get; set; }
    public required List<WordMeaning> Meanings { get; set; }
    public required List<Translation> Translations { get; set; }
    public required List<string> SimilarWords { get; set; }
    public required EstonianWordClass WordClass { get; set; }
    public required IEstonianCaseCollection CaseCollection { get; set; }
}
