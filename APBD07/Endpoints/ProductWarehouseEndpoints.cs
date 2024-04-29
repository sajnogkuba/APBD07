using APBD07.DTOs;
using APBD07.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace APBD07.Endpoints;

public static class ProductWarehouseEndpoints
{
    public static void RegisterProductWarehouseEndpoints(this WebApplication app)
    {
        var productWarehouse = app.MapGroup("Product_Warehouse");
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

        return Results.Created();
    }
}