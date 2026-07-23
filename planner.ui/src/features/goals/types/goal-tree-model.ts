export interface TreeNodeBase {
  id: string;
  title: string;
  description: string;
  createdAt: string;
  dueDate: string;
  isCompleted: boolean;
}

export type DailyGoalTreeModel = TreeNodeBase;

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

export type GoalLevel = "yearly" | "monthly" | "weekly" | "daily";

export interface GoalTreeItem extends TreeNodeBase {
  level: GoalLevel;
  children: GoalTreeItem[];
}

function dailyToItem(goal: DailyGoalTreeModel): GoalTreeItem {
  return { ...goal, level: "daily", children: [] };
}

function weeklyToItem(goal: WeeklyGoalTreeModel): GoalTreeItem {
  return {
    ...goal,
    level: "weekly",
    children: goal.dailyGoals.map(dailyToItem),
  };
}

function monthlyToItem(goal: MonthlyGoalTreeModel): GoalTreeItem {
  return {
    ...goal,
    level: "monthly",
    children: goal.weeklyGoals.map(weeklyToItem),
  };
}

function yearlyToItem(goal: YearlyGoalTreeModel): GoalTreeItem {
  return {
    ...goal,
    level: "yearly",
    children: goal.monthlyGoals.map(monthlyToItem),
  };
}

export function getTreeItems(tree: GoalTreeModel): GoalTreeItem[] {
  return tree.yearlyGoals.map(yearlyToItem);
}
