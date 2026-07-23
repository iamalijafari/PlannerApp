import React, { useEffect, useState } from 'react';
import { useRouter } from 'next/router';
import { Goal, YearlyGoal } from '../../types/goal';
import * as goalService from '../../services/goalService';
import YearlyGoalsTable from './YearlyGoalsTable';
import { useToast } from '../toast/ToastContext';
import { useTranslation } from "@/context/translationContext";
import { MessageKey } from '@/types/message-key';

interface Props {
  id?: string;
}

const GoalForm: React.FC<Props> = ({ id: propId }) => {
  const router = useRouter();
  const routeId = router.query?.id as string | undefined;
  const id = propId || routeId;
  const toast = useToast();

  const [model, setModel] = useState<Goal>({ title: '' });
  const [loading, setLoading] = useState(false);
  const [dirty, setDirty] = useState(false);

  const { t } = useTranslation();

  useEffect(() => {
    if (id) {
      setLoading(true);
      goalService
        .getGoal(id)
        .then((g) => setModel(g))
        .catch(() => {})
        .finally(() => setLoading(false));
    }
  }, [id]);

  async function handleSave(closeAfter = false) {
    setLoading(true);
    try {
      if (id) {
        await goalService.updateGoal(id, model);
        setDirty(false);
        toast.show(t(MessageKey.Toast_Goal_Updated));
      } else {
        const created = await goalService.createGoal(model);
        if (created && created.id) {
          router.replace(`/goals/${created.id}/edit`);
          setDirty(false);
          toast.show(t(MessageKey.Toast_Goal_Created));
        }
      }
    } catch (e) {
      console.error(e);
    } finally {
      setLoading(false);
      if (closeAfter) router.push('/goals');
    }
  }

  return (
    <div className="app-container">
      <div className="card">
        <div style={{ display: 'flex', gap: 8, marginBottom: 12 }}>
          <button className="muted-btn" onClick={() => router.push('/goals')}>
            {t(MessageKey.Back)}
          </button>
          <button className="btn" onClick={() => handleSave(false)} disabled={loading || !model.title || !model.title.trim()}>
            {t(MessageKey.Save)}
          </button>
          <button className="btn" onClick={() => handleSave(true)} disabled={loading || !model.title || !model.title.trim()}>
            {t(MessageKey.SaveAndClose)}
          </button>
        </div>

        <div style={{ display: 'grid', gap: 16 }}>
          <div>
            <label>{t(MessageKey.Title)}</label>
            <input type="text" className="max-w-lg" value={model.title || ''} onChange={(e) => { setModel({ ...model, title: e.target.value }); setDirty(true); }} />
          </div>
          <div>
            <label>{t(MessageKey.Description)}</label>
            <textarea className="max-w-lg" value={model.description || ''} onChange={(e) => { setModel({ ...model, description: e.target.value }); setDirty(true); }} />
          </div>

          <div style={{ display: 'flex', gap: 12 }}>
            <div>
              <label>{t(MessageKey.Field_StartDate)}</label>
              <input type="date" className="max-w-xs" value={model.startDate || ''} onChange={(e) => { setModel({ ...model, startDate: e.target.value }); setDirty(true); }} />
            </div>
            <div>
              <label>{t(MessageKey.Field_DueDate)}</label>
              <input type="date" className="max-w-xs" value={model.dueDate || ''} onChange={(e) => { setModel({ ...model, dueDate: e.target.value }); setDirty(true); }} />
            </div>
          </div>

          <YearlyGoalsTable values={model.yearlyGoals || []} onChange={(v: YearlyGoal[]) => { setModel({ ...model, yearlyGoals: v }); setDirty(true); }} />
        </div>
      </div>
    </div>
  );
};

export default GoalForm;
