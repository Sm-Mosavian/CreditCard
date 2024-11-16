using CreditCard.Core;


var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureCoreServices();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();

builder.Services.AddApiVersioning();

builder.Services.AddCors(o =>
    o.AddPolicy("CorePolicy", builder =>
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod())
);

var app = builder.Build();

app.UseExceptionHandler("/error");

app.MapControllers();

app.UseCors("CorsPolicy");

app.Run();

public partial class Program { }