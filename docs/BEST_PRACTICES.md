# Best Practices

## Użycie Dependency Injection

Zawsze używaj DI zamiast bezpośredniego tworzenia instancji:

✅ **Dobrze:**
```csharp
services.AddSemanticSpace();
var space = serviceProvider.GetRequiredService<ISemanticSpace>();
```

❌ **Źle:**
```csharp
var space = new SemanticSpace();
```

## Cache wektorów

`SemanticVector` automatycznie cache'uje obliczone wektory. Dla większych aplikacji rozważ własną implementację `IVectorCache`:

```csharp
services.AddSingleton<IVectorCache, RedisVectorCache>(); // Przykład
```

## Indeks prefiksowy

Zawsze buduj indeks prefiksowy po załadowaniu wektorów:

```csharp
space.Load(vectors);
space.BuildPrefixIndex(); // Ważne dla wydajności!
```

## Threshold values

Dostosuj progi do swoich potrzeb:

- **TopK threshold: 0.65** - dla ogólnego użycia
- **TopK threshold: 0.75** - dla bardziej precyzyjnych wyników
- **AntiLoop threshold: 0.85** - dla wykrywania powtórzeń
- **Resonance threshold: 0.75** - dla obliczania rezonansu

## Lifetime serwisów

- `ISemanticSpace` - **Singleton** (duże dane, kosztowne ładowanie)
- `AntiLoopDetector` - **Scoped** (historia per sesja)
- Pozostałe - **Singleton** (stateless)

## Testowanie

Używaj mocków dla interfejsów:

```csharp
var mockSpace = new Mock<ISemanticSpace>();
var mockCalculator = new Mock<ISimilarityCalculator>();
```

## Wydajność

- Używaj indeksu prefiksowego dla dużych zbiorów (>1000 wektorów)
- Cache wektorów znacząco przyspiesza obliczenia
- Rozważ równoległe przetwarzanie dla bardzo dużych zbiorów

## Rozszerzanie

Dodawaj własne implementacje przez interfejsy:

```csharp
// Własna metryka podobieństwa
public class EuclideanSimilarityCalculator : ISimilarityCalculator { ... }

// Własny tokenizer
public class CustomTokenizer : ITextTokenizer { ... }

// Rejestracja
services.AddSingleton<ISimilarityCalculator, EuclideanSimilarityCalculator>();
```

