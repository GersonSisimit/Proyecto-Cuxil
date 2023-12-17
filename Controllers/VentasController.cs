using AgroservicioCuxil.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

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
    }
}
