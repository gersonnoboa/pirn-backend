namespace PirnBackend.Models.Words;

public interface IDefinition
{
    public string? Definition { get; set; }
    public PartOfSpeech? PartOfSpeech { get; set; }
    public List<string>? Examples { get; set; }
}