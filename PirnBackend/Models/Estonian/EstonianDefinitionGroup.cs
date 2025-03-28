using PirnBackend.Models.EstonianCaseCollection;
using PirnBackend.Models.Words;

namespace PirnBackend.Models.Estonian;

public class EstonianDefinitionGroup: IDefinitionGroup
{
    public required IEnumerable<IDefinition> Definitions { get; set; }    
    public required EstonianWordClass? WordClass { get; set; }
    public List<EstonianNounCase>? NounCases { get; set; }
}
