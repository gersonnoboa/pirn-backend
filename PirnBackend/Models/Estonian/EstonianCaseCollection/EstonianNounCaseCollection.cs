namespace PirnBackend.Models.EstonianCaseCollection;

public class EstonianNounCaseCollection: IEstonianCaseCollection
{
    public required List<EstonianCase> SingularCases { get; set; }
    public required List<EstonianCase> PluralCases { get; set; }
}