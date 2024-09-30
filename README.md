Action App
Beskrivelse:

Dette er en Action-app udviklet med det formål at håndtere auktioner for køretøjer, hvor klassificering af køretøjer som personbiler, busser, tunge køretøjer og normale køretøjer er en integreret del af systemet. Applikationen benytter SQL Server Stored Procedures (ST's) til at gemme oplysninger og administrere brugere samt køretøjer i auktionen. Brugere kan oprette og opdatere køretøjer, og informationen bliver sikkert gemt i en SQL Server-database.

Auktionen understøtter flere typer af køretøjer, herunder:

Personbiler
Busser
Normale køretøjer
Tunge køretøjer
Der er opsat regler for klassificeringen af disse køretøjer for at sikre, at de passer ind i deres respektive kategorier, hvilket hjælper med at organisere og strukturere auktionerne korrekt.

Funktioner:
Oprettelse af nye brugere og køretøjer gennem Stored Procedures
Opdatering af bruger- og køretøjsoplysninger
Klassifikation af køretøjer (personbiler, busser, tunge køretøjer mv.)
Sikker lagring af bruger- og køretøjsinformation i SQL Server
Effektiv håndtering af data ved hjælp af SQL Server ST's
Krav:
SQL Server: Applikationen kræver en SQL Server-database, der er opsat og konfigureret til at køre Stored Procedures, som bruges til at håndtere bruger- og køretøjsoplysninger.
.NET Framework / Node.js (afhængigt af den valgte teknologi): Applikationen er bygget på denne platform og kræver installation af de nødvendige afhængigheder.
Tilgængelighed af Stored Procedures: ST's skal være korrekt oprettet i SQL Server til at oprette, opdatere og hente bruger- og køretøjsdata.
Installation:
Klon projektet:

bash
Kopier kode
git clone <repository-url>
cd ActionApp
Konfiguration:
Opdater databaseforbindelsesstrengen i projektets konfigurationsfil for at forbinde til din SQL Server. Sørg for at have de nødvendige tilladelser og databaseoplysninger klar.

Opsætning af Stored Procedures:
Importer eller opret de nødvendige Stored Procedures i din SQL Server-database. Eksempler på nødvendige ST's, som kan findes i SQL query-filen:

CreateUser
UpdateUser
GetUser
CreateVehicle (til oprettelse af køretøjer)
UpdateVehicle (til opdatering af køretøjsoplysninger)
GetVehicle (til visning af køretøjer)
Kør applikationen:

bash
Kopier kode
dotnet run / npm start
Afhængigt af hvilken teknologi der er brugt, kør den relevante kommando for at starte applikationen.

Brug:
Når applikationen kører, kan følgende funktioner anvendes:

Opret ny bruger: Brug formularen i brugerfladen eller API'en til at indsende nye brugeroplysninger, som automatisk bliver gemt i SQL Server gennem en Stored Procedure.
Opret nyt køretøj: Brug formularen til at indsende oplysninger om nye køretøjer, der vil blive klassificeret efter de fastsatte regler og gemt i databasen.
Opdater oplysninger: Oplysninger om eksisterende brugere og køretøjer kan opdateres via brugerfladen eller API'en.
Vis data: Hent og vis bruger- og køretøjsdata fra SQL Server-databasen ved hjælp af de implementerede Stored Procedures.
Teknologi Stak:
Backend: C# / Node.js
Database: SQL Server med Stored Procedures
Frontend: Avalonia (for UI-baserede interaktioner)
Licens:
Dette projekt er licenseret under MIT-licensen. Se LICENSE-filen for flere detaljer.
