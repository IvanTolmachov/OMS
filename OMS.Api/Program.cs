using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OMS.Api.Handlers;
using OMS.Api.Models;
using OMS.Application.Interfaces;
using OMS.Application.Services;
using OMS.Domain.Entities;
using OMS.Infrastructure;
using OMS.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<OMS.Api.Validators.ProductValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.AddConsole();

// Add other app services as needed

builder.Services.AddSingleton<TimeProvider>(TimeProvider.System);

builder.Services.AddDbContext<OmsDbContext>(options =>
    options.UseInMemoryDatabase("OmsDb"));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductAppService, ProductAppService>();
builder.Services.AddScoped<OMS.Api.Services.IProductService, OMS.Api.Services.ProductService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderAppService, OrderAppService>();
builder.Services.AddScoped<OMS.Api.Services.IOrderService, OMS.Api.Services.OrderService>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<ProductDto, Product>().ReverseMap();
    cfg.CreateMap<OrderDto, Order>().ReverseMap();
    cfg.CreateMap<OrderItemDto, OrderItem>().ReverseMap();
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapDefaultEndpoints();
app.Run();
