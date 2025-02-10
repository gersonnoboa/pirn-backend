namespace PirnBackend.Models;

public enum PartOfSpeech
{
    Noun,
    Adjective,
    Verb,
    Adverb,
    Conjugation,
    Pronoun,
    Unknown,
}

public static class PartOfSpeechExtensions
{
    public static PartOfSpeech GetEstonianPartOfSpeech(string partOfSpeech)
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
