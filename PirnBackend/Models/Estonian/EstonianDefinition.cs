using PirnBackend.Models.EstonianCaseCollection;
using PirnBackend.Models.Words;

namespace PirnBackend.Models.Estonian;

public class EstonianDefinition: IDefinition
{
    public required string? Definition { get; set; }
    public required PartOfSpeech? PartOfSpeech { get; set; }
    public required List<string>? Examples { get; set; }
    public required IEstonianCaseCollection? CaseCollection { get; set; }
}
