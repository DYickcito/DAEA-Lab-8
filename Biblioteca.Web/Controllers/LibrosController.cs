using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biblioteca.Web.Controllers;

public class LibrosController : Controller
{
    private readonly ILibroRepositorio _libroRepositorio;

    public LibrosController(ILibroRepositorio libroRepositorio)
    {
        _libroRepositorio = libroRepositorio;
    }

    // GET: /Libros o /Libros?titulo=...
    public async Task<IActionResult> Index(string? titulo)
    {
        IEnumerable<Libro> libros;
        if (string.IsNullOrWhiteSpace(titulo))
        {
            libros = await _libroRepositorio.GetAllAsync();
        }
        else
        {
            libros = await _libroRepositorio.SearchByTitleAsync(titulo.Trim());
        }

        ViewData["TituloBusqueda"] = titulo;
        return View(libros);
    }

    // GET: /Libros/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var libro = await _libroRepositorio.GetByIdAsync(id);
        if (libro == null)
        {
            return NotFound();
        }

        return View(libro);
    }

    // GET: /Libros/Create
    public async Task<IActionResult> Create()
    {
        await CargarAutoresAsync();
        return View();
    }

    // POST: /Libros/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Libro libro)
    {
        if (!ModelState.IsValid)
        {
            await CargarAutoresAsync(libro.AutorId);
            return View(libro);
        }

        await _libroRepositorio.InsertAsync(libro);

        TempData["Mensaje"] = "Libro registrado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Libros/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var libro = await _libroRepositorio.GetByIdAsync(id);
        if (libro == null)
        {
            return NotFound();
        }

        await CargarAutoresAsync(libro.AutorId);
        return View(libro);
    }

    // POST: /Libros/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Libro libro)
    {
        if (!ModelState.IsValid)
        {
            await CargarAutoresAsync(libro.AutorId);
            return View(libro);
        }

        await _libroRepositorio.UpdateAsync(libro);

        TempData["Mensaje"] = "Libro actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Libros/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var libro = await _libroRepositorio.GetByIdAsync(id);
        if (libro == null)
        {
            return NotFound();
        }

        return View(libro);
    }

    // POST: /Libros/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _libroRepositorio.DeleteAsync(id);

        TempData["Mensaje"] = "Libro eliminado correctamente (eliminación lógica).";
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarAutoresAsync(int? autorIdSeleccionado = null)
    {
        var autores = await _libroRepositorio.GetAutoresActivosAsync();
        ViewData["Autores"] = new SelectList(autores, "AutorId", "Nombre", autorIdSeleccionado);
    }
}
