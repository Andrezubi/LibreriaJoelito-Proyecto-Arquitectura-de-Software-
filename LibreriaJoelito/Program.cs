using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Aplicacion.Servicios;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Dominio.Validators;
using LibreriaJoelito.Infraestructura.FactoryCreators;
using LibreriaJoelito.Infraestructura.Persistencia;
using LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts;
using LibreriaJoelito.Infraestructura.ServiciosExternos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
builder.Services.AddTransient<IClienteRepository, ClienteRepository>();

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
builder.Services.AddTransient<IProductoRepository, ProductoRepository>();

builder.Services.AddScoped<IVentaRepository>(provider => {
    return (IVentaRepository)new VentaCreatorRepository().CreateRepository();
});

builder.Services.AddScoped<IDetalleVentaRepository>(provider => {
    return (IDetalleVentaRepository)new DetalleVentaCreatorRepository().CreateRepository();
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
builder.Services.AddScoped<IVentaService, VentaService>();

// AGREGAR AUTENTICACION
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };

        // 🔥 CLAVE: leer el token desde la cookie
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["AuthToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };
    });



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

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
