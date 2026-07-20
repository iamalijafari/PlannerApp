"use client";

import { ResponseModel } from "@/types/response-model";
import { MessageKey } from "@/types/message-key";

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5010/api";

export interface CreateLevelRequest {
  parentId: string;
  title: string;
  description: string;
  dueDate: string;
}

export interface UpdateLevelRequest {
  id: string;
  title: string;
  description: string;
  dueDate: string;
  isCompleted: boolean;
}

async function handleResponse<T>(res: Response): Promise<ResponseModel<T>> {
  if (!res.ok) throw new Error(`HTTP ${res.status}: ${res.statusText}`);
  return res.json();
}

export function createGoalLevelApi(endpoint: string, parentIdField: string) {
  const base = `${API_BASE_URL}/${endpoint}`;

  return {
    async create(req: CreateLevelRequest): Promise<ResponseModel<any>> {
      try {
        const body = {
          [parentIdField]: req.parentId,
          title: req.title,
          description: req.description,
          dueDate: req.dueDate,
        };
        const res = await fetch(base, {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(body),
        });
        return await handleResponse(res);
      } catch (error) {
        console.error(`Failed to create ${endpoint}:`, error);
        return { success: false, messageKey: MessageKey.Operation_Failed, result: null };
      }
    },

    async update(req: UpdateLevelRequest): Promise<ResponseModel<boolean>> {
      try {
        const res = await fetch(`${base}/${req.id}`, {
          method: "PUT",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(req),
        });
        return await handleResponse(res);
      } catch (error) {
        console.error(`Failed to update ${endpoint} ${req.id}:`, error);
        return { success: false, messageKey: MessageKey.Operation_Failed, result: false };
      }
    },

    async remove(id: string): Promise<ResponseModel<boolean>> {
      try {
        const res = await fetch(`${base}/${id}`, { method: "DELETE" });
        return await handleResponse(res);
      } catch (error) {
        console.error(`Failed to delete ${endpoint} ${id}:`, error);
        return { success: false, messageKey: MessageKey.Operation_Failed, result: false };
      }
    },

    async complete(id: string): Promise<ResponseModel<boolean>> {
      try {
        const res = await fetch(`${base}/${id}/complete`, { method: "PUT" });
        return await handleResponse(res);
      } catch (error) {
        console.error(`Failed to complete ${endpoint} ${id}:`, error);
        return { success: false, messageKey: MessageKey.Operation_Failed, result: false };
      }
    },
  };
}