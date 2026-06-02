using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SibersDataManager.Migrations
{
    /// <inheritdoc />
    public partial class FixedEntityes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeEntityProjectEntity_EmployeeEntity_EmployeesId",
                table: "EmployeeEntityProjectEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeEntityProjectEntity_ProjectEntity_ProjectsId",
                table: "EmployeeEntityProjectEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectEntity_EmployeeEntity_ManagerId",
                table: "ProjectEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTaskEntity_EmployeeEntity_AuthorId",
                table: "ProjectTaskEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTaskEntity_EmployeeEntity_WorkerId",
                table: "ProjectTaskEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTaskEntity_ProjectEntity_ProjectId",
                table: "ProjectTaskEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectTaskEntity",
                table: "ProjectTaskEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectEntity",
                table: "ProjectEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeEntity",
                table: "EmployeeEntity");

            migrationBuilder.RenameTable(
                name: "ProjectTaskEntity",
                newName: "Tasks");

            migrationBuilder.RenameTable(
                name: "ProjectEntity",
                newName: "Projects");

            migrationBuilder.RenameTable(
                name: "EmployeeEntity",
                newName: "Employees");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTaskEntity_WorkerId",
                table: "Tasks",
                newName: "IX_Tasks_WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTaskEntity_ProjectId",
                table: "Tasks",
                newName: "IX_Tasks_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTaskEntity_AuthorId",
                table: "Tasks",
                newName: "IX_Tasks_AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectEntity_ManagerId",
                table: "Projects",
                newName: "IX_Projects_ManagerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects",
                table: "Projects",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeEntityProjectEntity_Employees_EmployeesId",
                table: "EmployeeEntityProjectEntity",
                column: "EmployeesId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeEntityProjectEntity_Projects_ProjectsId",
                table: "EmployeeEntityProjectEntity",
                column: "ProjectsId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Employees_ManagerId",
                table: "Projects",
                column: "ManagerId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Employees_AuthorId",
                table: "Tasks",
                column: "AuthorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Employees_WorkerId",
                table: "Tasks",
                column: "WorkerId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Projects_ProjectId",
                table: "Tasks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeEntityProjectEntity_Employees_EmployeesId",
                table: "EmployeeEntityProjectEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeEntityProjectEntity_Projects_ProjectsId",
                table: "EmployeeEntityProjectEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Employees_ManagerId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Employees_AuthorId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Employees_WorkerId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Projects_ProjectId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "ProjectTaskEntity");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "ProjectEntity");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "EmployeeEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_WorkerId",
                table: "ProjectTaskEntity",
                newName: "IX_ProjectTaskEntity_WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ProjectId",
                table: "ProjectTaskEntity",
                newName: "IX_ProjectTaskEntity_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_AuthorId",
                table: "ProjectTaskEntity",
                newName: "IX_ProjectTaskEntity_AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_ManagerId",
                table: "ProjectEntity",
                newName: "IX_ProjectEntity_ManagerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectTaskEntity",
                table: "ProjectTaskEntity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectEntity",
                table: "ProjectEntity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeEntity",
                table: "EmployeeEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeEntityProjectEntity_EmployeeEntity_EmployeesId",
                table: "EmployeeEntityProjectEntity",
                column: "EmployeesId",
                principalTable: "EmployeeEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeEntityProjectEntity_ProjectEntity_ProjectsId",
                table: "EmployeeEntityProjectEntity",
                column: "ProjectsId",
                principalTable: "ProjectEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectEntity_EmployeeEntity_ManagerId",
                table: "ProjectEntity",
                column: "ManagerId",
                principalTable: "EmployeeEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTaskEntity_EmployeeEntity_AuthorId",
                table: "ProjectTaskEntity",
                column: "AuthorId",
                principalTable: "EmployeeEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTaskEntity_EmployeeEntity_WorkerId",
                table: "ProjectTaskEntity",
                column: "WorkerId",
                principalTable: "EmployeeEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTaskEntity_ProjectEntity_ProjectId",
                table: "ProjectTaskEntity",
                column: "ProjectId",
                principalTable: "ProjectEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
