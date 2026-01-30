using BibliotecaBlazor.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Configuración de Blazor Server con componentes interactivos
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 🔹 Configuración de EF Core con PostgreSQL
builder.Services.AddDbContext<BibliotecaContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 Configuración de opciones para subir carátulas
builder.Services.Configure<UploadsOptions>(builder.Configuration.GetSection("Uploads"));

// 🔹 Registro de servicios de la biblioteca
builder.Services.AddScoped<LibroService>();
builder.Services.AddScoped<EstudianteService>();
builder.Services.AddScoped<PrestamoService>();
builder.Services.AddScoped<DevolucionService>();

var app = builder.Build();

// 🔹 Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();

// 🔹 Renderizado de la aplicación principal
app.MapRazorComponents<BibliotecaBlazor.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();
