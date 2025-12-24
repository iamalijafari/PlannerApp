import { Goal } from '../types/goal';

const base = process.env.NEXT_PUBLIC_GOAL_API_URL || '/api/goal';

export async function listGoals(): Promise<Goal[]> {
  const res = await fetch(`${base}/GetAll`, { method: 'POST' });
  if (!res.ok) throw new Error('Failed to fetch goals');
  const body = await res.json();
  return body;
}

export async function getGoal(id: string): Promise<Goal> {
  const res = await fetch(`${base}/Get`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(id) });
  if (!res.ok) throw new Error('Failed to fetch goal');
  return res.json();
}

export async function createGoal(payload: Goal): Promise<Goal> {
  const res = await fetch(`${base}/Create`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload),
  });
  if (!res.ok) throw new Error('Failed to create goal');
  return res.json();
}

export async function updateGoal(id: string, payload: Goal): Promise<Goal> {
  // backend expects Update DTO — send payload including id
  const body = { ...payload, id };
  const res = await fetch(`${base}/Update`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body),
  });
  if (!res.ok) throw new Error('Failed to update goal');
  return res.json();
}

export async function deleteGoal(id: string): Promise<void> {
  const res = await fetch(`${base}/Delete`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(id) });
  if (!res.ok) throw new Error('Failed to delete goal');
}

export async function completeGoal(id: string): Promise<void> {
  const res = await fetch(`${base}/Complete`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(id) });
  if (!res.ok) throw new Error('Failed to complete goal');
}
