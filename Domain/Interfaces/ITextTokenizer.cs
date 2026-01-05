namespace Reflectum.SemanticSpace.Domain.Interfaces
{
    /// <summary>
    /// Interfejs dla tokenizera tekstu.
    /// </summary>
    public interface ITextTokenizer
    {
        /// <summary>
        /// Tokenizuje tekst na komponenty (słowa, frazy).
        /// </summary>
        IEnumerable<string> Tokenize(string input);
    }
}

