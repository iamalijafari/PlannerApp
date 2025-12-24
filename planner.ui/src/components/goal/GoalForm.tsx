import React, { useEffect, useState } from 'react';
import { useRouter } from 'next/router';
import { Goal, YearlyGoal } from '../../types/goal';
import * as goalService from '../../services/goalService';
import YearlyGoalsTable from './YearlyGoalsTable';
import { t } from '../../services/translationService';
import { useToast } from '../toast/ToastContext';

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

  const [labelBack, setLabelBack] = useState('Back');
  const [labelSave, setLabelSave] = useState('Save');
  const [labelSaveClose, setLabelSaveClose] = useState('Save & Close');
  const [labelTitle, setLabelTitle] = useState('Title');
  const [labelDescription, setLabelDescription] = useState('Description');
  const [labelStartDate, setLabelStartDate] = useState('Start Date');
  const [labelDueDate, setLabelDueDate] = useState('Due Date');

  useEffect(() => {
    // load translations
    Promise.all([
      t('Back'),
      t('Save'),
      t('SaveAndClose'),
      t('Title'),
      t('Description'),
      t('Field_StartDate'),
      t('Field_DueDate'),
    ]).then((res) => {
      setLabelBack(res[0] || 'Back');
      setLabelSave(res[1] || 'Save');
      setLabelSaveClose(res[2] || 'Save & Close');
      setLabelTitle(res[3] || 'Title');
      setLabelDescription(res[4] || 'Description');
      setLabelStartDate(res[5] || 'Start Date');
      setLabelDueDate(res[6] || 'Due Date');
    });

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
        t('Toast_Goal_Updated').then((m) => toast.show(m));
      } else {
        const created = await goalService.createGoal(model);
        if (created && created.id) {
          router.replace(`/goals/${created.id}/edit`);
          setDirty(false);
          t('Toast_Goal_Created').then((m) => toast.show(m));
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
            {labelBack}
          </button>
          <button className="btn" onClick={() => handleSave(false)} disabled={loading || !model.title || !model.title.trim()}>
            {labelSave}
          </button>
          <button className="btn" onClick={() => handleSave(true)} disabled={loading || !model.title || !model.title.trim()}>
            {labelSaveClose}
          </button>
        </div>

        <div style={{ display: 'grid', gap: 16 }}>
          <div>
            <label>{labelTitle}</label>
            <input type="text" className="max-w-lg" value={model.title || ''} onChange={(e) => { setModel({ ...model, title: e.target.value }); setDirty(true); }} />
          </div>
          <div>
            <label>{labelDescription}</label>
            <textarea className="max-w-lg" value={model.description || ''} onChange={(e) => { setModel({ ...model, description: e.target.value }); setDirty(true); }} />
          </div>

          <div style={{ display: 'flex', gap: 12 }}>
            <div>
              <label>{labelStartDate}</label>
              <input type="date" className="max-w-xs" value={model.startDate || ''} onChange={(e) => { setModel({ ...model, startDate: e.target.value }); setDirty(true); }} />
            </div>
            <div>
              <label>{labelDueDate}</label>
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
