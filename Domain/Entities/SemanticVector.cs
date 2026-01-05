using System;
using System.Collections.Generic;
using System.Linq;
using Reflectum.SemanticSpace.Domain.Interfaces;

namespace Reflectum.SemanticSpace.Domain.Entities
{
    /// <summary>
    /// Encja reprezentująca wektor semantyczny złożony z wielu komponentów tekstowych.
    /// </summary>
    public class SemanticVector
    {
        /// <summary>
        /// Komponenty tekstowe wektora (słowa, frazy)
        /// </summary>
        public List<string> Components { get; set; }

        private readonly IVectorCache _cache;
        private readonly ITextTokenizer _tokenizer;
        private readonly ISimilarityCalculator _similarityCalculator;

        /// <summary>
        /// Tworzy nowy SemanticVector z komponentów tekstowych.
        /// </summary>
        /// <param name="components">Komponenty tekstowe (słowa, frazy)</param>
        /// <param name="cache">Cache wektorów (opcjonalny)</param>
        /// <param name="tokenizer">Tokenizer tekstu (opcjonalny)</param>
        /// <param name="similarityCalculator">Kalkulator podobieństwa (opcjonalny)</param>
        public SemanticVector(
            IEnumerable<string>? components,
            IVectorCache? cache = null,
            ITextTokenizer? tokenizer = null,
            ISimilarityCalculator? similarityCalculator = null)
        {
            Components = components?
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c.Trim().ToLowerInvariant())
                .Distinct()
                .ToList()
                ?? new List<string>();

            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _tokenizer = tokenizer ?? throw new ArgumentNullException(nameof(tokenizer));
            _similarityCalculator = similarityCalculator ?? throw new ArgumentNullException(nameof(similarityCalculator));
        }

        /// <summary>
        /// Tworzy SemanticVector z tekstu wejściowego (tokenizacja).
        /// Wymaga wstrzyknięcia zależności przez DI.
        /// </summary>
        public static SemanticVector FromInput(
            string input,
            ITextTokenizer tokenizer,
            IVectorCache cache,
            ISimilarityCalculator similarityCalculator)
        {
            if (tokenizer == null) throw new ArgumentNullException(nameof(tokenizer));
            if (cache == null) throw new ArgumentNullException(nameof(cache));
            if (similarityCalculator == null) throw new ArgumentNullException(nameof(similarityCalculator));
            
            var parts = tokenizer.Tokenize(input);
            return new SemanticVector(parts, cache, tokenizer, similarityCalculator);
        }

        /// <summary>
        /// Konwertuje wektor semantyczny na średni wektor numeryczny w przestrzeni semantycznej.
        /// Używa cache dla wydajności.
        /// </summary>
        public double[] ToMeanVector(ISemanticSpace space)
        {
            string key = string.Join('_', Components);
            if (_cache.TryGet(key, out var cached) && cached != null)
                return cached;

            var vectors = Components
                .Select(s => space.Find(s)?.ToArray())
                .Where(v => v != null)
                .Select(v => v!)
                .ToList();

            if (!vectors.Any())
                return new double[300]; // Domyślny wymiar dla FastText

            var mean = new double[vectors[0].Length];
            foreach (var w in vectors)
                for (int i = 0; i < mean.Length; i++)
                    mean[i] += w[i];

            for (int i = 0; i < mean.Length; i++)
                mean[i] /= vectors.Count;

            _cache.Set(key, mean);
            return mean;
        }

        /// <summary>
        /// Oblicza podobieństwo semantyczne między dwoma wektorami przez przestrzeń semantyczną.
        /// </summary>
        public double SemanticSimilarity(SemanticVector other, ISemanticSpace space)
        {
            var v1 = this.ToMeanVector(space);
            var v2 = other.ToMeanVector(space);
            
            // Używamy bezpośrednio CosineSimilarity dla tablic
            return Reflectum.SemanticSpace.Math.CosineSimilarity.Between(v1, v2);
        }
    }
}

