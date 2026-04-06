using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Infraestructura.FactoryCreators;
using LibreriaJoelito.Infraestructura.Persistencia;
using LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts;
using LibreriaJoelito.Infraestructura.ServiciosExternos;

var builder = WebApplication.CreateBuilder(args);

// Registro de IEmailService
builder.Services.AddTransient<IEmailService, EmailService>();

//Dependency inyection IRepository Empleados
builder.Services.AddScoped<IRepository<Empleado>>(provider => {
    return new EmpleadoCreateRepository().CreateRepository();
});

//Dependency inyection IRepository Clientes
builder.Services.AddScoped<IRepository<Cliente>>(provider =>
{
    return new ClienteRepositoryCreator().CreateRepository();
});

// Add services to the container.
builder.Services.AddRazorPages();

//Dependency inyection IRepository Productos
builder.Services.AddScoped<IRepository<Producto>>(provider => {
    return new ProductoCreatorRepository().CreateRepository();
});

var app = builder.Build();

// select connection string from appsettings
var connectionString = builder.Configuration.GetConnectionString("ConnectionMySql");
RepositorioBD.Initiate(connectionString);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
