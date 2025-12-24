using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace internetprogramciligi1.Migrations
{
    /// <inheritdoc />
    public partial class KursBitirmeEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Enrollments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Enrollments");
        }
    }
}
