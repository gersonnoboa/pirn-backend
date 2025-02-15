namespace PirnBackend.Models.EstonianCaseCollection;

public class EstonianNounCase
{
    public required string Name { get; set; }
    public string? Singular { get; set; }
    public string? Plural { get; set; }
}