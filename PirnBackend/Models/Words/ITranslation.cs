namespace PirnBackend.Models;

public interface ITranslation
{
    public string? OriginalLanguageCode { get; set; }
    public string? TranslatedLanguageCode { get; set; }
    public List<string>? Translations { get; set; }
}