using APBD07.DTOs;
using APBD07.Interfaces;
using FluentValidation;

namespace APBD07.Endpoints;

public static class ProductWarehouseEndpoints
{
    public static void RegisterProductWarehouseEndpoints(this WebApplication app)
    {
        var productWarehouse = app.MapGroup("ProductWarehouse");
        productWarehouse.MapPost("", AddProductWarehouse);
    }

    private static async Task<IResult> AddProductWarehouse(
        CreateProductWarehouseDto request,
        IConfiguration configuration,
        IValidator<CreateProductWarehouseDto> validator,
        IDbService db)
        
    {
        var validate = await validator.ValidateAsync(request);
        if (!validate.IsValid)
        {
            return Results.ValidationProblem(validate.ToDictionary());
        }

        var warehouse = await db.GetWarehouseById(request.IdWarehouse);
        if (warehouse is null)
        {
            return Results.NotFound($"Warehouse with id: {request.IdWarehouse} does not exist");
        }

        var product = await db.GetProductById(request.IdProduct);
        if (product is null)
        {
            return Results.NotFound($"Product with id: {request.IdProduct} does not exist");
        }

        if (request.Amount < 1)
        {
            return Results.BadRequest($"Amount must be bigger than 0, you typed: {request.Amount} ");
        }


        var order = await db.GetOrderByProductIdAndAmount(request.IdProduct, request.Amount);
        if (order is null)
        {
            return Results.NotFound($"Order with product id: {request.IdProduct} " +
                                    $"and amount: {@request.Amount} does not exist");
        }

        if (order.CreatedAt > request.CreatedAt)
        {
            return Results.BadRequest($"Your date must be later than the date on the order: {order.CreatedAt} " +
                                      $"You type in: {request.CreatedAt}");
        }

        var productWarehouse = await db.GetProductWarehouseByOrderId(order.Id);
        if (productWarehouse is not null)
        {
            return Results.Conflict($"The order with id {order.Id} is already completed.");
        }

        db.UpdateOrderFulfilledAt(DateTime.Now);
        var createdProductWarehouse = await db.InsertToProductWarehouse(request.IdWarehouse, request.IdProduct, order.Id, request.Amount, DateTime.Now);

        return Results.Created(
            "ProductWarehouse",
            $"Generated Product_Warehouse Id: {createdProductWarehouse.Id}"
        );
    }
}