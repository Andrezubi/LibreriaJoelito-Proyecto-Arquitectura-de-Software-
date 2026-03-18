<<<<<<< HEAD
using LibreriaJoelito;
=======
using LibreriaJoelito.FactoryCreators;
using LibreriaJoelito.FactoryProducts;
using LibreriaJoelito.Models;
>>>>>>> 019a7adaae8de863a50bc79165c3f778499acbf6

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IRepository<Empleado>>(provider => {
    return new EmpleadoCreateRepository().CreateRepository();
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
