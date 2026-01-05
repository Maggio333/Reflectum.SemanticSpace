# CI/CD - Po co to komu?

## Co to jest CI/CD?

**CI (Continuous Integration)** - ciągła integracja:
- Automatyczne testy przy każdym push/PR
- Weryfikacja kompilacji
- Sprawdzanie jakości kodu

**CD (Continuous Deployment)** - ciągłe wdrażanie:
- Automatyczne publikowanie pakietów NuGet
- Automatyczne tworzenie release'ów

## Po co CI w tym projekcie?

### 1. **Automatyczna weryfikacja**

Gdy ktoś wyśle Pull Request:
- ✅ Sprawdzamy czy kod się kompiluje
- ✅ Sprawdzamy czy testy przechodzą
- ✅ Sprawdzamy czy nie ma błędów

**Bez CI**: Musisz ręcznie sprawdzać każdy PR
**Z CI**: GitHub automatycznie sprawdza wszystko

### 2. **Weryfikacja na różnych platformach**

CI testuje kod na:
- ✅ Windows
- ✅ Linux
- ✅ macOS

**Dlaczego ważne**: Upewniamy się że biblioteka działa wszędzie

### 3. **Automatyczne tworzenie pakietów NuGet**

Gdy pushujesz do `main`:
- ✅ Automatycznie tworzy pakiet NuGet
- ✅ Gotowy do publikacji

**Bez CI**: Musisz ręcznie tworzyć pakiety
**Z CI**: Automatycznie przy każdym release

### 4. **Jakość kodu**

CI sprawdza:
- ✅ Czy kod się kompiluje bez błędów
- ✅ Czy nie ma ostrzeżeń
- ✅ Czy dokumentacja XML jest kompletna

## Jak to działa?

### Workflow: `ci.yml`

1. **Trigger** - uruchamia się przy:
   - Push do `main` lub `develop`
   - Pull Request do `main` lub `develop`

2. **Build Job** - kompiluje kod na:
   - Ubuntu, Windows, macOS
   - .NET 8.0

3. **Test Job** - uruchamia testy (gdy będą)

4. **Package Job** - tworzy pakiet NuGet (tylko na `main`)

5. **Quality Checks** - sprawdza jakość kodu

## Status badge

W README widzisz:
```
[![CI](https://github.com/.../badge.svg)](...)
```

To pokazuje status ostatniego builda:
- ✅ Zielony = wszystko działa
- ❌ Czerwony = coś nie działa

## Jak używać?

### Dla kontrybutorów

1. Stwórz Pull Request
2. CI automatycznie sprawdzi kod
3. Zobacz status w PR (✅ lub ❌)
4. Jeśli ✅ - możesz merge'ować

### Dla maintainerów

1. Push do `main`
2. CI automatycznie tworzy pakiet NuGet
3. Pobierz pakiet z artifacts
4. Opublikuj na NuGet.org

## Konfiguracja

Plik `.github/workflows/ci.yml` zawiera:
- Konfigurację buildów
- Matrycę platform (.NET wersje, OS)
- Kroki CI/CD

Możesz modyfikować według potrzeb.

## Podsumowanie

**CI to automatyczny asystent** który:
- ✅ Sprawdza czy kod działa
- ✅ Testuje na różnych platformach
- ✅ Tworzy pakiety
- ✅ Zapewnia jakość

**Bez CI**: Wszystko ręcznie, łatwo o błędy
**Z CI**: Automatycznie, mniej błędów, więcej czasu na kod

