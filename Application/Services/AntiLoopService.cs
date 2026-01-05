using System;
using System.Collections.Generic;
using Reflectum.SemanticSpace.Domain.Interfaces;

namespace Reflectum.SemanticSpace.Application.Services
{
    /// <summary>
    /// Serwis aplikacyjny do wykrywania powtórzeń w interakcjach tekstowych.
    /// </summary>
    public class AntiLoopService : IAntiLoopService
    {
        private readonly List<string> _history;
        private readonly Dictionary<string, float> _echoScores;
        private readonly float _decayFactor;
        private readonly ISimilarityMetric _similarityMetric;

        /// <summary>
        /// Tworzy nowy serwis anty-powtórzeniowy.
        /// </summary>
        /// <param name="decayFactor">Współczynnik zanikania (0-1), domyślnie 0.95</param>
        /// <param name="similarityMetric">Metryka podobieństwa</param>
        public AntiLoopService(
            float decayFactor = 0.95f,
            ISimilarityMetric? similarityMetric = null)
        {
            _history = new List<string>();
            _echoScores = new Dictionary<string, float>();
            _decayFactor = decayFactor;
            _similarityMetric = similarityMetric ?? throw new ArgumentNullException(nameof(similarityMetric));
        }

        /// <summary>
        /// Sprawdza czy dane wejście jest powtórzeniem (echo).
        /// </summary>
        /// <param name="input">Tekst wejściowy</param>
        /// <param name="threshold">Próg wykrywania echo (domyślnie 0.85)</param>
        /// <returns>True jeśli wykryto echo</returns>
        public bool IsEcho(string input, float threshold = 0.85f)
        {
            float score = ComputeEchoScore(input);
            return score > threshold;
        }

        private float ComputeEchoScore(string input)
        {
            float score = 0.0f;

            foreach (var past in _history)
            {
                float sim = _similarityMetric.Calculate(input, past);
                score = System.Math.Max(score, sim);
            }

            if (!_echoScores.ContainsKey(input))
                _echoScores[input] = score;
            else
                _echoScores[input] = _echoScores[input] * _decayFactor + score * (1 - _decayFactor);

            _history.Add(input);
            return _echoScores[input];
        }

        /// <summary>
        /// Zwraca raport o wykrytych echo (użyteczne do debugowania).
        /// </summary>
        public string GetEchoReport()
        {
            return string.Join("\n", _echoScores.Select(kvp => $"{kvp.Key} => {kvp.Value:F2}"));
        }

        /// <summary>
        /// Czyści historię i echo scores.
        /// </summary>
        public void Clear()
        {
            _history.Clear();
            _echoScores.Clear();
        }
    }
}

