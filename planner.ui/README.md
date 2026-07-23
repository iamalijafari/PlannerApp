# Planner UI

The Planner front end is a Next.js App Router application. Its source lives
entirely under `src`.

## Run locally

```bash
npm ci
npm run dev
```

The UI uses `http://localhost:5010/api` by default. Override it when needed:

```bash
NEXT_PUBLIC_API_URL=https://example.test/api npm run dev
```

## Routes

- `/goals` lists and creates goals.
- `/goals/[id]/edit` edits a goal.
- `/goals/[id]/tree` manages its yearly, monthly, weekly, and daily plan.

## Quality checks

```bash
npm run lint
npm run typecheck
npm run build
```
