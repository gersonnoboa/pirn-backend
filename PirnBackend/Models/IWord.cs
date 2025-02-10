namespace PirnBackend.Models;

public interface IWord
{
    public string OriginalLanguageText { get; set; }
    public List<WordMeaning> Meanings { get; set; }
}
