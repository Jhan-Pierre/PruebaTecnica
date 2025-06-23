using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Server.Data;
using PruebaTecnica.Server.Interfaces.Repositories;
using PruebaTecnica.Server.Interfaces.Services;
using PruebaTecnica.Server.Repositories;
using PruebaTecnica.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Configuración de Servicios ---

// Añadir servicios para los controladores de la API
builder.Services.AddControllers();

// Configuración de la conexión a la base de datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Inyección de Dependencias
builder.Services.AddScoped<ITrabajadorRepository, TrabajadorRepository>();
builder.Services.AddScoped<ITrabajadorService, TrabajadorService>();
builder.Services.AddScoped<IUbicacionRepository, UbicacionRepository>();
builder.Services.AddScoped<IUbicacionService, UbicacionService>();

// --- Documentación de la API (Swagger) ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// --- 2. Construcción de la Aplicación ---
var app = builder.Build();


// --- 3. Configuración del Pipeline de Peticiones HTTP ---
app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.MapFallbackToFile("index.html");


// --- 4. Ejecución de la Aplicación ---
app.Run();