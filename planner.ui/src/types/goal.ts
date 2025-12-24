export interface YearlyGoal {
  id?: string;
  year: number;
  title: string;
  target?: string;
  dueDate?: string;
  completed?: boolean;
}

export interface Goal {
  id?: string;
  title: string;
  description?: string;
  startDate?: string;
  dueDate?: string;
  completed?: boolean;
  yearlyGoals?: YearlyGoal[];
}

export default Goal;
