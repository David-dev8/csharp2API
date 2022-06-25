# Quiz royale 
**Configuratie handleiding.**

In dit document wordt uitgelegd welke stappen moeten worden uitgevoerd om Quiz royale te kunnen spelen. Deze README geld voor beide de app en de API.

## API
Ten eerste moet de API worden opgezet. Hiervoor heeft u de volgende dingen nodig:
- Microsoft Visual studio 2022 met ASP NET Core
- MySQL

Download en open eerst de solution in de QuizRoyaleAPI repository in Microsoft Visual Studio 2022. In deze solution bevinden zich 2 projecten: QuizRoyaleAPI en Testtool. Kijk eerst of de volgende NuGet packages aanwezig zijn:
- QuizRoyaleAPi:
  - Microsoft.AspNetCore.Authentication.JwtBearer
  - Microsoft.EntityFrameworkCore
  - Microsoft.EntityFrameworkCore.Proxies
  - Microsoft.EntityFrameworkCore.Tools
  - Pomelo.EntityFrameworkCore.MySql
  - Swashbuckle.AspNetCore
 - Testtool:
   - Microsoft.AspNetCore.SignalR.Client

Zodra u zeker weet dat deze packages aanwezig zijn kunt u de database instellen. Maak in MySQL een nieuwe database aan genaamd `quizroyale`, Importeer daarna het SQL bestand dat bijgevoegd is in de repository in deze database. Zodra dit is gedaan dan is het opzetten van de API klaar. Om de API te draaien kunt u deze starten door de grote groene knop bovenin bij Microsoft Visual Studio 2022, of door te drukken op het kleinere groene pijltje naast deze knop om te starten zonder debuggen. Let goed op dat u wel QuizRoyaleAPI heeft geselecteerd en niet de Testtool.
**Belangrijk:** De API moet worden gehost op localhost via poort 7264, en de hub moet worden gehost op localhost via  poort 5264. Dit zou automatisch moeten gebeuren, maar check voor de zekerheid altijd dat dit ook klopt.

### De Testtool
De testtool is een gereedschap om de API en app te testen. Omdat QuizRoyale een multiplayer spel is kan dit natuurlijk moeilijk worden getest door maar 1 persoon. Daarom is de Testtool ontwikkeld. De Testtool biedt je de mogelijk om een spel te bevolken met robotspelers. Om dit te doen moet u eerst de Testtool opstarten. Let op dat u dit doet zonder debuggen, anders kunt u de API niet meer aanzetten. Nadat de Testtool is geopend kunt u de API starten. Vanuit hier kunt u de knoppen in de testtool gebruiken.
**Let op:** Als u genoeg robotspelers aan een match toevoegt om de minimum spelers te overschrijden, dan kan het zijn dat een spel al begint voordat je zelf deel kan nemen. De API kan namelijk geen onderscheid maken tussen echte en robot spelers.
**Disclaimer:** De testtool is zo geprogrammeerd dat er maar 10 bots per seconden joinen, dus als u 500 bots toevoegt dan zal dat 50 seconden duren. Dit is dus bewust toegevoegd aan de Testtool om het joinen er beter uit te laten zien in de app, en ligt dus niet aan de snelheid van de API. Als u dit niet wilt kunt u de `Thread.Sleep(100);` weghalen op regel 65 van MainWindow.xaml.cs in de Testtool.
**Deze testtool dient uitsluitend om de API en applicatie mee te testen als je geen medespelers hebt  tijdens het testen of ontwikkelen en moet niet worden gezien als een deel van het project of QuizRoyale.**


## Applicatie

Daarna moet de applicatie worden opgezet. Hiervoor heeft u de volgende dingen nodig:
- Microsoft Visual studio 2022 met WPF
- De QuizRoyaleAPI

Download en open eerst de solution in de QuizRoyaleAplicatie repository in Microsoft Visual Studio 2022. In deze solution bevindt zich 1 project: Quiz Royale. Kijk eerst of de volgende NuGet packages aanwezig zijn:
- Quiz Royale:
  - Castle.Windsor
  - FontAwesome.WPF
  - Microsoft.AspNetCore.SignalR.Client
  - Microsoft.Toolkit
  - SharpVectors

Zodra u zeker weet dat deze packages aanwezig zijn kunt u de API aanzetten. Als de API aan is, kunt u de applicatie starten door te drukken op de grote groene knop bovenin bij Microsoft Visual Studio 2022, of door te drukken op het kleinere groene pijltje naast deze knop om te starten zonder debuggen. Vanuit hier kunt u een spel joinen en de applicatie gebruiken. Maak gebruik van de Testtool om het spel te testen zoals beschreven staat bij de API. 

**Let op:** Lokaal kan het er maar een spel tegelijk plaatsvinden. Als u niet kan joinen kan het dus zijn dat er nog een spel actief is in de hub. Dit kan worden veroorzaakt door bots die nog doorspelen nadat u heeft verloren, of als u direct na het verlaten opnieuw probeert te joinen voordat de game is gedisposed/geëindigd in de hub.
