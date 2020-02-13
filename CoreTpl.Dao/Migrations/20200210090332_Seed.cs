using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreTpl.Dao.Migrations
{
    public partial class Seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RoleInfo",
                columns: new[] { "RoleId", "AllowActList", "CreateBy", "CreateDate", "ModifyBy", "ModifyDate", "RemarkText", "RoleName", "UseStatus" },
                values: new object[] { 1, "RoleSetting,UserSetting", 1, new DateTimeOffset(new DateTime(2020, 2, 10, 17, 3, 32, 108, DateTimeKind.Unspecified).AddTicks(5599), new TimeSpan(0, 8, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2020, 2, 10, 17, 3, 32, 111, DateTimeKind.Unspecified).AddTicks(5601), new TimeSpan(0, 8, 0, 0, 0)), null, "管理者", "Enable" });

            migrationBuilder.InsertData(
                table: "UserInfo",
                columns: new[] { "UserId", "Account", "AllowActList", "CreateBy", "CreateDate", "DenyActList", "Department", "Email", "ExtensionNum", "ModifyBy", "ModifyDate", "Password", "RemarkText", "UseStatus", "UserName", "UserTitle", "UserType" },
                values: new object[,]
                {
                    { 1, "system", null, 1, new DateTimeOffset(new DateTime(2020, 2, 10, 17, 3, 32, 113, DateTimeKind.Unspecified).AddTicks(5602), new TimeSpan(0, 8, 0, 0, 0)), null, null, null, null, 1, new DateTimeOffset(new DateTime(2020, 2, 10, 17, 3, 32, 113, DateTimeKind.Unspecified).AddTicks(5602), new TimeSpan(0, 8, 0, 0, 0)), null, null, "Disable", "系統", null, "System" },
                    { 11, "admin", null, 1, new DateTimeOffset(new DateTime(2020, 2, 10, 17, 3, 32, 114, DateTimeKind.Unspecified).AddTicks(5603), new TimeSpan(0, 8, 0, 0, 0)), null, null, null, null, 1, new DateTimeOffset(new DateTime(2020, 2, 10, 17, 3, 32, 114, DateTimeKind.Unspecified).AddTicks(5603), new TimeSpan(0, 8, 0, 0, 0)), "YP50QG5/NT7ZefNQ8vu2ouhpCl+n0bDDKYPR2LP5X2c=", null, "Enable", "Admin", null, "Manager" }
                });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "UserId", "RoleId", "CreateBy", "CreateDate", "ModifyBy", "ModifyDate" },
                values: new object[] { 11, 1, 1, new DateTimeOffset(new DateTime(2020, 2, 10, 17, 3, 32, 114, DateTimeKind.Unspecified).AddTicks(5603), new TimeSpan(0, 8, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2020, 2, 10, 17, 3, 32, 114, DateTimeKind.Unspecified).AddTicks(5603), new TimeSpan(0, 8, 0, 0, 0)) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserInfo",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { 11, 1 });

            migrationBuilder.DeleteData(
                table: "RoleInfo",
                keyColumn: "RoleId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserInfo",
                keyColumn: "UserId",
                keyValue: 11);
        }
    }
}
