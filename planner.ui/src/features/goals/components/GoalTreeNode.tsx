"use client";

import { useState } from "react";

interface TreeNode {
  id: string;
  title: string;
  description: string;
  dueDate: string;
  isCompleted: boolean;
  [key: string]: any;
}

interface LevelApi {
  create: (req: { parentId: string; title: string; description: string; dueDate: string }) => Promise<any>;
  update: (req: { id: string; title: string; description: string; dueDate: string; isCompleted: boolean }) => Promise<any>;
  remove: (id: string) => Promise<any>;
  complete: (id: string) => Promise<any>;
}

export interface LevelConfig {
  api: LevelApi;
  label: string;
  childField: string | null;
}

interface GoalTreeNodeProps {
  node: TreeNode;
  depth: number;
  levels: LevelConfig[];
  onChanged: () => void;
}

export default function GoalTreeNode({ node, depth, levels, onChanged }: GoalTreeNodeProps) {
  const level = levels[depth];
  const childLevel = levels[depth + 1] ?? null;

  const [editing, setEditing] = useState(false);
  const [addingChild, setAddingChild] = useState(false);
  const [title, setTitle] = useState(node.title);
  const [description, setDescription] = useState(node.description);
  const [dueDate, setDueDate] = useState(node.dueDate?.slice(0, 10) ?? "");
  const [childTitle, setChildTitle] = useState("");
  const [childDescription, setChildDescription] = useState("");
  const [childDueDate, setChildDueDate] = useState("");
  const [busy, setBusy] = useState(false);

  const children: TreeNode[] = level.childField ? (node[level.childField] ?? []) : [];

  async function handleSave() {
    setBusy(true);
    try {
      await level.api.update({ id: node.id, title, description, dueDate, isCompleted: node.isCompleted });
      setEditing(false);
      onChanged();
    } finally {
      setBusy(false);
    }
  }

  async function handleComplete() {
    setBusy(true);
    try {
      await level.api.complete(node.id);
      onChanged();
    } finally {
      setBusy(false);
    }
  }

  async function handleDelete() {
    const message =
      children.length > 0
        ? `Delete "${node.title}" and its ${children.length} sub-item(s)? This cannot be undone.`
        : `Delete "${node.title}"?`;
    if (!confirm(message)) return;

    setBusy(true);
    try {
      await level.api.remove(node.id);
      onChanged();
    } finally {
      setBusy(false);
    }
  }

  async function handleAddChild() {
    if (!childLevel || !childTitle.trim()) return;
    setBusy(true);
    try {
      await childLevel.api.create({
        parentId: node.id,
        title: childTitle,
        description: childDescription,
        dueDate: childDueDate,
      });
      setChildTitle("");
      setChildDescription("");
      setChildDueDate("");
      setAddingChild(false);
      onChanged();
    } finally {
      setBusy(false);
    }
  }

  return (
    <div style={{ marginLeft: depth * 20 }} className="border-l border-zinc-200 dark:border-zinc-800 pl-4 my-2">
      {editing ? (
        <div className="flex flex-col gap-2 mb-2 max-w-sm">
          <input value={title} onChange={(e) => setTitle(e.target.value)} />
          <textarea value={description} onChange={(e) => setDescription(e.target.value)} />
          <input type="date" value={dueDate} onChange={(e) => setDueDate(e.target.value)} />
          <div className="flex gap-2">
            <button className="btn text-sm" disabled={busy} onClick={handleSave}>Save</button>
            <button className="muted-btn text-sm" onClick={() => setEditing(false)}>Cancel</button>
          </div>
        </div>
      ) : (
        <div className="flex items-center gap-2 flex-wrap">
          <span className="text-xs uppercase text-zinc-400">{level.label}</span>
          <span className={node.isCompleted ? "line-through text-zinc-400" : "font-medium"}>{node.title}</span>
          {!node.isCompleted && (
            <button className="muted-btn text-xs" disabled={busy} onClick={handleComplete}>Complete</button>
          )}
          <button className="muted-btn text-xs" onClick={() => setEditing(true)}>Edit</button>
          <button className="muted-btn text-xs" disabled={busy} onClick={handleDelete}>Delete</button>
          {childLevel && (
            <button className="muted-btn text-xs" onClick={() => setAddingChild((v) => !v)}>
              + Add {childLevel.label}
            </button>
          )}
        </div>
      )}

      {addingChild && childLevel && (
        <div className="flex flex-col gap-2 mt-2 mb-2 max-w-sm">
          <input placeholder={`${childLevel.label} title`} value={childTitle} onChange={(e) => setChildTitle(e.target.value)} />
          <textarea placeholder="Description" value={childDescription} onChange={(e) => setChildDescription(e.target.value)} />
          <input type="date" value={childDueDate} onChange={(e) => setChildDueDate(e.target.value)} />
          <div className="flex gap-2">
            <button className="btn text-sm" disabled={busy || !childTitle.trim()} onClick={handleAddChild}>Add</button>
            <button className="muted-btn text-sm" onClick={() => setAddingChild(false)}>Cancel</button>
          </div>
        </div>
      )}

      {children.map((child) => (
        <GoalTreeNode key={child.id} node={child} depth={depth + 1} levels={levels} onChanged={onChanged} />
      ))}
    </div>
  );
}