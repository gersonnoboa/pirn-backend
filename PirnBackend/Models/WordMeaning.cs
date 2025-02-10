using PirnBackend.Extensions;

namespace PirnBackend.Models;

public class WordMeaning
{
    public required string Definition  { get; set; }
    public required PartOfSpeech PartOfSpeech  { get; set; }
    public required string Examples { get ; set ; }
}
