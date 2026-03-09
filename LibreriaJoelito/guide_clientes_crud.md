# Guía: Implementación de CRUD de Clientes en Razor Pages

Esta guía detalla los pasos para crear un sistema CRUD para la tabla `clientes` en un nuevo proyecto de ASP.NET Core Razor Pages, siguiendo el patrón del proyecto de ejemplo"Oficina".

## 1. Configuración de la Base de Datos
Primero, crea la base de datos y la tabla ejecutando el siguiente script en tu gestor de MySQL (como MySQL Workbench):

```sql
CREATE DATABASE bdlibreria;
USE bdlibreria;

CREATE TABLE `clientes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(100) NOT NULL,
  `Apellido` varchar(100) NOT NULL,
  `CI` varchar(20) NOT NULL,
  `Email` varchar(150) DEFAULT NULL,
  `EsClienteFrecuente` tinyint(1) NOT NULL DEFAULT '0',
  `Estado` tinyint(1) NOT NULL DEFAULT '1',
  `FechaRegistro` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `UltimaActualizacion` datetime DEFAULT NULL,
  `IdUsuario` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `CI` (`CI`)
);
```

## 2. Dependencias del Proyecto
Necesitas instalar el conector de MySQL. Abre la terminal en tu proyecto y ejecuta:

```bash
dotnet add package MySql.Data
```

## 3. Configuración de la Conexión ([appsettings.json](file:///d:/TORREZ/Escritorio/UNIVERSIDAD/SEMESTRE%201-2026/Arquitectura%20de%20Software/CRUDBasicoRazor/CRUDBasicoRazor/CRUDBasicoRazor/CRUDBasicoRazor/appsettings.json))
Agrega tu cadena de conexión en el archivo [appsettings.json](file:///d:/TORREZ/Escritorio/UNIVERSIDAD/SEMESTRE%201-2026/Arquitectura%20de%20Software/CRUDBasicoRazor/CRUDBasicoRazor/CRUDBasicoRazor/CRUDBasicoRazor/appsettings.json). Asegúrate de poner tu contraseña correcta.

```json
{
  "ConnectionStrings": {
    "MySqlConnection": "Server=localhost;Database=bdlibreria;Uid=root;Pwd=TU_CONTRASEÑA;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

## 4. Crear el Modelo (`Models/Cliente.cs`)
Crea una carpeta llamada `Models` (si no existe) y agrega el archivo `Cliente.cs`:

```csharp
namespace TuProyecto.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string CI { get; set; }
        public string Email { get; set; }
        public bool EsClienteFrecuente { get; set; }
        public byte Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? UltimaActualizacion { get; set; }

        public Cliente() { }

        public Cliente(string nombre, string apellido, string ci, string email)
        {
            Nombre = nombre;
            Apellido = apellido;
            CI = ci;
            Email = email;
        }
    }
}
```

## 5. Página de Listado (`Pages/Clientes.cshtml`)
Crea la página para mostrar los clientes.

### `Pages/Clientes.cshtml.cs` (Lógica)
```csharp
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;

namespace TuProyecto.Pages
{
    public class ClientesModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public DataTable ClientesDataTable { get; set; } = new DataTable();

        public ClientesModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            string connectionString = _configuration.GetConnectionString("MySqlConnection")!;
            string query = "SELECT Id, Nombre, Apellido, CI, Email, FechaRegistro FROM clientes WHERE Estado = 1 ORDER BY Apellido, Nombre";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(ClientesDataTable);
            }
        }
    }
}
```

### `Pages/Clientes.cshtml` (Vista)
```html
@page
@model TuProyecto.Pages.ClientesModel
@{
    ViewData["Title"] = "Listado de Clientes";
}

<h1>Clientes</h1>

<div class="mb-3">
    <a asp-page="ClienteCreate" class="btn btn-primary">Nuevo Cliente</a>
</div>

<table class="table table-striped">
    <thead>
        <tr>
            <th>Nombre</th>
            <th>Apellido</th>
            <th>CI</th>
            <th>Email</th>
            <th>Registro</th>
            <th>Acciones</th>
        </tr>
    </thead>
    <tbody>
        @foreach (System.Data.DataRow row in Model.ClientesDataTable.Rows)
        {
            <tr>
                <td>@row["Nombre"]</td>
                <td>@row["Apellido"]</td>
                <td>@row["CI"]</td>
                <td>@row["Email"]</td>
                <td>@row["FechaRegistro"]</td>
                <td>
                    <a asp-page="ClienteEdit" asp-route-id="@row["Id"]" class="btn btn-warning btn-sm">Editar</a>
                    <a asp-page="ClienteDelete" asp-route-id="@row["Id"]" class="btn btn-danger btn-sm">Eliminar</a>
                </td>
            </tr>
        }
    </tbody>
</table>
```

## 6. Página de Creación (`Pages/ClienteCreate.cshtml`)

### `Pages/ClienteCreate.cshtml.cs` (Lógica)
```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace TuProyecto.Pages
{
    public class ClienteCreateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public string Nombre { get; set; }
        [BindProperty]
        public string Apellido { get; set; }
        [BindProperty]
        public string CI { get; set; }
        [BindProperty]
        public string Email { get; set; }

        public ClienteCreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            string connectionString = _configuration.GetConnectionString("MySqlConnection")!;
            string query = "INSERT INTO clientes (Nombre, Apellido, CI, Email) VALUES (@nombre, @apellido, @ci, @email)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@nombre", Nombre);
                command.Parameters.AddWithValue("@apellido", Apellido);
                command.Parameters.AddWithValue("@ci", CI);
                command.Parameters.AddWithValue("@email", Email);
                command.ExecuteNonQuery();
            }

            return RedirectToPage("Clientes");
        }
    }
}
```

### `Pages/ClienteCreate.cshtml` (Vista)
```html
@page
@model TuProyecto.Pages.ClienteCreateModel
@{
    ViewData["Title"] = "Nuevo Cliente";
}

<h1>Registrar Cliente</h1>

<form method="post">
    <div class="mb-3">
        <label>Nombre</label>
        <input asp-for="Nombre" class="form-control" required />
    </div>
    <div class="mb-3">
        <label>Apellido</label>
        <input asp-for="Apellido" class="form-control" required />
    </div>
    <div class="mb-3">
        <label>CI</label>
        <input asp-for="CI" class="form-control" required />
    </div>
    <div class="mb-3">
        <label>Email</label>
        <input asp-for="Email" type="email" class="form-control" />
    </div>
    <button type="submit" class="btn btn-success">Guardar</button>
    <a asp-page="Clientes" class="btn btn-secondary">Cancelar</a>
</form>
```
