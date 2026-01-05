using System.Collections.Generic;
using Reflectum.SemanticSpace.Domain.Entities;

namespace Reflectum.SemanticSpace.Domain.Interfaces
{
    /// <summary>
    /// Interfejs reprezentujący przestrzeń semantyczną dla wyszukiwania wektorów znaczenia.
    /// </summary>
    public interface ISemanticSpace
    {
        /// <summary>
        /// Wyszukuje wektor dla danego słowa.
        /// </summary>
        MeaningVector? Find(string word);

        /// <summary>
        /// Zwraca wszystkie wektory w przestrzeni.
        /// </summary>
        IEnumerable<MeaningVector> GetAll();
    }
}

