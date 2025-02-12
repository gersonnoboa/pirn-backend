namespace PirnBackend.Models.Estonian;

public class EstonianTranslation: ITranslation
{
    public required string? OriginalLanguageCode { get; set; }
    public required string? TranslatedLanguageCode { get; set; }
    public required List<string>? Translations { get; set; }
}