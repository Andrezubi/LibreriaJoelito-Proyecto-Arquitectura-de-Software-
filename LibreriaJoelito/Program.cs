using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Aplicacion.Servicios;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Dominio.Validators;
using LibreriaJoelito.Infraestructura.Encryptacion;
using LibreriaJoelito.Infraestructura.FactoryCreators;
using LibreriaJoelito.Infraestructura.Persistencia;
using LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts;
using LibreriaJoelito.Infraestructura.ServiciosExternos;

var builder = WebApplication.CreateBuilder(args);

// Registro de IEmailService
builder.Services.AddTransient<IEmailService, EmailService>();

// Registro de Password Hasher
builder.Services.AddTransient<IPasswordHasher, LibreriaJoelito.Infraestructura.Encryptacion.SimpleHasher>();

//Dependency inyection IRepository Empleados
builder.Services.AddScoped<IRepository<Usuario>>(provider => {
    return new UsuarioCreatorRepository().CreateRepository();
});
builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();

//Dependency inyection IRepository Clientes
builder.Services.AddScoped<IRepository<Cliente>>(provider =>
{
    return new ClienteRepositoryCreator().CreateRepository();
});

//Dependency inyection IRepository Marcas
builder.Services.AddScoped<IRepository<Marca>>(provider => {
    return new MarcaCreatorRepository().CreateRepository();
});

// Add services to the container.
builder.Services.AddRazorPages();

//Dependency inyection IRepository Productos
builder.Services.AddScoped<IRepository<Producto>>(provider => {
    return new ProductoCreatorRepository().CreateRepository();
});

//Dependency inyection token service
builder.Services.AddScoped<ITokenService, TokenService>();

// Validadores
builder.Services.AddScoped<ClienteValidator>();
builder.Services.AddScoped<ProductValidator>();
builder.Services.AddScoped<MarcaValidator>();

// Servicios de Aplicación
builder.Services.AddScoped<ClienteServicio>();
builder.Services.AddScoped<ProductoServicio>();
builder.Services.AddScoped<UsuarioServicio>();
builder.Services.AddScoped<MarcaServicio>();

var app = builder.Build();

var bd = RepositorioBD.Instancia;

// select connection string from appsettings
var connectionString = builder.Configuration.GetConnectionString("ConnectionMySql");
bd.Initiate(connectionString);

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
