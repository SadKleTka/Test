using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SibersDataManager.Migrations
{
    /// <inheritdoc />
    public partial class AddedEntityes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SecondName = table.Column<string>(type: "text", nullable: false),
                    ThirdName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CustomerCompany = table.Column<string>(type: "text", nullable: false),
                    WorkerCompany = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    ManagerId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectEntity_EmployeeEntity_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "EmployeeEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeEntityProjectEntity",
                columns: table => new
                {
                    EmployeesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEntityProjectEntity", x => new { x.EmployeesId, x.ProjectsId });
                    table.ForeignKey(
                        name: "FK_EmployeeEntityProjectEntity_EmployeeEntity_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "EmployeeEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeEntityProjectEntity_ProjectEntity_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "ProjectEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTaskEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkerId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTaskEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTaskEntity_EmployeeEntity_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "EmployeeEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectTaskEntity_EmployeeEntity_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "EmployeeEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectTaskEntity_ProjectEntity_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "ProjectEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEntityProjectEntity_ProjectsId",
                table: "EmployeeEntityProjectEntity",
                column: "ProjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectEntity_ManagerId",
                table: "ProjectEntity",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTaskEntity_AuthorId",
                table: "ProjectTaskEntity",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTaskEntity_ProjectId",
                table: "ProjectTaskEntity",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTaskEntity_WorkerId",
                table: "ProjectTaskEntity",
                column: "WorkerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeEntityProjectEntity");

            migrationBuilder.DropTable(
                name: "ProjectTaskEntity");

            migrationBuilder.DropTable(
                name: "ProjectEntity");

            migrationBuilder.DropTable(
                name: "EmployeeEntity");
        }
    }
}
