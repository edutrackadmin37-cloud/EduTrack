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
```
Proposal Submitted → Proposal Approved → Team Formation → Planning → 
Research → Implementation → Reflection → Assessment → Reporting → Closure
```
- Timestamped status changes with user tracking.

### 💬 Internal Messaging & Collaboration
- Team group chat, teacher announcements, subject‑wide discussion boards.
- File sharing (max 10 MB, stored securely).

### 📊 Engagement & Reflection Tracking
- Teachers mark engagement indicators (participation, problem‑solving, collaboration).
- Students submit weekly reflection journals.
- Export engagement trends (weekly percentages).

### 📈 Reporting Engine
- Student report cards (per subject, PDF/Excel/Word).
- Teacher performance, departmental analytics, project portfolios, parent reports.

### 🏫 Comprehensive Administration
- School, programme, subject, department, staff, parent, attendance, academic calendar, timetable, curriculum mapping, project monitoring, audit logs, approval workflows.

---

## 🧱 Technology Stack

| Layer | Technology |
|-------|------------|
| **Frontend** | ASP.NET Web Forms (.aspx + Code-behind), Bootstrap 5.3.2 (CDN), Bootstrap Icons 1.11.3, Vanilla JavaScript |
| **Backend** | C# on .NET Framework 4.7.2 (or 4.8) |
| **Architecture** | Strict Layered Architecture: UI → BLL → DAL → Database |
| **Database** | SQL Server 2022, ADO.NET, Stored Procedures |
| **Authentication** | Forms Authentication with RolePrincipal |
| **Email/SMS** | SendGrid (email), Twilio/Hubtel (SMS) with stubs for local development |
| **Security** | PBKDF2‑SHA256 password hashing (10,000+ iterations, 16‑byte salt, 32‑byte hash) |
| **Export** | PDF (iTextSharp), Excel (EPPlus), Word (HTML/Word MIME) |

---

## 📁 Project Structure

```
EduTrack/
├── EduTrack.sln
├── EduTrack/
│   ├── Properties/
│   ├── References/
│   ├── App_Data/
│   │   ├── Logs/
│   │   │   ├── email_log.txt
│   │   │   └── sms_log.txt
│   │   └── Uploads/
│   ├── Auth/                    # Login, Register, Password reset, Profile
│   ├── Admin/                   # Full admin CRUD (users, classes, subjects, etc.)
│   ├── Teacher/                 # Teacher dashboard, projects, grading, attendance, tests
│   ├── Student/                 # Student dashboard, projects, teams, reflections, grades
│   ├── Parent/                  # Parent dashboard, child performance, attendance, reports
│   ├── Headmaster/              # School overview, pending approvals
│   ├── AssistantHeadmaster/     # Academic monitoring, reports
│   ├── AcademicCoordinator/     # Curriculum implementation, teacher performance
│   ├── HOD/                     # Department subjects, teacher assignments, project approval
│   ├── Shared/                  # Site.Master, Footer.ascx, etc.
│   ├── BLL/                     # Business Logic Layer (~50 classes)
│   ├── DAL/                     # Data Access Layer (ADO.NET + stored procs)
│   ├── Models/                  # Domain models & DTOs (~50 classes)
│   ├── Helpers/                 # PasswordHelper, SessionManager, ValidationHelper, etc.
│   ├── Services/                # Email & SMS service interfaces and implementations
│   ├── Global.asax
│   ├── Web.config
│   ├── Default.aspx
│   └── ...
├── Database/
│   ├── Schema.sql               # Full database script (tables, procs, views, seed data)
│   └── StoredProcedures.sql
└── Documentation/
    ├── SystemArchitecture.md
    ├── API_Integration.md
    └── UserGuide.md
```

---

## 🚀 Getting Started

### Prerequisites

- **Windows** (IIS or IIS Express)
- **Visual Studio 2022** (or later) with **ASP.NET and web development** workload
- **SQL Server 2022** (Express or higher) – or use `localhost\SQLEXPRESS`
- **.NET Framework 4.7.2** (or 4.8)

### 1. Clone the Repository

```bash
git clone https://github.com/linguistic247/EduTrack.git
cd EduTrack
```

### 2. Create the Database

Open **SQL Server Management Studio** (or `sqlcmd`), connect to your SQL Server, and run the **complete database script**:

```sql
-- The script is located at:
-- Database/Schema.sql
-- OR use the single script provided in the project root.
```

This script will:
- Drop any existing `EduTrack_DB` database.
- Create all tables (37+), stored procedures, functions, and views.
- Seed the GES curriculum, departments, programmes, subjects, and a default admin user.

### 3. Update `Web.config`

- **Connection Strings**: Update `Data Source`, `Initial Catalog`, and credentials if needed.
- **Email/SMS**: Replace placeholder API keys for SendGrid, Twilio, or Hubtel if you intend to use real services. For development, you can leave them as `stub` (logs to file).
- **Platform URL**: Set `LocalResetBaseUrl` to your application’s base URL (e.g., `https://localhost:44363`).
- **Custom Errors**: For development, keep `mode="Off"`; for production, set `mode="RemoteOnly"`.

### 4. Restore NuGet Packages

In Visual Studio, open the **Package Manager Console** and run:

```powershell
Update-Package -Reinstall
```

Or simply right‑click the solution and select **Restore NuGet Packages**.

### 5. Build & Run

- Build the solution (Ctrl+Shift+B).
- Press **F5** to run with IIS Express.

The application will launch at `https://localhost:44363/`.  
Log in with the seeded admin credentials:

```
Email:    edutrackadmin37@gmail.com
Password: Nt5437132#37
```

> **Note**: The password hash was generated using PBKDF2‑SHA256; if you need to reset it, use the built‑in password reset flow or generate a new hash using `PasswordHelper.HashPassword()`.

---

## 🔧 Configuration

### Web.config Sections

| Section | Description |
|---------|-------------|
| `<appSettings>` | Email/SMS provider selection, API keys, platform URL, file limits, feature toggles, SMTP fallback |
| `<connectionStrings>` | Two names: `EduTrackDb` and `EduTrackConnection` (both point to the same database) |
| `<system.web>` | Forms Authentication, session state, custom error handling, machine key |
| `<location>` | Authorization rules for public pages (Auth, About, FAQ, Help, Documentation, Contact, Error pages) |
| `<runtime>` | Assembly binding redirects for `System.Runtime.CompilerServices.Unsafe` |
| `<system.webServer>` | Security headers, caching |

---

## 📊 Database Schema Highlights

- **Users**: Supports 8 roles (SystemAdministrator, Headmaster, AssistantHeadmaster, AcademicCoordinator, HOD, Teacher, Student, Parent).
- **Subject‑siloed analytics**: The `vw_StudentSubjectPerformance` view and stored procedures like `sp_GetStudentSubjectReports` prevent cross‑subject aggregation.
- **PBL Lifecycle**: Projects have a `Status` field with 11 stages; `ProjectStatusHistory` tracks all transitions.
- **Team & Individual Grading**: `TeamAssessments` and `IndividualContributions` tables store separate scores.
- **Engagement & Reflection**: `EngagementChecklists` and `Reflections` tables track student engagement and journals.
- **All tables** include `CreatedAt`, `UpdatedAt`, and `IsDeleted` for audit and soft‑delete.

---

## 🧪 Testing

The system includes a comprehensive suite of pages for each role. To test all features:

1. **Login as admin** – create a school, programmes, subjects, academic years, streams, classes, and staff.
2. **Login as teacher** – create projects, assignments, rubrics, tests, grade submissions, mark attendance.
3. **Login as student** – join teams, submit work, take tests, write reflections.
4. **Login as parent** – view child’s subject‑specific performance and attendance.

---

## 📦 Deployment

### Option 1: Deploy to IIS

1. Publish the project from Visual Studio (right‑click → Publish → Web Server (IIS) → Web Deploy).
2. Set up an IIS website pointing to the published folder.
3. Ensure the application pool uses **.NET Framework 4.x** (Integrated pipeline).
4. Grant write permissions to the IIS user for `App_Data\Logs`, `App_Data\Uploads`, `App_Data\Resources`, and `App_Data\Submissions`.

### Option 2: Deploy to Azure

1. In Visual Studio, right‑click → Publish → Azure App Service.
2. Follow the wizard to create an App Service and deploy.
3. Update connection strings and API keys in the Azure portal.

### Environment Variables

For production, consider setting these in `Web.config` or as environment variables:

- `EmailProvider` (smtp | sendgrid)
- `SmsProvider` (stub | twilio | hubtel)
- `SendGridApiKey`
- `TwilioAccountSid` / `TwilioAuthToken`
- `HubtelClientId` / `HubtelClientSecret`
- `PlatformURL` (used for password reset links)

---

## 🤝 Contributing

We welcome contributions! Please follow these guidelines:

1. Fork the repository.
2. Create a feature branch (`git checkout -b feature/your-feature`).
3. Commit your changes (`git commit -m 'Add some feature'`).
4. Push to the branch (`git push origin feature/your-feature`).
5. Open a Pull Request.

---

## 📄 License

This project is licensed under the MIT License – see the [LICENSE](LICENSE) file for details.

---

## 👥 Team

- **Nyarko Timothy Akwasi** – Lead Developer & Architect  
- **Sumaila Abdul‑Rahman** – Quality Assurance & Support  
- **Dr. Daniel Danso Essel** – Subject Matter Expert

---

## 📬 Contact

- **Email**: nyarkoakwasi36@gmail.com  
- **Phone**: +233 54 371 3237  
- **GitHub**: [linguistic247](https://github.com/linguistic247)  
- **LinkedIn**: [Timothy Akwasi Nyarko](https://www.linkedin.com/in/timothy-akwasi-nyarko-22a364357)

---

## 🏁 Acknowledgements

- Ghana Education Service (GES) for the competency‑based curriculum framework.
- NaSIA for educational standards.
- The open‑source community for the tools and libraries used.

---

**EduTrack** – *Bridging the gap between ICT theory and practical problem solving in SHS.*
