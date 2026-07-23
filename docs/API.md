# PlannerApp API

PlannerApp exposes an ASP.NET Core REST API for goals, hierarchical plans, and localized messages.

## Interactive documentation

Start the API through Docker Compose or the `Docker`/`Development` environment, then open:

- Swagger UI: http://localhost:5010/swagger
- OpenAPI JSON: http://localhost:5010/swagger/v1/swagger.json
- Health check: http://localhost:5010/health

The default API base URL is:

```text
http://localhost:5010/api
```

## Response envelope

Goal and plan operations return a consistent envelope:

```json
{
  "success": true,
  "result": {},
  "messageKey": 0
}
```

`success` reports application-level success, `result` contains the requested value, and `messageKey` identifies a localizable application message.

## Date conventions

- Send due dates as ISO 8601 values.
- Prefer UTC values with a trailing `Z`, for example `2027-06-01T00:00:00Z`.
- The domain normalizes incoming goal and plan dates to UTC before PostgreSQL persistence.
- Collection endpoints and nested tree collections return items in ascending `dueDate` order.

## Goals

| Method | Route | Description |
| --- | --- | --- |
| `GET` | `/api/goal` | List all goals |
| `GET` | `/api/goal/{id}` | Get one goal |
| `GET` | `/api/goal/{id}/tree` | Get the complete goal and plan hierarchy |
| `POST` | `/api/goal` | Create a goal |
| `PUT` | `/api/goal/{id}` | Update a goal |
| `PUT` | `/api/goal/{id}/complete` | Mark a goal completed |
| `DELETE` | `/api/goal/{id}` | Delete a goal |

Create request:

```json
{
  "title": "Relocate to Ireland",
  "description": "Prepare the professional and practical move.",
  "dueDate": "2027-06-01T00:00:00Z"
}
```

Update request:

```json
{
  "title": "Relocate to Ireland",
  "description": "Prepare the professional and practical move.",
  "dueDate": "2027-06-01T00:00:00Z",
  "isCompleted": false
}
```

## Yearly plans

| Method | Route | Description |
| --- | --- | --- |
| `GET` | `/api/yearlyplan/by-goal/{goalId}` | List yearly plans for a goal |
| `GET` | `/api/yearlyplan/{id}` | Get one yearly plan |
| `POST` | `/api/yearlyplan` | Create a yearly plan |
| `PUT` | `/api/yearlyplan/{id}` | Update a yearly plan |
| `PUT` | `/api/yearlyplan/{id}/complete` | Mark a yearly plan completed |
| `DELETE` | `/api/yearlyplan/{id}` | Delete a yearly plan |

The create body uses `goalId` as its parent identifier.

## Monthly plans

| Method | Route | Description |
| --- | --- | --- |
| `GET` | `/api/monthlyplan/by-yearly-plan/{yearlyPlanId}` | List monthly plans for a yearly plan |
| `GET` | `/api/monthlyplan/{id}` | Get one monthly plan |
| `POST` | `/api/monthlyplan` | Create a monthly plan |
| `PUT` | `/api/monthlyplan/{id}` | Update a monthly plan |
| `PUT` | `/api/monthlyplan/{id}/complete` | Mark a monthly plan completed |
| `DELETE` | `/api/monthlyplan/{id}` | Delete a monthly plan |

The create body uses `yearlyPlanId` as its parent identifier.

## Weekly plans

| Method | Route | Description |
| --- | --- | --- |
| `GET` | `/api/weeklyplan/by-monthly-plan/{monthlyPlanId}` | List weekly plans for a monthly plan |
| `GET` | `/api/weeklyplan/{id}` | Get one weekly plan |
| `POST` | `/api/weeklyplan` | Create a weekly plan |
| `PUT` | `/api/weeklyplan/{id}` | Update a weekly plan |
| `PUT` | `/api/weeklyplan/{id}/complete` | Mark a weekly plan completed |
| `DELETE` | `/api/weeklyplan/{id}` | Delete a weekly plan |

The create body uses `monthlyPlanId` as its parent identifier.

## Daily plans

| Method | Route | Description |
| --- | --- | --- |
| `GET` | `/api/dailyplan/by-weekly-plan/{weeklyPlanId}` | List daily plans for a weekly plan |
| `GET` | `/api/dailyplan/{id}` | Get one daily plan |
| `POST` | `/api/dailyplan` | Create a daily plan |
| `PUT` | `/api/dailyplan/{id}` | Update a daily plan |
| `PUT` | `/api/dailyplan/{id}/complete` | Mark a daily plan completed |
| `DELETE` | `/api/dailyplan/{id}` | Delete a daily plan |

The create body uses `weeklyPlanId` as its parent identifier.

Plan create bodies otherwise share this shape:

```json
{
  "parentId": "replace-with-the-level-specific-parent-field",
  "title": "Complete a focused milestone",
  "description": "A measurable step toward the parent plan.",
  "dueDate": "2027-03-31T00:00:00Z"
}
```

Plan update bodies use `title`, `description`, `dueDate`, and `isCompleted`; the route supplies the plan identifier.

## Translations

| Method | Route | Description |
| --- | --- | --- |
| `POST` | `/api/translation/translate` | Translate one message key into English or Persian |

Request:

```json
{
  "messageKey": 1,
  "language": 0
}
```

The response is the translated string. Swagger exposes the current enum names and values.
