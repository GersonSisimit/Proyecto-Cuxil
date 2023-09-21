using Microsoft.EntityFrameworkCore.Migrations;

namespace AgroservicioCuxil.Migrations
{
    public partial class EliminarTablasInnecesarias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TipoMaterialProducto");

            migrationBuilder.DropTable(
                name: "UnidadMedidaProducto");

            migrationBuilder.DropTable(
                name: "ValorPresentacionProducto");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipoMaterialProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TipoMaterialProducto__D5946642B600C305", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnidadMedidaProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTipoMaterialProducto = table.Column<int>(type: "int", unicode: false, nullable: false),
                    Nombre = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UnidadMedidaProducto__D5946642B600C305", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ValorPresentacionProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ValorPresentacionProducto__D5946642B600C305", x => x.Id);
                });
        }
    }
}
