using System.Collections.Generic;
using Reflectum.SemanticSpace.Domain.Interfaces;

namespace Reflectum.SemanticSpace.Infrastructure.Caching
{
    /// <summary>
    /// Implementacja cache wektorów w pamięci (Infrastructure layer).
    /// </summary>
    public class InMemoryVectorCache : IVectorCache
    {
        private readonly Dictionary<string, double[]> _cache = new();

        /// <inheritdoc/>
        public bool TryGet(string key, out double[]? vector)
        {
            return _cache.TryGetValue(key, out vector);
        }

        /// <inheritdoc/>
        public void Set(string key, double[] vector)
        {
            _cache[key] = vector;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            _cache.Clear();
        }
    }
}

