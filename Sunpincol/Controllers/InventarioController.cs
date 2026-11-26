using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sunpincol.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sunpincol.Controllers;

public class InventarioController : Controller
{
    private readonly BDSunpincolContext _context;

    public InventarioController(BDSunpincolContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? idSede, string? codigoProducto)
    {
        ViewBag.Sedes = await _context.Sedes
            .Where(s => s.EstadoSede == "Activa")
            .ToListAsync();

        ViewBag.Productos = await _context.Productos
            .Where(p => p.EstadoProducto)
            .ToListAsync();

        ViewBag.IdSedeSeleccionada = idSede;
        ViewBag.CodigoProductoSeleccionado = codigoProducto;

        if (idSede != null && !string.IsNullOrEmpty(codigoProducto))
        {
            var resultado = await (
                from i in _context.Inventarios
                join p in _context.Productos
                    on i.CodigoProducto equals p.CodigoProducto
                join s in _context.Sedes
                    on i.IdSede equals s.IdSede
                where i.IdSede == idSede
                    && i.CodigoProducto == codigoProducto
                select new
                {
                    s.NombreSede,
                    p.NombreProducto,
                    i.StockActual,
                    i.StockMinimo,
                    i.FechaUltimaActualizacion
                }
            ).FirstOrDefaultAsync();

            if (resultado == null)
            {
                ViewBag.Mensaje = "No se encontró inventario para el producto seleccionado en esta sede.";
            }
            else if (resultado.StockActual <= 0)
            {
                ViewBag.Mensaje = "El producto no tiene unidades disponibles en esta sede.";
            }
            else
            {
                ViewBag.Resultado = resultado;
            }
        }

        return View();
    }


    [HttpPost]
    public async Task<IActionResult> RegistrarGuia(int idSedeOrigen, int idSedeDestino, string referenciaGuia, DateTime fechaEmision)
    {
        var guia = new Guia
        {
            IdSedeOrigen = idSedeOrigen,
            IdSedeDestino = idSedeDestino,
            ReferenciaGuia = referenciaGuia,
            FechaEmision = fechaEmision,
            EstadoGuia = "Recibida"
        };

        _context.Guias.Add(guia);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "La guía se registró correctamente.";

        return RedirectToAction("Entradas");
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarEntrada(int idSede, string codigoProducto, int cantidad)
    {
        if (cantidad <= 0)
        {
            TempData["Mensaje"] = "La cantidad debe ser mayor que cero.";
            return RedirectToAction("Entradas");
        }

        var inventario = await _context.Inventarios
            .FirstOrDefaultAsync(i => i.IdSede == idSede && i.CodigoProducto == codigoProducto);

        if (inventario == null)
        {
            inventario = new Inventario
            {
                IdSede = idSede,
                CodigoProducto = codigoProducto,
                StockActual = cantidad,
                StockMinimo = 0,
                FechaUltimaActualizacion = DateTime.Now
            };

            _context.Inventarios.Add(inventario);
        }
        else
        {
            inventario.StockActual += cantidad;
            inventario.FechaUltimaActualizacion = DateTime.Now;
        }

        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "La entrada de mercancía se registró correctamente.";

        return RedirectToAction("Entradas");
    }


    public async Task<IActionResult> Entradas()
    {
        ViewBag.Sedes = await _context.Sedes
            .Where(s => s.EstadoSede == "Activa")
            .ToListAsync();

        ViewBag.Productos = await _context.Productos
            .Where(p => p.EstadoProducto)
            .ToListAsync();

        return View();
    }



    public async Task<IActionResult> Salidas(int? idSede)
    {
        ViewBag.Sedes = await _context.Sedes
            .Where(s => s.EstadoSede == "Activa")
            .ToListAsync();

        ViewBag.IdSedeSeleccionada = idSede;

        ViewBag.Productos = new List<Producto>();

        if (idSede != null)
        {
            ViewBag.Productos = await (
                from i in _context.Inventarios
                join p in _context.Productos
                    on i.CodigoProducto equals p.CodigoProducto
                where i.IdSede == idSede
                    && p.EstadoProducto
                select p
            ).ToListAsync();

            ViewBag.Inventarios = await (
                from i in _context.Inventarios
                join p in _context.Productos
                    on i.CodigoProducto equals p.CodigoProducto
                where i.IdSede == idSede
                select new
                {
                    p.NombreProducto,
                    i.CodigoProducto,
                    i.StockActual
                }
            ).ToListAsync();
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarSalida(int idSede, string codigoProducto, int cantidad)
    {
        if (cantidad <= 0)
        {
            TempData["Mensaje"] = "La cantidad debe ser mayor que cero.";
            return RedirectToAction("Salidas", new { idSede });
        }

        var inventario = await _context.Inventarios
            .FirstOrDefaultAsync(i =>
                i.IdSede == idSede &&
                i.CodigoProducto == codigoProducto);

        if (inventario == null)
        {
            TempData["Mensaje"] = "El producto no tiene inventario registrado en esta sede.";
            return RedirectToAction("Salidas", new { idSede });
        }

        if (cantidad > inventario.StockActual)
        {
            TempData["Mensaje"] = "No hay existencias suficientes para registrar esta salida.";
            return RedirectToAction("Salidas", new { idSede });
        }

        inventario.StockActual -= cantidad;
        inventario.FechaUltimaActualizacion = DateTime.Now;

        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "La salida de mercancía se registró correctamente.";

        return RedirectToAction("Salidas", new { idSede });
    }



    public async Task<IActionResult> Ajustes(int? idSede)
    {
        ViewBag.Sedes = await _context.Sedes
            .Where(s => s.EstadoSede == "Activa")
            .ToListAsync();

        ViewBag.IdSedeSeleccionada = idSede;

        ViewBag.Usuarios = await _context.Usuarios
            .Where(u => u.EstadoActivo && u.Rol == "Empleado")
            .Select(u => new SelectListItem
            {
                Value = u.IdUsuario.ToString(),
                Text = u.Nombre
            })
            .ToListAsync();

        ViewBag.Productos = new List<SelectListItem>();
        ViewBag.Inventarios = new List<object>();

        if (idSede != null)
        {
            ViewBag.Productos = await _context.Inventarios
                .Where(i => i.IdSede == idSede)
                .Join(_context.Productos,
                    i => i.CodigoProducto,
                    p => p.CodigoProducto,
                    (i, p) => new SelectListItem
                    {
                        Value = i.CodigoProducto,
                        Text = p.NombreProducto + " - " + i.StockActual + " unidades"
                    })
                .ToListAsync();

            ViewBag.Inventarios = await _context.Inventarios
                .Where(i => i.IdSede == idSede)
                .Join(_context.Productos,
                    i => i.CodigoProducto,
                    p => p.CodigoProducto,
                    (i, p) => new
                    {
                        i.CodigoProducto,
                        p.NombreProducto,
                        i.StockActual
                    })
                .ToListAsync();
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarAjuste(int idSede, string codigoProducto, int cantidadNueva, string motivoCambio, int idUsuarioResponsable)
    {
        if (cantidadNueva < 0 || string.IsNullOrWhiteSpace(motivoCambio))
        {
            TempData["Error"] = "Ingresa una cantidad válida y el motivo del ajuste.";
            return RedirectToAction("Ajustes", new { idSede });
        }

        var inventario = await _context.Inventarios
            .FirstOrDefaultAsync(i => i.IdSede == idSede && i.CodigoProducto == codigoProducto);

        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.IdUsuario == idUsuarioResponsable
                && u.EstadoActivo && u.Rol == "Empleado");

        if (inventario == null || usuario == null)
        {
            TempData["Error"] = "No se encontró el inventario o el empleado seleccionado.";
            return RedirectToAction("Ajustes", new { idSede });
        }

        int cantidadAnterior = inventario.StockActual;

        var cambio = new Cambio
        {
            IdInventario = inventario.IdInventario,
            IdUsuarioResponsable = idUsuarioResponsable,
            CantidadAnterior = cantidadAnterior,
            CantidadNueva = cantidadNueva,
            MotivoCambio = motivoCambio,
            FechaCambio = DateTime.Now
        };

        inventario.StockActual = cantidadNueva;
        inventario.FechaUltimaActualizacion = DateTime.Now;

        _context.Cambios.Add(cambio);

        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "El ajuste de existencias se registró correctamente.";

        return RedirectToAction("Ajustes", new { idSede });
    }

    public async Task<IActionResult> StockGeneral()
    {
        ViewBag.Inventarios = await _context.Inventarios
            .Join(_context.Productos,
                i => i.CodigoProducto,
                p => p.CodigoProducto,
                (i, p) => new { i, p })
            .Join(_context.Sedes,
                x => x.i.IdSede,
                s => s.IdSede,
                (x, s) => new
                {
                    NombreSede = s.NombreSede,
                    NombreProducto = x.p.NombreProducto,
                    CodigoProducto = x.i.CodigoProducto,
                    StockActual = x.i.StockActual,
                    StockMinimo = x.i.StockMinimo,
                    FechaUltimaActualizacion = x.i.FechaUltimaActualizacion
                })
            .OrderBy(i => i.NombreSede)
            .ThenBy(i => i.NombreProducto)
            .ToListAsync();

        return View();
    }













}
