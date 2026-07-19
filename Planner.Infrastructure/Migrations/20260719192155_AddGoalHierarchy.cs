using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGoalHierarchy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonthlyGoals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    YearlyGoalId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyGoals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyGoals_YearlyGoals_YearlyGoalId",
                        column: x => x.YearlyGoalId,
                        principalTable: "YearlyGoals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeeklyGoals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MonthlyGoalId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyGoals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeeklyGoals_MonthlyGoals_MonthlyGoalId",
                        column: x => x.MonthlyGoalId,
                        principalTable: "MonthlyGoals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyGoals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WeeklyGoalId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyGoals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyGoals_WeeklyGoals_WeeklyGoalId",
                        column: x => x.WeeklyGoalId,
                        principalTable: "WeeklyGoals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyGoals_WeeklyGoalId",
                table: "DailyGoals",
                column: "WeeklyGoalId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyGoals_YearlyGoalId",
                table: "MonthlyGoals",
                column: "YearlyGoalId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyGoals_MonthlyGoalId",
                table: "WeeklyGoals",
                column: "MonthlyGoalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyGoals");

            migrationBuilder.DropTable(
                name: "WeeklyGoals");

            migrationBuilder.DropTable(
                name: "MonthlyGoals");
        }
    }
}
