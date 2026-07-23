"use client";

import { useEffect, useMemo, useState } from "react";
import DatePicker, { todayIso } from "@/components/DatePicker";
import { useLanguage } from "@/context/language-context";
import { useTranslation } from "@/context/translation-context";
import { Language } from "@/types/language";
import { MessageKey } from "@/types/message-key";
import { ResponseModel } from "@/types/response-model";
import {
  dailyGoalApi,
  monthlyGoalApi,
  weeklyGoalApi,
  yearlyGoalApi,
} from "../api/goal-level-apis";
import { GoalLevelApi } from "../api/goal-level-api-factory";
import { GoalLevel, GoalTreeItem } from "../types/goal-tree-model";

interface LevelConfig {
  api: GoalLevelApi;
  labelKey: MessageKey;
  childLevel: GoalLevel | null;
}

const LEVEL_CONFIG: Record<GoalLevel, LevelConfig> = {
  yearly: {
    api: yearlyGoalApi,
    labelKey: MessageKey.Yearly,
    childLevel: "monthly",
  },
  monthly: {
    api: monthlyGoalApi,
    labelKey: MessageKey.Monthly,
    childLevel: "weekly",
  },
  weekly: {
    api: weeklyGoalApi,
    labelKey: MessageKey.Weekly,
    childLevel: "daily",
  },
  daily: {
    api: dailyGoalApi,
    labelKey: MessageKey.Daily,
    childLevel: null,
  },
};

interface GoalTreeNodeProps {
  node: GoalTreeItem;
  onChanged: () => Promise<void>;
  onError: (messageKey: MessageKey) => void;
}

export default function GoalTreeNode({
  node,
  onChanged,
  onError,
}: GoalTreeNodeProps) {
  const { language } = useLanguage();
  const { t } = useTranslation();
  const config = LEVEL_CONFIG[node.level];
  const childConfig = config.childLevel
    ? LEVEL_CONFIG[config.childLevel]
    : null;

  const [isEditing, setIsEditing] = useState(false);
  const [isAddingChild, setIsAddingChild] = useState(false);
  const [title, setTitle] = useState(node.title);
  const [description, setDescription] = useState(node.description);
  const [dueDate, setDueDate] = useState(node.dueDate.slice(0, 10));
  const [childTitle, setChildTitle] = useState("");
  const [childDescription, setChildDescription] = useState("");
  const [childDueDate, setChildDueDate] = useState(todayIso);
  const [isBusy, setIsBusy] = useState(false);

  useEffect(() => {
    if (isEditing) return;
    setTitle(node.title);
    setDescription(node.description);
    setDueDate(node.dueDate.slice(0, 10));
  }, [isEditing, node.description, node.dueDate, node.title]);

  const dateFormatter = useMemo(
    () =>
      new Intl.DateTimeFormat(
        language === Language.fa ? "fa-IR-u-ca-persian" : "en",
        { dateStyle: "medium" },
      ),
    [language],
  );

  const runMutation = async (
    action: () => Promise<ResponseModel<unknown>>,
  ) => {
    setIsBusy(true);
    try {
      const response = await action();
      if (!response.success) {
        onError(response.messageKey);
        return false;
      }
      await onChanged();
      return true;
    } catch (error) {
      console.error(`Failed to update ${node.level} goal ${node.id}:`, error);
      onError(MessageKey.ServerError);
      return false;
    } finally {
      setIsBusy(false);
    }
  };

  const handleSave = async () => {
    if (!title.trim() || !dueDate) return;
    const wasSaved = await runMutation(() =>
      config.api.update({
        id: node.id,
        title: title.trim(),
        description: description.trim(),
        dueDate,
        isCompleted: node.isCompleted,
      }),
    );
    if (wasSaved) setIsEditing(false);
  };

  const handleDelete = async () => {
    const messageKey =
      node.children.length > 0
        ? MessageKey.DeleteTreeItemWithChildrenConfirm
        : MessageKey.DeleteTreeItemConfirm;
    const confirmed = window.confirm(
      t(messageKey, {
        title: node.title,
        count: node.children.length,
      }),
    );
    if (!confirmed) return;
    await runMutation(() => config.api.remove(node.id));
  };

  const handleAddChild = async () => {
    if (!childConfig || !childTitle.trim() || !childDueDate) return;
    const wasAdded = await runMutation(() =>
      childConfig.api.create({
        parentId: node.id,
        title: childTitle.trim(),
        description: childDescription.trim(),
        dueDate: childDueDate,
      }),
    );

    if (wasAdded) {
      setChildTitle("");
      setChildDescription("");
      setChildDueDate(todayIso());
      setIsAddingChild(false);
    }
  };

  return (
    <li className="tree-node">
      <div className="tree-node-content">
        {isEditing ? (
          <div className="tree-form">
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
                disabled={isBusy || !title.trim() || !dueDate}
                onClick={() => void handleSave()}
              >
                {t(MessageKey.Save)}
              </button>
              <button
                className="muted-btn text-sm"
                disabled={isBusy}
                onClick={() => setIsEditing(false)}
              >
                {t(MessageKey.Cancel)}
              </button>
            </div>
          </div>
        ) : (
          <>
            <div className="min-w-0 flex-1">
              <div className="flex flex-wrap items-center gap-2">
                <span className={`level-badge level-${node.level}`}>
                  {t(config.labelKey)}
                </span>
                <h3
                  className={
                    node.isCompleted
                      ? "font-semibold text-zinc-400 line-through"
                      : "font-semibold"
                  }
                >
                  {node.title}
                </h3>
              </div>
              {node.description && (
                <p className="mt-2 text-sm text-zinc-600 dark:text-zinc-300">
                  {node.description}
                </p>
              )}
              <p className="mt-2 text-xs text-zinc-500">
                {t(MessageKey.DueDate)}:{" "}
                {dateFormatter.format(new Date(node.dueDate))}
              </p>
            </div>

            <div className="tree-actions">
              {!node.isCompleted && (
                <button
                  className="muted-btn text-xs"
                  disabled={isBusy}
                  onClick={() =>
                    void runMutation(() => config.api.complete(node.id))
                  }
                >
                  {t(MessageKey.Complete)}
                </button>
              )}
              <button
                className="muted-btn text-xs"
                disabled={isBusy}
                onClick={() => setIsEditing(true)}
              >
                {t(MessageKey.Edit)}
              </button>
              <button
                className="danger-btn text-xs"
                disabled={isBusy}
                onClick={() => void handleDelete()}
              >
                {t(MessageKey.Delete)}
              </button>
              {childConfig && (
                <button
                  className="muted-btn text-xs"
                  disabled={isBusy}
                  onClick={() => setIsAddingChild((current) => !current)}
                >
                  {t(MessageKey.Add)} {t(childConfig.labelKey)}
                </button>
              )}
            </div>
          </>
        )}
      </div>

      {isAddingChild && childConfig && (
        <div className="tree-form ms-5 mt-3">
          <label>
            {t(MessageKey.Title)}
            <input
              className="mt-1 w-full"
              value={childTitle}
              onChange={(event) => setChildTitle(event.target.value)}
            />
          </label>
          <label>
            {t(MessageKey.Description)}
            <textarea
              className="mt-1 w-full"
              value={childDescription}
              onChange={(event) => setChildDescription(event.target.value)}
            />
          </label>
          <fieldset>
            <legend>{t(MessageKey.DueDate)}</legend>
            <DatePicker value={childDueDate} onChange={setChildDueDate} />
          </fieldset>
          <div className="flex gap-2">
            <button
              className="btn text-sm"
              disabled={isBusy || !childTitle.trim() || !childDueDate}
              onClick={() => void handleAddChild()}
            >
              {t(MessageKey.Add)}
            </button>
            <button
              className="muted-btn text-sm"
              disabled={isBusy}
              onClick={() => setIsAddingChild(false)}
            >
              {t(MessageKey.Cancel)}
            </button>
          </div>
        </div>
      )}

      {node.children.length > 0 && (
        <ul className="tree-children">
          {node.children.map((child) => (
            <GoalTreeNode
              key={child.id}
              node={child}
              onChanged={onChanged}
              onError={onError}
            />
          ))}
        </ul>
      )}
    </li>
  );
}
