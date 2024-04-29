namespace APBD07.DTOs;

public record CreateProductWarehouseDto(int IdProduct, int IdWarehouse, int Amount, DateTime CreatedAt);