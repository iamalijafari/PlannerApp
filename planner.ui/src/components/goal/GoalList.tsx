import React, { useEffect, useState } from 'react';
import Link from 'next/link';
import { Goal } from '../../types/goal';
import * as goalService from '../../services/goalService';
import { t } from '../../services/translationService';
import { useToast } from '../toast/ToastContext';

export const GoalList: React.FC = () => {
  const [items, setItems] = useState<Goal[]>([]);
  const [loading, setLoading] = useState(true);

  const [labelLoading, setLabelLoading] = useState('Loading...');
  const [labelAdd, setLabelAdd] = useState('Add');
  const [labelNoGoals, setLabelNoGoals] = useState('No goals found. Add one.');
  const [labelEdit, setLabelEdit] = useState('Edit');
  const [labelDelete, setLabelDelete] = useState('Delete');
  const [labelComplete, setLabelComplete] = useState('Complete');
  const [labelCompleted, setLabelCompleted] = useState('Completed');
  const toast = useToast();

  useEffect(() => {
    // load translations for visible labels
    Promise.all([
      t('GoalListTitle'),
      t('Add'),
      t('Goals_Empty'),
      t('Edit'),
      t('Delete'),
      t('Complete'),
      t('Complete'),
    ]).then((res) => {
      setLabelAdd(res[1] || 'Add');
      setLabelNoGoals(res[2] || 'No goals found. Add one.');
      setLabelEdit(res[3] || 'Edit');
      setLabelDelete(res[4] || 'Delete');
      setLabelComplete(res[5] || 'Complete');
      setLabelLoading('Loading...');
    });

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
      t('Toast_Goal_Deleted').then((m) => toast.show(m));
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
      t('Goal_Completed_Message').then((m) => toast.show(m));
    } catch (e) {
      console.error(e);
      setItems(prev);
    }
  }

  if (loading) return <div className="p-4">{labelLoading}</div>;
  if (!items.length)
    return (
      <div className="container py-8">
        <div className="text-center">
          <p className="text-zinc-600 dark:text-zinc-300 mb-4">{labelNoGoals}</p>
          <Link href="/goals/new" className="inline-block px-4 py-2 rounded-md primary-btn">{labelAdd}</Link>
        </div>
      </div>
    );

  return (
    <div className="container py-8">
        <div className="flex items-center justify-between mb-6">
        <h2 className="text-2xl font-bold">{labelLoading === 'Loading...' ? 'Goals' : t('GoalListTitle')}</h2>
        <Link href="/goals/new" className="primary-btn">{labelAdd}</Link>
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
                <button className="muted-btn" aria-label={`Edit ${g.title}`}>{labelEdit}</button>
              </Link>
              <button onClick={() => handleDelete(g.id)} className="muted-btn">{labelDelete}</button>
              <button onClick={() => handleComplete(g.id)} disabled={g.completed} className={`ml-2 px-3 py-1.5 rounded-md text-sm ${g.completed ? 'completed-badge' : 'primary-btn'}`}>
                {g.completed ? labelCompleted : labelComplete}
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default GoalList;
