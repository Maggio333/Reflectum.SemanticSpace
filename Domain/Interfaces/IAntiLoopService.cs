namespace Reflectum.SemanticSpace.Domain.Interfaces
{
    /// <summary>
    /// Interfejs dla serwisu wykrywania powtórzeń w interakcjach tekstowych.
    /// </summary>
    public interface IAntiLoopService
    {
        /// <summary>
        /// Sprawdza czy dane wejście jest powtórzeniem (echo).
        /// </summary>
        /// <param name="input">Tekst wejściowy</param>
        /// <param name="threshold">Próg wykrywania echo (domyślnie 0.85)</param>
        /// <returns>True jeśli wykryto echo</returns>
        bool IsEcho(string input, float threshold = 0.85f);

        /// <summary>
        /// Zwraca raport o wykrytych echo (użyteczne do debugowania).
        /// </summary>
        string GetEchoReport();

        /// <summary>
        /// Czyści historię i echo scores.
        /// </summary>
        void Clear();
    }
}

