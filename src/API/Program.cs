using Infra;
using Microsoft.EntityFrameworkCore;
using Produtos_rabbitMq.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices();

builder.Services.AddInfra(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<Infra.Context.AppDbContext>();

        // Garante que o banco seja criado e as migrations aplicadas
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
            Console.WriteLine("--> Migrations aplicadas com sucesso.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"--> Erro ao aplicar migrations: {ex.Message}");
    }
}

app.Run();
