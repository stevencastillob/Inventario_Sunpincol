
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sunpincol.Models;

namespace Sunpincol.Controllers;

public class PedidosController : Controller
{
    private readonly BDSunpincolContext _context;

    public PedidosController(BDSunpincolContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Solicitar()
    {
        ViewBag.Empleados = await _context.Empleados.ToListAsync();
        ViewBag.Productos = await _context.Productos
            .Where(p => p.EstadoProducto)
            .ToListAsync();

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarSolicitud(int idEmpleadoSolicitante, string codigoProducto, int cantidadSolicitada)
    {
        var empleado = await _context.Empleados
            .FirstOrDefaultAsync(e => e.IdEmpleado == idEmpleadoSolicitante);

        var producto = await _context.Productos
            .FirstOrDefaultAsync(p => p.CodigoProducto == codigoProducto && p.EstadoProducto);

        if (empleado == null || producto == null || cantidadSolicitada <= 0)
        {
            ViewBag.Empleados = await _context.Empleados.ToListAsync();
            ViewBag.Productos = await _context.Productos
                .Where(p => p.EstadoProducto)
                .ToListAsync();

            ViewBag.Error = "Verifica los datos ingresados e intenta nuevamente.";
            return View("Solicitar");
        }

        var pedido = new Pedido
        {
            IdSedeSolicitante = empleado.IdSedeAsignada,
            IdEmpleadoSolicitante = idEmpleadoSolicitante,
            CodigoProducto = codigoProducto,
            CantidadSolicitada = cantidadSolicitada,
            EstadoPedido = "Pendiente",
            FechaSolicitud = DateTime.Now
        };

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        return RedirectToAction("Comprobante", new { id = pedido.IdPedido });
    }

    public async Task<IActionResult> Comprobante(int id)
    {
        var pedido = await _context.Pedidos
            .FirstOrDefaultAsync(p => p.IdPedido == id);

        if (pedido == null)
        {
            return NotFound();
        }

        ViewBag.Producto = await _context.Productos
            .FirstOrDefaultAsync(p => p.CodigoProducto == pedido.CodigoProducto);

        ViewBag.Empleado = await _context.Empleados
            .FirstOrDefaultAsync(e => e.IdEmpleado == pedido.IdEmpleadoSolicitante);

        ViewBag.Sede = await _context.Sedes
            .FirstOrDefaultAsync(s => s.IdSede == pedido.IdSedeSolicitante);

        return View(pedido);
    }


    public async Task<IActionResult> Solicitudes()
    {
        var bodeguero = await _context.Bodegueros.FirstOrDefaultAsync();

        if (bodeguero == null)
        {
            ViewBag.Error = "No hay un bodeguero registrado.";
            return View();
        }

        ViewBag.IdBodeguero = bodeguero.IdBodeguero;

        ViewBag.Pedidos = await (
            from p in _context.Pedidos
            join pr in _context.Productos on p.CodigoProducto equals pr.CodigoProducto
            join s in _context.Sedes on p.IdSedeSolicitante equals s.IdSede
            where p.EstadoPedido == "Pendiente"
            select new
            {
                Pedido = p,
                Producto = pr.NombreProducto,
                Sede = s.NombreSede
            }
        ).ToListAsync();

        return View();
    }

    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> GestionarPedido(int idPedido, int idBodeguero, bool aprobar)
    {
        var pedido = await _context.Pedidos
            .FirstOrDefaultAsync(p => p.IdPedido == idPedido);

        var bodeguero = await _context.Bodegueros
            .FirstOrDefaultAsync(b => b.IdBodeguero == idBodeguero);

        if (pedido == null || bodeguero == null || pedido.EstadoPedido != "Pendiente")
        {
            TempData["Mensaje"] = "No fue posible procesar la solicitud.";
            return RedirectToAction("Solicitudes");
        }

        if (aprobar)
        {
            var inventario = await _context.Inventarios
                .FirstOrDefaultAsync(i =>
                    i.IdSede == bodeguero.IdBodegaAsignada &&
                    i.CodigoProducto == pedido.CodigoProducto);

            if (inventario == null || inventario.StockActual < pedido.CantidadSolicitada)
            {
                TempData["Mensaje"] = "No hay suficiente inventario para aprobar este pedido.";
                return RedirectToAction("Solicitudes");
            }

            var historial = new Historial
            {
                IdPedido = pedido.IdPedido,
                IdUsuarioResponsable = bodeguero.IdEmpleado,
                EstadoAnterior = pedido.EstadoPedido,
                EstadoActual = "Aprobado",
                FechaRegistro = DateTime.Now
            };

            pedido.EstadoPedido = "Aprobado";

            _context.Historiales.Add(historial);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "El pedido fue aprobado correctamente.";
        }
        else
        {
            TempData["Mensaje"] = "El pedido no fue aprobado.";
        }

        return RedirectToAction("Solicitudes");
    }

    public async Task<IActionResult> Despachos()
    {
        var bodeguero = await _context.Bodegueros.FirstOrDefaultAsync();

        if (bodeguero == null)
        {
            ViewBag.Error = "No hay un bodeguero registrado.";
            return View();
        }

        ViewBag.IdBodeguero = bodeguero.IdBodeguero;

        ViewBag.Pedidos = await (
            from p in _context.Pedidos
            join pr in _context.Productos on p.CodigoProducto equals pr.CodigoProducto
            join s in _context.Sedes on p.IdSedeSolicitante equals s.IdSede
            where p.EstadoPedido == "Aprobado"
            select new
            {
                Pedido = p,
                Producto = pr.NombreProducto,
                Sede = s.NombreSede
            }
        ).ToListAsync();

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarDespacho(int idPedido, int idBodeguero)
    {
        var pedido = await _context.Pedidos
            .FirstOrDefaultAsync(p => p.IdPedido == idPedido);

        var bodeguero = await _context.Bodegueros
            .FirstOrDefaultAsync(b => b.IdBodeguero == idBodeguero);

        if (pedido == null || bodeguero == null || pedido.EstadoPedido != "Aprobado")
        {
            TempData["Mensaje"] = "No fue posible registrar el despacho.";
            return RedirectToAction("Despachos");
        }

        var inventario = await _context.Inventarios
            .FirstOrDefaultAsync(i =>
                i.IdSede == bodeguero.IdBodegaAsignada &&
                i.CodigoProducto == pedido.CodigoProducto);

        if (inventario == null || inventario.StockActual < pedido.CantidadSolicitada)
        {
            TempData["Mensaje"] = "No hay suficiente inventario para realizar el despacho.";
            return RedirectToAction("Despachos");
        }

        var guia = new GuiaDespacho
        {
            IdPedido = pedido.IdPedido,
            IdBodegueroEmisor = bodeguero.IdBodeguero,
            NumeroGuia = "GD-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
            FechaEmision = DateTime.Now,
            EstadoEntrega = "En tránsito"
        };

        var historial = new Historial
        {
            IdPedido = pedido.IdPedido,
            IdUsuarioResponsable = bodeguero.IdEmpleado,
            EstadoAnterior = pedido.EstadoPedido,
            EstadoActual = "Despachado",
            FechaRegistro = DateTime.Now
        };

        inventario.StockActual -= pedido.CantidadSolicitada;
        inventario.FechaUltimaActualizacion = DateTime.Now;

        pedido.EstadoPedido = "Despachado";

        _context.GuiasDespacho.Add(guia);
        _context.Historiales.Add(historial);

        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "El despacho fue registrado correctamente. Número de guía: " + guia.NumeroGuia;

        return RedirectToAction("Despachos");
    }

    public async Task<IActionResult> Recepciones()
    {
        ViewBag.Empleados = await _context.Empleados.ToListAsync();

        ViewBag.Pedidos = await (
            from g in _context.GuiasDespacho
            join p in _context.Pedidos on g.IdPedido equals p.IdPedido
            join s in _context.Sedes on p.IdSedeSolicitante equals s.IdSede
            where g.EstadoEntrega == "En tránsito"
            select new
            {
                Guia = g,
                Pedido = p,
                Sede = s.NombreSede
            }
        ).ToListAsync();

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarRecepcion(
        int idPedido,
        int idGuiaDespacho,
        int idEmpleadoReceptor,
        int cantidadRecibida)
    {
        var pedido = await _context.Pedidos
            .FirstOrDefaultAsync(p => p.IdPedido == idPedido);

        var guia = await _context.GuiasDespacho
            .FirstOrDefaultAsync(g =>
                g.IdGuiaDespacho == idGuiaDespacho &&
                g.IdPedido == idPedido);

        var empleado = await _context.Empleados
            .FirstOrDefaultAsync(e => e.IdEmpleado == idEmpleadoReceptor);

        if (pedido == null || guia == null || empleado == null ||
            pedido.EstadoPedido != "Despachado" ||
            guia.EstadoEntrega != "En tránsito" ||
            empleado.IdSedeAsignada != pedido.IdSedeSolicitante)
        {
            TempData["Mensaje"] = "No fue posible registrar la recepción. Verifica los datos.";
            return RedirectToAction("Recepciones");
        }

        if (cantidadRecibida != pedido.CantidadSolicitada)
        {
            TempData["Mensaje"] = "La cantidad recibida no coincide con la cantidad indicada en la guía.";
            return RedirectToAction("Recepciones");
        }

        var inventario = await _context.Inventarios
            .FirstOrDefaultAsync(i =>
                i.IdSede == pedido.IdSedeSolicitante &&
                i.CodigoProducto == pedido.CodigoProducto);

        if (inventario == null)
        {
            TempData["Mensaje"] = "No existe un registro de inventario para este producto en la sede.";
            return RedirectToAction("Recepciones");
        }

        var recepcion = new Recepcion
        {
            IdPedido = pedido.IdPedido,
            IdGuiaDespacho = guia.IdGuiaDespacho,
            IdEmpleadoReceptor = empleado.IdEmpleado,
            CantidadRecibida = cantidadRecibida,
            Conformidad = true,
            FechaRecepcion = DateTime.Now
        };

        var historial = new Historial
        {
            IdPedido = pedido.IdPedido,
            IdUsuarioResponsable = empleado.IdEmpleado,
            EstadoAnterior = pedido.EstadoPedido,
            EstadoActual = "Recibido",
            FechaRegistro = DateTime.Now
        };

        inventario.StockActual += cantidadRecibida;
        inventario.FechaUltimaActualizacion = DateTime.Now;

        pedido.EstadoPedido = "Recibido";
        guia.EstadoEntrega = "Recibido";

        _context.Recepciones.Add(recepcion);
        _context.Historiales.Add(historial);

        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "La recepción fue registrada correctamente.";

        return RedirectToAction("Recepciones");
    }

    public async Task<IActionResult> HistorialPedidos(int? idSede)
    {
        ViewBag.Sedes = await _context.Sedes.ToListAsync();

        if (idSede == null)
        {
            idSede = await _context.Sedes
                .Select(s => (int?)s.IdSede)
                .FirstOrDefaultAsync();
        }

        ViewBag.IdSede = idSede;

        ViewBag.Pedidos = await (
            from p in _context.Pedidos
            join pr in _context.Productos on p.CodigoProducto equals pr.CodigoProducto
            join e in _context.Empleados on p.IdEmpleadoSolicitante equals e.IdEmpleado
            where p.IdSedeSolicitante == idSede
            orderby p.FechaSolicitud descending
            select new
            {
                Pedido = p,
                Producto = pr.NombreProducto,
                Empleado = e.IdEmpleado
            }
        ).ToListAsync();

        return View();
    }



















































}
