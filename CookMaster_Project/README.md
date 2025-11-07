<h1 style="color: purple;">CookMaster Project 🍳</h1>
CookMaster is a WPF recipe management application. Features include:<br>
- 📝 Add, edit, and delete recipes<br>
- 🔒 Supports login and user management<br>
- 🍰 Displays a list of recipes by category, such as desserts, main courses, and appetizers<br>

<h2 style="color: blue;">How to use: 🛠️</h2>
1. ➕ Click the "Add Recipe" button to add a new recipe.<br>
2. 👀 Select a recipe to view details or edit it.<br>
3. 🔍 Use the search feature to find the recipe you're looking for.<br>

<h2 style="color: orange;">Technologies used 💻</h2>
- .NET 8<br>
- WPF (Windows Presentation Foundation)<br>
- C#<br>

<h2 style="color: pink;">Future Plans 🚀</h2>
- 🗄️ Integrating a database into my project<br>
- 🎨 UX/UI improvements<br>

<h3 style="color: red;">Learning Outcomes 📚</h3>
This project gave me a much clearer grasp of:<br>
- 💻 C#, OOP, and MVVM<br>
- 🔄 Data structures and data flow<br>
- 🛠️ Debugging and problem-solving<br>

---

<h2 style="color: green;">Projektöversikt</h2>
Projektöversikt<br>
Detta projekt är en WPF-applikation som utvecklats enligt MVVM-arkitekturen (Model-View-ViewModel) för att skapa en tydlig separation mellan användargränssnitt, programlogik och datamodeller. Syftet är att bygga en välstrukturerad, underhållsvänlig och skalbar applikation.<br>
<br>
➢ Systemets huvuddelar<br>
● Views – Fönster för inloggning, registrering, tvåfaktorsautentisering (2FA), recepthantering och användarprofil.<br>
● ViewModels – Hanterar UI-tillstånd, kommandon och databindning.<br>
● Managers / Services – Ansvarar för systemlogik såsom autentisering, hantering av recept och skapande av standarddata.<br>
● Models – Innehåller huvudklasser som <code>User</code>, <code>AdminUser</code> och <code>Recipe</code>.<br>
<br>
Systemet initieras med <code>DefaultUserSeed</code>. Kommandon implementeras via <code>RelayCommand</code> enligt Command Pattern och uppdateras med <code>CommandManager.InvalidateRequerySuggested()</code>. Lösenord hanteras genom händelsen <code>PasswordChanged</code> istället för direkt databindning, på grund av begränsningar i WPF:s <code>PasswordBox</code>.<br>
<br>
➢ Styrkor<br>
● Tydlig och logisk struktur mellan UI, logik och data.<br>
● Korrekt tillämpning av MVVM-principer, utan att affärslogik hamnar i ViewModels.<br>
● Konsekvent användning av <code>INotifyPropertyChanged</code> och <code>RelayCommand</code> som ger responsivt gränssnitt.<br>
● Centraliserad hantering av autentisering och användarstatus, vilket förenklar flödet i applikationen.<br>
<br>
➢ Förbättringsplan<br>
● Införa en DialogService för att hantera meddelanden utan direkt beroende av UI.<br>
● Rensa upp i Models och korrigera duplicerade eller felaktiga egenskaper.<br>
● Implementera Dependency Injection (DI) för bättre testbarhet och flexibilitet.<br>
● Dela upp UserManager, som idag har för många uppgifter (inloggning, profil, återställning).<br>
● Förenkla ViewModels genom att införa Interfaces, Services och ett tydligare Infrastructure-lager.<br>
● Skapa en Navigation Service för mer strukturerad navigering.<br>
● Införa ett riktigt datalager med beständighet.<br>
● Utveckla enhetstester (Unit Tests) för autentisering, 2FA, recept och validering.<br>
<br>
➢ Fördelar och nackdelar med olika tekniska approacher<br>
Tjänsteupplösning<br>
● Application.Resources (Service Locator) – Enkel setup men svår testat och svag livscykel kontroll.<br>
● DI-container (Microsoft.Extensions.DependencyInjection) – Mer testbarhet och flexibilitet, men kräver mer konfiguration.<br>
<br>
Lösenordshantering<br>
● PasswordChanged-event + string – Enkelt men mindre säkert.<br>
● Attached Property + SecureString – Säkrare och mer MVVM-korrekt men kräver mer kod.<br>
<br>
Dialoger<br>
● MessageBox i ViewModel – Snabb lösning men hårt kopplad till UI.<br>
● IDialogService-abstraktion – Testbar och modulär men kräver extra implementation.<br>
<br>
Navigering<br>
● Flera fönster – Enkel modell men fragmenterad hantering.<br>
● Shell + NavigationService – Centraliserad och flexibel men mer komplex.<br>
<br>
Validering<br>
● ErrorMessage i ViewModel – Enkel men ger ingen fältvis feedback.<br>
● INotifyDataErrorInfo / ValidationRules – Ger bättre UX men kräver mer struktur.<br>
<br>
➢ Samlad bedömning<br>
Projektet visar en god förståelse för MVVM-arkitektur och är väl lämpat som en lärande prototyp. För att nå produktionsnivå bör fokus ligga på att tydligare separera UI-logik, införa DI, förbättra lösenordshantering och validering samt implementera datalagring. En stegvis refaktorering enligt denna plan kommer att göra systemet mer modulärt, säkrare och lättare att testa.<br>

---
<h2 style="color: green;">Project Summary</h2>
This project is a WPF application developed using the MVVM architecture. The goal is to separate the user interface (UI), application logic, and data models to create a clean, maintainable, and scalable system.<br>
<b>The system consists of:</b><br>
<b>Views</b> – Windows for login, registration, two-factor authentication (2FA), recipe management, and user profiles.<br>
<b>ViewModels</b> – Manage UI state, commands, and data binding.<br>
<b>Managers / Services</b> – Handle domain logic (authentication, recipe operations, seed initialization).<br>
<b>Models</b> – Data entities: <code>User</code>, <code>AdminUser</code>, <code>Recipe</code>.<br>
Data is initialized via <code>DefaultUserSeed</code>. Commands follow the Command Pattern using <code>RelayCommand</code>, refreshed with <code>CommandManager.InvalidateRequerySuggested()</code>. Password handling uses the <code>PasswordChanged</code> event due to WPF <code>PasswordBox</code> limitations.<br>

<h3>Strengths</h3>
- Clear and well-structured architecture.<br>
- Correct MVVM separation — business logic kept out of ViewModels.<br>
- Consistent <code>INotifyPropertyChanged</code>/<code>RelayCommand</code> usage yields responsive UI.<br>
- Centralized authentication and user state streamline flow control.<br>

<h3>Improvement Plan</h3>
- Introduce a DialogService to decouple dialogs from ViewModels.<br>
- Clean and correct Models with duplicated/invalid members.<br>
- Implement Dependency Injection (DI) for flexibility and testing.<br>
- Refactor UserManager into focused services (auth/profile/recovery).<br>
- Simplify ViewModels via Interfaces, Services, Infrastructure layer.<br>
- Add a Navigation Service for multi-window flow.<br>
- Build a real persistence layer.<br>
- Write unit tests (auth, 2FA, recipes, validation).<br>

<h3>Overall Outlook</h3>
Suitable for learning and prototyping; solid MVVM foundation. Next steps: further UI decoupling, improve data security, field-level validation, and modular services. Following the plan will evolve it into a stable, secure, production-ready system that is easy to maintain and test.<br>