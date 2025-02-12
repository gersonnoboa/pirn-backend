using PirnBackend.Extensions;

namespace PirnBackend.Models;

public enum PartOfSpeech
{
    [StringValue("noun")]
    Noun,
    [StringValue("adjective")]
    Adjective,
    [StringValue("verb")]
    Verb,
    [StringValue("adverb")]
    Adverb,
    [StringValue("conjugation")]
    Conjugation,
    [StringValue("pronoun")]
    Pronoun,
    [StringValue("unknown")]
    Unknown,
}

public static class PartOfSpeechExtensions
{
    public static PartOfSpeech GetEstonianPartOfSpeech(string? partOfSpeech)
    {
        return partOfSpeech switch
        {
            "s" => PartOfSpeech.Noun,
            "adj" => PartOfSpeech.Adjective,
            "v" => PartOfSpeech.Verb,
            "adv" => PartOfSpeech.Adverb,
            "konj" => PartOfSpeech.Conjugation,
            "pron" => PartOfSpeech.Pronoun,
            _ => PartOfSpeech.Unknown
        };
    }    
}
