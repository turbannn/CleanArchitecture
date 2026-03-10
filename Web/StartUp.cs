using Application;
using Application.OrderItems.Commands.CreateOrderItem;
using Application.OrderItems.Commands.UpdateOrderItem;
using Application.OrderItems.Dto;
using Application.OrderItems.Queries.GetOrderItemById;
using Application.Orders.Commands.CreateOrder;
using Application.Orders.Commands.UpdateOrder;
using Carter;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Mappers;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Web;

public class StartUp
{
    private readonly IConfiguration _configuration;

    public StartUp(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<OrdersDbContext>(options => options.UseNpgsql(connectionString));

        var presentationAssembly = typeof(Presentation.WeatherForecast).Assembly;

        // Repos
        services.AddScoped<IOrdersRepository, OrdersRepository>();
        services.AddScoped<IOrderItemsRepository, OrderItemsRepository>();

        // Routing modules
        services.AddCarter();

        // Add services to the container.
        services.AddControllers().AddApplicationPart(presentationAssembly);

        // Mappers
        services.AddScoped<IMapper<CreateOrderItemCommand, OrderItem>, GenericMapper<CreateOrderItemCommand, OrderItem>>();
        services.AddScoped<IMapper<UpdateOrderItemCommand, OrderItem>, GenericMapper<UpdateOrderItemCommand, OrderItem>>();
        services.AddScoped<IMapper<CreateOrderCommand, Order>, GenericMapper<CreateOrderCommand, Order>>();
        services.AddScoped<IMapper<UpdateOrderCommand, Order>, GenericMapper<UpdateOrderCommand, Order>>();
        services.AddScoped<IMapper<CreateOrderItemDto, OrderItem>, GenericMapper<CreateOrderItemDto, OrderItem>>();

        // TEST
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyRefference).Assembly));

        //Validators
        services.AddValidatorsFromAssembly(typeof(Application.AssemblyRefference).Assembly);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();
    }
}
