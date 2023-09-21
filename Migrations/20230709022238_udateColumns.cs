using Microsoft.EntityFrameworkCore.Migrations;

namespace AgroservicioCuxil.Migrations
{
    public partial class udateColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Existencia",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "IdPresentacion",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "Precio",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "IdValorPresentacion",
                table: "PresentacionProducto");

            migrationBuilder.RenameColumn(
                name: "IdTipoProducto",
                table: "Producto",
                newName: "IdDetalleProducto");

            migrationBuilder.RenameColumn(
                name: "IdUnidadMedida",
                table: "PresentacionProducto",
                newName: "IdProducto");

            migrationBuilder.RenameColumn(
                name: "IdTipoMaterialProducto",
                table: "PresentacionProducto",
                newName: "Precio");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Producto",
                type: "varchar(max)",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Existencia",
                table: "PresentacionProducto",
                type: "float",
                unicode: false,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "IdDetallePresentacion",
                table: "PresentacionProducto",
                type: "int",
                unicode: false,
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Existencia",
                table: "PresentacionProducto");

            migrationBuilder.DropColumn(
                name: "IdDetallePresentacion",
                table: "PresentacionProducto");

            migrationBuilder.RenameColumn(
                name: "IdDetalleProducto",
                table: "Producto",
                newName: "IdTipoProducto");

            migrationBuilder.RenameColumn(
                name: "Precio",
                table: "PresentacionProducto",
                newName: "IdTipoMaterialProducto");

            migrationBuilder.RenameColumn(
                name: "IdProducto",
                table: "PresentacionProducto",
                newName: "IdUnidadMedida");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Producto",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Existencia",
                table: "Producto",
                type: "float",
                unicode: false,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "IdPresentacion",
                table: "Producto",
                type: "int",
                unicode: false,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Precio",
                table: "Producto",
                type: "float",
                unicode: false,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "IdValorPresentacion",
                table: "PresentacionProducto",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
