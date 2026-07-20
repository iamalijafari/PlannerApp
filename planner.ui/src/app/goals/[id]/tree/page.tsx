"use client";

import { useParams } from "next/navigation";
import GoalTree from "@/src/features/goals/components/GoalTree";

export default function GoalTreePage() {
  const { id } = useParams<{ id: string }>();
  return (
    <div className="max-w-2xl mx-auto py-8">
      <GoalTree goalId={id} />
    </div>
  );
}