"use client";

import Link from "next/link";
import { useCallback, useEffect, useState } from "react";
import DatePicker, { todayIso } from "@/components/DatePicker";
import Modal from "@/components/Modal";
import { useTranslation } from "@/context/translation-context";
import { MessageKey } from "@/types/message-key";
import { yearlyGoalApi } from "../api/goal-level-apis";
import { getGoalTree } from "../api/goal-tree-api";
import {
  getTreeItems,
  GoalTreeModel,
} from "../types/goal-tree-model";
import GoalTreeNode from "./GoalTreeNode";

interface GoalTreeProps {
  goalId: string;
}

export default function GoalTree({ goalId }: GoalTreeProps) {
  const { t } = useTranslation();
  const [tree, setTree] = useState<GoalTreeModel | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isAddingYearly, setIsAddingYearly] = useState(false);
  const [isSaving, setIsSaving] = useState(false);
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [dueDate, setDueDate] = useState(todayIso);
  const [errorKey, setErrorKey] = useState<MessageKey | null>(null);

  const loadTree = useCallback(async () => {
    try {
      const response = await getGoalTree(goalId);
      if (response.success) {
        setTree(response.result);
      } else {
        setTree(null);
        setErrorKey(response.messageKey);
      }
    } catch (error) {
      console.error(`Failed to load goal tree ${goalId}:`, error);
      setTree(null);
      setErrorKey(MessageKey.ServerError);
    } finally {
      setIsLoading(false);
    }
  }, [goalId]);

  useEffect(() => {
    void loadTree();
  }, [loadTree]);

  const handleAddYearly = async () => {
    if (!title.trim() || !dueDate) return;
    setIsSaving(true);

    try {
      const response = await yearlyGoalApi.create({
        parentId: goalId,
        title: title.trim(),
        description: description.trim(),
        dueDate,
      });
      if (!response.success) {
        setErrorKey(response.messageKey);
        return;
      }

      setTitle("");
      setDescription("");
      setDueDate(todayIso());
      setIsAddingYearly(false);
      await loadTree();
    } catch (error) {
      console.error(`Failed to add yearly goal to ${goalId}:`, error);
      setErrorKey(MessageKey.ServerError);
    } finally {
      setIsSaving(false);
    }
  };

  if (isLoading) {
    return <p className="status-message">{t(MessageKey.Loading)}</p>;
  }

  return (
    <>
      <section className="card">
        <div className="mb-6 flex flex-wrap items-center justify-between gap-3">
          <div>
            <p className="text-sm font-medium text-sky-600">
              {t(MessageKey.GoalTreeTitle)}
            </p>
            <h1 className="mt-1 text-2xl font-bold">
              {tree?.title ?? t(MessageKey.Goal_NotFound)}
            </h1>
          </div>
          <Link href="/goals" className="muted-btn text-sm">
            {t(MessageKey.Back)}
          </Link>
        </div>

        {tree && (
          <>
            <button
              className="btn mb-5 text-sm"
              onClick={() => setIsAddingYearly((current) => !current)}
            >
              {t(MessageKey.Add)} {t(MessageKey.Yearly)}
            </button>

            {isAddingYearly && (
              <div className="tree-form mb-6">
                <label>
                  {t(MessageKey.Title)}
                  <input
                    className="mt-1 w-full"
                    value={title}
                    onChange={(event) => setTitle(event.target.value)}
                  />
                </label>
                <label>
                  {t(MessageKey.Description)}
                  <textarea
                    className="mt-1 w-full"
                    value={description}
                    onChange={(event) => setDescription(event.target.value)}
                  />
                </label>
                <fieldset>
                  <legend>{t(MessageKey.DueDate)}</legend>
                  <DatePicker value={dueDate} onChange={setDueDate} />
                </fieldset>
                <div className="flex gap-2">
                  <button
                    className="btn text-sm"
                    disabled={isSaving || !title.trim() || !dueDate}
                    onClick={() => void handleAddYearly()}
                  >
                    {t(MessageKey.Add)}
                  </button>
                  <button
                    className="muted-btn text-sm"
                    disabled={isSaving}
                    onClick={() => setIsAddingYearly(false)}
                  >
                    {t(MessageKey.Cancel)}
                  </button>
                </div>
              </div>
            )}

            {tree.yearlyGoals.length === 0 ? (
              <div className="empty-state">{t(MessageKey.NoYearlyGoals)}</div>
            ) : (
              <ul className="goal-tree">
                {getTreeItems(tree).map((item) => (
                  <GoalTreeNode
                    key={item.id}
                    node={item}
                    onChanged={loadTree}
                    onError={setErrorKey}
                  />
                ))}
              </ul>
            )}
          </>
        )}
      </section>

      <Modal
        isOpen={errorKey !== null}
        onClose={() => setErrorKey(null)}
        title={t(MessageKey.ErrorTitle)}
        message={errorKey === null ? "" : t(errorKey)}
        closeLabel={t(MessageKey.Close)}
      />
    </>
  );
}
