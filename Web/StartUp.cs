using Application;
using Application.OrderItems.Commands.CreateOrderItem;
using Application.OrderItems.Queries.GetOrderItemById;
using Carter;
using Domain.Interfaces;
using Infrastructure.Data;
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
        services.AddScoped<IOrderItemsRepository, OrderItemsRepository>();

        // Routing modules
        services.AddCarter();

        // Add services to the container.
        services.AddControllers().AddApplicationPart(presentationAssembly);

        // TEST
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyRefference).Assembly));

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();
    }
}
