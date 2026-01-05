namespace Reflectum.SemanticSpace.Domain.Interfaces
{
    /// <summary>
    /// Interfejs dla metryki podobieństwa tekstowego (używanej w wykrywaniu powtórzeń).
    /// </summary>
    public interface ISimilarityMetric
    {
        /// <summary>
        /// Oblicza podobieństwo między dwoma tekstami.
        /// </summary>
        float Calculate(string textA, string textB);
    }
}

