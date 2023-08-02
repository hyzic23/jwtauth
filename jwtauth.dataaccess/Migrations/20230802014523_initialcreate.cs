using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace jwtauth.dataaccess.Migrations
{
    /// <inheritdoc />
    public partial class initialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NationalIDNumber = table.Column<string>(type: "character varying(15)", unicode: false, maxLength: 15, nullable: true),
                    EmployeeName = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    LoginID = table.Column<string>(type: "character varying(256)", unicode: false, maxLength: 256, nullable: true),
                    JobTitle = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", unicode: false, nullable: false),
                    MaritalStatus = table.Column<string>(type: "character varying(1)", unicode: false, maxLength: 1, nullable: true),
                    Gender = table.Column<string>(type: "character varying(1)", unicode: false, maxLength: 1, nullable: true),
                    HireDate = table.Column<DateTime>(type: "timestamp with time zone", unicode: false, nullable: false),
                    VacationHours = table.Column<short>(type: "smallint", unicode: false, nullable: false),
                    SickLeaveHours = table.Column<short>(type: "smallint", unicode: false, nullable: false),
                    RowGuid = table.Column<Guid>(type: "uuid", unicode: false, maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeID);
                });

            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(60)", unicode: false, maxLength: 60, nullable: true),
                    UserName = table.Column<string>(type: "character varying(30)", unicode: false, maxLength: 30, nullable: true),
                    Email = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    Password = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "UserInfo");
        }
    }
}
