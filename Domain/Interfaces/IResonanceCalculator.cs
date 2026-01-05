using System.Collections.Generic;

namespace Reflectum.SemanticSpace.Domain.Interfaces
{
    /// <summary>
    /// Interfejs dla kalkulatora rezonansu (podobieństwa) między zbiorami słów.
    /// </summary>
    public interface IResonanceCalculator
    {
        /// <summary>
        /// Oblicza "rezonans" (podobieństwo) między dwoma zbiorami słów.
        /// </summary>
        double CalculateResonance(IEnumerable<string> setA, IEnumerable<string> setB, double threshold = 0.75);
    }
}

