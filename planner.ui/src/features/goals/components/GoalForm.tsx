"use client";

import { FormEvent, useEffect, useState } from "react";
import DatePicker, { todayIso } from "@/components/DatePicker";
import { useTranslation } from "@/context/translation-context";
import { MessageKey } from "@/types/message-key";
import { CreateGoalRequestModel } from "../types/create-goal-request-model";

interface GoalFormProps {
  heading: string;
  initialValue?: CreateGoalRequestModel;
  onSubmit: (model: CreateGoalRequestModel) => Promise<boolean>;
  onCancel?: () => void;
  resetOnSuccess?: boolean;
}

const emptyGoal = (): CreateGoalRequestModel => ({
  title: "",
  description: "",
  dueDate: todayIso(),
});

export default function GoalForm({
  heading,
  initialValue,
  onSubmit,
  onCancel,
  resetOnSuccess = false,
}: GoalFormProps) {
  const { t } = useTranslation();
  const [model, setModel] = useState<CreateGoalRequestModel>(
    initialValue ?? emptyGoal,
  );
  const [isSaving, setIsSaving] = useState(false);

  useEffect(() => {
    if (initialValue) setModel(initialValue);
  }, [initialValue]);

  const submitHandler = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setIsSaving(true);

    try {
      const wasSaved = await onSubmit({
        ...model,
        title: model.title.trim(),
        description: model.description.trim(),
      });
      if (wasSaved && resetOnSuccess) setModel(emptyGoal());
    } finally {
      setIsSaving(false);
    }
  };

  return (
    <form onSubmit={submitHandler} className="card grid gap-4">
      <h2 className="text-xl font-bold">{heading}</h2>

      <label>
        {t(MessageKey.Title)}
        <input
          className="mt-1 w-full"
          value={model.title}
          onChange={(event) =>
            setModel((current) => ({ ...current, title: event.target.value }))
          }
          required
        />
      </label>

      <label>
        {t(MessageKey.Description)}
        <textarea
          className="mt-1 w-full"
          value={model.description}
          onChange={(event) =>
            setModel((current) => ({
              ...current,
              description: event.target.value,
            }))
          }
        />
      </label>

      <fieldset>
        <legend className="mb-1 text-sm font-medium text-zinc-600 dark:text-zinc-400">
          {t(MessageKey.DueDate)}
        </legend>
        <DatePicker
          value={model.dueDate}
          onChange={(dueDate) =>
            setModel((current) => ({ ...current, dueDate }))
          }
        />
      </fieldset>

      <div className="flex justify-end gap-2">
        {onCancel && (
          <button type="button" className="muted-btn" onClick={onCancel}>
            {t(MessageKey.Cancel)}
          </button>
        )}
        <button
          type="submit"
          className="btn"
          disabled={isSaving || !model.title.trim() || !model.dueDate}
        >
          {t(MessageKey.Save)}
        </button>
      </div>
    </form>
  );
}
