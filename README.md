# Prueba Técnica: Aplicación de Gestión de Trabajadores

Esta es una aplicación web full-stack para la gestión de trabajadores, desarrollada como parte de una prueba técnica. La solución completa incluye un backend de API REST con ASP.NET Core 8 y un frontend de Single-Page Application (SPA) con Vue 3.

## Características Generales

-   **Operaciones CRUD completas:** Permite crear, leer, actualizar y eliminar registros de trabajadores.
-   **Interfaz Reactiva:** Construida con Vue 3 y Composition API para una experiencia de usuario fluida.
-   **Backend Robusto:** Sigue una arquitectura en capas (Servicios y Repositorios) para una lógica de negocio limpia y testeable.
-   **Validación Completa:** Incluye validación tanto en el frontend (para feedback inmediato) como en el backend (para garantizar la integridad de los datos).
-   **Diseño Limpio:** Interfaz de usuario implementada con Bootstrap 5.

Para más detalles sobre la arquitectura específica de cada parte, consulta los `README` individuales:
-   [README del Backend](./PruebaTecnica.Server/README.md)
-   [README del Frontend](./pruebatecnica.client/README.md)
---

## Guía de Instalación y Ejecución (Solución Completa)

Sigue estos pasos para configurar y ejecutar la aplicación completa en tu entorno local.

### Prerrequisitos

-   **.NET 8 SDK**
-   **Node.js** (v18.x o superior)
-   **SQL Server** (cualquier edición)
-   **Visual Studio 2022** (recomendado) o **VS Code** con las extensiones de C# y Vue.

### 1. Clonar el Repositorio

```bash
https://github.com/Jhan-Pierre/PruebaTecnica.git
cd PruebaTecnica
```

### 2. Configuración del Backend y la Base de Datos

1.  **Restaurar Base de Datos:**
    -   Abre SQL Server Management Studio (SSMS).
    -   Crea una nueva base de datos llamada `TrabajadoresPrueba`.
    -   Ejecuta el script que se encuentra en `Database/script.sql` sobre la nueva base de datos. Esto creará las tablas y Stored Procedures.

2.  **Configurar Cadena de Conexión:**
    -   Abre el archivo `PruebaTecnica.Server/appsettings.json`.
    -   Modifica la `ConnectionString` para que apunte a tu instancia local de SQL Server.
      ```json
      "ConnectionStrings": {
        "DefaultConnection": "Server=TU_SERVIDOR;Database=TrabajadoresPrueba;Integrated Security=True;TrustServerCertificate=True;"
      }
      ```

### 3. Configuración del Frontend

1.  **Instalar Dependencias de Node.js:**
    -   Abre una terminal y navega a la carpeta del proyecto frontend.
      ```bash
      cd pruebatecnica.client
      ```
    -   Ejecuta el comando para instalar las dependencias:
      ```bash
      npm install
      ```

### 4. Ejecutar la Aplicación

La forma más sencilla es abrir el archivo de solución (`.sln`) en Visual Studio 2022 y presionar `F5`.


Visual Studio se encargará de:
1.  Construir e iniciar el backend de ASP.NET Core.
2.  Construir e iniciar el servidor de desarrollo de Vite para el frontend.
3.  Abrir el navegador en la URL correcta.

> 🔧 **Importante:** Antes de ejecutar la solución, asegúrate de que el proyecto `PruebaTecnica.Server` esté configurado como **proyecto de inicio** en Visual Studio.

Para hacerlo:
1. En el Explorador de Soluciones de Visual Studio, haz clic derecho sobre `PruebaTecnica.Server`.
2. Selecciona la opción **"Establecer como proyecto de inicio"**.

La aplicación estará lista para usarse.



