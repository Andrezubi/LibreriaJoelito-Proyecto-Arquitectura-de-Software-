using LibreriaJoelito.FactoryCreators;
using LibreriaJoelito.FactoryProducts;
using LibreriaJoelito.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IRepository<Empleado>>(provider => {
    return new EmpleadoCreateRepository().CreateRepository();
});

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

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
