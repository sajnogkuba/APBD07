using APBD07.Models;

namespace APBD07.Interfaces;

public interface IDbService
{
    Task<ProductWarehouse> CreateProductWareHouse();
    Task<Warehouse?> GetWarehouseById(int id);
    Task<Product?> GetProductById(int idProduct);
    Task<Order?> GetOrderByProductIdAndAmount(int idProduct, int amount);
    Task<ProductWarehouse?> GetProductWarehouseByOrderId(int orderId);
}