namespace Reflectum.SemanticSpace.Domain.Interfaces
{
    /// <summary>
    /// Interfejs dla cache wektorów numerycznych.
    /// </summary>
    public interface IVectorCache
    {
        /// <summary>
        /// Próbuje pobrać wektor z cache.
        /// </summary>
        bool TryGet(string key, out double[]? vector);

        /// <summary>
        /// Dodaje wektor do cache.
        /// </summary>
        void Set(string key, double[] vector);

        /// <summary>
        /// Czyści cache.
        /// </summary>
        void Clear();
    }
}

