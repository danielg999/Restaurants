using Restaurants.Infrastructure.Extensions;
using Restaurants.Application.Extensions;
using Restaurants.Infrastructure.Seeders;
using Serilog;
using Restaurants.API.Middlewares;
using Restaurants.Domain.Entities;
using Restaurants.API.Extensions;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.AddPresentation();
    builder.Services.AddApplication();
    builder.Services.AddInfrastracture(builder.Configuration);

    var app = builder.Build();

    var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetService<IRestaurantSeeder>();
    await seeder!.Seed();

    // Configure the HTTP request pipeline.
    app.UseCors("AllowFrontend");
    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMiddleware<RequestTimeLoggingMiddleware>();

    app.UseSerilogRequestLogging(); // to capture request logging on API

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.MapGroup("api/identity")
        .WithTags("Identity")
        .MapIdentityApi<User>();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "Application startup failed");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }