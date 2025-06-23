# Prueba Técnica - Gestión de Trabajadores (Backend)

Este repositorio contiene el backend para la aplicación de gestión de trabajadores, desarrollado como parte de una prueba técnica. La API está construida con **ASP.NET Core 8** y sigue una arquitectura en capas para promover la separación de responsabilidades, la mantenibilidad y la testeabilidad.

## Características Principales

-   **API RESTful** para operaciones CRUD completas sobre la entidad `Trabajador`.
-   **Arquitectura en Capas** (Controlador, Servicio, Repositorio) para una clara separación de responsabilidades.
-   **Acceso a Datos** mediante Entity Framework Core, utilizando Stored Procedures para todas las operaciones de base de datos.
-   **Endpoints adicionales** para catálogos de ubicación (Departamentos, Provincias, Distritos).
-   **Filtrado de datos** del lado del servidor (ej. filtrar trabajadores por sexo).
-   **Validación de Modelos** del lado del servidor para garantizar la integridad de los datos.
-   **Documentación de API** con Swagger/OpenAPI habilitada para el entorno de desarrollo.

## Arquitectura del Proyecto

El backend sigue una arquitectura limpia para separar la lógica de la aplicación en las siguientes capas:

-   **Capa de Controladores (`Controllers`):** Responsable de manejar las peticiones HTTP, validar las entradas (a través de `ModelState`) y devolver las respuestas HTTP correspondientes. No contiene lógica de negocio.
-   **Capa de Servicios (`Services`):** Contiene la lógica de negocio de la aplicación. Orquesta las llamadas al repositorio y puede implementar validaciones complejas o flujos de trabajo. Se accede a ella a través de interfaces (`Interfaces/Services`).
-   **Capa de Repositorios (`Repositories`):** Actúa como una capa de abstracción sobre el acceso a datos. Es la única capa que interactúa directamente con el `DbContext` y ejecuta los Stored Procedures. Se accede a ella a través de interfaces (`Interfaces/Repositories`).
-   **Capa de Datos (`Data`):** Contiene el `AppDbContext` de Entity Framework.
-   **Modelos y DTOs (`Models`, `DTOs`):** Define las entidades de la base de datos y los objetos de transferencia de datos utilizados para la comunicación a través de la API.

## Tecnologías Utilizadas

-   **.NET 8**
-   **ASP.NET Core Web API**
-   **Entity Framework Core 8**
-   **SQL Server**
-   **Swagger / OpenAPI** para documentación de API.

---

## Guía de Instalación y Configuración

Sigue estos pasos para configurar y ejecutar el proyecto en tu entorno local.

### Prerrequisitos

-   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
-   [SQL Server](https://www.microsoft.com/es-es/sql-server/sql-server-downloads) (Express, Developer o cualquier otra edición).
-   Un IDE como [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) o [Visual Studio Code](https://code.visualstudio.com/).
-   [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) o Azure Data Studio.

### 1. Clonar el Repositorio

```bash
git clone https://github.com/Jhan-Pierre/PruebaTecnica.git
cd PruebaTecnica
```

### 2. Configuración de la Base de Datos

1.  **Abre SSMS** y conéctate a tu instancia de SQL Server.
2.  Crea una nueva base de datos llamada `TrabajadoresPrueba`.
    ```sql
    CREATE DATABASE TrabajadoresPrueba;
    ```
3.  **Ejecuta el script de creación:** Abre el archivo `Database/script.sql` que se encuentra al inicio de este repositorio. Copia todo su contenido y ejecútalo en una nueva consulta sobre la base de datos `TrabajadoresPrueba`. Esto creará todas las tablas y Stored Procedures necesarios.

### 3. Configurar la Cadena de Conexión

1.  En la raíz del proyecto del servidor (ej. `PruebaTecnica.Server`), busca y abre el archivo `appsettings.json`.
2.  **Modifica la `ConnectionString`** para que apunte a tu instancia de SQL Server.

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=NOMBRE_DE_TU_SERVIDOR;Database=TrabajadoresPrueba;User Id=TU_USUARIO;Password=TU_CONTRASENA;Integrated Security=True;TrustServerCertificate=True;"
      }
    }
    ```
    - Reemplaza `NOMBRE_DE_TU_SERVIDOR` por el nombre de tu servidor local (ej. `localhost`, `(localdb)\\mssqllocaldb`, `TU-PC\\SQLEXPRESS`).
    - Reemplaza `TU_USUARIO` por el usuario con el que ingresas a tu servidor local.
    - Reemplaza `TU_CONTRASENA` por la contraseña con la que ingresas a tu servidor local.

### 4. Ejecutar la Aplicación

Puedes ejecutar el proyecto de las siguientes maneras:

-   **Desde Visual Studio:** Simplemente abre el archivo `.sln` y presiona `F5`.
-   **Desde la línea de comandos:** Navega a la carpeta del proyecto del servidor y ejecuta:
    ```bash
    dotnet run
    ```
La API estará disponible en la URL especificada en `Properties/launchSettings.json` (normalmente algo como `https://localhost:7001`).

---

## Documentación de la API (Swagger)

Una vez que la aplicación esté en ejecución en modo de desarrollo, puedes acceder a la documentación interactiva de la API a través de Swagger.

Navega a: `https://localhost:7001/swagger/index.html`

Desde esta interfaz, puedes ver todos los endpoints disponibles, sus parámetros, y probarlos directamente desde el navegador.
