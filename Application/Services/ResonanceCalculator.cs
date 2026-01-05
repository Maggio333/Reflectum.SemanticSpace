using System;
using System.Collections.Generic;
using System.Linq;
using Reflectum.SemanticSpace.Domain.Interfaces;

namespace Reflectum.SemanticSpace.Application.Services
{
    /// <summary>
    /// Serwis aplikacyjny do obliczania rezonansu między zbiorami słów.
    /// </summary>
    public class ResonanceCalculator : IResonanceCalculator
    {
        private readonly ISemanticSpace _semanticSpace;
        private readonly ISimilarityCalculator _similarityCalculator;

        /// <summary>
        /// Tworzy nowy kalkulator rezonansu.
        /// </summary>
        public ResonanceCalculator(ISemanticSpace semanticSpace, ISimilarityCalculator similarityCalculator)
        {
            _semanticSpace = semanticSpace ?? throw new ArgumentNullException(nameof(semanticSpace));
            _similarityCalculator = similarityCalculator ?? throw new ArgumentNullException(nameof(similarityCalculator));
        }

        /// <inheritdoc/>
        public double CalculateResonance(IEnumerable<string> setA, IEnumerable<string> setB, double threshold = 0.75)
        {
            int matches = 0;
            int total = setA.Count();

            foreach (var wordA in setA)
            {
                var vectorA = _semanticSpace.Find(wordA);
                if (vectorA == null) continue;

                foreach (var wordB in setB)
                {
                    var vectorB = _semanticSpace.Find(wordB);
                    if (vectorB == null) continue;

                    double sim = _similarityCalculator.Calculate(vectorA, vectorB);
                    if (sim >= threshold)
                    {
                        matches++;
                        break; // tylko jedno trafienie wystarczy
                    }
                }
            }

            return total == 0 ? 0.0 : (double)matches / total;
        }
    }
}

