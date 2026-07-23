import { createPlanLevelApi } from "./plan-level-api-factory";

export const yearlyPlanApi = createPlanLevelApi("yearlyplan", "goalId");
export const monthlyPlanApi = createPlanLevelApi("monthlyplan", "yearlyPlanId");
export const weeklyPlanApi = createPlanLevelApi("weeklyplan", "monthlyPlanId");
export const dailyPlanApi = createPlanLevelApi("dailyplan", "weeklyPlanId");
