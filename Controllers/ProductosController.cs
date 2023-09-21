using AgroservicioCuxil.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RegistroLogin.Data;
using RegistroLogin.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AgroservicioCuxil.Controllers
{

    public class ProductosController : Controller
    {
        private readonly AgroservicioContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductosController(AgroservicioContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        [AuthorizeUsers(Policy = "ADMINISTRADORES")]
        public IActionResult Index()
        {

            _context.SaveChanges(); // Guardar cambios en la base de datos
            return View();
        }

        [AuthorizeUsers(Policy = "ADMINISTRADORES")]
        public IActionResult CrearProducto()
        {
            var marcas = _context.Marca.ToList();
            var presentaciones = _context.PresentacionProducto.ToList();

            var RelacionTipoSubtipoProducto = _context.DetalleProducto.ToList();
            var tiposProductos = _context.TipoProducto.ToList();
            var SubtipoProducto = _context.SubtipoProducto.ToList();

            ViewBag.Marcas = marcas;
            ViewBag.tiposProductos = tiposProductos;
            ViewBag.presentaciones = presentaciones;
            ViewBag.RelacionTipoSubtipoProducto = RelacionTipoSubtipoProducto;
            ViewBag.SubtipoProducto = SubtipoProducto;

            return View();
        }

        [AuthorizeUsers(Policy = "ADMINISTRADORES")]
        [HttpPost]
        public IActionResult CrearProducto(string Nombre, int IdMarca, int IdSubtipo, int IdTipoProducto)
        {
            try
            {
                var relacion = _context.DetalleProducto
                    .FirstOrDefault(r => r.IdTipoProducto == IdTipoProducto && r.IdSubtipoProducto == IdSubtipo);

                if (relacion != null)
                {
                    bool existeProducto = _context.Producto
                        .AsEnumerable()
                        .Any(p => string.Equals(p.Nombre, Nombre, StringComparison.OrdinalIgnoreCase) &&
                                  p.IdMarca == IdMarca && p.IdDetalleProducto == relacion.Id);

                    if (existeProducto)
                    {
                        var errorRespuesta = new { resultado = false, mensaje = "El nombre del producto ya existe con la misma configuración." };
                        return Json(errorRespuesta);
                    }
                }
                else
                {
                    var errorRespuesta = new { resultado = false, mensaje = "No hay relación de datos, contacte a soporte" };
                    return Json(errorRespuesta);
                }

                Producto nuevoProducto = new Producto
                {
                    Nombre = Nombre,
                    IdMarca = IdMarca,
                    IdDetalleProducto = relacion.Id
                };

                _context.Producto.Add(nuevoProducto);
                _context.SaveChanges();

                int idProductoCreado = nuevoProducto.Id;

                var respuesta = new { resultado = true, mensaje = "Producto creado", mensaje2 = idProductoCreado.ToString() };
                return Json(respuesta);
            }
            catch (Exception error)
            {
                var errorRespuesta = new { resultado = false, mensaje = error.Message };
                return Json(errorRespuesta);
            }
        }


        [AuthorizeUsers(Policy = "ADMINISTRADORES")]
        [HttpPost]
        public IActionResult BuscarSubtipos(int id)
        {
            var subtiposProductos = _context.SubtipoProducto
                .Where(sp => _context.DetalleProducto
                    .Any(rtsp => rtsp.IdTipoProducto == id && rtsp.IdSubtipoProducto == sp.Id))
                .ToList();

            if (subtiposProductos.Count > 0)
            {
                var respuesta = new { resultado = true, mensaje = "La búsqueda de subtipos fue exitosa", subtipos = subtiposProductos };
                return Json(respuesta);
            }
            else
            {
                var respuesta = new { resultado = false, mensaje = "No hay subtipos asociados", subtipos = new List<SubtipoProducto>() };
                return Json(respuesta);
            }
        }


        [AuthorizeUsers]
        [HttpPost]
        public IActionResult AgregarImagen(int id, IFormFile imagen)
        {
            try
            {
                // Verificar si se ha proporcionado una imagen
                if (imagen != null && imagen.Length > 0)
                {
                    // Obtener la ruta de la carpeta donde se guardarán las imágenes
                    string carpetaImagenes = Path.Combine(_hostEnvironment.WebRootPath, "ImagenesProductos");

                    // Crear un nombre de archivo único para la imagen
                    string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(imagen.FileName);

                    // Crear la ruta completa para guardar la imagen
                    string rutaImagen = Path.Combine(carpetaImagenes, nombreArchivo);

                    // Guardar la imagen en el servidor
                    using (var stream = new FileStream(rutaImagen, FileMode.Create))
                    {
                        imagen.CopyTo(stream);
                    }

                    // Actualizar el atributo RutaImagen del producto en la base de datos
                    var producto = _context.Producto.Find(id);
                    producto.RutaImagen = nombreArchivo;
                    _context.SaveChanges();

                    TempData["CreacionExito"] = "Si";
                    TempData["Mensaje"] = "La imagen ha sido agregada exitosamente.";
                }
                else
                {
                    TempData["Error"] = "Si";
                    TempData["Mensaje"] = "No se ha proporcionado una imagen válida.";
                }
            }
            catch (Exception error)
            {
                TempData["Error"] = "Si";
                TempData["Mensaje"] = error.Message;
            }
            return RedirectToAction("DetalleProducto", new { id = id });
        }


        [AuthorizeUsers]
        [HttpGet]
        public IActionResult BuscarProductos(string busqueda)
        {
            // Obtener los productos que coincidan con la búsqueda por nombre
            var productos = _context.Producto
                .Where(p => p.Nombre.Contains(busqueda))
                .ToList();

            ViewBag.Productos = productos;
            ViewBag.Busqueda = busqueda;

            return View();
        }

        [AuthorizeUsers]
        [HttpGet]
        public IActionResult DetalleProducto(int id)
        {
            var detallesProducto = (from producto in _context.Producto
                                    join relacion in _context.DetalleProducto on producto.IdDetalleProducto equals relacion.Id
                                    join tipoProducto in _context.TipoProducto on relacion.IdTipoProducto equals tipoProducto.Id
                                    join subtipoProducto in _context.SubtipoProducto on relacion.IdSubtipoProducto equals subtipoProducto.Id
                                    join marca in _context.Marca on producto.IdMarca equals marca.Id
                                    where producto.Id == id
                                    select new
                                    {
                                        producto.Id,
                                        producto.Nombre,
                                        TipoProducto = tipoProducto.Nombre,
                                        SubtipoProducto = subtipoProducto.Nombre,
                                        Marca = marca.Nombre,
                                        RutaImagen = producto.RutaImagen
                                    })
                             .FirstOrDefault();
            // Obtener la lista de presentaciones del producto
            var presentacionesProducto = _context.PresentacionProducto
                .Where(p => p.IdProducto == id)
                .ToList();

            // Obtener la lista de detalles de presentaciones
            var DetallepresentacionesProducto = _context.DetallePresentacionProducto.ToList();


            if (detallesProducto != null)
            {
                ViewBag.IdProducto = detallesProducto.Id;
                ViewBag.NombreProducto = detallesProducto.Nombre;
                ViewBag.TipoProducto = detallesProducto.TipoProducto;
                ViewBag.SubtipoProducto = detallesProducto.SubtipoProducto;
                ViewBag.NombreMarca = detallesProducto.Marca;
                ViewBag.RutaImagen = detallesProducto.RutaImagen;
                ViewBag.presentacionesProducto = presentacionesProducto;
                ViewBag.DetallepresentacionesProducto = DetallepresentacionesProducto;

                return View(detallesProducto);
            }
            else
            {
                return View();
            }
        }

        [AuthorizeUsers]
        [HttpGet]
        public IActionResult DetallePresentacionProducto(int idProducto, int IdPresentacion)
        {
            var detallesProducto = (from producto in _context.Producto
                                    join relacion in _context.DetalleProducto on producto.IdDetalleProducto equals relacion.Id
                                    join tipoProducto in _context.TipoProducto on relacion.IdTipoProducto equals tipoProducto.Id
                                    join subtipoProducto in _context.SubtipoProducto on relacion.IdSubtipoProducto equals subtipoProducto.Id
                                    join marca in _context.Marca on producto.IdMarca equals marca.Id
                                    where producto.Id == idProducto
                                    select new
                                    {
                                        producto.Id,
                                        producto.Nombre,
                                        TipoProducto = tipoProducto.Nombre,
                                        SubtipoProducto = subtipoProducto.Nombre,
                                        Marca = marca.Nombre,
                                        RutaImagen = producto.RutaImagen
                                    })
                             .FirstOrDefault();

            if (detallesProducto != null)
            {
                ViewBag.IdProducto = detallesProducto.Id;
                ViewBag.NombreProducto = detallesProducto.Nombre;
                ViewBag.TipoProducto = detallesProducto.TipoProducto;
                ViewBag.SubtipoProducto = detallesProducto.SubtipoProducto;
                ViewBag.NombreMarca = detallesProducto.Marca;
                ViewBag.RutaImagen = detallesProducto.RutaImagen;

                // Obtener presentacion
                var Presentacion = _context.PresentacionProducto
                    .Where(p => p.IdProducto == idProducto && p.Id == IdPresentacion)
                    .FirstOrDefault();

                // Obtener detallePresentacion
                var DetallePresentacion = _context.DetallePresentacionProducto
                    .Where(p => p.Id == Presentacion.IdDetallePresentacion)
                    .FirstOrDefault();

                //Detalles de producto
                ViewBag.IdProducto = detallesProducto.Id;
                ViewBag.NombreProducto = detallesProducto.Nombre;
                ViewBag.TipoProducto = detallesProducto.TipoProducto;
                ViewBag.SubtipoProducto = detallesProducto.SubtipoProducto;
                ViewBag.NombreMarca = detallesProducto.Marca;
                ViewBag.RutaImagen = detallesProducto.RutaImagen;

                //Detalles de presentacion
                ViewBag.Presentacion = Presentacion;
                ViewBag.DetallePresentacion = DetallePresentacion;

                return View();
            }
            else
            {
                return View();
            }
        }


        [AuthorizeUsers]
        [HttpPost]
        public IActionResult getPresentaciones()
        {
            var detallesPresentacion = _context.DetallePresentacionProductos.ToList();
            return Json(detallesPresentacion);
        }

        [AuthorizeUsers]
        [HttpPost]
        public IActionResult GuardarPresentacionProducto(int idProducto, int idDetallePresentacion)
        {
            try
            {
                // Verificar si ya existe una presentación con esos datos
                var existePresentacion = _context.PresentacionProducto.Any(p => p.IdProducto == idProducto && p.IdDetallePresentacion == idDetallePresentacion);
                if (existePresentacion)
                {
                    var respuestaError = new { resultado = false, mensaje = "Ya existe una presentacion creada" };
                    return Json(respuestaError);
                }

                // Si no existe, crear y guardar la nueva presentación
                var nuevaPresentacion = new PresentacionProducto
                {
                    IdProducto = idProducto,
                    IdDetallePresentacion = idDetallePresentacion
                };
                _context.PresentacionProducto.Add(nuevaPresentacion);
                _context.SaveChanges();
                var respuesta = new { resultado = true, mensaje = "Creado exitosamente" };
                return Json(respuesta);
            }
            catch (Exception error)
            {
                var respuesta = new { resultado = false, mensaje = error.Message };
                return Json(respuesta);
            }
        }


        [AuthorizeUsers]
        [HttpPost]
        public IActionResult AgregarImagenPresentacion(int id, int idProducto, IFormFile imagen)
        {
            int idDetalle = 0;
            try
            {
                // Verificar si se ha proporcionado una imagen
                if (imagen != null && imagen.Length > 0)
                {
                    // Obtener la ruta de la carpeta donde se guardarán las imágenes
                    string carpetaImagenes = Path.Combine(_hostEnvironment.WebRootPath, "ImagenesProductos");

                    // Crear un nombre de archivo único para la imagen
                    string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(imagen.FileName);

                    // Crear la ruta completa para guardar la imagen
                    string rutaImagen = Path.Combine(carpetaImagenes, nombreArchivo);

                    // Guardar la imagen en el servidor
                    using (var stream = new FileStream(rutaImagen, FileMode.Create))
                    {
                        imagen.CopyTo(stream);
                    }

                    // Actualizar el atributo RutaImagen del producto en la base de datos
                    var Presentacionproducto = _context.PresentacionProducto.Find(id);
                    Presentacionproducto.RutaImagen = nombreArchivo;
                    idDetalle = Presentacionproducto.IdDetallePresentacion;
                    _context.SaveChanges();

                    TempData["CreacionExito"] = "Si";
                    TempData["Mensaje"] = "La imagen ha sido agregada exitosamente.";
                }
                else
                {
                    TempData["Error"] = "Si";
                    TempData["Mensaje"] = "No se ha proporcionado una imagen válida.";
                }
            }
            catch (Exception error)
            {
                TempData["Error"] = "Si";
                TempData["Mensaje"] = error.Message;
            }
            return RedirectToAction("DetallePresentacionProducto", new { idProducto = idProducto, IdPresentacion = idDetalle });
        }

        [AuthorizeUsers]
        [HttpPost]
        public IActionResult ActualizarPresentacionProducto(IFormCollection frm)
        {
            int idProducto = int.Parse(frm["IdProducto"]);
            int idDetalle = int.Parse(frm["IdPresentacion"]);
            double Precio = double.Parse(frm["Precio"]);
            double Existencia = double.Parse(frm["Existencia"]);
            try
            {

                // Actualizar el atributo RutaImagen del producto en la base de datos
                var Presentacionproducto = _context.PresentacionProducto.Find(idDetalle);
                Presentacionproducto.Precio = Precio;
                Presentacionproducto.Existencia = Existencia;
                _context.SaveChanges();

                TempData["CreacionExito"] = "Si";
                TempData["Mensaje"] = "Detalle agregado exitosamente";

                //TempData["Error"] = "Si";
                //TempData["Mensaje"] = "No se ha proporcionado una imagen válida.";
            }
            catch (Exception error)
            {
                TempData["Error"] = "Si";
                TempData["Mensaje"] = error.Message;
            }
            return RedirectToAction("DetallePresentacionProducto", new { idProducto = idProducto, IdPresentacion = idDetalle });
        }

        #region Crud para definir detalles de producto


        [AuthorizeUsers]
        public IActionResult GestionProducto()
        {

            return View();
        }
        #endregion
    }
}
