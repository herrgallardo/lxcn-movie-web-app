# HollyView Movie Database Application

## Course Context

- **Program**: Arbetsmarknadsutbildning - IT Påbyggnad/Programmerare
- **Course**: C# .NET Fullstack System Developer
- **Minitask**: ASP.NET MVC Movie Web Application with Identity Authentication

## Learning Objectives

This web application demonstrates comprehensive ASP.NET Core MVC development concepts:

- Model-View-Controller (MVC) Architecture Pattern
- ASP.NET Core Identity Authentication and Authorization
- Entity Framework Core with SQL Server Integration
- Role-Based Access Control (RBAC) Implementation
- Responsive Web Design with Bootstrap 5
- CRUD Operations with Data Validation
- Azure SQL Database Cloud Integration
- Modern Web UI/UX Development

## Overview

A full-featured movie database web application built with ASP.NET Core MVC that allows users to manage a comprehensive movie collection. The application implements a three-tier authorization system (Admin, User, Non-user) with role-based permissions, modern responsive design, and complete CRUD functionality. All data is stored persistently in Azure SQL Database with ASP.NET Core Identity handling user authentication and role management.

## Features

- **Movie Management**: Complete CRUD operations for movie data
- **Advanced Search & Filtering**: Search by title/description, filter by genre, multiple sorting options
- **Role-Based Authentication**: Three-tier authorization system with specific permissions
- **User Management**: Admin panel for user role assignment and system administration
- **Responsive Design**: Modern Bootstrap 5 interface optimized for all devices
- **Real Movie Posters**: Integration with TMDB for authentic movie artwork
- **Security Features**: ASP.NET Core Identity with comprehensive access control
- **Error Handling**: Custom error pages and access denied handling
- **Azure Integration**: Cloud-based SQL Server database deployment

## Key Components

### Controllers

- `HomeController`: Landing page and application overview
- `MoviesController`: Complete movie CRUD operations with role-based authorization
- `AdminController`: Administrative panel for user and system management
- `ErrorController`: Custom error handling and access denied pages

### Models

- `Movie`: Entity model with comprehensive movie data and validation attributes
- `ApplicationDbContext`: Entity Framework database context with Identity integration
- `AdminViewModels`: View models for complex admin operations and statistics

### Services

- `UserRoleService`: Centralized role checking and permission validation
- `RoleSeeder`: Automatic role creation and default admin user setup
- `SeedData`: Sample movie data initialization with TMDB poster integration

### Authentication & Authorization

- `RoleConstants`: Centralized role definitions (Admin, User)
- `CustomAuthorizeAttribute`: Enhanced authorization with user-friendly error handling
- Three-tier permission system with granular access control

## Technical Skills Demonstrated

- ASP.NET Core MVC 9.0 architecture and design patterns
- Entity Framework Core with SQL Server and Azure SQL Database
- ASP.NET Core Identity for authentication and role management
- Bootstrap 5 responsive web design and modern UI components
- Razor view engine with strongly-typed views and partial views
- LINQ queries for data filtering, searching, and sorting
- Dependency injection and service-oriented architecture
- Model validation with data annotations and custom validation
- Security best practices including CSRF protection and XSS prevention
- Git version control with atomic, commit-based development

## Application Flow

1. **Database Initialization**: Automatic role creation and admin user seeding
2. **Authentication System**: User registration, login, and role assignment
3. **Movie Browsing**: Public access to movie collection with search and filtering
4. **Role-Based Operations**:
   - **Non-users**: View-only access with login prompts
   - **Users**: Read and update permissions for movie editing
   - **Admins**: Full CRUD access plus user management capabilities
5. **Admin Dashboard**: Comprehensive system management and user administration
6. **Error Handling**: Graceful error pages and access denied scenarios

## Requirements

- .NET 9.0 SDK or later
- Azure account with SQL Database (free tier available)
- Visual Studio 2022 or Visual Studio Code
- Internet connection for TMDB movie poster integration
- Modern web browser (Chrome, Firefox, Edge, Safari)

## How to Run

### Setup

1. **Create Azure SQL Database** (free tier available)

2. **Clone Repository**:

```bash
git clone <repository-url>
cd lxcn-movie-web-app
```

3. **Configure Database Connection**:
   Create/update `appsettings.json` with your Azure SQL connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:your-server.database.windows.net,1433;Initial Catalog=movie-web-db;User ID=your-username;Password=your-password;Encrypt=True;TrustServerCertificate=False;"
  }
}
```

4. **Install Dependencies**:

```bash
dotnet restore
```

5. **Apply Database Migrations**:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

6. **Run Application**:

```bash
dotnet build
dotnet run
```

7. **Access Application**:
   - Open browser to `https://localhost:5001`
   - Default admin account: `admin@hollyview.com` / `Admin123!`

## Implementation Details

### Role-Based Authorization System

- **Admin**: Full system access (Create, Read, Update, Delete movies + User Management)
- **User**: Limited access (Read and Update movies only)
- **Non-user**: View-only access (Browse movies and details with login prompts)

### User Roles & Permissions

| Feature            | Non-user (Guest) | User | Admin |
| ------------------ | ---------------- | ---- | ----- |
| Browse Movies      | ✅               | ✅   | ✅    |
| View Movie Details | ✅               | ✅   | ✅    |
| Search & Filter    | ✅               | ✅   | ✅    |
| Edit Movies        | ❌               | ✅   | ✅    |
| Create Movies      | ❌               | ❌   | ✅    |
| Delete Movies      | ❌               | ❌   | ✅    |
| User Management    | ❌               | ❌   | ✅    |
| Admin Dashboard    | ❌               | ❌   | ✅    |

### Movie Data Management

- Comprehensive movie information (Title, Genre, Release Date, Price, Rating, Duration)
- TMDB integration for authentic movie poster artwork
- Advanced search with title and description filtering
- Multi-criteria sorting (Title, Date, Price, Genre)
- Form validation with server-side and client-side validation

### Security Features

- ASP.NET Core Identity with secure password requirements
- Anti-forgery token protection on all forms
- Role-based authorization attributes on controller actions
- SQL injection prevention through Entity Framework parameterized queries
- XSS protection via Razor view engine automatic encoding
- Custom access denied pages with user-friendly messaging

### Database Schema

```sql
-- Movies table
Movies (Id, Title, ReleaseDate, Genre, Price, Description, ImageUrl, Rating, Duration, DateCreated, CreatedBy)

-- ASP.NET Identity tables (automatically created)
AspNetUsers, AspNetRoles, AspNetUserRoles, etc.
```

### UI/UX Features

- Bootstrap 5 responsive design with modern components
- Card-based movie display with poster images
- Role indicators and permission badges
- Interactive search and filtering interface
- Mobile-optimized navigation and layouts
- Custom error pages with helpful navigation options

## Learning Notes

This application represents a comprehensive introduction to modern web development with ASP.NET Core MVC. It demonstrates the integration of authentication, authorization, database operations, and responsive web design in a real-world application context. The role-based permission system provides practical experience with enterprise-level security patterns, while the Bootstrap integration showcases modern frontend development practices.

## Educational Program Details

- **Type**: Arbetsmarknadsutbildning (Labor Market Training)
- **Focus**: IT Påbyggnad/Programmerare (IT Advanced/Programmer)
- **Course**: C# .NET Fullstack System Developer
- **Application Type**: ASP.NET Core MVC Web Application
- **Development Approach**: Incremental, commit-based tutorial methodology

## Potential Improvements for Future Learning

- **File Upload System**: Implement local movie poster upload functionality
- **API Development**: Create REST API endpoints for mobile app integration
- **Advanced Authentication**: Add OAuth integration (Google, Facebook, Microsoft)
- **Caching**: Implement Redis caching for improved performance
- **Search Enhancement**: Add full-text search capabilities
- **Recommendation System**: Build movie recommendation engine
- **Export Features**: Generate movie collection reports (PDF, Excel)
- **Real-time Features**: Add SignalR for real-time notifications
- **Microservices**: Refactor into microservices architecture
- **Docker Deployment**: Containerize application for modern deployment
- **CI/CD Pipeline**: Implement automated testing and deployment
- **Performance Monitoring**: Add Application Insights integration
