using APBD07.Endpoints;
using APBD07.Interfaces;
using APBD07.Services;
using APBD07.Validators;
using FluentValidation;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDbService, DbService>();

// Register validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductWarehouseValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Register Endpoints
app.RegisterProductWarehouseEndpoints();

app.Run();