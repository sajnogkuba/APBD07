namespace APBD07.Models;

public class ProductWarehouse
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int OrderId { get; set; }
    public int WarehouseId { get; set; }
    public int Amount { get; set; }
    public double Price { get; set; }
    public DateTime CreatedAt { get; set; }
    
}