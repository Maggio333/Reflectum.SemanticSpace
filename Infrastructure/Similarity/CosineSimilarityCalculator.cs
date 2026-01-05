using Reflectum.SemanticSpace.Domain.Entities;
using Reflectum.SemanticSpace.Domain.Interfaces;
using Reflectum.SemanticSpace.Math;

namespace Reflectum.SemanticSpace.Infrastructure.Similarity
{
    /// <summary>
    /// Implementacja kalkulatora podobieństwa używająca Cosine Similarity (Infrastructure layer).
    /// </summary>
    public class CosineSimilarityCalculator : ISimilarityCalculator
    {
        /// <inheritdoc/>
        public double Calculate(MeaningVector vectorA, MeaningVector vectorB)
        {
            return CosineSimilarity.Between(vectorA.ToArray(), vectorB.ToArray());
        }
    }
}

