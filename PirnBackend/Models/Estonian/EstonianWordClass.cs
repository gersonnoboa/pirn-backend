using PirnBackend.Extensions;

namespace PirnBackend.Models.Estonian;

public enum EstonianWordClass
{
    [StringValue("noomen")]
    Noomen,
    [StringValue("verb")]
    Verb,
    [StringValue("muutumatu")]
    Muutumatu
}