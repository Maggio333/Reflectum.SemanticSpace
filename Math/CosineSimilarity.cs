using System;

namespace Reflectum.SemanticSpace.Math
{
    /// <summary>
    /// Oblicza podobieństwo cosinusowe między dwoma wektorami.
    /// </summary>
    public static class CosineSimilarity
    {
        /// <summary>
        /// Oblicza podobieństwo cosinusowe między dwoma wektorami.
        /// </summary>
        /// <param name="vecA">Pierwszy wektor</param>
        /// <param name="vecB">Drugi wektor</param>
        /// <returns>Wartość z zakresu [-1, 1], gdzie 1 oznacza identyczne wektory</returns>
        /// <exception cref="ArgumentException">Gdy wektory mają różną długość</exception>
        public static double Between(double[] vecA, double[] vecB)
        {
            if (vecA.Length != vecB.Length)
                throw new ArgumentException("Wektory muszą mieć taką samą długość.");

            double dot = 0.0, normA = 0.0, normB = 0.0;
            for (int i = 0; i < vecA.Length; i++)
            {
                dot += vecA[i] * vecB[i];
                normA += vecA[i] * vecA[i];
                normB += vecB[i] * vecB[i];
            }

            if (normA == 0 || normB == 0) return 0.0;
            return dot / (System.Math.Sqrt(normA) * System.Math.Sqrt(normB));
        }
    }
}

