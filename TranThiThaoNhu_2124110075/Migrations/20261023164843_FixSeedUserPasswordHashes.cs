using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TranThiThaoNhu_2124110075.API.Migrations
{
    /// <summary>
    /// Intentionally empty because Employees is renamed to Users in the next migration.
    /// The password hashes are updated by RenameEmployeesToUsers after the rename.
    /// Keep this migration ID for compatibility with existing migration history.
    /// </summary>
    public partial class FixSeedUserPasswordHashes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) { }
        protected override void Down(MigrationBuilder migrationBuilder) { }
    }
}
