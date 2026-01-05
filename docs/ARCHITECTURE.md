# Architektura Reflectum.SemanticSpace

## Architektura warstwowa (Layered Architecture)

Biblioteka została zorganizowana zgodnie z zasadami Clean Architecture i Domain-Driven Design (DDD), z wyraźnym podziałem na trzy warstwy:

### 1. Domain Layer (Warstwa domeny)

**Lokalizacja**: `Domain/`

**Zawartość**:
- **Entities** (`Domain/Entities/`): Encje domenowe reprezentujące koncepty biznesowe
  - `MeaningVector` - reprezentuje wektor znaczenia słowa
  - `SemanticVector` - reprezentuje wektor semantyczny złożony z wielu komponentów
- **Interfaces** (`Domain/Interfaces/`): Interfejsy definiujące kontrakty domenowe
  - `ISemanticSpace` - przestrzeń semantyczna
  - `ISemanticSearchService` - wyszukiwanie semantyczne
  - `ISimilarityCalculator` - obliczanie podobieństwa
  - `IResonanceCalculator` - obliczanie rezonansu
  - `IIndexBuilder` - budowanie indeksów
  - `IVectorCache` - cache wektorów
  - `ITextTokenizer` - tokenizacja tekstu
  - `ISimilarityMetric` - metryka podobieństwa

**Zasady**:
- ✅ **Brak zależności zewnętrznych** - Domain nie zależy od żadnych innych warstw
- ✅ **Czyste encje** - tylko logika biznesowa, bez szczegółów implementacji
- ✅ **Interfejsy** - definicje kontraktów, nie implementacje

### 2. Application Layer (Warstwa aplikacji)

**Lokalizacja**: `Application/`

**Zawartość**:
- **Services** (`Application/Services/`): Serwisy aplikacyjne implementujące logikę biznesową
  - `SemanticSearchService` - implementuje `ISemanticSearchService`, orkiestruje wyszukiwanie
  - `ResonanceCalculator` - implementuje `IResonanceCalculator`, oblicza rezonans między zbiorami
  - `AntiLoopService` - wykrywa powtórzenia w interakcjach tekstowych

**Zasady**:
- ✅ **Zależy tylko od Domain** - używa interfejsów z Domain
- ✅ **Orkiestracja** - koordynuje operacje między różnymi komponentami
- ✅ **Logika biznesowa** - implementuje przypadki użycia aplikacji

### 3. Infrastructure Layer (Warstwa infrastruktury)

**Lokalizacja**: `Infrastructure/`

**Zawartość**:
- **SemanticSpace** (`Infrastructure/SemanticSpace/`): Implementacja przestrzeni semantycznej
  - `SemanticSpace` - implementuje `ISemanticSpace`
- **Similarity** (`Infrastructure/Similarity/`): Implementacje kalkulatorów podobieństwa
  - `CosineSimilarityCalculator` - implementuje `ISimilarityCalculator`
- **Indexing** (`Infrastructure/Indexing/`): Implementacje indeksów
  - `PrefixIndexBuilder` - implementuje `IIndexBuilder`
- **Caching** (`Infrastructure/Caching/`): Implementacje cache
  - `InMemoryVectorCache` - implementuje `IVectorCache`
- **Tokenization** (`Infrastructure/Tokenization/`): Implementacje tokenizerów
  - `DefaultTextTokenizer` - implementuje `ITextTokenizer`
- **AntiLoop** (`Infrastructure/AntiLoop/`): Implementacje metryk podobieństwa
  - `JaccardSimilarityMetric` - implementuje `ISimilarityMetric`

**Zasady**:
- ✅ **Implementuje interfejsy z Domain** - konkretne realizacje abstrakcji
- ✅ **Szczegóły techniczne** - bazy danych, cache, API, etc.
- ✅ **Możliwość wymiany** - łatwa zamiana implementacji bez zmiany logiki biznesowej

### 4. Math (Wspólne narzędzia)

**Lokalizacja**: `Math/`

**Zawartość**:
- `CosineSimilarity` - statyczne metody matematyczne (bezstanowe, używane przez wszystkie warstwy)

## Zasady SOLID

Biblioteka została zaprojektowana zgodnie z zasadami SOLID:

### Single Responsibility Principle (SRP)

Każda klasa ma jedną odpowiedzialność:

- `SemanticSpace` - zarządzanie wektorami i indeksem
- `SemanticSearchService` - wyszukiwanie semantyczne
- `ResonanceCalculator` - obliczanie rezonansu
- `AntiLoopService` - wykrywanie powtórzeń
- `CosineSimilarityCalculator` - obliczanie podobieństwa

### Open/Closed Principle (OCP)

Klasy są otwarte na rozszerzenia, zamknięte na modyfikacje:

- `ISimilarityCalculator` - można dodać nowe metryki (Euclidean, Manhattan)
- `ISimilarityMetric` - można dodać nowe metryki dla AntiLoop (Levenshtein, etc.)
- `ITextTokenizer` - można dodać własne tokenizery
- `IVectorCache` - można dodać cache Redis, MemoryCache, etc.

### Liskov Substitution Principle (LSP)

Wszystkie implementacje mogą być zamieniane bez zmiany zachowania:

- `ISemanticSpace` → `SemanticSpace`
- `ISimilarityCalculator` → `CosineSimilarityCalculator`
- `IVectorCache` → `InMemoryVectorCache`

### Interface Segregation Principle (ISP)

Interfejsy są małe i specyficzne:

- `ISemanticSpace` - tylko podstawowe operacje
- `ISemanticSearchService` - tylko wyszukiwanie
- `IResonanceCalculator` - tylko rezonans
- `ISimilarityCalculator` - tylko podobieństwo

### Dependency Inversion Principle (DIP)

Zależności są od abstrakcji, nie od konkretnych implementacji:

- `SemanticVector` używa `ISemanticSpace`, nie `SemanticSpace`
- `SemanticSearchService` używa `ISimilarityCalculator`
- `AntiLoopService` używa `ISimilarityMetric`

## Dependency Injection

Biblioteka używa Microsoft.Extensions.DependencyInjection:

```csharp
services.AddSemanticSpace(options =>
{
    options.SemanticSpaceLifetime = ServiceLifetime.Singleton;
});
```

### Rejestrowane serwisy

**Infrastructure (Singleton)**:
- `ISemanticSpace` → `Infrastructure.SemanticSpace.SemanticSpace`
- `ISimilarityCalculator` → `Infrastructure.Similarity.CosineSimilarityCalculator`
- `IIndexBuilder` → `Infrastructure.Indexing.PrefixIndexBuilder`
- `IVectorCache` → `Infrastructure.Caching.InMemoryVectorCache`
- `ITextTokenizer` → `Infrastructure.Tokenization.DefaultTextTokenizer`
- `ISimilarityMetric` → `Infrastructure.AntiLoop.JaccardSimilarityMetric`

**Application (Singleton)**:
- `ISemanticSearchService` → `Application.Services.SemanticSearchService`
- `IResonanceCalculator` → `Application.Services.ResonanceCalculator`
- `IAntiLoopService` → `Application.Services.AntiLoopService` (Scoped)

## Struktura projektu

```
Reflectum.SemanticSpace/
├── Domain/                          # Warstwa domeny
│   ├── Entities/                    # Encje domenowe
│   │   ├── MeaningVector.cs
│   │   └── SemanticVector.cs
│   └── Interfaces/                  # Interfejsy domenowe
│       ├── ISemanticSpace.cs
│       ├── ISemanticSearchService.cs
│       ├── ISimilarityCalculator.cs
│       ├── IResonanceCalculator.cs
│       ├── IIndexBuilder.cs
│       ├── IVectorCache.cs
│       ├── ITextTokenizer.cs
│       └── ISimilarityMetric.cs
├── Application/                     # Warstwa aplikacji
│   └── Services/                    # Serwisy aplikacyjne
│       ├── SemanticSearchService.cs
│       ├── ResonanceCalculator.cs
│       └── AntiLoopService.cs
├── Infrastructure/                  # Warstwa infrastruktury
│   ├── SemanticSpace/               # Implementacja przestrzeni semantycznej
│   │   └── SemanticSpace.cs
│   ├── Similarity/                  # Implementacje podobieństwa
│   │   └── CosineSimilarityCalculator.cs
│   ├── Indexing/                    # Implementacje indeksów
│   │   └── PrefixIndexBuilder.cs
│   ├── Caching/                     # Implementacje cache
│   │   └── InMemoryVectorCache.cs
│   ├── Tokenization/                # Implementacje tokenizerów
│   │   └── DefaultTextTokenizer.cs
│   └── AntiLoop/                    # Implementacje metryk
│       └── JaccardSimilarityMetric.cs
├── Math/                            # Wspólne narzędzia matematyczne
│   └── CosineSimilarity.cs
├── DependencyInjection/             # Konfiguracja DI
│   └── ServiceCollectionExtensions.cs
└── Examples/                        # Przykłady użycia
    └── BasicUsage.cs
```

## Wzorce projektowe

### Strategy Pattern

- `ISimilarityCalculator` - różne strategie obliczania podobieństwa
- `ISimilarityMetric` - różne metryki dla AntiLoop
- `ITextTokenizer` - różne strategie tokenizacji

### Factory Pattern

- `SemanticVector.FromInput()` - factory method

### Cache Pattern

- `IVectorCache` - abstrakcja cache
- `InMemoryVectorCache` - implementacja w pamięci

### Builder Pattern

- `IIndexBuilder` - budowanie indeksów

## Testowanie

Biblioteka jest zaprojektowana z myślą o testowaniu:

- Wszystkie zależności są wstrzykiwane
- Interfejsy umożliwiają mockowanie
- Brak zależności od zewnętrznych serwisów
- Warstwy są rozdzielone - łatwe testowanie jednostkowe

### Przykład testu

```csharp
// Arrange
var mockSpace = new Mock<ISemanticSpace>();
var mockCalculator = new Mock<ISimilarityCalculator>();
var mockIndexBuilder = new Mock<IIndexBuilder>();
var searchService = new SemanticSearchService(
    mockSpace.Object, 
    mockCalculator.Object, 
    mockIndexBuilder.Object
);

// Act
var result = searchService.TopK("test", k: 5);

// Assert
Assert.NotNull(result);
```

## Zalety architektury warstwowej

1. **Separacja odpowiedzialności** - każda warstwa ma jasno określone zadanie
2. **Testowalność** - łatwe mockowanie i testowanie jednostkowe
3. **Elastyczność** - łatwa zamiana implementacji bez zmiany logiki biznesowej
4. **Czytelność** - jasna struktura ułatwia zrozumienie kodu
5. **Skalowalność** - łatwe dodawanie nowych funkcji bez naruszania istniejącego kodu
6. **Niezależność domeny** - logika biznesowa nie zależy od szczegółów implementacji
