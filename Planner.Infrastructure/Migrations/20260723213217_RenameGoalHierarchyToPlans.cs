using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameGoalHierarchyToPlans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyGoals_WeeklyGoals_WeeklyGoalId",
                table: "DailyGoals");

            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyGoals_YearlyGoals_YearlyGoalId",
                table: "MonthlyGoals");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklyGoals_MonthlyGoals_MonthlyGoalId",
                table: "WeeklyGoals");

            migrationBuilder.DropForeignKey(
                name: "FK_YearlyGoals_Goals_GoalId",
                table: "YearlyGoals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DailyGoals",
                table: "DailyGoals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MonthlyGoals",
                table: "MonthlyGoals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WeeklyGoals",
                table: "WeeklyGoals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_YearlyGoals",
                table: "YearlyGoals");

            migrationBuilder.RenameTable(
                name: "DailyGoals",
                newName: "DailyPlans");

            migrationBuilder.RenameTable(
                name: "MonthlyGoals",
                newName: "MonthlyPlans");

            migrationBuilder.RenameTable(
                name: "WeeklyGoals",
                newName: "WeeklyPlans");

            migrationBuilder.RenameTable(
                name: "YearlyGoals",
                newName: "YearlyPlans");

            migrationBuilder.RenameColumn(
                name: "WeeklyGoalId",
                table: "DailyPlans",
                newName: "WeeklyPlanId");

            migrationBuilder.RenameColumn(
                name: "YearlyGoalId",
                table: "MonthlyPlans",
                newName: "YearlyPlanId");

            migrationBuilder.RenameColumn(
                name: "MonthlyGoalId",
                table: "WeeklyPlans",
                newName: "MonthlyPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_DailyGoals_WeeklyGoalId",
                table: "DailyPlans",
                newName: "IX_DailyPlans_WeeklyPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_MonthlyGoals_YearlyGoalId",
                table: "MonthlyPlans",
                newName: "IX_MonthlyPlans_YearlyPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_WeeklyGoals_MonthlyGoalId",
                table: "WeeklyPlans",
                newName: "IX_WeeklyPlans_MonthlyPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_YearlyGoals_GoalId",
                table: "YearlyPlans",
                newName: "IX_YearlyPlans_GoalId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DailyPlans",
                table: "DailyPlans",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MonthlyPlans",
                table: "MonthlyPlans",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WeeklyPlans",
                table: "WeeklyPlans",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_YearlyPlans",
                table: "YearlyPlans",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyPlans_WeeklyPlans_WeeklyPlanId",
                table: "DailyPlans",
                column: "WeeklyPlanId",
                principalTable: "WeeklyPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyPlans_YearlyPlans_YearlyPlanId",
                table: "MonthlyPlans",
                column: "YearlyPlanId",
                principalTable: "YearlyPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklyPlans_MonthlyPlans_MonthlyPlanId",
                table: "WeeklyPlans",
                column: "MonthlyPlanId",
                principalTable: "MonthlyPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_YearlyPlans_Goals_GoalId",
                table: "YearlyPlans",
                column: "GoalId",
                principalTable: "Goals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyPlans_WeeklyPlans_WeeklyPlanId",
                table: "DailyPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyPlans_YearlyPlans_YearlyPlanId",
                table: "MonthlyPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklyPlans_MonthlyPlans_MonthlyPlanId",
                table: "WeeklyPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_YearlyPlans_Goals_GoalId",
                table: "YearlyPlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DailyPlans",
                table: "DailyPlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MonthlyPlans",
                table: "MonthlyPlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WeeklyPlans",
                table: "WeeklyPlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_YearlyPlans",
                table: "YearlyPlans");

            migrationBuilder.RenameTable(
                name: "DailyPlans",
                newName: "DailyGoals");

            migrationBuilder.RenameTable(
                name: "MonthlyPlans",
                newName: "MonthlyGoals");

            migrationBuilder.RenameTable(
                name: "WeeklyPlans",
                newName: "WeeklyGoals");

            migrationBuilder.RenameTable(
                name: "YearlyPlans",
                newName: "YearlyGoals");

            migrationBuilder.RenameColumn(
                name: "WeeklyPlanId",
                table: "DailyGoals",
                newName: "WeeklyGoalId");

            migrationBuilder.RenameColumn(
                name: "YearlyPlanId",
                table: "MonthlyGoals",
                newName: "YearlyGoalId");

            migrationBuilder.RenameColumn(
                name: "MonthlyPlanId",
                table: "WeeklyGoals",
                newName: "MonthlyGoalId");

            migrationBuilder.RenameIndex(
                name: "IX_DailyPlans_WeeklyPlanId",
                table: "DailyGoals",
                newName: "IX_DailyGoals_WeeklyGoalId");

            migrationBuilder.RenameIndex(
                name: "IX_MonthlyPlans_YearlyPlanId",
                table: "MonthlyGoals",
                newName: "IX_MonthlyGoals_YearlyGoalId");

            migrationBuilder.RenameIndex(
                name: "IX_WeeklyPlans_MonthlyPlanId",
                table: "WeeklyGoals",
                newName: "IX_WeeklyGoals_MonthlyGoalId");

            migrationBuilder.RenameIndex(
                name: "IX_YearlyPlans_GoalId",
                table: "YearlyGoals",
                newName: "IX_YearlyGoals_GoalId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DailyGoals",
                table: "DailyGoals",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MonthlyGoals",
                table: "MonthlyGoals",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WeeklyGoals",
                table: "WeeklyGoals",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_YearlyGoals",
                table: "YearlyGoals",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyGoals_WeeklyGoals_WeeklyGoalId",
                table: "DailyGoals",
                column: "WeeklyGoalId",
                principalTable: "WeeklyGoals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyGoals_YearlyGoals_YearlyGoalId",
                table: "MonthlyGoals",
                column: "YearlyGoalId",
                principalTable: "YearlyGoals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklyGoals_MonthlyGoals_MonthlyGoalId",
                table: "WeeklyGoals",
                column: "MonthlyGoalId",
                principalTable: "MonthlyGoals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_YearlyGoals_Goals_GoalId",
                table: "YearlyGoals",
                column: "GoalId",
                principalTable: "Goals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
