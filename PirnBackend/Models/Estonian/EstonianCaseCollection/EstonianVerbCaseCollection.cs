namespace PirnBackend.Models.EstonianCaseCollection;

public class EstonianVerbCaseCollection: IEstonianCaseCollection
{
    public required List<EstonianCase> Cases { get; set; }
}