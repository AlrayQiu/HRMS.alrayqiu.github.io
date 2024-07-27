using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Application.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HRMS_Group",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRMS_Group", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HRMS_Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mac = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Time = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRMS_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HRMS_ManagerGroupUser",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRMS_ManagerGroupUser", x => new { x.GroupId, x.UserId });
                    table.ForeignKey(
                        name: "FK_HRMS_ManagerGroupUser_HRMS_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "HRMS_Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HRMS_ManagerGroupUser_HRMS_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "HRMS_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HRMS_MemberGroupUser",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRMS_MemberGroupUser", x => new { x.GroupId, x.UserId });
                    table.ForeignKey(
                        name: "FK_HRMS_MemberGroupUser_HRMS_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "HRMS_Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HRMS_MemberGroupUser_HRMS_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "HRMS_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HRMS_ManagerGroupUser_UserId",
                table: "HRMS_ManagerGroupUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HRMS_MemberGroupUser_UserId",
                table: "HRMS_MemberGroupUser",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HRMS_ManagerGroupUser");

            migrationBuilder.DropTable(
                name: "HRMS_MemberGroupUser");

            migrationBuilder.DropTable(
                name: "HRMS_Group");

            migrationBuilder.DropTable(
                name: "HRMS_Users");
        }
    }
}
