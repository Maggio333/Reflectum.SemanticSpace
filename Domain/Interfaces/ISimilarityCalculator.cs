using Reflectum.SemanticSpace.Domain.Entities;

namespace Reflectum.SemanticSpace.Domain.Interfaces
{
    /// <summary>
    /// Interfejs dla kalkulatora podobieństwa między wektorami.
    /// </summary>
    public interface ISimilarityCalculator
    {
        /// <summary>
        /// Oblicza podobieństwo między dwoma wektorami.
        /// </summary>
        double Calculate(MeaningVector vectorA, MeaningVector vectorB);
    }
}

