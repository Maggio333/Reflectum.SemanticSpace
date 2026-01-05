using Microsoft.Extensions.DependencyInjection;
using Reflectum.SemanticSpace.Application.Services;
using Reflectum.SemanticSpace.Domain.Interfaces;
using Reflectum.SemanticSpace.Infrastructure.AntiLoop;
using Reflectum.SemanticSpace.Infrastructure.Caching;
using Reflectum.SemanticSpace.Infrastructure.Indexing;
using Reflectum.SemanticSpace.Infrastructure.SemanticSpace;
using Reflectum.SemanticSpace.Infrastructure.Similarity;
using Reflectum.SemanticSpace.Infrastructure.Tokenization;

namespace Reflectum.SemanticSpace.DependencyInjection
{
    /// <summary>
    /// Rozszerzenia dla rejestracji serwisów w kontenerze DI.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Dodaje wszystkie serwisy Reflectum.SemanticSpace do kontenera DI.
        /// </summary>
        /// <param name="services">Kolekcja serwisów</param>
        /// <param name="configure">Opcjonalna konfiguracja</param>
        /// <returns>Kolekcja serwisów dla chainowania</returns>
        public static IServiceCollection AddSemanticSpace(
            this IServiceCollection services,
            Action<SemanticSpaceOptions>? configure = null)
        {
            var options = new SemanticSpaceOptions();
            configure?.Invoke(options);

            // Infrastructure - implementacje
            services.AddSingleton<ISimilarityCalculator, CosineSimilarityCalculator>();
            services.AddSingleton<IIndexBuilder, PrefixIndexBuilder>();
            services.AddSingleton<IVectorCache, InMemoryVectorCache>();
            services.AddSingleton<ITextTokenizer, DefaultTextTokenizer>();
            services.AddSingleton<ISimilarityMetric, JaccardSimilarityMetric>();

            // SemanticSpace - można użyć jako singleton lub scoped
            if (options.SemanticSpaceLifetime == ServiceLifetime.Singleton)
            {
                services.AddSingleton<ISemanticSpace, Infrastructure.SemanticSpace.SemanticSpace>();
            }
            else
            {
                services.AddScoped<ISemanticSpace, Infrastructure.SemanticSpace.SemanticSpace>();
            }

            // Application - serwisy aplikacyjne
            services.AddSingleton<ISemanticSearchService, SemanticSearchService>();
            services.AddSingleton<IResonanceCalculator, ResonanceCalculator>();
            services.AddScoped<IAntiLoopService, AntiLoopService>();

            return services;
        }
    }

    /// <summary>
    /// Opcje konfiguracji dla SemanticSpace.
    /// </summary>
    public class SemanticSpaceOptions
    {
        /// <summary>
        /// Określa lifetime dla SemanticSpace (domyślnie Singleton).
        /// </summary>
        public ServiceLifetime SemanticSpaceLifetime { get; set; } = ServiceLifetime.Singleton;
    }
}
