using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtemVasilevKt_42_23.Migrations
{
    /// <inheritdoc />
    public partial class AddDisciplineDirection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Direction",
                table: "Disciplines",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Direction",
                table: "Disciplines");
        }
    }
}
