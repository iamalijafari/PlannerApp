import { apiRequest } from "@/services/api-client";
import { ResponseModel } from "@/types/response-model";

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

export interface GoalLevelApi {
  create: (request: CreateLevelRequest) => Promise<ResponseModel<unknown>>;
  update: (request: UpdateLevelRequest) => Promise<ResponseModel<boolean>>;
  remove: (id: string) => Promise<ResponseModel<boolean>>;
  complete: (id: string) => Promise<ResponseModel<boolean>>;
}

export function createGoalLevelApi(
  endpoint: string,
  parentIdField: string,
): GoalLevelApi {
  const basePath = `/${endpoint}`;

  return {
    create(request) {
      return apiRequest<unknown>(basePath, {
        method: "POST",
        body: JSON.stringify({
          [parentIdField]: request.parentId,
          title: request.title,
          description: request.description,
          dueDate: request.dueDate,
        }),
      });
    },

    update(request) {
      return apiRequest<boolean>(`${basePath}/${request.id}`, {
        method: "PUT",
        body: JSON.stringify(request),
      });
    },

    remove(id) {
      return apiRequest<boolean>(`${basePath}/${id}`, { method: "DELETE" });
    },

    complete(id) {
      return apiRequest<boolean>(`${basePath}/${id}/complete`, {
        method: "PUT",
      });
    },
  };
}
