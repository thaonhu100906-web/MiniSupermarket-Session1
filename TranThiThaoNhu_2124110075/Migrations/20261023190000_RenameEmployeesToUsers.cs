using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TranThiThaoNhu_2124110075.API.Migrations
{
    public partial class RenameEmployeesToUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'Users', N'U') IS NULL AND OBJECT_ID(N'Employees', N'U') IS NOT NULL
BEGIN
    EXEC sp_rename N'Employees', N'Users';
    IF COL_LENGTH(N'Users', N'EmployeeId') IS NOT NULL
        EXEC sp_rename N'Users.EmployeeId', N'UserId', N'COLUMN';
    ELSE IF COL_LENGTH(N'Users', N'Id') IS NOT NULL
        EXEC sp_rename N'Users.Id', N'UserId', N'COLUMN';
END

-- Update seeded login hashes only after the table has the final name.
IF OBJECT_ID(N'Users', N'U') IS NOT NULL AND COL_LENGTH(N'Users', N'PasswordHash') IS NOT NULL
BEGIN
    UPDATE dbo.Users
    SET PasswordHash = N'AQAAAAEAAYagAAAAEIocP11+myBBYnOElaa3yNnwvZSPNhu86vCuPheoO+PdYggXvbd91eKiO1vCsEndYg=='
    WHERE UserId BETWEEN 1 AND 15;
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'Users', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH(N'Users', N'UserId') IS NOT NULL
        EXEC sp_rename N'Users.UserId', N'EmployeeId', N'COLUMN';
    EXEC sp_rename N'Users', N'Employees';
END
");
        }
    }
}
