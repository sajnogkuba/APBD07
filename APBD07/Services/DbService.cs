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

    public async Task<ProductWarehouse?> InsertToProductWarehouse(int warehouseId, int productId, int orderId, int amount, DateTime createdAt)
    {
        await using var connection = await GetConnection();
        var product = GetProductById(productId);
        var price = product.Result.Price * amount;
        var commandInsert = new SqlCommand("INSERT INTO Product_Warehouse " +
                                           "(IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) " +
                                           "VALUES " +
                                           "(@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt);", 
            connection);
        commandInsert.Parameters.AddWithValue("IdWarehouse", warehouseId);
        commandInsert.Parameters.AddWithValue("IdProduct", productId);
        commandInsert.Parameters.AddWithValue("IdOrder", orderId);
        commandInsert.Parameters.AddWithValue("Amount", amount);
        commandInsert.Parameters.AddWithValue("Price", price);
        commandInsert.Parameters.AddWithValue("CreatedAt", createdAt);
        await commandInsert.ExecuteNonQueryAsync();
        return await GetLastCreatedProductWarehouse();
    }

    public async Task<ProductWarehouse?> GetLastCreatedProductWarehouse()
    {
        await using var connection = await GetConnection();
        var command =
            new SqlCommand(
                "SELECT * FROM Product_Warehouse WHERE IdProductWarehouse =(SELECT MAX(IdProductWarehouse) FROM Product_Warehouse)",
                connection);
        var reader =  await command.ExecuteReaderAsync();
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
}