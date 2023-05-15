using AsistenteCompras_Entities.Entities;
using AsistenteCompras_Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IEventoService,EventoService>();
builder.Services.AddScoped<IOfertaService, OfertaService>();
builder.Services.AddScoped<AsistenteComprasContext>();



var app = builder.Build();

app.UseCors(builder =>
{
    builder.AllowAnyHeader()
           .AllowAnyMethod()
           .AllowAnyOrigin();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
