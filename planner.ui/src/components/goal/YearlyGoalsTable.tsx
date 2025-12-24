import React, { useState, useEffect } from 'react';
import { YearlyGoal } from '../../types/goal';
import YearlyGoalRowEditor from './YearlyGoalRowEditor';
import { t } from '../../services/translationService';

interface Props {
  values?: YearlyGoal[];
  onChange: (v: YearlyGoal[]) => void;
}

export const YearlyGoalsTable: React.FC<Props> = ({ values = [], onChange }) => {
  const [editingIndex, setEditingIndex] = useState<number | null>(null);
  const [labelYearlyGoals, setLabelYearlyGoals] = useState('Yearly Goals');
  const [labelAdd, setLabelAdd] = useState('Add');
  const [labelEdit, setLabelEdit] = useState('Edit');
  const [labelDelete, setLabelDelete] = useState('Delete');
  const [labelCancel, setLabelCancel] = useState('Cancel');
  const [placeholderTitle, setPlaceholderTitle] = useState('Title');
  const [placeholderTarget, setPlaceholderTarget] = useState('Target');

  useEffect(() => {
    Promise.all([
      t('YearlyGoals_Title'),
      t('Add'),
      t('Edit'),
      t('Delete'),
      t('Cancel'),
      t('Title'),
      t('Field_Priority'),
    ]).then((res) => {
      setLabelYearlyGoals(res[0] || 'Yearly Goals');
      setLabelAdd(res[1] || 'Add');
      setLabelEdit(res[2] || 'Edit');
      setLabelDelete(res[3] || 'Delete');
      setLabelCancel(res[4] || 'Cancel');
      setPlaceholderTitle(res[5] || 'Title');
      setPlaceholderTarget('Target');
    });
  }, []);

  function handleAdd() {
    if (values.length >= 5) {
      t('Max_YearlyGoals_Message').then((m) => alert(m));
      return;
    }
    const next: YearlyGoal = { id: `tmp-${Date.now()}`, year: new Date().getFullYear(), title: '', target: '', completed: false };
    onChange([...values, next]);
    setEditingIndex(values.length);
  }

  function handleDelete(idx: number) {
    if (!confirm('Are you sure you want to delete this yearly goal?')) return;
    const copy = [...values];
    copy.splice(idx, 1);
    onChange(copy);
    setEditingIndex(null);
  }

  function handleRowChange(idx: number, v: YearlyGoal) {
    const copy = [...values];
    copy[idx] = v;
    onChange(copy);
  }

  function handleRowSave(idx: number, v: YearlyGoal) {
    // basic validation
    if (!v.title || !v.title.trim()) {
      alert('Title is required for yearly goal.');
      return;
    }
    if (!v.year || Number.isNaN(Number(v.year))) {
      alert('Year must be a valid number.');
      return;
    }
    handleRowChange(idx, v);
    setEditingIndex(null);
  }

  return (
    <div className="space-y-4">
      <div className="flex items-center justify-between">
        <h4 className="text-lg font-semibold">{labelYearlyGoals}</h4>
        <div className="flex items-center">
          <button onClick={handleAdd} disabled={values.length >= 5} className="primary-btn mr-3">{labelAdd}</button>
          <span className="text-sm text-zinc-500 dark:text-zinc-400">{values.length}/5</span>
        </div>
      </div>

      <div className="grid gap-3">
        {values.map((row, idx) => (
          <div key={row.id ?? idx} className="yearly-row">
            {editingIndex === idx ? (
              <YearlyGoalRowEditor
                value={row}
                onChange={(v) => handleRowChange(idx, v)}
                onSave={(v) => handleRowSave(idx, v)}
                onCancel={() => setEditingIndex(null)}
                saveLabel="Save"
                cancelLabel={labelCancel}
                titlePlaceholder={placeholderTitle}
                targetPlaceholder={placeholderTarget}
              />
            ) : (
              <>
                <div className="w-20 text-sm font-medium">{row.year}</div>
                <div className="flex-1 text-sm">{row.title}</div>
                <div className="w-32 text-sm text-zinc-500">{row.target}</div>
                <div className="w-36 text-sm text-zinc-500">{row.dueDate || ''}</div>
                <div className="ml-4">
                  <button onClick={() => setEditingIndex(idx)} className="muted-btn mr-2">{labelEdit}</button>
                  <button onClick={() => handleDelete(idx)} className="muted-btn">{labelDelete}</button>
                </div>
              </>
            )}
          </div>
        ))}
      </div>
    </div>
  );
};

export default YearlyGoalsTable;
