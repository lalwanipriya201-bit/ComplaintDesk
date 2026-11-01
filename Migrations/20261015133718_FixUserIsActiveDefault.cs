using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComplaintMgtSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixUserIsActiveDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(
                "UPDATE Users SET IsActive = 1 WHERE IsActive = 0;");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldDefaultValue: false);
            }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
