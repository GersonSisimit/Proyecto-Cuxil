using Microsoft.EntityFrameworkCore.Migrations;

namespace AgroservicioCuxil.Migrations
{
    public partial class adddetalleProducto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DetallePresentacionProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUnidadMedida = table.Column<int>(type: "int", unicode: false, nullable: false),
                    IdValorPresentacion = table.Column<int>(type: "int", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DetallePresentacionProducto__D5946642B600C305", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallePresentacionProducto");
        }
    }
}
