export interface TreeNodeBase {
  id: string;
  title: string;
  description: string;
  createdAt: string;
  dueDate: string;
  isCompleted: boolean;
}

export interface DailyGoalTreeModel extends TreeNodeBase {}

export interface WeeklyGoalTreeModel extends TreeNodeBase {
  dailyGoals: DailyGoalTreeModel[];
}

export interface MonthlyGoalTreeModel extends TreeNodeBase {
  weeklyGoals: WeeklyGoalTreeModel[];
}

export interface YearlyGoalTreeModel extends TreeNodeBase {
  monthlyGoals: MonthlyGoalTreeModel[];
}

export interface GoalTreeModel extends TreeNodeBase {
  yearlyGoals: YearlyGoalTreeModel[];
}