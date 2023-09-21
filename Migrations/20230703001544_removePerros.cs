using Microsoft.EntityFrameworkCore.Migrations;

namespace AgroservicioCuxil.Migrations
{
    public partial class removePerros : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
     name: "Perros");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
