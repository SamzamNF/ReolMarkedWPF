### fix: Reparer repository-logik og databinding

Denne commit retter en række kritiske fejl, der forhindrede applikationen i at hente, vise og opdatere data korrekt.

- **Repository:** Genaktiveret udkommenteret kode i `SqlShelfVendorRepository` og `SqlShelfRepository` for at genetablere grundlæggende databasefunktionalitet (læs/skriv).
- **Dependency Injection:** Registreret den manglende `IProductRepository` i `DIContainer` for at forhindre crash.
- **Databinding:** Tilføjet `IsOccupied`, `X`, og `Y` properties til `Shelf`-modellen for at muliggøre korrekte bindings i reolkort-viewet.
- **Kodekvalitet:** Opdateret access modifiers fra `internal` til `public` på nødvendige modeller og interfaces for at sikre konsistens og tilgængelighed.
