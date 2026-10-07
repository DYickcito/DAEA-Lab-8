using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers;

public class PrestamosController : Controller
{
    private readonly IPrestamoRepositorio _prestamoRepositorio;

    public PrestamosController(IPrestamoRepositorio prestamoRepositorio)
    {
        _prestamoRepositorio = prestamoRepositorio;
    }

    // GET: /Prestamos -> Redirige al reporte
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Reporte));
    }

    // GET: /Prestamos/Reporte?desde=2026-09-01&hasta=2026-10-07
    public async Task<IActionResult> Reporte(DateTime? desde, DateTime? hasta)
    {
        var reporte = await _prestamoRepositorio.GetReporteAsync(desde, hasta);

        // Conservar el rango elegido en ViewData
        ViewData["Desde"] = desde?.ToString("yyyy-MM-dd");
        ViewData["Hasta"] = hasta?.ToString("yyyy-MM-dd");

        return View(reporte);
    }
}
