import { createGoalLevelApi } from "./goal-level-api-factory";

export const yearlyGoalApi = createGoalLevelApi("yearlygoal", "goalId");
export const monthlyGoalApi = createGoalLevelApi("monthlygoal", "yearlyGoalId");
export const weeklyGoalApi = createGoalLevelApi("weeklygoal", "monthlyGoalId");
export const dailyGoalApi = createGoalLevelApi("dailygoal", "weeklyGoalId");