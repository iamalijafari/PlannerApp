"use client";

import { useState, useEffect } from "react";
import { useApiResponse } from "@/utils/use-api-response";
import { useTranslation } from "@/context/translationContext";
import { MessageKey } from "@/types/message-key";

const API_BASE = process.env.NEXT_PUBLIC_GOAL_API_URL;

if (!API_BASE) {
  throw new Error("NEXT_PUBLIC_GOAL_API_URL is not defined in .env.local");
}

function GoalsPageContent() {
  const [goals, setGoals] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [selectedId, setSelectedId] = useState<string | null>(null);
  const [showForm, setShowForm] = useState(false);
  const [editing, setEditing] = useState(false);

  // form fields
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [dueDate, setDueDate] = useState("");

  const { t } = useTranslation();
  const { handleResponse } = useApiResponse();

  async function loadGoals() {
    setLoading(true);
    try {
      const res = await fetch(`${API_BASE}/GetAll`, { method: "POST" });
      const data = await res.json();
      await handleResponse<any[] | null>(
        data,
        (result) => setGoals(result || []),
        (msg) => alert(msg)
      );
    } catch (e) {
      console.error(e);
      alert("Failed to load goals");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => { loadGoals(); }, []);

  function clearForm() {
    setTitle("");
    setDescription("");
    setDueDate("");
    setSelectedId(null);
  }

  function openAdd() {
    clearForm();
    setEditing(false);
    setShowForm(true);
  }

  async function openEdit() {
    if (!selectedId) { alert(t(MessageKey.ErrorTitle)); return; }
    setLoading(true);
    try {
      const res = await fetch(`${API_BASE}/Get`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(selectedId) });
      const data = await res.json();
      await handleResponse<any>(data, (g) => {
        console.log('Loaded goal:', g);
        setTitle(g.title || '');
        setDescription(g.description || '');
        setDueDate(g.dueDate ? String(g.dueDate).split('T')[0] : '');
        setEditing(true);
        setShowForm(true);
      }, (m) => alert(m));
    } catch (e) {
      console.error(e);
      alert('Failed to load goal');
    } finally { setLoading(false); }
  }

  async function handleSave(e?: React.FormEvent) {
    e?.preventDefault();
    try {
      const payload = { title, description, dueDate };
      if (editing && selectedId) {
        await fetch(`${API_BASE}/Update`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify({ ...payload, id: selectedId }) });
      } else {
        await fetch(`${API_BASE}/Create`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(payload) });
      }
      setShowForm(false);
      clearForm();
      await loadGoals();
    } catch (e) {
      console.error(e);
      alert('Save failed');
    }
  }

  async function handleDelete() {
    if (!selectedId) { alert('Please select a goal to delete'); return; }
    if (!confirm('Are you sure?')) return;
    try {
      await fetch(`${API_BASE}/Delete`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(selectedId) });
      setSelectedId(null);
      await loadGoals();
    } catch (e) {
      console.error(e);
      alert('Delete failed');
    }
  }

  async function handleComplete() {
    if (!selectedId) { alert('Please select a goal to complete'); return; }
    try {
      await fetch(`${API_BASE}/Complete`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(selectedId) });
      await loadGoals();
    } catch (e) {
      console.error(e);
      alert('Complete failed');
    }
  }

  if (loading) return <p className="p-4">Loading...</p>;

  return (
    <div style={{ minHeight: '100vh', display: 'flex', alignItems: 'center', justifyContent: 'center', padding: 24 }}>
      <div style={{ width: '100%', maxWidth: 720, background: '#fff', borderRadius: 10, padding: 20, boxShadow: '0 6px 18px rgba(13,40,71,0.04)' }}>
        <h1 style={{ textAlign: 'center', margin: 0, marginBottom: 16 }}>{t(MessageKey.GoalListTitle)}</h1>

        <div style={{ display: 'flex', flexDirection: 'column', gap: 12 }}>
          {goals.map((g: any) => (
            <div key={g.id}
                 onClick={() => setSelectedId(g.id)}
                 style={{
                   padding: 12,
                   borderRadius: 8,
                   border: selectedId === g.id ? '2px solid #2563eb' : '1px solid #e5e7eb',
                   display: 'flex',
                   justifyContent: 'space-between',
                   alignItems: 'center',
                   cursor: 'pointer',
                   background: '#fff',
                   textDecoration: g.isCompleted || g.completed ? 'line-through' : 'none',
                   opacity: g.isCompleted || g.completed ? 0.6 : 1
                 }}>
              <div style={{ minWidth: 0 }}>
                <div style={{ fontWeight: 600 }}>{g.title}</div>
                <div style={{ fontSize: 13, color: '#6b7280' }}>{g.dueDate ? `Due: ${g.dueDate}` : ''}</div>
              </div>
              <div>
                {(g.isCompleted || g.completed) ? <span style={{ background: '#16a34a', color: '#fff', padding: '6px 8px', borderRadius: 999 }}>Completed</span> : null}
              </div>
            </div>
          ))}
        </div>

        <div style={{ display: 'flex', gap: 8, justifyContent: 'center', marginTop: 18 }}>
          <button onClick={openAdd} style={{ padding: '10px 16px', borderRadius: 8, background: '#2563eb', color: '#fff' }}>{t(MessageKey.Add)}</button>
          <button onClick={openEdit} style={{ padding: '10px 16px', borderRadius: 8, background: '#6b7280', color: '#fff' }}>{t(MessageKey.Edit)}</button>
          <button onClick={handleDelete} style={{ padding: '10px 16px', borderRadius: 8, background: '#ef4444', color: '#fff' }}>{t(MessageKey.Delete)}</button>
          <button onClick={handleComplete} style={{ padding: '10px 16px', borderRadius: 8, background: '#10b981', color: '#fff' }}>{t(MessageKey.Complete)}</button>
        </div>

        {showForm && (
          <form onSubmit={(e) => handleSave(e)} style={{ marginTop: 18 }}>
            <div style={{ display: 'grid', gap: 10 }}>
              <input required placeholder={t(MessageKey.Title)} value={title} onChange={(e) => setTitle(e.target.value)} style={{ padding: 10, borderRadius: 8, border: '1px solid #e5e7eb' }} />
              <textarea placeholder={t(MessageKey.Description)} value={description} onChange={(e) => setDescription(e.target.value)} style={{ padding: 10, borderRadius: 8, border: '1px solid #e5e7eb' }} />
              <div style={{ display: 'flex', gap: 8 }}>
                <input type="date" value={dueDate} onChange={(e) => setDueDate(e.target.value)} style={{ padding: 8, borderRadius: 8, border: '1px solid #e5e7eb', flex: 1 }} />
              </div>
              <div style={{ display: 'flex', gap: 8, justifyContent: 'flex-end' }}>
                <button type="button" onClick={() => { setShowForm(false); clearForm(); }} style={{ padding: '8px 12px', borderRadius: 8 }}>{t(MessageKey.Cancel)}</button>
                <button type="submit" style={{ padding: '8px 12px', borderRadius: 8, background: '#2563eb', color: '#fff' }}>{t(MessageKey.Save)}</button>
              </div>
            </div>
          </form>
        )}
      </div>
    </div>
  );
}

import { TranslationProvider } from "@/context/translationContext";

export default function GoalsPageWithProvider() {
  return (
    <TranslationProvider>
      <GoalsPageContent />
    </TranslationProvider>
  );
}
