import React, { useEffect, useState } from 'react';
import { ToastProvider, useToast } from '../../components/toast/ToastContext';
import * as goalService from '../../services/goalService';
import { Goal } from '../../types/goal';

const CenteredContainer: React.FC<{ children: React.ReactNode }> = ({ children }) => (
  <div style={{ minHeight: '100vh', display: 'flex', alignItems: 'center', justifyContent: 'center', padding: 24 }}>
    <div style={{ width: '100%', maxWidth: 720 }}>{children}</div>
  </div>
);

const Page: React.FC = () => {
  const [goals, setGoals] = useState<Goal[]>([]);
  const [loading, setLoading] = useState(true);
  const [selectedId, setSelectedId] = useState<string | null>(null);
  const [showForm, setShowForm] = useState(false);
  const [editing, setEditing] = useState(false);

  // form state
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [startDate, setStartDate] = useState('');
  const [dueDate, setDueDate] = useState('');

  const toast = useToast();

  useEffect(() => {
    load();
  }, []);

  async function load() {
    setLoading(true);
    try {
      const items = await goalService.listGoals();
      setGoals(items || []);
    } catch (e) {
      console.error(e);
    } finally {
      setLoading(false);
    }
  }

  function clearForm() {
    setTitle('');
    setDescription('');
    setStartDate('');
    setDueDate('');
    setSelectedId(null);
  }

  function openAdd() {
    clearForm();
    setEditing(false);
    setShowForm(true);
  }

  async function openEdit() {
    if (!selectedId) {
      alert('Please select a goal to edit');
      return;
    }
    setLoading(true);
    try {
      const g = await goalService.getGoal(selectedId);
      setTitle(g.title || '');
      setDescription(g.description || '');
      setStartDate(g.startDate || '');
      setDueDate(g.dueDate || '');
      setEditing(true);
      setShowForm(true);
    } catch (e) {
      console.error(e);
      alert('Failed to load goal');
    } finally {
      setLoading(false);
    }
  }

  async function handleSave(e?: React.FormEvent) {
    e?.preventDefault();
    try {
      const payload: any = { title, description, startDate, dueDate };
      if (editing && selectedId) {
        await goalService.updateGoal(selectedId, payload as any);
      } else {
        await goalService.createGoal(payload as any);
      }
      setShowForm(false);
      clearForm();
      await load();
      toast?.show && toast.show(editing ? 'Goal updated' : 'Goal created');
    } catch (err) {
      console.error(err);
      alert('Save failed');
    }
  }

  async function handleDelete() {
    if (!selectedId) { alert('Please select a goal to delete'); return; }
    if (!confirm('Are you sure?')) return;
    try {
      await goalService.deleteGoal(selectedId);
      setSelectedId(null);
      await load();
      toast?.show && toast.show('Goal deleted');
    } catch (e) {
      console.error(e);
      alert('Delete failed');
    }
  }

  async function handleComplete() {
    if (!selectedId) { alert('Please select a goal to complete'); return; }
    try {
      await goalService.completeGoal(selectedId);
      await load();
      toast?.show && toast.show('Goal completed');
    } catch (e) {
      console.error(e);
      alert('Complete failed');
    }
  }

  return (
    <ToastProvider>
      <CenteredContainer>
        <div style={{ background: '#fff', borderRadius: 10, padding: 20, boxShadow: '0 6px 18px rgba(13,40,71,0.04)' }}>
          <h2 style={{ margin: 0, marginBottom: 12, textAlign: 'center' }}>Goals</h2>

          {loading ? (
            <div style={{ padding: 20, textAlign: 'center' }}>Loading...</div>
          ) : (
            <div style={{ display: 'flex', flexDirection: 'column', gap: 12 }}>
              {goals.map((g) => (
                <div
                  key={g.id}
                  onClick={() => setSelectedId(g.id)}
                  style={{
                    padding: 12,
                    borderRadius: 8,
                    border: selectedId === g.id ? '2px solid #2563eb' : '1px solid #e5e7eb',
                    display: 'flex',
                    justifyContent: 'space-between',
                    alignItems: 'center',
                    cursor: 'pointer',
                    background: '#fff'
                  }}
                >
                  <div style={{ minWidth: 0 }}>
                    <div style={{ fontWeight: 600, textDecoration: g.completed ? 'line-through' : 'none', opacity: g.completed ? 0.6 : 1 }}>{g.title}</div>
                    <div style={{ fontSize: 13, color: '#6b7280' }}>{g.dueDate ? `Due: ${g.dueDate}` : ''}</div>
                  </div>
                  <div>{g.completed ? <span style={{ background: '#16a34a', color: '#fff', padding: '6px 8px', borderRadius: 999 }}>Completed</span> : null}</div>
                </div>
              ))}
            </div>
          )}

          <div style={{ display: 'flex', gap: 8, justifyContent: 'center', marginTop: 18 }}>
            <button onClick={openAdd} style={{ padding: '10px 16px', borderRadius: 8, background: '#2563eb', color: '#fff' }}>Add</button>
            <button onClick={openEdit} style={{ padding: '10px 16px', borderRadius: 8, background: '#6b7280', color: '#fff' }}>Edit</button>
            <button onClick={handleDelete} style={{ padding: '10px 16px', borderRadius: 8, background: '#ef4444', color: '#fff' }}>Delete</button>
            <button onClick={handleComplete} style={{ padding: '10px 16px', borderRadius: 8, background: '#10b981', color: '#fff' }}>Complete</button>
          </div>

          {showForm && (
            <form onSubmit={(e) => handleSave(e)} style={{ marginTop: 18 }}>
              <div style={{ display: 'grid', gap: 10 }}>
                <input required placeholder="Title" value={title} onChange={(e) => setTitle(e.target.value)} style={{ padding: 10, borderRadius: 8, border: '1px solid #e5e7eb' }} />
                <textarea placeholder="Description" value={description} onChange={(e) => setDescription(e.target.value)} style={{ padding: 10, borderRadius: 8, border: '1px solid #e5e7eb' }} />
                <div style={{ display: 'flex', gap: 8 }}>
                  <input type="date" value={startDate} onChange={(e) => setStartDate(e.target.value)} style={{ padding: 8, borderRadius: 8, border: '1px solid #e5e7eb' }} />
                  <input type="date" value={dueDate} onChange={(e) => setDueDate(e.target.value)} style={{ padding: 8, borderRadius: 8, border: '1px solid #e5e7eb' }} />
                </div>
                <div style={{ display: 'flex', gap: 8, justifyContent: 'flex-end' }}>
                  <button type="button" onClick={() => { setShowForm(false); clearForm(); }} style={{ padding: '8px 12px', borderRadius: 8 }}>Cancel</button>
                  <button type="submit" style={{ padding: '8px 12px', borderRadius: 8, background: '#2563eb', color: '#fff' }}>Save</button>
                </div>
              </div>
            </form>
          )}
        </div>
      </CenteredContainer>
    </ToastProvider>
  );
};

export default Page;
