import { Goal } from '../types/goal';

const base = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5010/api';
const goalBase = `${base}/goal`;

export async function listGoals(): Promise<Goal[]> {
  const res = await fetch(goalBase, { method: 'GET' });
  if (!res.ok) throw new Error('Failed to fetch goals');
  const body = await res.json();
  return body?.result || [];
}

export async function getGoal(id: string): Promise<Goal> {
  const res = await fetch(`${goalBase}/${id}`, { method: 'GET' });
  if (!res.ok) throw new Error('Failed to fetch goal');
  const data = await res.json();
  return data?.result;
}

export async function createGoal(payload: Goal): Promise<Goal> {
  const res = await fetch(goalBase, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload),
  });
  if (!res.ok) throw new Error('Failed to create goal');
  const data = await res.json();
  return data?.result;
}

export async function updateGoal(id: string, payload: Goal): Promise<Goal> {
  const res = await fetch(`${goalBase}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload),
  });
  if (!res.ok) throw new Error('Failed to update goal');
  const data = await res.json();
  return data?.result;
}

export async function deleteGoal(id: string): Promise<void> {
  const res = await fetch(`${goalBase}/${id}`, { method: 'DELETE' });
  if (!res.ok) throw new Error('Failed to delete goal');
}

export async function completeGoal(id: string): Promise<void> {
  const res = await fetch(`${goalBase}/${id}/complete`, { method: 'PUT' });
  if (!res.ok) throw new Error('Failed to complete goal');
}

