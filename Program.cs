using Microsoft.EntityFrameworkCore;
using WebApi_Proyecto_Final.Database;
using WebApi_Proyecto_Final.Models.APIResponse;
using WebApi_Proyecto_Final.Mappers;
using WebApi_Proyecto_Final.Repository;
using WebApi_Proyecto_Final.Repository.IRepository;
using WebApi_Proyecto_Final.Services;
using WebApi_Proyecto_Final.Services.IService;
using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using WebApi_Proyecto_Final.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AplicationDbContext>(option => //Aqu� se establece c�mo el contexto de la base de datos se conectar� a la base de datos,                                                             //qu� proveedor de base de datos se utilizar� y otra configuraci�n relacionada con la conexi�n.
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
//fluent validation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation();
//unit of work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//conexi�n con el frontend (utilize un frontEnd echo en React)
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
