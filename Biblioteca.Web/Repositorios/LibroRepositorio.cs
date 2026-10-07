using System.Data;
using Dapper;
using Biblioteca.Web.Models;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Repositorios;

public class LibroRepositorio : ILibroRepositorio
{
    private readonly string _connectionString;

    public LibroRepositorio(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("BibliotecaDB")
            ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'BibliotecaDB' en appsettings.json.");
    }

    public async Task<IEnumerable<Libro>> GetAllAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<Libro>(
            "usp_Libros_Listar",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Libro>> SearchByTitleAsync(string? titulo)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<Libro>(
            "usp_Libros_BuscarPorTitulo",
            new { Titulo = titulo },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Libro?> GetByIdAsync(int libroId)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<Libro>(
            "usp_Libros_ObtenerPorId",
            new { LibroId = libroId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task InsertAsync(Libro libro)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(
            "usp_Libros_Insertar",
            new { libro.Titulo, libro.ISBN, libro.AutorId, libro.Ejemplares },
            commandType: CommandType.StoredProcedure);
    }

    public async Task UpdateAsync(Libro libro)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(
            "usp_Libros_Actualizar",
            new { libro.LibroId, libro.Titulo, libro.ISBN, libro.AutorId, libro.Ejemplares },
            commandType: CommandType.StoredProcedure);
    }

    public async Task DeleteAsync(int libroId)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(
            "usp_Libros_Eliminar",
            new { LibroId = libroId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Autor>> GetAutoresActivosAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<Autor>(
            "usp_Autores_ListarActivos",
            commandType: CommandType.StoredProcedure);
    }
}
