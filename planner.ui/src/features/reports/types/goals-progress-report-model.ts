export type GoalProgressStatus =
  | "planned"
  | "in-progress"
  | "completed"
  | "overdue";

export interface GoalProgressModel {
  id: string;
  title: string;
  description: string;
  dueDate: string;
  isCompleted: boolean;
  completedLeafPlans: number;
  totalLeafPlans: number;
  progressPercentage: number;
  status: GoalProgressStatus;
}

export interface GoalsProgressReportModel {
  totalGoals: number;
  activeGoals: number;
  completedGoals: number;
  overdueGoals: number;
  completedLeafPlans: number;
  totalLeafPlans: number;
  overallProgressPercentage: number;
  goals: GoalProgressModel[];
}
