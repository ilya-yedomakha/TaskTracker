using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tasktracker_3.Migrations
{
    /// <inheritdoc />
    public partial class SelfReferencingTask1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskSelfRelation",
                columns: table => new
                {
                    ChildId = table.Column<long>(type: "bigint", nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskSelfRelation", x => new { x.ChildId, x.ParentId });
                    table.ForeignKey(
                        name: "FK_TaskSelfRelation_tasks_ChildId",
                        column: x => x.ChildId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskSelfRelation_tasks_ParentId",
                        column: x => x.ParentId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskSelfRelation_ParentId",
                table: "TaskSelfRelation",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskSelfRelation");
        }
    }
}
