# Contributing to Reflectum.SemanticSpace

Dziękujemy za zainteresowanie projektem! Oto jak możesz pomóc.

## Jak możesz pomóc

1. **Raportowanie błędów** - Jeśli znajdziesz błąd, stwórz issue
2. **Proponowanie funkcji** - Masz pomysł? Stwórz issue z propozycją
3. **Pull Requests** - Naprawki i nowe funkcje są mile widziane
4. **Dokumentacja** - Poprawki i rozszerzenia dokumentacji

## Proces kontrybucji

1. Fork projektu
2. Stwórz branch dla swojej zmiany (`git checkout -b feature/amazing-feature`)
3. Commit zmian (`git commit -m 'Add amazing feature'`)
4. Push do brancha (`git push origin feature/amazing-feature`)
5. Otwórz Pull Request

## Zasady kodu

- Zgodność z SOLID
- Komentarze XML w kodzie dla publicznych API
- Testy jednostkowe dla nowych funkcji
- Formatowanie zgodne z .NET conventions

## Testowanie

Projekt zawiera **87 testów jednostkowych** pokrywających wszystkie komponenty.

### Uruchamianie testów

```bash
# Z katalogu głównego
dotnet test

# Z katalogu testów
cd tests/Reflectum.SemanticSpace.Tests
dotnet test
```

### Wymagania przed PR

Przed wysłaniem PR upewnij się że:
- ✅ Wszystkie testy przechodzą (`dotnet test`)
- ✅ Kod się kompiluje bez błędów
- ✅ Nie ma ostrzeżeń kompilatora
- ✅ Komentarze XML w kodzie są kompletne dla nowych publicznych API
- ✅ Nowe funkcje mają odpowiednie testy jednostkowe
- ✅ Testy używają Moq do mockowania zależności

### Struktura testów

Testy są zorganizowane zgodnie z architekturą warstwową:
- `Domain/` - testy encji i interfejsów domenowych
- `Application/` - testy serwisów aplikacyjnych
- `Infrastructure/` - testy implementacji technicznych
- `Math/` - testy narzędzi matematycznych

### Framework testowy

- **xUnit** - framework testowy
- **Moq** - mockowanie zależności
- **Microsoft.NET.Test.Sdk** - SDK testowe

## Pytania?

Stwórz issue z pytaniem - chętnie pomogę!

