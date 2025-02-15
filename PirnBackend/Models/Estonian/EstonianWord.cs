using PirnBackend.Extensions;
using PirnBackend.Models.Estonian;
using PirnBackend.Models.EstonianCaseCollection;

namespace PirnBackend.Models;

public class EstonianWord: IWord
{
    public required string OriginalLanguageText { get; set; }
    public required List<EstonianDefinitionGroup> DefinitionGroups { get; set; }
    public required List<EstonianTranslation> Translations { get; set; }
}
