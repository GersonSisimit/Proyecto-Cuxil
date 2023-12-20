using AgroservicioCuxil.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using RegistroLogin.Filters;
using System.Linq;

namespace AgroservicioCuxil.Controllers
{
    public class VentasController : Controller
    {
        private readonly AgroservicioContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public VentasController(AgroservicioContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Venta()
        {
            return View();
        }

        [AuthorizeUsers]
        [HttpPost]
        public IActionResult DetalleProducto()
        {
            return View();
        }

        [AuthorizeUsers]
        [HttpPost]
        public IActionResult InsertarProducto(int IdProductoInsertando)
        {

            
            return View();
        }

        [AuthorizeUsers]
        [HttpPost]
        public IActionResult BuscarProducto(string busqueda)
        {
            // Obtener los productos que coincidan con la búsqueda por nombre
            var productos = _context.Producto
                .Where(p => p.Nombre.Contains(busqueda))
                .ToList();

            return Json(new { productos, busqueda }); ;
        }
    }
}
