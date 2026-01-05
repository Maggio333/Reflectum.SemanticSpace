using System.Collections.Generic;
using System.Linq;
using Reflectum.SemanticSpace.Domain.Interfaces;

namespace Reflectum.SemanticSpace.Infrastructure.AntiLoop
{
    /// <summary>
    /// Implementacja metryki podobieństwa używająca Jaccard Similarity (Infrastructure layer).
    /// </summary>
    public class JaccardSimilarityMetric : ISimilarityMetric
    {
        /// <inheritdoc/>
        public float Calculate(string textA, string textB)
        {
            var setA = new HashSet<string>(textA.Split(' ', System.StringSplitOptions.RemoveEmptyEntries));
            var setB = new HashSet<string>(textB.Split(' ', System.StringSplitOptions.RemoveEmptyEntries));

            var intersection = setA.Intersect(setB).Count();
            var union = setA.Union(setB).Count();

            if (union == 0) return 0.0f;
            return (float)intersection / union;
        }
    }
}

