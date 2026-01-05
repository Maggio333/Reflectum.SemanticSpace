# API Reference

## Namespace: Reflectum.SemanticSpace.Core

### ISemanticSpace

Interfejs reprezentujący przestrzeń semantyczną.

```csharp
public interface ISemanticSpace
{
    MeaningVector? Find(string word);
    IEnumerable<MeaningVector> GetAll();
}
```

**Metody:**
- `Find(string word)` - Wyszukuje wektor dla danego słowa
- `GetAll()` - Zwraca wszystkie wektory

### ISemanticSearchService

Interfejs dla usługi wyszukiwania semantycznego.

```csharp
public interface ISemanticSearchService
{
    List<MeaningVector> TopK(string word, int k = 5, double threshold = 0.65);
    MeaningVector? FindClosest(string word);
    List<MeaningVector> Project(string word, int topK = 3, double threshold = 0.75);
}
```

**Metody:**
- `TopK(string word, int k, double threshold)` - Znajduje top K podobnych słów
- `FindClosest(string word)` - Znajduje najbliższy wektor
- `Project(string word, int topK, double threshold)` - Projekcja semantyczna

### SemanticVector

Reprezentuje wektor semantyczny złożony z wielu komponentów tekstowych.

```csharp
public class SemanticVector
{
    public List<string> Components { get; set; }
    
    public SemanticVector(IEnumerable<string>? components, ...);
    public static SemanticVector FromInput(string input, ...);
    public double[] ToMeanVector(ISemanticSpace space);
    public double SemanticSimilarity(SemanticVector other, ISemanticSpace space);
}
```

**Użycie:**
```csharp
var vector = SemanticVector.FromInput("miłość i przyjaźń");
double similarity = vector1.SemanticSimilarity(vector2, semanticSpace);
```

## Namespace: Reflectum.SemanticSpace.AntiLoop

### AntiLoopDetector

Wykrywa powtórzenia (echo) w interakcjach tekstowych.

```csharp
public class AntiLoopDetector
{
    public AntiLoopDetector(float decayFactor = 0.95f, ISimilarityMetric? similarityMetric = null);
    public bool IsEcho(string input, float threshold = 0.85f);
    public string GetEchoReport();
    public void Clear();
}
```

**Użycie:**
```csharp
var detector = new AntiLoopDetector(decayFactor: 0.95f);
if (detector.IsEcho("Cześć, jak się masz?", threshold: 0.85f))
{
    // Wykryto powtórzenie
}
```

## Namespace: Reflectum.SemanticSpace.DependencyInjection

### ServiceCollectionExtensions

Rozszerzenia dla rejestracji serwisów.

```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSemanticSpace(
        this IServiceCollection services,
        Action<SemanticSpaceOptions>? configure = null);
}
```

**Użycie:**
```csharp
services.AddSemanticSpace(options =>
{
    options.SemanticSpaceLifetime = ServiceLifetime.Singleton;
});
```

## Komentarze XML w kodzie

Wszystkie publiczne typy i metody mają komentarze XML w kodzie źródłowym. Użyj IntelliSense w IDE (Visual Studio, Rider, VS Code) aby zobaczyć pełną dokumentację podczas kodowania. Komentarze XML są automatycznie generowane do plików .xml podczas builda (gdy `<GenerateDocumentationFile>true</GenerateDocumentationFile>` jest włączone).

