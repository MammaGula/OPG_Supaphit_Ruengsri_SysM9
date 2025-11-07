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
Detta projekt är en WPF-applikation som är utvecklad enligt MVVM-arkitekturen (Model-View-ViewModel). Målet är att separera användargränssnittet (UI), programlogiken och datamodellerna för att skapa ett strukturerat, lättunderhållet och skalbart system.<br>
<b>Systemet består av följande huvuddelar:</b><br>
<b>Views</b> – Fönster för inloggning, registrering, tvåfaktorsautentisering (2FA), recepthantering och användarprofil.<br>
<b>ViewModels</b> – Hanterar tillstånd i gränssnittet, kommandon och databindning.<br>
<b>Managers / Services</b> – Ansvarar för programmets logik, som autentisering, recepthantering och initialisering av standarddata.<br>
<b>Models</b> – Representerar dataklasserna <code>User</code>, <code>AdminUser</code> och <code>Recipe</code>.<br>
Data initieras via <code>DefaultUserSeed</code>. Kommandon implementeras med Command Pattern genom <code>RelayCommand</code> och uppdateras manuellt med <code>CommandManager.InvalidateRequerySuggested()</code>. Lösenord hanteras genom händelsen <code>PasswordChanged</code> istället för databindning, på grund av begränsningar i WPF:s <code>PasswordBox</code>.<br>

<h3>Styrkor</h3>
- Tydlig och väldefinierad struktur.<br>
- MVVM-principerna följs korrekt – affärslogik ligger inte i ViewModels.<br>
- Konsekvent användning av <code>INotifyPropertyChanged</code> och <code>RelayCommand</code> gör UI responsivt.<br>
- Centraliserad hantering av autentisering och användarstatus förenklar programflödet.<br>

<h3>Förbättringsplan</h3>
- Införa en DialogService för att hantera dialogrutor och meddelanden utan direkt UI-beroende.<br>
- Rensa upp och korrigera Models som innehåller duplicerade eller felaktiga egenskaper.<br>
- Implementera Dependency Injection (DI) för ökad flexibilitet och testbarhet.<br>
- Förbättra och dela upp UserManager (login, profil, återställning).<br>
- Förenkla ViewModels genom Interfaces, Services och ett tydligare Infrastructure-lager.<br>
- Skapa en Navigation Service för strukturerad fönsternavigering.<br>
- Införa ett riktigt datapersisteringslager.<br>
- Utveckla enhetstester för autentisering, 2FA, recept och valideringsflöden.<br>

<h3>Samlad bedömning</h3>
Projektet har en stabil grund och tydlig förståelse för MVVM. Nästa steg är att separera UI-logik, förbättra datasäkerhet, stärka fältvis validering och modulärisera tjänster. Stegvis förbättring kan ta projektet till ett produktionsklart system.<br>

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
Suitable for learning and prototyping; solid MVVM foundation. Next steps: further UI decoupling, improve data security, field-level validation, and modular services. Following the plan will evolve it into a stable, secure, production-ready system.<br>