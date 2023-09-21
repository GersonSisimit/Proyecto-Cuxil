using Microsoft.EntityFrameworkCore.Migrations;

namespace AgroservicioCuxil.Migrations
{
    public partial class detallePresentacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdUnidadMedida",
                table: "DetallePresentacionProducto");

            migrationBuilder.RenameColumn(
                name: "IdValorPresentacion",
                table: "DetallePresentacionProducto",
                newName: "Tipo");

            migrationBuilder.AddColumn<string>(
                name: "RutaImagen",
                table: "PresentacionProducto",
                type: "varchar(max)",
                unicode: false,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "DetallePresentacionProducto",
                type: "varchar(max)",
                unicode: false,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RutaImagen",
                table: "PresentacionProducto");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "DetallePresentacionProducto");

            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "DetallePresentacionProducto",
                newName: "IdValorPresentacion");

            migrationBuilder.AddColumn<int>(
                name: "IdUnidadMedida",
                table: "DetallePresentacionProducto",
                type: "int",
                unicode: false,
                nullable: false,
                defaultValue: 0);
        }
    }
}
