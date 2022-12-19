using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreAvancado.Migrations
{
    /// <inheritdoc />
    public partial class Rg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RG",
                table: "Funcionarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RG",
                table: "Funcionarios");
        }
    }
}
