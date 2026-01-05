using System;
using System.Collections.Generic;

namespace Reflectum.SemanticSpace.Domain.Entities
{
    /// <summary>
    /// Encja reprezentująca wektor znaczenia słowa w przestrzeni semantycznej.
    /// </summary>
    public class MeaningVector
    {
        /// <summary>
        /// Słowo reprezentowane przez ten wektor
        /// </summary>
        public string Word { get; set; } = string.Empty;

        /// <summary>
        /// Opcjonalna definicja słowa
        /// </summary>
        public string? Definition { get; set; }

        /// <summary>
        /// Wektor numeryczny reprezentujący znaczenie (np. z FastText, Word2Vec)
        /// </summary>
        public List<double> Vector { get; set; } = new();

        /// <summary>
        /// Opcjonalne powiązania semantyczne
        /// </summary>
        public List<string> Relations { get; set; } = new();

        /// <summary>
        /// Opcjonalne tagi/kategorie
        /// </summary>
        public List<string> Tags { get; set; } = new();

        /// <summary>
        /// Normalizuje wektor do długości 1 (L2 normalization)
        /// </summary>
        public void Normalize()
        {
            double norm = 0.0;
            foreach (var x in Vector)
                norm += x * x;

            norm = System.Math.Sqrt(norm);
            if (norm == 0.0) return;

            for (int i = 0; i < Vector.Count; i++)
                Vector[i] /= norm;
        }

        /// <summary>
        /// Konwertuje wektor do tablicy double[]
        /// </summary>
        public double[] ToArray()
        {
            return Vector.ToArray();
        }
    }
}

