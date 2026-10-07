using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers;

public class SociosController : Controller
{
    private readonly ISocioRepositorio _socioRepositorio;

    public SociosController(ISocioRepositorio socioRepositorio)
    {
        _socioRepositorio = socioRepositorio;
    }

    // GET: /Socios
    public async Task<IActionResult> Index()
    {
        var socios = await _socioRepositorio.GetAllAsync();
        return View(socios);
    }

    // GET: /Socios/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Socios/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Socio socio)
    {
        // Validación de regla de negocio: el DNI no debe estar duplicado
        if (!string.IsNullOrWhiteSpace(socio.DNI) && await _socioRepositorio.ExistsDniAsync(socio.DNI.Trim()))
        {
            ModelState.AddModelError("DNI", "Ya existe un socio registrado con este número de DNI.");
        }

        if (!ModelState.IsValid)
        {
            return View(socio);
        }

        await _socioRepositorio.InsertAsync(socio);

        TempData["Mensaje"] = "Socio registrado correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
