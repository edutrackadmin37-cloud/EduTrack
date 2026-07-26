# 📚 EduTrack PBL-LMS

**Project-Based Learning Management System for Ghanaian Senior High Schools**  
*Built for the Ghana Education Service (GES) & NaSIA Competency-Based Curriculum*

---

## 🎯 Overview

EduTrack is a comprehensive **Project-Based Learning (PBL) Management System** designed specifically for Ghanaian Senior High Schools. It solves the critical research gap of **accurately separating subject-specific performance analytics and individual contributions within team-based projects** – a feature missing from traditional LMS platforms.

The system provides a complete ecosystem for managing **projects, assignments, teams, grading, attendance, tests, rubrics, reflections, and reporting**, all aligned with the GES curriculum structure.

---

## ✨ Key Features

### 🔬 Subject‑Siloed Analytics *(The Research Gap Fix)*
- Each subject is an isolated analytics container.
- **Scores from different subjects are NEVER aggregated**.
- Separate dashboards, report cards, and summaries per subject.

### 👥 Team Management & Individual Marking
- Teams are tied to a specific subject, class, and academic year.
- Teachers grade both **Team Score** (collaboration, final output) and **Individual Scores** for each member.
- Generate performance breakdowns showing both team and individual contributions.

### 📋 Full PBL Lifecycle Workflow
Proposal Submitted → Proposal Approved → Team Formation → Planning →
Research → Implementation → Reflection → Assessment → Reporting → Closure

text
- Timestamped status changes with user tracking via `ProjectStatusHistory` table.

### 💬 Internal Messaging & Collaboration
- Team group chat (`TeamChatMessage`), teacher announcements, subject‑wide discussion boards (`DiscussionBoard`).
- File sharing (max 10 MB, stored securely).

### 📊 Engagement & Reflection Tracking
- Teachers mark engagement indicators (`EngagementChecklists`) – participation, problem‑solving, collaboration.
- Students submit weekly reflection journals (`Reflections`).
- Export engagement trends (weekly percentages).

### 📈 Reporting Engine
- Student report cards (per subject, PDF/Excel/Word) via `sp_GetStudentSubjectReports`.
- Teacher performance (`sp_GetTeacherSubjectPerformance`), departmental analytics (`sp_GetDepartmentSubjectAnalytics`), project portfolios (`sp_GetProjectPortfolio`), parent reports.

### 🏫 Comprehensive Administration
- School, programme, subject, department, staff, parent, attendance, academic calendar, timetable, curriculum mapping, project monitoring, audit logs (`ActivityLog`), approval workflows (`ApprovalWorkflow`, `ApprovalStep`).

---

## 🧱 Technology Stack

| Layer / Component | Technology |
|-------------------|------------|
| **Frontend** | ASP.NET Web Forms (.aspx + Code-behind), Bootstrap 5.3.2 (CDN), Bootstrap Icons 1.11.3, Vanilla JavaScript |
| **Backend Language** | C# 8.0 |
| **Framework** | .NET Framework 4.8 (Windows / IIS only) |
| **Target Runtime** | v4.0 (.NET 4.x) |
| **Compilation** | Release mode (`debug="false"`) |
| **Architecture** | Strict Layered Architecture: UI → BLL → DAL → Database |
| **Database** | SQL Server 2022, ADO.NET, Stored Procedures (40+ procs) |
| **Authentication** | Forms Authentication with RolePrincipal |
| **Email / SMS** | SendGrid (email), Twilio/Hubtel (SMS) with stubs for local development |
| **Security** | PBKDF2‑SHA256 password hashing (10,000+ iterations, 16‑byte salt, 32‑byte hash) |
| **Export** | PDF (iTextSharp), Excel (EPPlus), Word (HTML/Word MIME) |

### 📌 C# 8.0 Features Utilized

The project leverages modern C# 8.0 language features to improve code clarity, safety, and maintainability:

- ✅ **Nullable reference types** – Enables compile‑time null safety.
- ✅ **Switch expressions** – Cleaner, expression‑based `=>` syntax for multi‑branch logic.
- ✅ **Pattern matching** – Simplified type and value checks.
- ✅ **Default interface methods** – Allows safe evolution of interfaces without breaking implementations.
- ✅ **Async streams** – Enables asynchronous enumeration of data sequences (`IAsyncEnumerable`).
- ✅ **Using declarations** – Automatic resource disposal with cleaner, scoped syntax.

---

## 📁 Project Structure
EduTrack/
├── EduTrack.sln
├── EduTrack/
│ ├── Properties/
│ ├── References/
│ ├── App_Data/
│ │ ├── Logs/
│ │ │ ├── email_log.txt
│ │ │ └── sms_log.txt
│ │ ├── Uploads/
│ │ ├── Resources/
│ │ └── Submissions/
│ ├── Auth/ # Login, Register, Password reset, Profile
│ ├── Admin/ # Full admin CRUD (users, classes, subjects, etc.)
│ ├── Teacher/ # Teacher dashboard, projects, grading, attendance, tests
│ ├── Student/ # Student dashboard, projects, teams, reflections, grades
│ ├── Parent/ # Parent dashboard, child performance, attendance, reports
│ ├── Headmaster/ # School overview, pending approvals
│ ├── AssistantHeadmaster/ # Academic monitoring, reports
│ ├── AcademicCoordinator/ # Curriculum implementation, teacher performance
│ ├── HOD/ # Department subjects, teacher assignments, project approval
│ ├── Shared/ # Site.Master, Footer.ascx, etc.
│ ├── BLL/ # Business Logic Layer (~50 classes)
│ ├── DAL/ # Data Access Layer (ADO.NET + stored procs)
│ ├── Models/ # Domain models & DTOs (~50 classes)
│ ├── Helpers/ # PasswordHelper, SessionManager, ValidationHelper, etc.
│ ├── Services/ # Email & SMS service interfaces and implementations
│ ├── Global.asax
│ ├── Web.config
│ ├── Default.aspx
│ └── ...
├── Database/
│ ├── Schema.sql # Complete database script (37+ tables, 40+ procs, views, functions, seed data)
│ └── StoredProcedures.sql
└── Documentation/
├── SystemArchitecture.md
├── API_Integration.md
└── UserGuide.md

text

---

## 🚀 Getting Started

### Prerequisites

- **Windows** (IIS or IIS Express)
- **Visual Studio 2022** (or later) with **ASP.NET and web development** workload
- **SQL Server 2022** (Express or higher) – or use `localhost\SQLEXPRESS`
- **.NET Framework 4.8**

### 1. Clone the Repository

```bash
git clone https://github.com/edutrackadmin37-cloud/EduTrack.git
cd EduTrack
2. Create the Database
Open SQL Server Management Studio (or sqlcmd), connect to your SQL Server, and run the complete database script:

sql
-- The complete script is located at:
-- Database/Schema.sql
This single script (Schema.sql) will:

Drop any existing EduTrack_DB database.

Create all tables (37+ tables) including:

Core: Users, Departments, Schools, Staff, Classes, Subjects, Programmes

PBL: Projects, ProjectTeams, ProjectTeamMembers, ProjectStatusHistory

Assessment: Rubrics, RubricCriteria, RubricCriterionLevels, TeamAssessments, IndividualContributions

Academic: AcademicYears, Semesters, Terms, Streams, ClassSubjectTeacher, ClassStudent

Assignments & Tests: Assignments, Submissions, Grades, Tests, Questions, StudentAnswers, TestResults

Engagement: EngagementChecklists, Reflections

Communication: Messages, Notifications, Announcements, TeamChatMessage, DiscussionBoard

Administration: ActivityLog, SystemSettings, ApprovalWorkflow, ApprovalStep

Extended: AcademicCalendar, ContinuousAssessment, NotificationPreference, PeerAssessment, SelfAssessment, Timetable, TimetableEntry, ParentStudentMap, UserSubjects

Create helper functions: fn_GetGradeLetter(), fn_GetRemark()

Create 40+ stored procedures for CRUD operations and reporting, including:

Core CRUD: sp_GetUserById, sp_CreateUser, sp_UpdateUser, sp_SoftDeleteUser, and similar for all entities

Reporting: sp_GetStudentSubjectReports, sp_GetTeacherSubjectPerformance, sp_GetDepartmentSubjectAnalytics, sp_GetProjectPortfolio, sp_GetTeamPortfolios, sp_GetParentAttendanceSummary

Analytics: sp_GetSchoolPerformanceOverview, sp_GetSchoolOverallPerformance, sp_GetDepartmentGradingConsistency

Create views including vw_StudentSubjectPerformance for subject-siloed analytics.

Seed the GES curriculum (7 departments, 8 programmes, 33 subjects), default school, and admin user.

3. Update Web.config
Connection Strings: Update Data Source, Initial Catalog, and credentials if needed.

Email/SMS: Replace placeholder API keys for SendGrid, Twilio, or Hubtel if you intend to use real services. For development, you can leave them as stub (logs to file).

Platform URL: Set LocalResetBaseUrl to your application’s base URL (e.g., https://localhost:44363).

Custom Errors: For development, keep mode="Off"; for production, set mode="RemoteOnly".

4. Restore NuGet Packages
In Visual Studio, open the Package Manager Console and run:

powershell
Update-Package -Reinstall
Or simply right‑click the solution and select Restore NuGet Packages.

5. Build & Run
Build the solution (Ctrl+Shift+B).

Press F5 to run with IIS Express.

The application will launch at https://localhost:44363/.
Log in with the seeded admin credentials:

text
Email:    edutrackadmin37@gmail.com
Password: Nt5437132#37
Note: The password hash was generated using PBKDF2‑SHA256 with 10,000 iterations, 16‑byte salt, and 32‑byte hash. If you need to reset it, use the built‑in password reset flow or generate a new hash using PasswordHelper.HashPassword().

🔧 Configuration
Web.config Sections
Section	Description
<appSettings>	Email/SMS provider selection, API keys, platform URL, file limits, feature toggles, SMTP fallback
<connectionStrings>	Two names: EduTrackDb and EduTrackConnection (both point to the same database)
<system.web>	Forms Authentication, session state, custom error handling, machine key
<location>	Authorization rules for public pages (Auth, About, FAQ, Help, Documentation, Contact, Error pages)
<runtime>	Assembly binding redirects for System.Runtime.CompilerServices.Unsafe
<system.webServer>	Security headers, caching
📊 Database Schema Highlights
37+ Tables covering the entire PBL ecosystem.

8 User Roles: SystemAdministrator, Headmaster, AssistantHeadmaster, AcademicCoordinator, HOD, Teacher, Student, Parent.

Subject‑siloed analytics: The vw_StudentSubjectPerformance view and stored procedures like sp_GetStudentSubjectReports prevent cross‑subject aggregation.

PBL Lifecycle: Projects have a Status field with 11 stages; ProjectStatusHistory tracks all transitions with user tracking.

Team & Individual Grading: TeamAssessments and IndividualContributions tables store separate scores. TeamAssessments has a 1:1 relationship with teams; IndividualContributions links to TeamAssessments for per-student scoring.

Engagement & Reflection: EngagementChecklists (6 metrics per student per project per week) and Reflections (weekly journals) tables track student engagement.

Complete Test System: Tests, Questions, StudentAnswers, and TestResults tables support MCQ, Text, True/False, and Fill-in question types.

Approval Workflow: ApprovalWorkflow and ApprovalStep tables support multi-step approval processes with role-based routing.

All tables include CreatedAt, UpdatedAt, and IsDeleted for audit and soft‑delete.

🧪 Testing
The system includes a comprehensive suite of pages for each role. To test all features:

Login as admin (edutrackadmin37@gmail.com / Nt5437132#37) – create a school, programmes, subjects, academic years, streams, classes, and staff.

Login as teacher – create projects, assignments, rubrics, tests, grade submissions, mark attendance, create engagement checklists.

Login as student – join teams, submit work, take tests, write reflections.

Login as parent – view child’s subject‑specific performance and attendance.

📦 Deployment
Option 1: Deploy to IIS
Publish the project from Visual Studio (right‑click → Publish → Web Server (IIS) → Web Deploy).

Set up an IIS website pointing to the published folder.

Ensure the application pool uses .NET Framework 4.8 (Integrated pipeline).

Grant write permissions to the IIS user for App_Data\Logs, App_Data\Uploads, App_Data\Resources, and App_Data\Submissions.

Option 2: Deploy to Azure
In Visual Studio, right‑click → Publish → Azure App Service.

Follow the wizard to create an App Service and deploy.

Update connection strings and API keys in the Azure portal.

Environment Variables
For production, consider setting these in Web.config or as environment variables:

EmailProvider (smtp | sendgrid)

SmsProvider (stub | twilio | hubtel)

SendGridApiKey

TwilioAccountSid / TwilioAuthToken

HubtelClientId / HubtelClientSecret

PlatformURL (used for password reset links)

🤝 Contributing
We welcome contributions! Please follow these guidelines:

Fork the repository.

Create a feature branch (git checkout -b feature/your-feature).

Commit your changes (git commit -m 'Add some feature').

Push to the branch (git push origin feature/your-feature).

Open a Pull Request.

📄 License
This project is licensed under the MIT License – see the LICENSE file for details.

👥 Team
Nyarko Timothy Akwasi – Lead Developer & Architect

Sumaila Abdul‑Rahman – Quality Assurance & Support

Dr. Daniel Danso Essel – Subject Matter Expert

📬 Contact
Email: nyarkoakwasi36@gmail.com

Phone: +233 54 371 3237

GitHub: edutrackadmin37-cloud

LinkedIn: Timothy Akwasi Nyarko

🏁 Acknowledgements
Ghana Education Service (GES) for the competency‑based curriculum framework.

NaSIA for educational standards.

The open‑source community for the tools and libraries used.

EduTrack – Bridging the gap between ICT theory and practical problem solving in SHS.
