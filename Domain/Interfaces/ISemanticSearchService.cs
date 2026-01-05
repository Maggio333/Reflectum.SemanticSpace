using System.Collections.Generic;
using Reflectum.SemanticSpace.Domain.Entities;

namespace Reflectum.SemanticSpace.Domain.Interfaces
{
    /// <summary>
    /// Interfejs dla usługi wyszukiwania semantycznego.
    /// </summary>
    public interface ISemanticSearchService
    {
        /// <summary>
        /// Znajduje top K najbardziej podobnych słów do danego słowa.
        /// </summary>
        List<MeaningVector> TopK(string word, int k = 5, double threshold = 0.65);

        /// <summary>
        /// Znajduje najbliższy wektor dla danego słowa.
        /// </summary>
        MeaningVector? FindClosest(string word);

        /// <summary>
        /// Projekcja semantyczna - znajduje podobne słowa w przestrzeni.
        /// </summary>
        List<MeaningVector> Project(string word, int topK = 3, double threshold = 0.75);
    }
}

