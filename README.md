# Middelby ReolMarked System

Middelby ReolMarked System er en desktop-applikation bygget med WPF (Windows Presentation Foundation), designet til at administrere driften af et reolmarked. Applikationen giver medarbejdere mulighed for at håndtere reollejere, reoludlejning, produktsalg, transaktioner og regnskab.

## Funktioner

* **Lejeradministration**: Opret, rediger og slet reollejere samt deres betalingsoplysninger.
* **Reoladministration**: Administrer butikkens reoler, herunder type og pris.
* **Lejeaftaler**: Opret og administrer lejeaftaler for reoler. Indeholder en visuel plantegning, der giver et overblik over ledige og optagede reoler.
* **Produktstyring**: Tilføj, rediger og slet produkter, der er tilknyttet specifikke lejere og deres lejede reoler.
* **Salg og Transaktioner**: Gennemfør salg af produkter via en simpel indkøbskurv-funktion. Se en oversigt over alle tidligere transaktioner og de varer, de indeholdt.
* **Regnskab**: Generer regnskabsrapporter for specifikke perioder, som beregner lejeindtægter, salg, kommission og udbetalinger til lejere. Det er også muligt at se salgsoversigter pr. lejer.

## Teknologier

* **Framework**: .NET
* **UI**: Windows Presentation Foundation (WPF)
* **Sprog**: C#
* **Database**: Microsoft SQL Server
* **Arkitektur**:
    * Model-View-ViewModel (MVVM)
    * Repository Pattern
    * Dependency Injection

## Arkitektur

Applikationen er struktureret efter anerkendte designmønstre for at sikre en vedligeholdelsesvenlig og skalerbar kodebase.

* **MVVM (Model-View-ViewModel)**: UI (`Views`) er adskilt fra applikationens logik (`ViewModels`) og data (`Models`), hvilket skaber en klar ansvarsfordeling og gør koden lettere at teste.
* **Repository Pattern**: Al databasekommunikation er abstraheret væk i `Repositories`, som implementerer interfaces (f.eks. `IProductRepository`). Dette gør det muligt at udskifte datakilden uden at påvirke resten af applikationen.
* **Dependency Injection**: Projektet bruger `Microsoft.Extensions.DependencyInjection` til at håndtere afhængigheder mellem ViewModels, Repositories og Views. Dette fremmer løs kobling og gør det nemt at administrere instanser af de forskellige komponenter.

## Installation og opsætning

Følg disse trin for at køre projektet lokalt.

1.  **Klon repository'et**
    ```bash
    git clone <repository-url>
    ```

2.  **Databaseopsætning**
    * Projektet kræver en Microsoft SQL Server-database.
    * Opret en ny, tom database på din SQL Server-instans.
    * (Antaget) Kør det medfølgende database-script for at oprette de nødvendige tabeller og stored procedures.

3.  **Konfigurer Connection String**
    * Åbn filen `ReolMarkedWPF/appsettings.json`.
    * Opdater `ConnectionString` under `ConnectionStrings`, så den peger korrekt på din database.

4.  **Byg og kør projektet**
    * Åbn `.sln`-filen i Visual Studio.
    * Byg løsningen (Build Solution).
    * Kør projektet (Tryk F5).

## Anvendelse

Efter opstart af applikationen præsenteres hovedvinduet med en navigationsmenu i venstre side. Herfra kan du tilgå de forskellige moduler:

* **Lejere**: Administrer information om personer, der lejer reoler.
* **Reoler**: Se og rediger de fysiske reoler i butikken.
* **Lejeaftaler**: Opret nye aftaler via den visuelle plantegning eller administrer eksisterende aftaler.
* **Salg**: Behandl kundekøb.
* **Regnskab**: Få økonomisk overblik over salg og udlejning.
* **Produkter**: Administrer varer tilknyttet en specifik lejer.
