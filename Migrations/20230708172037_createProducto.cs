using Microsoft.EntityFrameworkCore.Migrations;

namespace AgroservicioCuxil.Migrations
{
    public partial class createProducto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Existencia = table.Column<double>(type: "float", unicode: false, nullable: false),
                    Precio = table.Column<double>(type: "float", unicode: false, nullable: false),
                    IdMarca = table.Column<double>(type: "float", unicode: false, nullable: false),
                    IdTipoProducto = table.Column<int>(type: "int", unicode: false, nullable: false),
                    IdPresentacion = table.Column<int>(type: "int", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Producto__D5946642B600C305", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Producto");
        }
    }
}
