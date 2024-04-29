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

    public async Task<Product?> GetProductById(int id)
    {
        await using var connection = await GetConnection();
        var command = new SqlCommand(
            @"SELECT * FROM Product WHERE IdProduct = @id",
            connection
        );

        command.Parameters.AddWithValue("@id", id);
        var reader = await command.ExecuteReaderAsync();
        
        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new Product
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Description = reader.GetString(2),
            Price = reader.GetDecimal(3)
        };
    }

    public async Task<Order?> GetOrderByProductIdAndAmount(int id, int amount)
    {
        await using var connection = await GetConnection();
        var command = new SqlCommand(
            @"SELECT * FROM [Order] WHERE IdProduct = @id AND Amount = @amount",
            connection
        );

        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@amount", amount);
        var reader = await command.ExecuteReaderAsync();
        
        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new Order()
        {
            Id = reader.GetInt32(0),
            ProductId = reader.GetInt32(1),
            Amount = reader.GetInt32(2),
            CreatedAt = reader.GetDateTime(3),
            FulfilledAt = reader.IsDBNull(4) ? null : reader.GetDateTime(4)
        };
    }

    public async Task<ProductWarehouse?> GetProductWarehouseByOrderId(int orderId)
    {
        await using var connection = await GetConnection();
        var command = new SqlCommand(
            @"SELECT * FROM Product_Warehouse WHERE IdOrder = @id",
            connection
        );

        command.Parameters.AddWithValue("@id", orderId);
        var reader = await command.ExecuteReaderAsync();
        
        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new ProductWarehouse()
        {
            Id = reader.GetInt32(0),
            ProductId = reader.GetInt32(1),
            OrderId = reader.GetInt32(2),
            WarehouseId = reader.GetInt32(3),
            Amount = reader.GetInt32(4),
            Price = reader.GetDecimal(5),
            CreatedAt = reader.GetDateTime(6)
        };
    }

    public async void UpdateOrderFulfilledAt(DateTime now)
    {
        await using var connection = await GetConnection();
        var command = new SqlCommand(
            @"UPDATE [ORDER] SET FulfilledAt = @date",
            connection
        );
        command.Parameters.AddWithValue("@date", now);
        await command.ExecuteNonQueryAsync();
    }
}