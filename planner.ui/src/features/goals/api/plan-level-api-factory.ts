import { apiRequest } from "@/services/api-client";
import { ResponseModel } from "@/types/response-model";

export interface CreatePlanRequest {
  parentId: string;
  title: string;
  description: string;
  dueDate: string;
}

export interface UpdatePlanRequest {
  id: string;
  title: string;
  description: string;
  dueDate: string;
  isCompleted: boolean;
}

export interface PlanLevelApi {
  create: (request: CreatePlanRequest) => Promise<ResponseModel<unknown>>;
  update: (request: UpdatePlanRequest) => Promise<ResponseModel<boolean>>;
  remove: (id: string) => Promise<ResponseModel<boolean>>;
  complete: (id: string) => Promise<ResponseModel<boolean>>;
}

export function createPlanLevelApi(
  endpoint: string,
  parentIdField: string,
): PlanLevelApi {
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
