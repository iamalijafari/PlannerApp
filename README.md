# PlannerApp

A full-stack **Goal Planning & Management System** built with modern technologies. Create, track, and manage your goals with yearly sub-goals. Multi-language support (English & Persian/Farsi).

## 🎯 Project Overview

**PlannerApp** is a comprehensive goal planning application that helps users:

- ✅ Create and manage personal/professional goals
- ✅ Break down goals into yearly sub-goals
- ✅ Track progress and mark goals as completed
- ✅ Manage goals with title, description, and due dates
- ✅ Support multi-language translations (EN, FA)

## 🏗️ Architecture

PlannerApp follows **Clean Architecture** principles with clear separation of concerns:

```
Planner.Domain              → Core business entities (Goal, YearlyGoal)
    ↓
Planner.Application         → DTOs, Services, Repositories, Mappers, Utilities
    ↓
Planner.Infrastructure      → Database context, migrations, repository implementations
    ↓
Planner.Api                 → REST API controllers, middleware, API DTOs
    ↓
planner.ui                  → Next.js frontend, React components, API integration
```

### Backend Design Pattern

- **Repository Pattern**: Data access abstraction
- **Service Layer**: Business logic separation
- **Dependency Injection**: Loose coupling
- **Global Exception Middleware**: Centralized error handling

### Frontend Design Pattern

- **React Hooks**: State management with hooks
- **App Router**: Next.js 16 App Router (modern navigation)
- **Feature-based Structure**: Organized by features
- **API Integration**: Centralized API calls with error handling

## 🛠️ Technology Stack

### Backend

- **Runtime**: .NET 9.0
- **Framework**: ASP.NET Core 9.0
- **ORM**: Entity Framework Core 9.0
- **Database**: PostgreSQL
- **API Documentation**: Swagger/Swashbuckle 10.0.1
- **Language**: C# with nullable reference types

### Frontend

- **Framework**: Next.js 16.0.7
- **UI Library**: React 19.2.0
- **Language**: TypeScript 5.x
- **Styling**: Tailwind CSS 4.x with PostCSS
- **Code Quality**: ESLint, Prettier
- **CSS**: LightningCSS for optimized builds

### Database

- **Engine**: PostgreSQL 16
- **Docker Support**: Yes

## 🚀 Quick Start

### Prerequisites

- Docker & Docker Compose (for containerized setup)
- .NET 9.0 SDK (for local development)
- Node.js 22+ (for frontend development)
- PostgreSQL 16 (for local development)

### Option 1: Docker Compose (Recommended)

```bash
# Clone the repository
git clone <repository-url>
cd PlannerApp

# Start all services (API, UI, PostgreSQL)
docker-compose up -d

# The application will be available at:
# - Frontend: http://localhost:3000
# - API: http://localhost:5010/api
# - Swagger Docs: http://localhost:5010/swagger
```

### Option 2: Local Development

#### Backend Setup

```bash
cd Planner.Api

# Restore dependencies
dotnet restore

# Update database (migrations)
dotnet ef database update --project ../Planner.Infrastructure

# Run the API
dotnet run

# API will be available at http://localhost:5010/api
```

#### Frontend Setup

```bash
cd planner.ui

# Copy environment file
cp .env.example .env.local

# Install dependencies
npm install

# Run development server
npm run dev

# Frontend will be available at http://localhost:3000
```

## 📁 Project Structure

```
PlannerApp/
├── Planner.Api/                    # REST API layer
│   ├── Controllers/                # API endpoints
│   ├── DTOs/                       # Request/Response models
│   ├── Middlewares/                # Custom middleware (error handling)
│   ├── Mappers/                    # DTO mapping logic
│   ├── Program.cs                  # Dependency injection & configuration
│   └── appsettings.json            # Configuration
│
├── Planner.Application/            # Business logic layer
│   ├── DTOs/                       # Domain models for services
│   ├── Services/                   # Business logic implementation
│   ├── Interfaces/                 # Service & repository contracts
│   ├── Mappers/                    # Domain to DTO mapping
│   ├── Enumerations/               # Message keys, enums
│   └── Utilities/                  # Helper utilities (translation)
│
├── Planner.Domain/                 # Core domain layer
│   ├── Entities/                   # Goal, YearlyGoal
│   └── Enumerations/               # Domain-level enums
│
├── Planner.Infrastructure/         # Data access layer
│   ├── Persistence/                # DbContext
│   ├── Repositories/               # Data access implementations
│   └── Migrations/                 # Database migrations
│
├── planner.ui/                     # Next.js frontend
│   ├── src/
│   │   ├── app/                    # App Router pages
│   │   ├── features/               # Feature modules
│   │   │   └── goals/
│   │   │       ├── api/            # API integration
│   │   │       ├── components/     # React components
│   │   │       ├── types/          # TypeScript types
│   │   │       └── hooks/          # Custom hooks
│   │   ├── types/                  # Global types
│   │   └── context/                # React context
│   ├── public/                     # Static assets
│   ├── .env.example                # Environment template
│   ├── next.config.ts              # Next.js configuration
│   ├── tailwind.config.cjs         # Tailwind CSS configuration
│   └── package.json                # Dependencies
│
├── docker-compose.yml              # Container orchestration
├── Dockerfile.api                  # Backend Docker image
├── Dockerfile.ui                   # Frontend Docker image
└── README.md                       # This file
```

## 📝 API Endpoints

All endpoints return a `ResponseModel` with `success`, `messageKey`, and `result` fields.

### Goals

- `GET /api/goal` - Get all goals
- `GET /api/goal/{id}` - Get goal by ID
- `POST /api/goal` - Create new goal
- `PUT /api/goal/{id}` - Update goal
- `DELETE /api/goal/{id}` - Delete goal
- `PUT /api/goal/{id}/complete` - Mark goal as completed

### Yearly Goals

- `GET /api/yearlygoa/by-goal/{goalId}` - Get yearly goals for a goal
- `GET /api/yearlygoa/{id}` - Get yearly goal by ID
- `POST /api/yearlygoa` - Create yearly goal
- `PUT /api/yearlygoa/{id}` - Update yearly goal
- `DELETE /api/yearlygoa/{id}` - Delete yearly goal
- `PUT /api/yearlygoa/{id}/complete` - Mark yearly goal as completed

### Translations

- `GET /api/translation` - Get translations for specified language

## 🔧 Development

### Backend Development

```bash
cd Planner.Api

# Watch mode
dotnet watch run

# Run tests (when available)
dotnet test

# Code analysis
dotnet build /p:EnforceCodeStyleInBuild=true
```

### Frontend Development

```bash
cd planner.ui

# Development server with hot reload
npm run dev

# Build for production
npm run build

# Start production build
npm start

# Linting
npm run lint

# Format code
npx prettier --write .
```

## 🐳 Docker Commands

```bash
# Build images
docker-compose build

# Start all services in background
docker-compose up -d

# View logs
docker-compose logs -f

# Stop services
docker-compose down

# Remove all containers and volumes
docker-compose down -v

# Rebuild API only
docker-compose build api

# Rebuild UI only
docker-compose build ui
```

## 🔐 Environment Variables

### Backend (.env or docker-compose)

- `ASPNETCORE_ENVIRONMENT`: `Development`, `Docker`, or `Production`
- `ASPNETCORE_URLS`: Server URL (e.g., `http://+:5010`)
- `ConnectionStrings__DefaultConnection`: PostgreSQL connection string

### Frontend (.env.local)

- `NEXT_PUBLIC_API_URL`: Backend API URL (e.g., `http://localhost:5010/api`)
- `NODE_ENV`: `development` or `production`

## 📚 Code Quality

The project includes:

- ✅ **Null Safety**: C# nullable reference types enabled
- ✅ **Error Handling**: Global exception middleware + try-catch blocks
- ✅ **Input Validation**: DTO validation and null checks
- ✅ **Type Safety**: TypeScript for frontend
- ✅ **Linting**: ESLint for frontend, StyleCop for backend (recommended)
- ✅ **Code Formatting**: Prettier for frontend

## 🔄 Recent Improvements

✅ Fixed REST API endpoints (GET/POST/PUT/DELETE)  
✅ Removed async/await anti-patterns in repositories  
✅ Added comprehensive error handling and validation  
✅ Added environment variable configuration  
✅ Created Docker support (Dockerfiles + docker-compose)  
✅ Removed legacy Pages Router (Next.js App Router only)  
✅ Cleaned up junk files and unused code  
✅ Improved API error responses

## 📖 Database Schema

### Goals Table

| Column      | Type     | Notes                  |
| ----------- | -------- | ---------------------- |
| Id          | UUID     | Primary Key            |
| Title       | String   | Goal title             |
| Description | String   | Goal description       |
| DueDate     | DateTime | Target completion date |
| IsCompleted | Boolean  | Completion status      |
| CreatedAt   | DateTime | Creation timestamp     |
| UpdatedAt   | DateTime | Last update timestamp  |

### YearlyGoals Table

| Column      | Type     | Notes                  |
| ----------- | -------- | ---------------------- |
| Id          | UUID     | Primary Key            |
| GoalId      | UUID     | Foreign Key to Goals   |
| Title       | String   | Sub-goal title         |
| Description | String   | Sub-goal description   |
| DueDate     | DateTime | Target completion date |
| IsCompleted | Boolean  | Completion status      |
| CreatedAt   | DateTime | Creation timestamp     |
| UpdatedAt   | DateTime | Last update timestamp  |

## 🤝 Contributing

1. Create a feature branch (`git checkout -b feature/amazing-feature`)
2. Commit changes (`git commit -m 'Add amazing feature'`)
3. Push to branch (`git push origin feature/amazing-feature`)
4. Open a Pull Request

## 📝 License

This project is licensed under the MIT License.

## 📞 Support

For issues, questions, or suggestions, please create an issue in the repository.

---

**Happy Planning! 🎯**
