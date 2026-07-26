# PlannerApp engineering and refactoring notes

**Updated:** 26 July 2026

**Branch:** `main`

## Outcome

PlannerApp has been consolidated into a coherent full-stack portfolio project. The active Next.js App Router UI, ASP.NET Core API, application services, domain model, EF Core persistence, Docker packaging, automated tests, CI pipeline, and documentation now describe the same goal-and-plan hierarchy.

## Key engineering decisions

### Goal versus plan

The top-level outcome remains a `Goal`. Time-based children are consistently named `YearlyPlan`, `MonthlyPlan`, `WeeklyPlan`, and `DailyPlan` across entities, DTOs, repositories, services, controllers, routes, TypeScript types, UI components, and EF Core schema.

### Clean boundaries

- `Planner.Domain` owns entities and invariants.
- `Planner.Application` owns use cases, DTOs, repository/service contracts, mappings, translations, and result models.
- `Planner.Infrastructure` owns EF Core persistence, migrations, and repository implementations.
- `Planner.Api` owns HTTP transport, middleware, dependency composition, and Swagger.
- `planner.ui` owns presentation, localization context, and API clients.

### Date correctness

Domain entities normalize created and updated due dates to UTC before persistence. The UI supports Gregorian and Jalali selection, including Persian digits, while API collections and every nested tree level are returned in ascending due-date order.

### UI consolidation

The abandoned Pages Router implementation and duplicate UI modules were removed. The maintained UI lives under `planner.ui/src`, with centralized configuration and API calls. English is the initial language and left-to-right layout; users can switch to Persian and its right-to-left layout. The edit and tree routes are part of the production App Router build:

- `/goals/[id]/edit`
- `/goals/[id]/tree`

### Container packaging

The Docker build uses multi-stage API and UI images. Docker Compose starts PostgreSQL, waits for its health check, and then starts the API and UI. The API image restores only the deployable project graph; test-only dependencies remain in CI.

### Production deployment

The public demo runs at https://plannerapp-web.onrender.com/. Render hosts the Next.js frontend and ASP.NET Core API, while Neon provides managed PostgreSQL. Both containers honor Render's injected `PORT`; production CORS is restricted to the configured frontend origin, and database credentials remain in Render environment variables. EF Core applies pending migrations when the API starts.

The deployment remains a single-user portfolio demonstration. It has no authentication or per-user data isolation, so it must contain demonstration data only.

### Automated quality

`Planner.UnitTests` covers domain validation, UTC normalization, application-service behavior, repository collaboration, failure handling, and tree ordering. GitHub Actions performs:

1. .NET restore, Release build, unit tests, and coverage collection.
2. UI dependency installation, linting, TypeScript checking, and production build.
3. Docker Compose image builds after both application jobs pass.

## Documentation improvements

- Replaced stale endpoint names and test placeholders in the README.
- Added portfolio-focused architecture, feature, quality, and limitation sections.
- Added an API reference in `docs/API.md`.
- Enabled XML documentation in the API project and included it in Swagger.
- Enabled Swagger in both Development and Docker environments.
- Added accurate README, GitHub social-preview, and LinkedIn artwork under `assets/`.
- Linked the [MIT License](LICENSE) directly from the README and badge row.
- Added the public demo URL and a Render/Neon deployment guide.
- Replaced pre-deployment limitation wording with accurate public-demo security guidance.

## Validation

| Check                               | Location                                         |
| ----------------------------------- | ------------------------------------------------ |
| Backend restore/build/test/coverage | GitHub Actions `backend` job                     |
| ESLint, TypeScript, Next.js build   | Local commands and GitHub Actions `frontend` job |
| API and UI container builds         | GitHub Actions `containers` job                  |
| English/Persian message-key parity  | Repository consistency check                     |
| XML dictionary well-formedness      | Repository consistency check                     |
| README links and asset dimensions   | Repository consistency check                     |

The local authoring environment did not include the .NET SDK or Docker CLI, so the committed GitHub Actions workflow is the authoritative backend and container validation environment.

## Deliberate limitations

This repository is publicly accessible as a full-stack engineering portfolio demonstration, but it is not an internet-facing multi-user product. Visitors should use demonstration data only and never enter sensitive information. Before production use, prioritize:

- authentication and per-user authorization
- secrets management and production CORS configuration
- structured observability and distributed tracing
- API status-code and validation refinements
- integration and browser-level end-to-end tests
- infrastructure as code, automated deployment verification, and database backup policy
