using PirnBackend.Extensions;

namespace PirnBackend.Models.Estonian;

public enum EstonianWordClass
{
    [StringValue("noomen")]
    Noomen,
    [StringValue("verb")]
    Verb,
    [StringValue("muutumatu")]
    Muutumatu,
    [StringValue("unknown")]
    Unknown
}

public static class EstonianWordClassExtensions
{
    public static EstonianWordClass GetEstonianWordClass(string? wordClass)
    {
        return wordClass switch
        {
            "noomen" => EstonianWordClass.Noomen,
            "verb" => EstonianWordClass.Verb,
            "muutumatu" => EstonianWordClass.Muutumatu,
            _ => EstonianWordClass.Unknown
        };
    }
}