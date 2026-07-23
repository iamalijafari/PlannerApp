export interface TreeNodeBase {
  id: string;
  title: string;
  description: string;
  createdAt: string;
  dueDate: string;
  isCompleted: boolean;
}

export type DailyPlanTreeModel = TreeNodeBase;

export interface WeeklyPlanTreeModel extends TreeNodeBase {
  dailyPlans: DailyPlanTreeModel[];
}

export interface MonthlyPlanTreeModel extends TreeNodeBase {
  weeklyPlans: WeeklyPlanTreeModel[];
}

export interface YearlyPlanTreeModel extends TreeNodeBase {
  monthlyPlans: MonthlyPlanTreeModel[];
}

export interface GoalTreeModel extends TreeNodeBase {
  yearlyPlans: YearlyPlanTreeModel[];
}

export type PlanLevel = "yearly" | "monthly" | "weekly" | "daily";

export interface PlanTreeItem extends TreeNodeBase {
  level: PlanLevel;
  children: PlanTreeItem[];
}

function dailyToItem(plan: DailyPlanTreeModel): PlanTreeItem {
  return { ...plan, level: "daily", children: [] };
}

function weeklyToItem(plan: WeeklyPlanTreeModel): PlanTreeItem {
  return {
    ...plan,
    level: "weekly",
    children: plan.dailyPlans.map(dailyToItem),
  };
}

function monthlyToItem(plan: MonthlyPlanTreeModel): PlanTreeItem {
  return {
    ...plan,
    level: "monthly",
    children: plan.weeklyPlans.map(weeklyToItem),
  };
}

function yearlyToItem(plan: YearlyPlanTreeModel): PlanTreeItem {
  return {
    ...plan,
    level: "yearly",
    children: plan.monthlyPlans.map(monthlyToItem),
  };
}

export function getTreeItems(tree: GoalTreeModel): PlanTreeItem[] {
  return tree.yearlyPlans.map(yearlyToItem);
}
