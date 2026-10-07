using Biblioteca.Web.Models;

namespace Biblioteca.Web.Repositorios;

public interface ISocioRepositorio
{
    Task<IEnumerable<Socio>> GetAllAsync();
    Task InsertAsync(Socio socio);
    Task<bool> ExistsDniAsync(string dni);
}
