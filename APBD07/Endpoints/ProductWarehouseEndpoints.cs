using APBD07.DTOs;
using FluentValidation;

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
        IValidator<CreateProductWarehouseDto> validator)
    {
        var validate = await validator.ValidateAsync(request);
        if (!validate.IsValid)
        {
            return Results.ValidationProblem(validate.ToDictionary());
        }

        return Results.Created();
    }
}