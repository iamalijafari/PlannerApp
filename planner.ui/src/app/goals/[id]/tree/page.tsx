"use client";

import { useParams } from "next/navigation";
import GoalTree from "@/features/goals/components/GoalTree";

export default function GoalTreePage() {
  const { id } = useParams<{ id: string }>();
  return (
    <div className="app-container">
      <GoalTree goalId={id} />
    </div>
  );
}
