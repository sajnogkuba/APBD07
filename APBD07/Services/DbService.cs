using APBD07.Interfaces;
using APBD07.Models;

namespace APBD07.Services;

public class DbService(IConfiguration configuration) : IDbService
{
    public Task<ProductWarehouse> CreateProductWareHouse()
    {
        throw new NotImplementedException();
    }
}