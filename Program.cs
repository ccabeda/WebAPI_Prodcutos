using Microsoft.EntityFrameworkCore;
using WebApi_Proyecto_Final.Database;
using WebApi_Proyecto_Final.Models;
using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.Mappers;
using WebApi_Proyecto_Final.Repository;
using WebApi_Proyecto_Final.Repository.IRepository;
using WebApi_Proyecto_Final.Services;
using WebApi_Proyecto_Final.Services.IService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AplicationDbContext>(option => //Aquí se establece cómo el contexto de la base de datos se conectará a la base de datos,                                                             //qué proveedor de base de datos se utilizará y otra configuración relacionada con la conexión.
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
});

//service
builder.Services.AddScoped<IServiceUsuario, ServiceUsuario>();
builder.Services.AddScoped<IServiceVenta, ServiceVenta>();
builder.Services.AddScoped<IServiceProducto, ServiceProducto>();
builder.Services.AddScoped<IServiceProductoVendido, ServiceProductoVendido>();
builder.Services.AddScoped<IServiceNombre, ServiceNombre>();
//repository
builder.Services.AddScoped<IRepositoryVenta, RepositoryVenta>();
builder.Services.AddScoped<IRepositoryProductoVendido, RepositoryProductoVendido>();
builder.Services.AddScoped<IRepositoryProducto, RepositoryProducto>();
builder.Services.AddScoped<IRepositoryUsuario, RepositoryUsuario>();
//automapper
builder.Services.AddAutoMapper(typeof(AutomapperConfig));
//APIResponse (para que todos los endpoints devuelvan lo mismo)
builder.Services.AddScoped<APIResponse>();

//conexión con el frontend
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
