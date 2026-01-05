using System;
using System.Collections.Generic;
using System.Linq;
using Reflectum.SemanticSpace.Domain.Entities;
using Reflectum.SemanticSpace.Domain.Interfaces;

namespace Reflectum.SemanticSpace.Infrastructure.Indexing
{
    /// <summary>
    /// Implementacja budowniczego indeksu prefiksowego (Infrastructure layer).
    /// </summary>
    public class PrefixIndexBuilder : IIndexBuilder
    {
        private Dictionary<string, List<MeaningVector>> _prefixIndex = new();

        /// <inheritdoc/>
        public void BuildPrefixIndex(IEnumerable<MeaningVector> vectors, int prefixLength = 2)
        {
            _prefixIndex = vectors
                .GroupBy(w => w.Word.Substring(0, System.Math.Min(prefixLength, w.Word.Length)).ToLowerInvariant())
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        /// <summary>
        /// Pobiera wektory dla danego prefiksu.
        /// </summary>
        public List<MeaningVector>? GetVectorsByPrefix(string prefix)
        {
            _prefixIndex.TryGetValue(prefix.ToLowerInvariant(), out var vectors);
            return vectors;
        }

        /// <summary>
        /// Sprawdza czy indeks został zbudowany.
        /// </summary>
        public bool IsBuilt => _prefixIndex.Count > 0;
    }
}

