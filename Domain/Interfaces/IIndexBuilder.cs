using System.Collections.Generic;
using Reflectum.SemanticSpace.Domain.Entities;

namespace Reflectum.SemanticSpace.Domain.Interfaces
{
    /// <summary>
    /// Interfejs dla budowniczego indeksów przestrzeni semantycznej.
    /// </summary>
    public interface IIndexBuilder
    {
        /// <summary>
        /// Buduje indeks prefiksowy dla szybkiego wyszukiwania.
        /// </summary>
        void BuildPrefixIndex(IEnumerable<MeaningVector> vectors, int prefixLength = 2);
    }
}

