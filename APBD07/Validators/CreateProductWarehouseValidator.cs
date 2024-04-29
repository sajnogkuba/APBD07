using APBD07.DTOs;
using FluentValidation;

namespace APBD07.Validators;


public class CreateProductWarehouseValidator : AbstractValidator<CreateProductWarehouseDto>
{
    public CreateProductWarehouseValidator()
    {
        RuleFor(e => e.IdProduct).NotNull();
        RuleFor(e => e.IdWarehouse).NotNull();
        RuleFor(e => e.Amount).NotNull();
        RuleFor(e => e.CreatedAt).NotNull();
    }
}