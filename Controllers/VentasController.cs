using AgroservicioCuxil.Migrations;
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
        [AuthorizeUsers]
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
            // Lógica para buscar productos según la cadena de búsqueda
            var resultadosBusqueda = _context.Producto
                .Where(p => p.Nombre.Contains(busqueda))
                .Select(producto => new
                {
                    Producto = producto,
                    Marca = _context.Marca.FirstOrDefault(m => m.Id == producto.IdMarca),
                    Presentaciones = _context.PresentacionProducto
                        .Where(presentacion => presentacion.IdProducto == producto.Id)
                        .Select(presentacion => new
                        {
                            Presentacion = presentacion,
                            DetallePresentacion = _context.DetallePresentacionProducto
                                .FirstOrDefault(detalle => detalle.Id == presentacion.IdDetallePresentacion)
                        })
                        .ToList()
                })
                .ToList();

            // resultadosBusqueda ahora contiene una lista de productos con sus presentaciones y detalles asociados

            return PartialView("_ResultadosBusquedaPartial", resultadosBusqueda);
        }


    }
}
