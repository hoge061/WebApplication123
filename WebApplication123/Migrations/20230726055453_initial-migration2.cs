using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wing1.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Details",
                columns: table => new
                {
                    Userid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Starttime1 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Endtime1 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Content1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Starttime2 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Endtime2 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Content2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Starttime3 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Endtime3 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Content3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Starttime4 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Endtime4 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Content4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Starttime5 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Endtime5 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Content5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Starttime6 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Endtime6 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Content6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Starttime7 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Endtime7 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Content7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Starttime8 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Endtime8 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Content8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Starttime9 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Endtime9 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Content9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Starttime10 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Endtime10 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Content10 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Details", x => new { x.Userid, x.Date });
                });

            migrationBuilder.CreateTable(
                name: "Kintai",
                columns: table => new
                {
                    Userid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Workstyle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Starttime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Endtime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Break1start = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Break1end = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Break2start = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Break2end = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Break3start = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Break3end = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Break4start = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Break4end = table.Column<DateTime>(type: "datetime2", nullable: true),
                    biko = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kintai", x => new { x.Userid, x.Date });
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Userid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Pass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kstarttime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Kendtime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    adminflag = table.Column<int>(type: "int", nullable: true),
                    Jitudo = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Userid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Details");

            migrationBuilder.DropTable(
                name: "Kintai");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
