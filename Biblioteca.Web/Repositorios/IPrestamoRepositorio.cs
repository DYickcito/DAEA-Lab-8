using Biblioteca.Web.Models;

namespace Biblioteca.Web.Repositorios;

public interface IPrestamoRepositorio
{
    Task<IEnumerable<PrestamoReporte>> GetReporteAsync(DateTime? fechaInicio, DateTime? fechaFin);
}
