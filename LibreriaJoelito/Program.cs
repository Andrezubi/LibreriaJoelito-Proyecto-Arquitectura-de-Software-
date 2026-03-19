
using LibreriaJoelito;
using LibreriaJoelito.FactoryCreators;
using LibreriaJoelito.FactoryProducts;
using LibreriaJoelito.Models;


var builder = WebApplication.CreateBuilder(args);

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
