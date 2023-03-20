using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnoGotchiGame.Web.Migrations
{
    /// <inheritdoc />
    public partial class IsAlivePetStateReduce : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Statistic_IsAlive",
                table: "Pets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Statistic_IsAlive",
                table: "Pets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
