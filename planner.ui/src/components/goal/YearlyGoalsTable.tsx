import React, { useState, useEffect } from 'react';
import { YearlyGoal } from '../../types/goal';
import YearlyGoalRowEditor from './YearlyGoalRowEditor';
import { useTranslation } from "@/context/translationContext";
import { MessageKey } from '@/types/message-key';

interface Props {
  values?: YearlyGoal[];
  onChange: (v: YearlyGoal[]) => void;
}

export const YearlyGoalsTable: React.FC<Props> = ({ values = [], onChange }) => {
  const [editingIndex, setEditingIndex] = useState<number | null>(null);
  const { t } = useTranslation();


  function handleAdd() {
    if (values.length >= 5) {
      alert(t(MessageKey.Max_YearlyGoals_Message));
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
        <h4 className="text-lg font-semibold">{t(MessageKey.YearlyGoals)}</h4>
        <div className="flex items-center">
          <button onClick={handleAdd} disabled={values.length >= 5} className="primary-btn mr-3">{t(MessageKey.Add)}</button>
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
                cancelLabel={t(MessageKey.Cancel)}
                titlePlaceholder={t(MessageKey.Title)}
                targetPlaceholder={t(MessageKey.Target)}
              />
            ) : (
              <>
                <div className="w-20 text-sm font-medium">{row.year}</div>
                <div className="flex-1 text-sm">{row.title}</div>
                <div className="w-32 text-sm text-zinc-500">{row.target}</div>
                <div className="w-36 text-sm text-zinc-500">{row.dueDate || ''}</div>
                <div className="ml-4">
                  <button onClick={() => setEditingIndex(idx)} className="muted-btn mr-2">{t(MessageKey.Edit)}</button>
                  <button onClick={() => handleDelete(idx)} className="muted-btn">{t(MessageKey.Delete)}</button>
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
