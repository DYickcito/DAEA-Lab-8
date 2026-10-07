using System.Data;
using Dapper;
using Biblioteca.Web.Models;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Repositorios;

public class SocioRepositorio : ISocioRepositorio
{
    private readonly string _connectionString;

    public SocioRepositorio(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("BibliotecaDB")
            ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'BibliotecaDB' en appsettings.json.");
    }

    public async Task<IEnumerable<Socio>> GetAllAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<Socio>(
            "usp_Socios_Listar",
            commandType: CommandType.StoredProcedure);
    }

    public async Task InsertAsync(Socio socio)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(
            "usp_Socios_Insertar",
            new { socio.DNI, socio.Nombre, socio.Email },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> ExistsDniAsync(string dni)
    {
        using var connection = new SqlConnection(_connectionString);
        int count = await connection.ExecuteScalarAsync<int>(
            "usp_Socios_ExisteDni",
            new { DNI = dni },
            commandType: CommandType.StoredProcedure);

        return count > 0;
    }
}
