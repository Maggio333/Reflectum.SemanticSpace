# Reflectum.SemanticSpace

[![CI](https://github.com/Maggio333/Reflectum.SemanticSpace/actions/workflows/ci.yml/badge.svg)](https://github.com/Maggio333/Reflectum.SemanticSpace/actions)
[![NuGet](https://img.shields.io/nuget/vpre/Reflectum.SemanticSpace.svg?label=NuGet&color=blue)](https://www.nuget.org/packages/Reflectum.SemanticSpace)
[![Tests](https://img.shields.io/badge/tests-87%20passed-brightgreen)](https://github.com/Maggio333/Reflectum.SemanticSpace/actions)

Biblioteka .NET do pracy z przestrzenią semantyczną i wykrywania powtórzeń w interakcjach tekstowych. Wyciągnięte z projektu ReflectumEngine - najbardziej wartościowe komponenty bez danych behawioralnych.

## 🎯 Po co to komu?

### Przypadki użycia

- **Chatboty** - wykrywanie powtórzeń, unikanie zapętlenia odpowiedzi
- **Systemy rekomendacyjne** - rekomendacje oparte na treści, porównywanie podobieństwa
- **Wyszukiwarki semantyczne** - wyszukiwanie podobnych słów, ekspansja zapytań
- **Analiza tekstu** - porównywanie dokumentów, klasteryzacja, ekstrakcja konceptów

## ✨ Cechy

- ✅ **Architektura warstwowa** - Domain/Application/Infrastructure (Clean Architecture)
- ✅ **SOLID** - zaprojektowane zgodnie z zasadami SOLID
- ✅ **Dependency Injection** - pełne wsparcie dla Microsoft.Extensions.DependencyInjection
- ✅ **Wydajność** - indeks prefiksowy, cache wektorów
- ✅ **Rozszerzalność** - interfejsy umożliwiają własne implementacje
- ✅ **Testowalność** - łatwe mockowanie, brak zależności zewnętrznych
- ✅ **Testy jednostkowe** - 87 testów pokrywających wszystkie komponenty (100% passing)
- ✅ **Dokumentacja** - kompletna dokumentacja markdown (docs/) i komentarze XML w kodzie (IntelliSense)

## 📦 Instalacja


### NuGet (gdy opublikowane)

```bash
dotnet add package Reflectum.SemanticSpace
```

### Z kodu źródłowego

```bash
git clone https://github.com/Maggio333/Reflectum.SemanticSpace.git
cd Reflectum.SemanticSpace
dotnet build
dotnet test  # Uruchom testy
```

## 🚀 Szybki start

### 1. Konfiguracja z Dependency Injection

```csharp
using Reflectum.SemanticSpace.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddSemanticSpace();

var serviceProvider = services.BuildServiceProvider();
```

### 2. Załaduj wektory

```csharp
var semanticSpace = serviceProvider.GetRequiredService<ISemanticSpace>();

// Załaduj wektory (np. z FastText, Word2Vec)
var vectors = LoadVectorsFromFile("vectors.vec");

if (semanticSpace is Infrastructure.SemanticSpace.SemanticSpace space)
{
    space.Load(vectors);
    space.BuildPrefixIndex(); // Ważne dla wydajności!
}
```

### 3. Wyszukiwanie podobnych słów

```csharp
var searchService = serviceProvider.GetRequiredService<ISemanticSearchService>();

var similar = searchService.TopK("miłość", k: 5, threshold: 0.65);
foreach (var word in similar)
{
    Console.WriteLine($"Podobne: {word.Word}");
}
```

### 4. Wykrywanie powtórzeń

```csharp
using Reflectum.SemanticSpace.Domain.Interfaces;

var detector = serviceProvider.GetRequiredService<IAntiLoopService>();

if (detector.IsEcho("Cześć, jak się masz?", threshold: 0.85f))
{
    Console.WriteLine("Wykryto powtórzenie!");
}
```

### 5. Porównywanie wektorów

```csharp
using Reflectum.SemanticSpace.Domain.Entities;
using Reflectum.SemanticSpace.Domain.Interfaces;

var cache = serviceProvider.GetRequiredService<IVectorCache>();
var tokenizer = serviceProvider.GetRequiredService<ITextTokenizer>();
var calculator = serviceProvider.GetRequiredService<ISimilarityCalculator>();

var vector1 = SemanticVector.FromInput("miłość i przyjaźń", tokenizer, cache, calculator);
var vector2 = SemanticVector.FromInput("uczucie i bliskość", tokenizer, cache, calculator);

double similarity = vector1.SemanticSimilarity(vector2, semanticSpace);
Console.WriteLine($"Podobieństwo: {similarity:F2}");
```

## 📚 Dokumentacja

- **[Getting Started](docs/GETTING_STARTED.md)** - przewodnik dla początkujących
- **[Architecture](docs/ARCHITECTURE.md)** - architektura, SOLID, wzorce projektowe
- **[API Reference](docs/API.md)** - pełna dokumentacja API
- **[Best Practices](docs/BEST_PRACTICES.md)** - najlepsze praktyki użycia

## 🏗️ Architektura

Biblioteka została zaprojektowana zgodnie z **Clean Architecture** i zasadami **SOLID**:

### Warstwy

- **Domain** - encje domenowe (`MeaningVector`, `SemanticVector`) i interfejsy (`ISemanticSpace`, `ISimilarityCalculator`, etc.)
- **Application** - serwisy aplikacyjne (`SemanticSearchService`, `ResonanceCalculator`, `AntiLoopService`)
- **Infrastructure** - implementacje techniczne (`SemanticSpace`, `CosineSimilarityCalculator`, `InMemoryVectorCache`, etc.)

### Zasady SOLID

- **Single Responsibility** - każda klasa ma jedną odpowiedzialność
- **Open/Closed** - otwarte na rozszerzenia, zamknięte na modyfikacje
- **Liskov Substitution** - implementacje są zamienne
- **Interface Segregation** - małe, specyficzne interfejsy
- **Dependency Inversion** - zależności od abstrakcji

### Główne komponenty

- `ISemanticSpace` - przestrzeń semantyczna (Domain)
- `ISemanticSearchService` - wyszukiwanie semantyczne (Application)
- `ISimilarityCalculator` - obliczanie podobieństwa (Domain)
- `IResonanceCalculator` - obliczanie rezonansu (Application)
- `IAntiLoopService` - wykrywanie powtórzeń (Application)

Zobacz [ARCHITECTURE.md](docs/ARCHITECTURE.md) dla szczegółów.

## 🔧 Rozszerzanie

Możesz łatwo dodać własne implementacje:

```csharp
// Własna metryka podobieństwa
public class EuclideanSimilarityCalculator : ISimilarityCalculator { ... }

// Własny tokenizer
public class CustomTokenizer : ITextTokenizer { ... }

// Rejestracja
services.AddSingleton<ISimilarityCalculator, EuclideanSimilarityCalculator>();
services.AddSingleton<ITextTokenizer, CustomTokenizer>();
```

## ⚡ Wydajność

- **Indeks prefiksowy** - redukuje przestrzeń wyszukiwania z O(n) do O(k)
- **Cache wektorów** - automatyczne cache'owanie obliczonych wektorów
- **Normalizacja** - wektory są automatycznie normalizowane

## 🧪 Testowanie

Biblioteka jest w pełni pokryta testami jednostkowymi:

### Statystyki testów

- **87 testów jednostkowych** - wszystkie przechodzą ✅
- **Pokrycie**: Domain, Application, Infrastructure, Math
- **Framework**: xUnit
- **Mockowanie**: Moq

### Uruchamianie testów

```bash
# Z katalogu głównego
dotnet test

# Z katalogu testów
cd tests/Reflectum.SemanticSpace.Tests
dotnet test
```

### Struktura testów

```
tests/Reflectum.SemanticSpace.Tests/
├── Domain/              # Testy encji domenowych
├── Application/          # Testy serwisów aplikacyjnych
├── Infrastructure/       # Testy implementacji
└── Math/                 # Testy narzędzi matematycznych
```

### Przykład testu

```csharp
var mockSpace = new Mock<ISemanticSpace>();
var mockCalculator = new Mock<ISimilarityCalculator>();
var searchService = new SemanticSearchService(mockSpace.Object, mockCalculator.Object, ...);
```

Wszystkie testy są automatycznie uruchamiane w CI przy każdym push/PR.

## 🤝 Wkład

Zobacz [CONTRIBUTING.md](CONTRIBUTING.md) jak możesz pomóc.

## 📄 Licencja

MIT License - zobacz [LICENSE](LICENSE)

## 🔗 Linki

- [GitHub Repository](https://github.com/Maggio333/Reflectum.SemanticSpace) - kod źródłowy
- [Issues](https://github.com/Maggio333/Reflectum.SemanticSpace/issues) - zgłoś problem lub zaproponuj funkcję
- [Releases](https://github.com/Maggio333/Reflectum.SemanticSpace/releases) - wersje biblioteki

## 👤 Autor

Arkadiusz Słota

---

**Status**: Biblioteka gotowa do użycia. Architektura warstwowa (Domain/Application/Infrastructure), zgodna z SOLID, z pełnym wsparciem DI, 87 testów jednostkowych (100% passing), gotowa do publikacji na NuGet.
