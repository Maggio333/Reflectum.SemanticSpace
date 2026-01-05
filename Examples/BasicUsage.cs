using Reflectum.SemanticSpace.Application.Services;
using Reflectum.SemanticSpace.Domain.Entities;
using Reflectum.SemanticSpace.Domain.Interfaces;
using IAntiLoopService = Reflectum.SemanticSpace.Domain.Interfaces.IAntiLoopService;
using Reflectum.SemanticSpace.DependencyInjection;
using Reflectum.SemanticSpace.Infrastructure.SemanticSpace;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Reflectum.SemanticSpace.Examples
{
    /// <summary>
    /// Przykłady podstawowego użycia biblioteki z Dependency Injection
    /// </summary>
    public static class BasicUsage
    {
        /// <summary>
        /// Przykład 1: Konfiguracja DI i podstawowe użycie
        /// </summary>
        public static void Example1_DependencyInjection()
        {
            // 1. Skonfiguruj DI
            var services = new ServiceCollection();
            services.AddSemanticSpace();

            var serviceProvider = services.BuildServiceProvider();

            // 2. Pobierz serwisy
            var semanticSpace = serviceProvider.GetRequiredService<ISemanticSpace>();
            var searchService = serviceProvider.GetRequiredService<ISemanticSearchService>();
            var resonanceCalculator = serviceProvider.GetRequiredService<IResonanceCalculator>();

            // 3. Załaduj wektory (w praktyce załadujesz je z pliku FastText/Word2Vec)
            var vectors = new List<MeaningVector>
            {
                new MeaningVector { Word = "miłość", Vector = new List<double> { 0.1, 0.2, 0.3 } },
                new MeaningVector { Word = "przyjaźń", Vector = new List<double> { 0.15, 0.25, 0.35 } },
                // ... więcej wektorów
            };

            if (semanticSpace is Infrastructure.SemanticSpace.SemanticSpace space)
            {
                space.Load(vectors);
                space.BuildPrefixIndex();
            }

            // 4. Użyj serwisów
            var similar = searchService.TopK("miłość", k: 5, threshold: 0.65);
            foreach (var word in similar)
            {
                Console.WriteLine($"Podobne do 'miłość': {word.Word}");
            }
        }

        /// <summary>
        /// Przykład 2: Porównywanie wektorów semantycznych
        /// </summary>
        public static void Example2_VectorComparison(ISemanticSpace space, IVectorCache cache, ITextTokenizer tokenizer, ISimilarityCalculator calculator)
        {
            var vector1 = Domain.Entities.SemanticVector.FromInput("miłość i przyjaźń", tokenizer, cache, calculator);
            var vector2 = Domain.Entities.SemanticVector.FromInput("uczucie i bliskość", tokenizer, cache, calculator);

            double similarity = vector1.SemanticSimilarity(vector2, space);
            Console.WriteLine($"Podobieństwo: {similarity:F2}");
        }

        /// <summary>
        /// Przykład 2b: Porównywanie wektorów z DI
        /// </summary>
        public static void Example2b_VectorComparisonWithDI(IServiceProvider serviceProvider)
        {
            var space = serviceProvider.GetRequiredService<ISemanticSpace>();
            var cache = serviceProvider.GetRequiredService<IVectorCache>();
            var tokenizer = serviceProvider.GetRequiredService<ITextTokenizer>();
            var calculator = serviceProvider.GetRequiredService<ISimilarityCalculator>();

            var vector1 = Domain.Entities.SemanticVector.FromInput("miłość i przyjaźń", tokenizer, cache, calculator);
            var vector2 = Domain.Entities.SemanticVector.FromInput("uczucie i bliskość", tokenizer, cache, calculator);

            double similarity = vector1.SemanticSimilarity(vector2, space);
            Console.WriteLine($"Podobieństwo: {similarity:F2}");
        }

        /// <summary>
        /// Przykład 3: Wykrywanie powtórzeń w chatbotach
        /// </summary>
        public static void Example3_AntiLoop()
        {
            var services = new ServiceCollection();
            services.AddSemanticSpace();
            var serviceProvider = services.BuildServiceProvider();

            var detector = serviceProvider.GetRequiredService<IAntiLoopService>();

            string[] userInputs = {
                "Cześć, jak się masz?",
                "Cześć, jak się masz?", // Powtórzenie
                "Co u Ciebie słychać?",
            };

            foreach (var input in userInputs)
            {
                if (detector.IsEcho(input, threshold: 0.85f))
                {
                    Console.WriteLine($"⚠️ Wykryto powtórzenie: '{input}'");
                }
                else
                {
                    Console.WriteLine($"✅ Nowe: '{input}'");
                }
            }
        }

        /// <summary>
        /// Przykład 4: Obliczanie rezonansu między zbiorami słów
        /// </summary>
        public static void Example4_Resonance(IResonanceCalculator resonanceCalculator)
        {
            var setA = new[] { "miłość", "przyjaźń", "bliskość" };
            var setB = new[] { "uczucie", "relacja", "intymność" };

            double resonance = resonanceCalculator.CalculateResonance(setA, setB, threshold: 0.75);
            Console.WriteLine($"Rezonans między zbiorami: {resonance:F2}");
        }
    }
}
