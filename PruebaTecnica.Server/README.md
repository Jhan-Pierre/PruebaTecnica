# Backend API - Gestión de Trabajadores

Este proyecto contiene la API RESTful para la aplicación de gestión de trabajadores.

## Arquitectura

El backend sigue una **arquitectura en capas** para separar responsabilidades:

-   **Controllers:** Manejan las peticiones HTTP. Son "delgados" y delegan toda la lógica a la capa de servicio.
-   **Services:** Contienen la lógica de negocio (ej. validación de DNI duplicado). Interactúan con los repositorios.
-   **Repositories:** Capa de acceso a datos que ejecuta los Stored Procedures a través de Entity Framework Core. Es la única capa que interactúa con el `DbContext`.
-   **Models y DTOs:** Definen las entidades y los objetos de transferencia de datos.

## Endpoints Principales

-   `GET /api/trabajadores`: Lista todos los trabajadores (acepta filtro por `?sexo=`).
-   `GET /api/trabajadores/{id}`: Obtiene un trabajador por su ID.
-   `POST /api/trabajadores`: Crea un nuevo trabajador.
-   `PUT /api/trabajadores/{id}`: Actualiza un trabajador existente.
-   `DELETE /api/trabajadores/{id}`: Elimina un trabajador.
-   `GET /api/ubicacion/...`: Endpoints para obtener Departamentos, Provincias y Distritos.

## Documentación de la API (Swagger)

Una vez que la aplicación esté en ejecución en modo de desarrollo, puedes acceder a la documentación interactiva de la API a través de Swagger.

Navega a: `https://localhost:7001/swagger/index.html`

Desde esta interfaz, puedes ver todos los endpoints disponibles, sus parámetros, y probarlos directamente desde el navegador.
