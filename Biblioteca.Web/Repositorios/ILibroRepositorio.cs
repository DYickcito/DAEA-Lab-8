using Biblioteca.Web.Models;

namespace Biblioteca.Web.Repositorios;

public interface ILibroRepositorio
{
    Task<IEnumerable<Libro>> GetAllAsync();
    Task<IEnumerable<Libro>> SearchByTitleAsync(string? titulo);
    Task<Libro?> GetByIdAsync(int libroId);
    Task InsertAsync(Libro libro);
    Task UpdateAsync(Libro libro);
    Task DeleteAsync(int libroId);
    Task<IEnumerable<Autor>> GetAutoresActivosAsync();
}
