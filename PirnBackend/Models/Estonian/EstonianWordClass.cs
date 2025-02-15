using PirnBackend.Extensions;

namespace PirnBackend.Models.Estonian;

public enum EstonianWordClass
{
    [StringValue("noun")]
    Noun,
    [StringValue("verb")]
    Verb,
    [StringValue("constant")]
    Constant,
    [StringValue("unknown")]
    Unknown
}

public static class EstonianWordClassExtensions
{
    public static EstonianWordClass GetEstonianWordClass(string? wordClass)
    {
        return wordClass switch
        {
            "noomen" => EstonianWordClass.Noun,
            "verb" => EstonianWordClass.Verb,
            "muutumatu" => EstonianWordClass.Constant,
            _ => EstonianWordClass.Unknown
        };
    }
}