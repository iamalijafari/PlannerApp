# Deploy PlannerApp with Render and Neon

PlannerApp's public demo uses Render for the Next.js frontend and ASP.NET Core API, with Neon providing managed PostgreSQL.

- Live web application: https://plannerapp-web.onrender.com/
- Source repository: https://github.com/iamalijafari/PlannerApp

The application is a single-user portfolio demonstration. It does not provide authentication or per-user data isolation, so use demonstration data only and never enter sensitive information.

## Production topology

| Component | Platform | Repository configuration |
| --- | --- | --- |
| Next.js frontend | Render web service | `Dockerfile.ui` |
| ASP.NET Core API | Render web service | `Dockerfile.api` |
| PostgreSQL | Neon | Managed database outside the repository |

The frontend and API should use the same Render region. Render injects `PORT`; do not create that environment variable manually.

## Required secrets and configuration

Configure these values in Render, never in committed files.

### API service

| Variable | Value |
| --- | --- |
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `ConnectionStrings__DefaultConnection` | Semicolon-separated Npgsql connection string for Neon |
| `Cors__AllowedOrigins__0` | Exact frontend origin without a trailing slash |

Use this Npgsql format for the Neon connection:

```text
Host=<host>;Port=5432;Database=<database>;Username=<role>;Password=<password>;SSL Mode=Require
```

Do not commit the connection string or use Neon's `postgresql://` URL directly as the Npgsql value.

### Frontend service

| Variable | Value |
| --- | --- |
| `NEXT_PUBLIC_API_URL` | Public API URL ending in `/api`, without a trailing slash |

`NEXT_PUBLIC_API_URL` is required while the Next.js image is built, because browser-facing environment variables are embedded into the production bundle.

## Render service settings

### API

| Setting | Value |
| --- | --- |
| Runtime | Docker |
| Branch | `main` |
| Root directory | Leave blank |
| Dockerfile | `./Dockerfile.api` |
| Docker build context | `.` |
| Health check | `/health` |

The API applies pending Entity Framework Core migrations during startup. A failed database connection therefore prevents the service from becoming healthy.

### Frontend

| Setting | Value |
| --- | --- |
| Runtime | Docker |
| Branch | `main` |
| Root directory | Leave blank |
| Dockerfile | `./Dockerfile.ui` |
| Docker build context | `.` |
| Health check | `/` |

## Deployment order

1. Create the Neon project and database role.
2. Create the Render API service and add its database settings.
3. Confirm that the API `/health` endpoint returns `Healthy`.
4. Create the Render frontend service with the API URL.
5. Add the exact frontend origin to the API CORS setting and redeploy the API.
6. Run the production smoke test below.

## Production smoke test

1. Open the live frontend in a private browser window.
2. Create, edit, complete, and delete a test goal.
3. Add yearly, monthly, weekly, and daily plans.
4. Verify dashboard and goal progress percentages.
5. Refresh the browser and confirm that data persists.
6. Check mobile layout, browser console, and network requests.
7. Confirm that the API health check still returns `Healthy`.

## Operational notes

- Render free services may sleep after inactivity, so the first request can be slower.
- Keep the Neon password, connection string, and Render environment values private.
- Rotate any credential immediately if it appears in logs, screenshots, issues, or chat.
- Restrict production CORS to the exact frontend origin.
- Review Render logs and Neon usage periodically.
- Add authentication and per-user authorization before treating the application as a real multi-user service.
