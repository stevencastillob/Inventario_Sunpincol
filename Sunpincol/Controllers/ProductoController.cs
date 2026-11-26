
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sunpincol.Models;

namespace Sunpincol.Controllers;

public class ProductoController : Controller
{
    private readonly BDSunpincolContext _context;

    public ProductoController(BDSunpincolContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Productos.ToListAsync());
    }

    public async Task<IActionResult> Details(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var producto = await _context.Productos
            .FirstOrDefaultAsync(p => p.CodigoProducto == id);

        if (producto == null)
        {
            return NotFound();
        }

        return View(producto);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CodigoProducto,NombreProducto,Categoria,Descripcion,PrecioUnitario,EstadoProducto")] Producto producto)
    {
        if (ModelState.IsValid)
        {
            _context.Add(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(producto);
    }

    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var producto = await _context.Productos.FindAsync(id);

        if (producto == null)
        {
            return NotFound();
        }

        return View(producto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("CodigoProducto,NombreProducto,Categoria,Descripcion,PrecioUnitario,EstadoProducto")] Producto producto)
    {
        if (id != producto.CodigoProducto)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(producto);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(producto.CodigoProducto))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        return View(producto);
    }

    public async Task<IActionResult> Delete(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var producto = await _context.Productos
            .FirstOrDefaultAsync(p => p.CodigoProducto == id);

        if (producto == null)
        {
            return NotFound();
        }

        return View(producto);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var producto = await _context.Productos.FindAsync(id);

        if (producto != null)
        {
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool ProductoExists(string id)
    {
        return _context.Productos.Any(p => p.CodigoProducto == id);
    }
}
