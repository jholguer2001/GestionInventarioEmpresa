Proyecto Inventario - MyApp
MyApp es una aplicación web completa para la gestión de inventario, construida con una arquitectura N-Capas utilizando .NET y C#. Permite el seguimiento de artículos, la gestión de usuarios y roles, y el control de préstamos de artículos, todo con un sistema de auditoría robusto y generación de reportes.

Tabla de Contenidos
Características Principales

Tecnologías Utilizadas

Arquitectura y Patrones de Diseño

Arquitectura N-Capas

Patrones de Diseño

Instrucciones de Despliegue

Prerrequisitos

Configuración

Ejecución

Estructura del Proyecto

Características Principales
Gestión de Usuarios y Roles: Sistema de autenticación basado en cookies. Roles de Administrator y Operator con diferentes niveles de acceso.

CRUD de Artículos: Creación, lectura, actualización y eliminación de artículos del inventario, incluyendo código, nombre, categoría, estado y ubicación.

Sistema de Préstamos: Flujo de trabajo completo para préstamos: solicitud, aprobación, rechazo, entrega y devolución.

Auditoría Completa: Registro detallado de todas las acciones importantes (creación, actualización, eliminación, login, etc.) con información del usuario, fecha, IP y cambios realizados.

Panel de Control (Dashboard): Vista principal con estadísticas clave del sistema, como número de artículos, préstamos activos, pendientes y vencidos.

Reportes: Generación de reportes en formato PDF y Excel.

PDF: Listado de artículos, estado general del inventario.

Excel: Historial de préstamos, registro de actividad de usuarios.

Seguridad: Hashing de contraseñas con BCrypt, protección Anti-CSRF y manejo seguro de cookies de autenticación.

Tecnologías Utilizadas
Backend: .NET 8 / C#

Framework Web: ASP.NET Core MVC

Base de Datos: SQL Server

ORM: Entity Framework Core 8

Logging: Serilog

Generación de PDF: QuestPDF

Generación de Excel: ClosedXML

Seguridad: BCrypt.Net-Next

Frontend: HTML, CSS, Bootstrap 5, JavaScript

Arquitectura y Patrones de Diseño
El proyecto sigue una arquitectura limpia y desacoplada, organizada en N-Capas para separar responsabilidades y facilitar el mantenimiento y la escalabilidad.

Arquitectura N-Capas
La solución está dividida en las siguientes capas lógicas:

MyApp.Entities (Capa de Entidades)

Propósito: Es el núcleo de la aplicación. Contiene los modelos de dominio (POCOs - Plain Old CLR Objects) que representan los datos, como User, Item, Loan, etc.

Componentes:

Models: Clases de entidad que se mapean a la base de datos.

Interfaces: Contratos como IAuditableEntity que definen propiedades comunes (ej. CreatedDate, CreatedBy).

Enums: Enumeraciones como ItemStatus y LoanStatus para representar estados fijos.

MyApp.DataAccess (Capa de Acceso a Datos)

Propósito: Gestiona toda la comunicación con la base de datos. Abstrae la lógica de persistencia del resto de la aplicación.

Componentes:

Context: El ApplicationDbContext de Entity Framework Core, que representa la sesión con la base de datos.

Repositories: Implementaciones concretas del patrón Repository para cada entidad.

UnitOfWork: Implementación del patrón Unit of Work para gestionar transacciones atómicas.

Configurations: Configuraciones de Fluent API para el mapeo de entidades a tablas.

Interceptors: Interceptores de EF Core, como AuditInterceptor, para automatizar tareas.

MyApp.Business (Capa de Lógica de Negocio)

Propósito: Contiene la lógica de negocio central de la aplicación. Orquesta las operaciones, valida las reglas de negocio y transforma los datos entre la capa de acceso a datos y la de presentación.

Componentes:

Services: Clases que contienen la lógica de negocio (ej. ItemService, LoanService).

DTOs (Data Transfer Objects): Objetos planos utilizados para transferir datos entre capas, evitando exponer las entidades de dominio directamente a la UI.

MyApp.Presentation (Capa de Presentación)

Propósito: Es la interfaz de usuario (UI). En este caso, una aplicación web ASP.NET Core MVC.

Componentes:

Controllers: Reciben las peticiones HTTP, interactúan con los servicios de la capa de negocio y devuelven las vistas.

Views: Archivos Razor (.cshtml) que renderizan el HTML que se envía al cliente.

Models (ViewModels): Modelos específicos para las vistas, que contienen los datos y la lógica de validación necesarios para una UI particular.

Patrones de Diseño
Repository Pattern:

Descripción: Media entre el dominio y las capas de mapeo de datos usando una interfaz similar a una colección para acceder a los objetos de dominio.

Implementación: Se define una interfaz genérica IRepository<T> con operaciones CRUD comunes y repositorios específicos (IItemRepository, IUserRepository) que heredan de la genérica y añaden métodos de consulta propios. Esto desacopla la capa de negocio de la implementación concreta de Entity Framework Core.

Unit of Work Pattern:

Descripción: Mantiene una lista de objetos afectados por una transacción de negocio y coordina la escritura de cambios y la resolución de problemas de concurrencia.

Implementación: La clase UnitOfWork encapsula el ApplicationDbContext y expone todas las interfaces de los repositorios. Proporciona un método SaveChangesAsync() que confirma todas las operaciones (añadir, modificar, eliminar) en una única transacción de base de datos, garantizando la integridad de los datos.

Instrucciones de Despliegue
Sigue estos pasos para configurar y ejecutar el proyecto en un entorno de desarrollo.

Prerrequisitos
SDK de .NET: Asegúrate de tener instalado el SDK de .NET (versión 8.0 o superior).

Servidor de Base de Datos: SQL Server (versión 2017 o superior, incluyendo la edición Express).

IDE (Opcional): Visual Studio 2022 o Visual Studio Code.

Configuración
Clonar el Repositorio:

Bash

git clone <URL_DEL_REPOSITORIO>
cd <NOMBRE_DEL_DIRECTORIO>
Configurar la Cadena de Conexión:

Abre el archivo appsettings.json en el proyecto MyApp.Presentation.

Modifica la cadena de conexión DefaultConnection para que apunte a tu instancia de SQL Server. tanto en appsettings de MyApp.DataAccess y Appsettings de MyApp.Presentation

Ejemplo:

JSON

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=InventarioBD;Trusted_Connection=True;TrustServerCertificate=True;"
  },
}
TU_SERVIDOR: Puede ser . o localhost si es una instancia local, o el nombre de tu servidor (ej. MSI-JAIRO).

InventarioBD: El nombre que deseas para la base de datos. EF Core la creará si no existe.

Ejecución
Restaurar Dependencias:
Abre una terminal en la raíz de la solución y ejecuta:

Bash

dotnet restore
Aplicar Migraciones de la Base de Datos:
Para crear la base de datos y sus tablas, ejecuta los siguientes comandos desde la Consola del Administrador de Paquetes en Visual Studio o desde una terminal en la carpeta raíz del proyecto.

Navega al proyecto MyApp.Presentation (si usas la terminal):

Bash

cd MyApp.Presentation
Aplica las migraciones a la base de datos:

IMPORTANTE: En consola de adinistrador de paquetes ejecutar: 

Add-Migration InitialCreate -Project MyApp.DataAccess -StartupProject MyApp.DataAccess -Context ApplicationDbContext
Update-Database -Project MyApp.DataAccess -StartupProject MyApp.DataAccess -Context ApplicationDbContext


Bash

dotnet ef database update
Esto leerá las migraciones del proyecto DataAccess y creará el esquema en la base de datos que especificaste en la cadena de conexión.

Ejecutar la Aplicación:

Desde Visual Studio, presiona el botón de Play (▶️) o F5.

Desde la terminal (ubicada en la carpeta MyApp.Presentation), ejecuta:

Bash

dotnet run
La aplicación estará disponible en https://localhost:XXXX y http://localhost:YYYY, donde XXXX y YYYY son los puertos indicados en la consola.

Acceder a la Aplicación:

Abre tu navegador y ve a la URL de la aplicación.

Regístrate como un nuevo usuario. Por defecto, el primer usuario registrado obtendrá el rol de "Operator".

Estructura del Proyecto
/MyApp
├── MyApp.Business/
│   ├── Dtos/
│   └── Services/
├── MyApp.DataAccess/
│   ├── Configurations/
│   ├── Context/
│   ├── Repositories/
│   └── UnitOfWork/
├── MyApp.Entities/
│   ├── Enums/
│   ├── Interfaces/
│   └── Models/
└── MyApp.Presentation/
    ├── Controllers/
    ├── Models/ (ViewModels)
    ├── Views/
    └── Program.cs