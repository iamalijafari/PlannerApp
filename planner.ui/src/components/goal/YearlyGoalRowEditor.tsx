import React from 'react';
import { YearlyGoal } from '../../types/goal';

interface Props {
  value: YearlyGoal;
  onChange: (v: YearlyGoal) => void;
  onSave: (v: YearlyGoal) => void;
  onCancel?: () => void;
  saveLabel?: string;
  cancelLabel?: string;
  titlePlaceholder?: string;
  targetPlaceholder?: string;
}

export const YearlyGoalRowEditor: React.FC<Props> = ({ value, onChange, onSave, onCancel, saveLabel = 'Save', cancelLabel = 'Cancel', titlePlaceholder = 'Title', targetPlaceholder = 'Target' }) => {
  const isValid = () => {
    if (!value) return false;
    if (!value.title || !value.title.trim()) return false;
    if (!value.year || Number.isNaN(Number(value.year))) return false;
    return true;
  };

  return (
    <div className="flex items-center gap-3 w-full">
      <input
        aria-label="Year"
        type="number"
        value={value.year}
        onChange={(e) => onChange({ ...value, year: Number(e.target.value) || new Date().getFullYear() })}
        className="rounded-md px-2 py-1 w-20"
      />
      <input aria-label="Title" placeholder={titlePlaceholder} value={value.title} onChange={(e) => onChange({ ...value, title: e.target.value })} className="flex-1 rounded-md px-2 py-1" />
      <input aria-label="Target" placeholder={targetPlaceholder} value={value.target || ''} onChange={(e) => onChange({ ...value, target: e.target.value })} className="w-36 rounded-md px-2 py-1" />
      <input aria-label="Due Date" type="date" value={value.dueDate || ''} onChange={(e) => onChange({ ...value, dueDate: e.target.value })} className="w-40 rounded-md px-2 py-1" />
      <button onClick={() => onSave(value)} disabled={!isValid()} className="primary-btn">{saveLabel}</button>
      <button onClick={onCancel} className="muted-btn">{cancelLabel}</button>
    </div>
  );
};

export default YearlyGoalRowEditor;
