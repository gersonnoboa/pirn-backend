namespace PirnBackend.Models;

public class Translation
{
    public required string? OriginalLanguageCode { get; set; }
    public required string? TranslatedLanguageCode { get; set; }
    public required List<string>? Translations { get; set; }
}