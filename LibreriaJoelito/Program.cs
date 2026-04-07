using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Aplicacion.Servicios;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Infraestructura.Encryptacion;
using LibreriaJoelito.Infraestructura.FactoryCreators;
using LibreriaJoelito.Infraestructura.Persistencia;
using LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts;




var builder = WebApplication.CreateBuilder(args);

//Dependency inyection IRepository Empleados
builder.Services.AddScoped<IRepository<Usuario>>(provider => {
    return new UsuarioCreatorRepository().CreateRepository();
});
builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddTransient<IServicioUsuario, ServicioUsuario>();
builder.Services.AddScoped<IPasswordHasher, SimpleHasher>();

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

//Dependency inyection token service
builder.Services.AddScoped<ITokenService, TokenService>();

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
