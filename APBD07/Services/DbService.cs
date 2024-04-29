using System.Data;
using System.Data.SqlClient;
using APBD07.Interfaces;
using APBD07.Models;

namespace APBD07.Services;

public class DbService(IConfiguration configuration) : IDbService
{
    private async Task<SqlConnection> GetConnection()
    {
        var connection = new SqlConnection(configuration.GetConnectionString("Default"));
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        return connection;
    }

    public Task<ProductWarehouse> CreateProductWareHouse()
    {
        throw new NotImplementedException();
    }

    public async Task<Warehouse?> GetWarehouseById(int id)
    {
        await using var connection = await GetConnection();
        var command = new SqlCommand(
            @"SELECT * FROM Warehouse WHERE IdWarehouse = @id",
            connection
            );

        command.Parameters.AddWithValue("@id", id);
        var reader = await command.ExecuteReaderAsync();
        
        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new Warehouse
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Address = reader.GetString(2)
        };
    }
}