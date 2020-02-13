using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreTpl.Dao.Migrations
{
    public partial class IdStart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "RoleInfo",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreateDate", "ModifyDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 2, 11, 17, 4, 2, 736, DateTimeKind.Unspecified).AddTicks(8893), new TimeSpan(0, 8, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 2, 11, 17, 4, 2, 739, DateTimeKind.Unspecified).AddTicks(8895), new TimeSpan(0, 8, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreateDate", "ModifyDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 2, 11, 17, 4, 2, 742, DateTimeKind.Unspecified).AddTicks(8897), new TimeSpan(0, 8, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 2, 11, 17, 4, 2, 742, DateTimeKind.Unspecified).AddTicks(8897), new TimeSpan(0, 8, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "UserId",
                keyValue: 11,
                columns: new[] { "CreateDate", "ModifyDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 2, 11, 17, 4, 2, 742, DateTimeKind.Unspecified).AddTicks(8897), new TimeSpan(0, 8, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 2, 11, 17, 4, 2, 742, DateTimeKind.Unspecified).AddTicks(8897), new TimeSpan(0, 8, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { 11, 1 },
                columns: new[] { "CreateDate", "ModifyDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 2, 11, 17, 4, 2, 742, DateTimeKind.Unspecified).AddTicks(8897), new TimeSpan(0, 8, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 2, 11, 17, 4, 2, 742, DateTimeKind.Unspecified).AddTicks(8897), new TimeSpan(0, 8, 0, 0, 0)) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "RoleInfo",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreateDate", "ModifyDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 2, 11, 16, 46, 18, 46, DateTimeKind.Unspecified).AddTicks(9925), new TimeSpan(0, 8, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 2, 11, 16, 46, 18, 49, DateTimeKind.Unspecified).AddTicks(9927), new TimeSpan(0, 8, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreateDate", "ModifyDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 2, 11, 16, 46, 18, 51, DateTimeKind.Unspecified).AddTicks(9928), new TimeSpan(0, 8, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 2, 11, 16, 46, 18, 51, DateTimeKind.Unspecified).AddTicks(9928), new TimeSpan(0, 8, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserInfo",
                keyColumn: "UserId",
                keyValue: 11,
                columns: new[] { "CreateDate", "ModifyDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 2, 11, 16, 46, 18, 51, DateTimeKind.Unspecified).AddTicks(9928), new TimeSpan(0, 8, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 2, 11, 16, 46, 18, 51, DateTimeKind.Unspecified).AddTicks(9928), new TimeSpan(0, 8, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { 11, 1 },
                columns: new[] { "CreateDate", "ModifyDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 2, 11, 16, 46, 18, 51, DateTimeKind.Unspecified).AddTicks(9928), new TimeSpan(0, 8, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 2, 11, 16, 46, 18, 51, DateTimeKind.Unspecified).AddTicks(9928), new TimeSpan(0, 8, 0, 0, 0)) });
        }
    }
}
