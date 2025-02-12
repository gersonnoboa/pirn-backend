namespace PirnBackend.Models.EstonianCaseCollection;

public class EstonianUnchangeableCaseCollection: IEstonianCaseCollection
{
    public required EstonianCase Case { get; set; }
}