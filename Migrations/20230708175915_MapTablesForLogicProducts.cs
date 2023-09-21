using Microsoft.EntityFrameworkCore.Migrations;

namespace AgroservicioCuxil.Migrations
{
    public partial class MapTablesForLogicProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Marca",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Marca__D5946642B600C305", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PresentacionProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUnidadMedida = table.Column<int>(type: "int", unicode: false, nullable: false),
                    IdTipoMaterialProducto = table.Column<double>(type: "float", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PresentacionProducto__D5946642B600C305", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RelacionTipoSubtipoProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTipoProducto = table.Column<int>(type: "int", unicode: false, nullable: false),
                    IdSubtipoProducto = table.Column<int>(type: "int", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__RelacionTipoSubtipoProducto__D5946642B600C305", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubtipoProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SubtipoProducto__D5946642B600C305", x => x.Id);
                });

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
                name: "TipoProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TipoProducto__D5946642B600C305", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnidadMedidaProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    IdTipoMaterialProducto = table.Column<int>(type: "int", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UnidadMedidaProducto__D5946642B600C305", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Marca");

            migrationBuilder.DropTable(
                name: "PresentacionProducto");

            migrationBuilder.DropTable(
                name: "RelacionTipoSubtipoProducto");

            migrationBuilder.DropTable(
                name: "SubtipoProducto");

            migrationBuilder.DropTable(
                name: "TipoMaterialProducto");

            migrationBuilder.DropTable(
                name: "TipoProducto");

            migrationBuilder.DropTable(
                name: "UnidadMedidaProducto");
        }
    }
}
