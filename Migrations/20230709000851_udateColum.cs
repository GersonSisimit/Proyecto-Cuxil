using Microsoft.EntityFrameworkCore.Migrations;

namespace AgroservicioCuxil.Migrations
{
    public partial class udateColum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CantidadMateriaPrima",
                table: "PresentacionProducto");

            migrationBuilder.AddColumn<int>(
                name: "IdValorPresentacion",
                table: "PresentacionProducto",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdValorPresentacion",
                table: "PresentacionProducto");

            migrationBuilder.AddColumn<double>(
                name: "CantidadMateriaPrima",
                table: "PresentacionProducto",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
