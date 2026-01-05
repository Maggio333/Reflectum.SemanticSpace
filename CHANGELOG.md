# Changelog

Wszystkie znaczące zmiany w tym projekcie będą dokumentowane w tym pliku.

Format oparty na [Keep a Changelog](https://keepachangelog.com/pl/1.0.0/),
a projekt adheres to [Semantic Versioning](https://semver.org/lang/pl/).

## [1.0.0] - 2025-01-05

### Added
- **SOLID Architecture** - pełna refaktoryzacja zgodnie z zasadami SOLID
- **Architektura warstwowa** - Domain/Application/Infrastructure (Clean Architecture)
- **Dependency Injection** - wsparcie dla Microsoft.Extensions.DependencyInjection
- **Interfejsy** - wszystkie główne komponenty mają interfejsy
- **SemanticSpace** - przestrzeń semantyczna z indeksem prefiksowym
- **SemanticVector** - wektory semantyczne z cache
- **AntiLoopService** - wykrywanie powtórzeń w tekstach
- **CosineSimilarity** - metryka podobieństwa cosinusowego
- **ResonanceCalculator** - obliczanie rezonansu między zbiorami
- **Testy jednostkowe** - 87 testów pokrywających wszystkie komponenty (xUnit, Moq)
- **Dokumentacja** - kompletna dokumentacja XML i markdown
- **CI/CD** - automatyczne testy i tworzenie pakietów (GitHub Actions)
- **Przykłady** - przykłady użycia z DI

### Changed
- Refaktoryzacja z oryginalnego ReflectumEngine
- Usunięcie zależności od danych behawioralnych
- Uproszczenie API

### Removed
- Zależności od danych behawioralnych (formuły, nasiona)
- Zależności od konkretnych implementacji

## [Unreleased]

### Planned
- Więcej implementacji metryk podobieństwa (Euclidean, Manhattan)
- Wsparcie dla innych formatów wektorów (Word2Vec, GloVe)
- Optymalizacje wydajnościowe
- Code coverage reports
- Benchmarki wydajnościowe

