namespace PirnBackend.Models.Words;

public interface IDefinitionGroup
{
    public IEnumerable<IDefinition> Definitions { get; set; }
}