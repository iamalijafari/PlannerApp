import { apiRequest } from "@/services/api-client";
import { ResponseModel } from "@/types/response-model";
import { GoalsProgressReportModel } from "../types/goals-progress-report-model";

export function getGoalsProgress(): Promise<
  ResponseModel<GoalsProgressReportModel>
> {
  return apiRequest<GoalsProgressReportModel>("/report/goals-progress");
}
