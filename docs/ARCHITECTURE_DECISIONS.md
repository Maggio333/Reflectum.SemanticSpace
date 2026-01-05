# Decyzje architektoniczne

## SemanticVector jako Value Object

`SemanticVector` jest traktowany jako **Value Object** w DDD, mimo że ma zależności w konstruktorze.

### Uzasadnienie

1. **Brak tożsamości** - SemanticVector nie ma ID, jest identyfikowany przez swoje komponenty
2. **Niezmienność** - po utworzeniu, komponenty nie powinny się zmieniać
3. **Logika biznesowa** - Value Objects w DDD mogą zawierać logikę biznesową (np. Money, Address)

### Alternatywne podejście

Gdyby chcieć być bardziej "czystym" w Clean Architecture, można by:
- Przenieść logikę `ToMeanVector` i `SemanticSimilarity` do serwisu domenowego
- Zostawić `SemanticVector` jako prosty Value Object z samymi danymi

Obecne podejście jest akceptowalne, ponieważ:
- Zależności są od interfejsów domenowych (nie od Infrastructure)
- Logika jest związana z samym obiektem (enkapsulacja)
- Ułatwia użycie (nie trzeba przekazywać serwisu wszędzie)

## IAntiLoopService - interfejs w Domain

`AntiLoopService` ma teraz interfejs `IAntiLoopService` w Domain, zgodnie z Dependency Inversion Principle.

### Uzasadnienie

- **DIP** - zależności powinny być od abstrakcji, nie od konkretnych implementacji
- **Testowalność** - łatwe mockowanie w testach
- **Elastyczność** - możliwość zamiany implementacji bez zmiany kodu klienta

