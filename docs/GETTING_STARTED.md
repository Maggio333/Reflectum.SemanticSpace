# Getting Started with Reflectum.SemanticSpace

## Wprowadzenie

Reflectum.SemanticSpace to biblioteka .NET do pracy z przestrzenią semantyczną i wykrywania powtórzeń w interakcjach tekstowych. Została wyciągnięta z projektu ReflectumEngine i zawiera najbardziej wartościowe komponenty bez danych behawioralnych.

## Po co to komu?

### Przypadki użycia

1. **Chatboty i systemy dialogowe**
   - Wykrywanie powtórzeń w rozmowach
   - Unikanie zapętlenia odpowiedzi
   - Personalizacja stylu odpowiedzi

2. **Systemy rekomendacyjne**
   - Rekomendacje oparte na treści
   - Porównywanie podobieństwa dokumentów
   - Ekspansja zapytań

3. **Wyszukiwarki semantyczne**
   - Wyszukiwanie podobnych słów
   - Projekcja semantyczna
   - Analiza podobieństwa tekstów

4. **Analiza tekstu**
   - Porównywanie dokumentów
   - Klasteryzacja tekstów
   - Ekstrakcja podobnych konceptów

## Instalacja

### NuGet (gdy opublikowane)

```bash
dotnet add package Reflectum.SemanticSpace
```

### Z kodu źródłowego

```bash
git clone https://github.com/Maggio333/Reflectum.SemanticSpace.git
cd Reflectum.SemanticSpace
dotnet build
```

## Szybki start

### 1. Podstawowa konfiguracja z DI

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

if (semanticSpace is SemanticSpace space)
{
    space.Load(vectors);
    space.BuildPrefixIndex(); // Dla szybkiego wyszukiwania
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
var detector = serviceProvider.GetRequiredService<IAntiLoopService>();

if (detector.IsEcho("Cześć, jak się masz?", threshold: 0.85f))
{
    Console.WriteLine("Wykryto powtórzenie!");
}
```

## Przykłady użycia

Zobacz [Examples/BasicUsage.cs](../Examples/BasicUsage.cs) dla pełnych przykładów.

## Testowanie

Projekt zawiera 87 testów jednostkowych. Aby je uruchomić:

```bash
dotnet test
```

Wszystkie testy powinny przechodzić. Zobacz [CONTRIBUTING.md](../CONTRIBUTING.md) dla szczegółów o testowaniu.

## Następne kroki

- [Architektura i SOLID](ARCHITECTURE.md)
- [API Reference](API.md)
- [Best Practices](BEST_PRACTICES.md)

