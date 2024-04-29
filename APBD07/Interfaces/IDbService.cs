using APBD07.Models;

namespace APBD07.Interfaces;

public interface IDbService
{
    Task<ProductWarehouse> CreateProductWareHouse();
    Task<Warehouse?> GetWarehouseById(int id);
}