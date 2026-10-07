using System.Data;
using Dapper;
using Biblioteca.Web.Models;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Repositorios;

public class PrestamoRepositorio : IPrestamoRepositorio
{
    private readonly string _connectionString;

    public PrestamoRepositorio(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("BibliotecaDB")
            ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'BibliotecaDB' en appsettings.json.");
    }

    public async Task<IEnumerable<PrestamoReporte>> GetReporteAsync(DateTime? fechaInicio, DateTime? fechaFin)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<PrestamoReporte>(
            "usp_Prestamos_ReportePorFechas",
            new
            {
                FechaInicio = fechaInicio?.ToString("yyyy-MM-dd"),
                FechaFin = fechaFin?.ToString("yyyy-MM-dd")
            },
            commandType: CommandType.StoredProcedure);
    }
}
