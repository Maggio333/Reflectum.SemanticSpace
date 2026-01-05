using System;
using System.Collections.Generic;
using System.Linq;
using Reflectum.SemanticSpace.Domain.Entities;
using Reflectum.SemanticSpace.Domain.Interfaces;

namespace Reflectum.SemanticSpace.Infrastructure.SemanticSpace
{
    /// <summary>
    /// Implementacja przestrzeni semantycznej (Infrastructure layer).
    /// </summary>
    public class SemanticSpace : ISemanticSpace
    {
        private List<MeaningVector> _vectors = new();
        private Dictionary<string, MeaningVector> _index = new();
        private readonly IIndexBuilder _indexBuilder;

        /// <summary>
        /// Tworzy nową przestrzeń semantyczną.
        /// </summary>
        /// <param name="indexBuilder">Budowniczy indeksów</param>
        public SemanticSpace(IIndexBuilder indexBuilder)
        {
            _indexBuilder = indexBuilder ?? throw new ArgumentNullException(nameof(indexBuilder));
        }

        /// <summary>
        /// Buduje indeks prefiksowy dla szybkiego wyszukiwania.
        /// </summary>
        /// <param name="prefixLength">Długość prefiksu (domyślnie 2)</param>
        public void BuildPrefixIndex(int prefixLength = 2)
        {
            _indexBuilder.BuildPrefixIndex(_vectors, prefixLength);
        }

        /// <summary>
        /// Ładuje wektory do przestrzeni semantycznej.
        /// </summary>
        public void Load(IEnumerable<MeaningVector> vectors)
        {
            _vectors = vectors.ToList();
            _index = _vectors
                .GroupBy(v => v.Word.ToLowerInvariant())
                .ToDictionary(g => g.Key, g => g.First());

            foreach (var v in _vectors)
                v.Normalize();
        }

        /// <inheritdoc/>
        public MeaningVector? Find(string word)
        {
            _index.TryGetValue(word.ToLowerInvariant(), out var result);
            return result;
        }

        /// <inheritdoc/>
        public IEnumerable<MeaningVector> GetAll() => _vectors;
    }
}

