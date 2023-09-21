using Microsoft.EntityFrameworkCore.Migrations;

namespace AgroservicioCuxil.Migrations
{
    public partial class cambiarvalorColuma : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<int>(
                name: "IdMarca",
                table: "Producto",
                type: "int",
                unicode: false,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float",
                oldUnicode: false);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<double>(
                name: "IdMarca",
                table: "Producto",
                type: "float",
                unicode: false,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldUnicode: false);

        }
    }
}
