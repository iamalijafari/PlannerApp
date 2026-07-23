import React, { useEffect, useState } from 'react';
import Link from 'next/link';
import { Goal } from '../../types/goal';
import * as goalService from '../../services/goalService';
import { useToast } from '../toast/ToastContext';
import { useTranslation } from "@/context/translationContext";
import { MessageKey } from '@/types/message-key';

export const GoalList: React.FC = () => {
  const [items, setItems] = useState<Goal[]>([]);
  const [loading, setLoading] = useState(true);

  const toast = useToast();
  const { t } = useTranslation();

  useEffect(() => {
    goalService.listGoals().then((g) => setItems(g)).catch(() => setItems([])).finally(() => setLoading(false));
  }, []);

  async function handleDelete(id?: string) {
    if (!id) return;
    if (!confirm('Are you sure you want to delete this goal?')) return;
    // optimistic update
    const prev = items;
    setItems((s) => s.filter((x) => x.id !== id));
    try {
      await goalService.deleteGoal(id);
      // toast deletion
      toast.show(t(MessageKey.Toast_Goal_Deleted));
    } catch (e) {
      console.error(e);
      setItems(prev);
    }
  }

  async function handleComplete(id?: string) {
    if (!id) return;
    // optimistic toggle
    const prev = items;
    setItems((s) => s.map((g) => (g.id === id ? { ...g, completed: true } : g)));
    try {
      await goalService.completeGoal(id);
      toast.show(t(MessageKey.Goal_Completed_Message));
    } catch (e) {
      console.error(e);
      setItems(prev);
    }
  }

  if (loading) return <div className="p-4">{t(MessageKey.Loading)}</div>;
  if (!items.length)
    return (
      <div className="container py-8">
        <div className="text-center">
          <p className="text-zinc-600 dark:text-zinc-300 mb-4">{t(MessageKey.NoGoals)}</p>
          <Link href="/goals/new" className="inline-block px-4 py-2 rounded-md primary-btn">{t(MessageKey.Add)}</Link>
        </div>
      </div>
    );

  return (
    <div className="container py-8">
        <div className="flex items-center justify-between mb-6">
        <h2 className="text-2xl font-bold">{t(MessageKey.Loading) === 'Loading...' ? 'Goals' : t(MessageKey.GoalListTitle)}</h2>
        <Link href="/goals/new" className="primary-btn">{t(MessageKey.Add)}</Link>
      </div>

      <div className="grid gap-4">
        {items.map((g) => (
          <div key={g.id} className="goal-card flex items-start justify-between">
            <div>
              <div className="goal-title">{g.title}</div>
              <div className="goal-desc">{g.description}</div>
            </div>
            <div className="actions">
              <Link href={`/goals/${g.id}/edit`}>
                <button className="muted-btn" aria-label={`Edit ${g.title}`}>{t(MessageKey.Edit)}</button>
              </Link>
              <button onClick={() => handleDelete(g.id)} className="muted-btn">{t(MessageKey.Delete)}</button>
              <button onClick={() => handleComplete(g.id)} disabled={g.completed} className={`ml-2 px-3 py-1.5 rounded-md text-sm ${g.completed ? 'completed-badge' : 'primary-btn'}`}>
                {g.completed ? t(MessageKey.Completed) : t(MessageKey.Complete)}
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default GoalList;
