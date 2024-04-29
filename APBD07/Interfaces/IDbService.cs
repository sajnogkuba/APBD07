using APBD07.Models;

namespace APBD07.Interfaces;

public interface IDbService
{
    Task<Warehouse?> GetWarehouseById(int id);
    Task<Product?> GetProductById(int idProduct);
    Task<Order?> GetOrderByProductIdAndAmount(int idProduct, int amount);
    Task<ProductWarehouse?> GetProductWarehouseByOrderId(int orderId);
    void UpdateOrderFulfilledAt(DateTime date);
    Task<ProductWarehouse?> InsertToProductWarehouse(int warehouseId, int productId, int orderId, int amount, DateTime createdAt);
    Task<ProductWarehouse?> GetLastCreatedProductWarehouse();
}