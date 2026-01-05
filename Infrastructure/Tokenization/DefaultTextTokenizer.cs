using System;
using System.Collections.Generic;
using System.Linq;
using Reflectum.SemanticSpace.Domain.Interfaces;

namespace Reflectum.SemanticSpace.Infrastructure.Tokenization
{
    /// <summary>
    /// Domyślna implementacja tokenizera tekstu (Infrastructure layer).
    /// </summary>
    public class DefaultTextTokenizer : ITextTokenizer
    {
        private readonly char[] _separators;

        /// <summary>
        /// Tworzy nowy tokenizer z domyślnymi separatorami.
        /// </summary>
        public DefaultTextTokenizer()
        {
            _separators = new[] { ' ', '.', ',', ':', '·', '\t', '\n', '\r', ';', '!' };
        }

        /// <summary>
        /// Tworzy nowy tokenizer z niestandardowymi separatorami.
        /// </summary>
        public DefaultTextTokenizer(char[] separators)
        {
            _separators = separators ?? throw new ArgumentNullException(nameof(separators));
        }

        /// <inheritdoc/>
        public IEnumerable<string> Tokenize(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Enumerable.Empty<string>();

            return input
                .Split(_separators, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim().ToLowerInvariant())
                .Where(p => !string.IsNullOrWhiteSpace(p));
        }
    }
}

