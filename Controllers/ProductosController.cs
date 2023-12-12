using AgroservicioCuxil.Migrations;
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
        public IActionResult GuardarPresentacionProducto(int idProducto, int idDetallePresentacion,double Precio, double Existencia)
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
                    IdDetallePresentacion = idDetallePresentacion,
                    Precio = Precio,
                    Existencia = Existencia,
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
            string referrer = Request.Headers["Referer"].ToString();

            // Redireccionar de vuelta a la URL de referencia
            return Redirect(referrer);
            //return RedirectToAction("DetallePresentacionProducto", new { idProducto = idProducto, IdPresentacion = idDetalle });
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

        [AuthorizeUsers]
        public IActionResult GestionProducto()
        {
            // crear marcas
            //crear presentacion de producto
            //crear detalle de producto
            // crear tipo y subipo de productos
            //crear detalle presentacion producto

            return View();
        }
        #region Crud para Marca

        [AuthorizeUsers]
        public IActionResult MarcaProductos()
        {
            var Marcas = _context.Marca.ToList();
            return View(Marcas);
        }

        [HttpPost]
        public IActionResult EditarMarca(Marca marca)
        {
            if (ModelState.IsValid)
            {
                var marcaExistente = _context.Marca.Find(marca.Id);
                if (marcaExistente == null)
                {
                    return RedirectToAction("MarcaProductos", "Productos");
                }
                if (string.IsNullOrWhiteSpace(marca.Nombre))
                {
                    TempData["Error"] = "Si";
                    TempData["Mensaje"] = "No ingrese un nombre en blanco";
                    return RedirectToAction("MarcaProductos", "Productos");
                }

                marcaExistente.Nombre = marca.Nombre;
                _context.SaveChanges();
                TempData["CreacionExito"] = "Si";
                TempData["Mensaje"] = "Modificacion Exitosa";
                return RedirectToAction("MarcaProductos", "Productos");
            }
            return RedirectToAction("MarcaProductos", "Productos");
        }

        [HttpPost]
        public IActionResult CrearMarca(Marca marca)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Marca.Add(marca); // Agregar la marca al contexto
                    _context.SaveChanges(); // Guardar los cambios en la base de datos

                    TempData["CreacionExito"] = "Si";
                    TempData["Mensaje"] = "Creacion Exitosa";
                    return RedirectToAction("MarcaProductos", "Productos");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Si";
                    TempData["Mensaje"] = ex.Message;
                    return RedirectToAction("MarcaProductos", "Productos");
                }
            }
            return RedirectToAction("MarcaProductos", "Productos");

           
        }
        #endregion
        #region CRUD TipoProducto

        [AuthorizeUsers]
        public IActionResult TipoProducto()
        {
            var TipoProductos = _context.TipoProducto.ToList();
            return View(TipoProductos);
        }

        [HttpPost]
        public IActionResult EditarTipoProducto(TipoProducto TipoProducto)
        {
            if (ModelState.IsValid)
            {
                var RegistroExistente = _context.TipoProducto.Find(TipoProducto.Id);
                if (RegistroExistente == null)
                {
                    TempData["Error"] = "Si";
                    TempData["Mensaje"] = "No se encontro el tipo de producto";
                    return RedirectToAction("TipoProducto", "Productos");
                }
                if (string.IsNullOrWhiteSpace(TipoProducto.Nombre))
                {
                    TempData["Error"] = "Si";
                    TempData["Mensaje"] = "No ingrese un nombre en blanco";
                    return RedirectToAction("TipoProducto", "Productos");
                }

                RegistroExistente.Nombre = TipoProducto.Nombre;
                _context.SaveChanges();
                TempData["CreacionExito"] = "Si";
                TempData["Mensaje"] = "Modificacion Exitosa";
                return RedirectToAction("TipoProducto", "Productos");
            }
            return RedirectToAction("MarcaProductos", "Productos");
        }

        [HttpPost]
        public IActionResult CrearTipoProducto(TipoProducto TipoProducto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.TipoProducto.Add(TipoProducto); // Agregar la marca al contexto
                    _context.SaveChanges(); // Guardar los cambios en la base de datos

                    TempData["CreacionExito"] = "Si";
                    TempData["Mensaje"] = "Creacion Exitosa";
                    return RedirectToAction("TipoProducto", "Productos");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Si";
                    TempData["Mensaje"] = ex.Message;
                    return RedirectToAction("TipoProducto", "Productos");
                }
            }
            return RedirectToAction("TipoProducto", "Productos");
        }


        [AuthorizeUsers]
        public IActionResult SubtipoProducto()
        {
            var TiposProducto = _context.TipoProducto.ToList();
            var SubtipoProducto = _context.SubtipoProducto.ToList();
            ViewBag.SubtipoProducto = SubtipoProducto;
            return View(TiposProducto);
        }

        [HttpPost]
        public IActionResult CrearSubtipo(SubtipoProducto SubtipoProducto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Convierte el nombre a minúsculas antes de realizar la búsqueda en la base de datos
                    string nombreLowerCase = SubtipoProducto.Nombre.ToLower();

                    // Verificar si ya existe un registro con el mismo nombre (insensible a mayúsculas/minúsculas)
                    var existingSubtipo = _context.SubtipoProducto.FirstOrDefault(s =>
                        s.Nombre.ToLower() == nombreLowerCase);

                    if (existingSubtipo != null)
                    {
                        // Ya existe un registro con el mismo nombre (insensible a mayúsculas/minúsculas)
                        TempData["Error"] = "Si";
                        TempData["Mensaje"] = "Ya existe un subtipo con el mismo nombre.";
                        return RedirectToAction("SubtipoProducto", "Productos");
                    }

                    // Si no existe, entonces puedes agregar el nuevo subtipo
                    SubtipoProducto.Nombre = nombreLowerCase; // Guardar el nombre en minúsculas
                    _context.SubtipoProducto.Add(SubtipoProducto); // Agregar el subtipo al contexto
                    _context.SaveChanges(); // Guardar los cambios en la base de datos

                    TempData["CreacionExito"] = "Si";
                    TempData["Mensaje"] = "Creacion Exitosa";
                    return RedirectToAction("SubtipoProducto", "Productos");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Si";
                    TempData["Mensaje"] = ex.Message;
                    return RedirectToAction("SubtipoProducto", "Productos");
                }
            }

            return RedirectToAction("SubtipoProducto", "Productos");
        }

        [HttpPost]
        public IActionResult EditarSubtipo(SubtipoProducto SubtipoProducto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var SubtipoProductoExistente = _context.SubtipoProducto.Find(SubtipoProducto.Id);
                    if (SubtipoProductoExistente == null)
                    {
                        TempData["Error"] = "Si";
                        TempData["Mensaje"] = "El registro no existe";
                        return RedirectToAction("SubtipoProducto", "Productos");
                    }
                    if (string.IsNullOrWhiteSpace(SubtipoProducto.Nombre))
                    {
                        TempData["Error"] = "Si";
                        TempData["Mensaje"] = "No ingrese un nombre en blanco";
                        return RedirectToAction("SubtipoProducto", "Productos");
                    }

                    SubtipoProductoExistente.Nombre = SubtipoProducto.Nombre;
                    _context.SaveChanges();
                    TempData["CreacionExito"] = "Si";
                    TempData["Mensaje"] = "Modificacion Exitosa";
                    return RedirectToAction("SubtipoProducto", "Productos");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Si";
                    TempData["Mensaje"] = ex.Message;
                    return RedirectToAction("SubtipoProducto", "Productos");
                }
            }

            return RedirectToAction("SubtipoProducto", "Productos");
        }


        [HttpPost]
        public IActionResult CrearDetalleProducto(DetalleProducto DetalleProducto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si ya existe un registro con la misma combinación de IdTipoProducto e IdSubtipoProducto
                    var existingDetalle = _context.DetalleProducto.FirstOrDefault(d =>
                        d.IdTipoProducto == DetalleProducto.IdTipoProducto &&
                        d.IdSubtipoProducto == DetalleProducto.IdSubtipoProducto);

                    if (existingDetalle != null)
                    {
                        // Ya existe un registro con la misma combinación, puedes manejarlo aquí
                        TempData["Error"] = "Si";
                        TempData["Mensaje"] = "Ya existe un detalle con la misma combinacion de Tipo y Subtipo.";
                        return RedirectToAction("SubtipoProducto", "Productos");
                    }

                    // Si no existe, entonces puedes agregar el nuevo detalle
                    _context.DetalleProducto.Add(DetalleProducto); // Agregar el detalle al contexto
                    _context.SaveChanges(); // Guardar los cambios en la base de datos

                    TempData["CreacionExito"] = "Si";
                    TempData["Mensaje"] = "Relacion Exitosa";
                    return RedirectToAction("SubtipoProducto", "Productos");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Si";
                    TempData["Mensaje"] = ex.Message;
                    return RedirectToAction("SubtipoProducto", "Productos");
                }
            }

            return RedirectToAction("SubtipoProducto", "Productos");
        }


        [AuthorizeUsers]
        public IActionResult PresentacionesProducto()
        {
            var DetallePresentacionProducto = _context.DetallePresentacionProducto.ToList();
            ViewBag.DetallePresentacion = DetallePresentacionProducto;

            return View();
        }

        [HttpPost]
        public IActionResult CrearPresentacionProducto(DetallePresentacionProducto DetallePresentacionProducto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Convierte el nombre a minúsculas antes de realizar la búsqueda en la base de datos
                    string nombreLowerCase = DetallePresentacionProducto.Nombre.ToLower();

                    // Verificar si ya existe un registro con el mismo nombre (insensible a mayúsculas/minúsculas)
                    var existing = _context.DetallePresentacionProducto.FirstOrDefault(s =>
                        s.Nombre.ToLower() == nombreLowerCase);

                    if (existing != null)
                    {
                        // Ya existe un registro con el mismo nombre (insensible a mayúsculas/minúsculas)
                        TempData["Error"] = "Si";
                        TempData["Mensaje"] = "Ya existe un registro con el mismo nombre";
                        return RedirectToAction("PresentacionesProducto", "Productos");
                    }

                    _context.DetallePresentacionProducto.Add(DetallePresentacionProducto); // Agregar la marca al contexto
                    _context.SaveChanges(); // Guardar los cambios en la base de datos

                    TempData["CreacionExito"] = "Si";
                    TempData["Mensaje"] = "Creacion Exitosa";
                    return RedirectToAction("PresentacionesProducto", "Productos");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Si";
                    TempData["Mensaje"] = ex.Message;
                    return RedirectToAction("PresentacionesProducto", "Productos");
                }
            }
            return RedirectToAction("PresentacionesProducto", "Productos");
        }

        [HttpPost]
        public IActionResult EditarPresentacionProducto(DetallePresentacionProducto DetallePresentacionProducto)
        {
            if (ModelState.IsValid)
            {
                var Existente = _context.DetallePresentacionProducto.Find(DetallePresentacionProducto.Id);
                if (Existente == null)
                {
                    return RedirectToAction("PresentacionesProducto", "Productos");
                }
                if (string.IsNullOrWhiteSpace(DetallePresentacionProducto.Nombre))
                {
                    TempData["Error"] = "Si";
                    TempData["Mensaje"] = "No ingrese un nombre en blanco";
                    return RedirectToAction("PresentacionesProducto", "Productos");
                }

                Existente.Nombre = DetallePresentacionProducto.Nombre;
                Existente.Tipo = DetallePresentacionProducto.Tipo;
                _context.SaveChanges();
                TempData["CreacionExito"] = "Si";
                TempData["Mensaje"] = "Modificacion Exitosa";
                return RedirectToAction("PresentacionesProducto", "Productos");
            }
            return RedirectToAction("PresentacionesProducto", "Productos");
        }
        #endregion
    }
}
