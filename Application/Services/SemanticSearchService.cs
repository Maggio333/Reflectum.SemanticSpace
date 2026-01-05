using System;
using System.Collections.Generic;
using System.Linq;
using Reflectum.SemanticSpace.Domain.Entities;
using Reflectum.SemanticSpace.Domain.Interfaces;

namespace Reflectum.SemanticSpace.Application.Services
{
    /// <summary>
    /// Serwis aplikacyjny dla wyszukiwania semantycznego.
    /// </summary>
    public class SemanticSearchService : ISemanticSearchService
    {
        private readonly ISemanticSpace _semanticSpace;
        private readonly ISimilarityCalculator _similarityCalculator;
        private readonly IIndexBuilder _indexBuilder;

        /// <summary>
        /// Tworzy nową usługę wyszukiwania semantycznego.
        /// </summary>
        public SemanticSearchService(
            ISemanticSpace semanticSpace,
            ISimilarityCalculator similarityCalculator,
            IIndexBuilder indexBuilder)
        {
            _semanticSpace = semanticSpace ?? throw new ArgumentNullException(nameof(semanticSpace));
            _similarityCalculator = similarityCalculator ?? throw new ArgumentNullException(nameof(similarityCalculator));
            _indexBuilder = indexBuilder ?? throw new ArgumentNullException(nameof(indexBuilder));
        }

        /// <inheritdoc/>
        public List<MeaningVector> TopK(string word, int k = 5, double threshold = 0.65)
        {
            var target = _semanticSpace.Find(word);
            if (target == null) return new();

            var prefix = word.Substring(0, System.Math.Min(2, word.Length)).ToLowerInvariant();

            // Próbuj użyć indeksu prefiksowego jeśli dostępny
            List<MeaningVector>? localVectors = null;
            if (_indexBuilder is Infrastructure.Indexing.PrefixIndexBuilder prefixBuilder && prefixBuilder.IsBuilt)
            {
                localVectors = prefixBuilder.GetVectorsByPrefix(prefix);
            }

            if (localVectors != null)
            {
                return localVectors
                    .Where(w => !w.Word.Equals(word, StringComparison.OrdinalIgnoreCase))
                    .Select(w => new
                    {
                        Vector = w,
                        Score = _similarityCalculator.Calculate(target, w)
                    })
                    .Where(x => x.Score >= threshold)
                    .OrderByDescending(x => x.Score)
                    .Take(k)
                    .Select(x => x.Vector)
                    .ToList();
            }

            // Fallback: przeszukaj wszystkie wektory (gdy brak indeksu)
            return _semanticSpace.GetAll()
                .Where(w => !w.Word.Equals(word, StringComparison.OrdinalIgnoreCase))
                .Select(w => new
                {
                    Vector = w,
                    Score = _similarityCalculator.Calculate(target, w)
                })
                .Where(x => x.Score >= threshold)
                .OrderByDescending(x => x.Score)
                .Take(k)
                .Select(x => x.Vector)
                .ToList();
        }

        /// <inheritdoc/>
        public MeaningVector? FindClosest(string word)
        {
            var target = _semanticSpace.Find(word);
            if (target == null) return null;

            MeaningVector? closest = null;
            double maxSim = -1;

            foreach (var entry in _semanticSpace.GetAll())
            {
                if (entry.Word.Equals(word, StringComparison.OrdinalIgnoreCase))
                    continue;

                double sim = _similarityCalculator.Calculate(target, entry);
                if (sim > maxSim)
                {
                    maxSim = sim;
                    closest = entry;
                }
            }

            return closest;
        }

        /// <inheritdoc/>
        public List<MeaningVector> Project(string word, int topK = 3, double threshold = 0.75)
        {
            var prefix = word.Substring(0, System.Math.Min(2, word.Length)).ToLowerInvariant();

            // Próbuj użyć indeksu prefiksowego jeśli dostępny
            List<MeaningVector>? localVectors = null;
            if (_indexBuilder is Infrastructure.Indexing.PrefixIndexBuilder prefixBuilder && prefixBuilder.IsBuilt)
            {
                localVectors = prefixBuilder.GetVectorsByPrefix(prefix);
            }

            if (localVectors != null)
            {
                var target = _semanticSpace.Find(word);
                if (target == null) return new();

                return localVectors
                    .Where(w => !w.Word.Equals(word, StringComparison.OrdinalIgnoreCase))
                    .Select(w => new
                    {
                        W = w,
                        Sim = _similarityCalculator.Calculate(target, w)
                    })
                    .Where(x => x.Sim >= threshold)
                    .OrderByDescending(x => x.Sim)
                    .Take(topK)
                    .Select(x => x.W)
                    .ToList();
            }

            return new();
        }
    }
}

