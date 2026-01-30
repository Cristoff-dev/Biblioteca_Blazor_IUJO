\# 📚 IUJO Biblioteca

Sistema de gestión de biblioteca desarrollado en \*\*Blazor Server (.NET 9.0.0)\*\* para el \*\*Instituto Universitario Jesús Obrero (IUJO) Barquisimeto\*\*.

Este proyecto académico tiene como objetivo modernizar la administración de la biblioteca, ofreciendo una interfaz clara y funcional que permite gestionar \*\*libros, estudiantes, préstamos y devoluciones\*\* de manera eficiente.

La aplicación refleja la identidad institucional del IUJO mediante el uso de colores, estilos y un diseño coherente.

\---

\## 🚀 Tecnologías utilizadas

El sistema se apoya en un conjunto de tecnologías modernas que garantizan rendimiento, mantenibilidad y escalabilidad:

- .NET 9.0.0 → plataforma principal para ejecutar la aplicación.
- Blazor Server → framework para construir interfaces web interactivas con C# y Razor.
- Entity Framework Core 9.0.0 → ORM para interactuar con la base de datos PostgreSQL.
- PostgreSQL → motor de base de datos relacional utilizado para almacenar la información.
- Bootstrap → librería CSS para estilos responsivos y componentes predefinidos.
- CSS personalizado (app.css) → estilos propios que refuerzan la identidad institucional.

\---

\## ⚙️ Instalación y ejecución

\### Requisitos previos

- Tener instalado .NET SDK 9.0.0
- Contar con Visual Studio 2026 con soporte para Blazor y ASP.NET Core
- Tener un servidor PostgreSQL en ejecución

\### Configuración

En el archivo `appsettings.json` se define la conexión a la base de datos y las reglas de subida de carátulas:

"ConnectionStrings": {

"DefaultConnection": "Host=localhost;Database=bibliotecaiujo;Username=postgres;Password=CualquierContraseña"

},

"Uploads": {

"CoversRelativePath": "uploads/covers",

"AllowedExtensions": [ ".jpg", ".jpeg", ".png" ],

"MaxSizeBytes": 5242880

}

\### Ejecución paso a paso

1. Clonar el repositorio.
1. Abrir la solución en Visual Studio 2026.
1. Ejecutar migraciones para crear la base de datos:

dotnet ef database update

1. Iniciar la aplicación con F5 desde Visual Studio o:

dotnet run

1. Acceder en navegador:

http://localhost:5221

https://localhost:7026

\---

\## 🗂️ Arquitectura del proyecto

\### Program.cs

- Configura Blazor Server con componentes interactivos.
- Registra el BibliotecaContext para conectarse a PostgreSQL.
- Configura las opciones de subida (UploadsOptions).
- Registra los servicios de negocio: LibroService, EstudianteService, PrestamoService, DevolucionService.
- Define middleware: HTTPS, archivos estáticos, routing y antiforgery.
- Renderiza el componente raíz BibliotecaBlazor.Components.App.

\### BibliotecaContext.cs

- Define los DbSets: Libros, Estudiantes, Prestamos, PrestamosDetalles, Devoluciones.
- Configura relaciones:
- Estudiante ↔ Préstamos
- Préstamo ↔ Detalles ↔ Libro
- Préstamo ↔ Devolución
- Convención: nombres en minúsculas para compatibilidad con PostgreSQL.

\---

\## 📑 Modelos

- Estudiante

Representa a los alumnos registrados en la biblioteca.

Campos: cédula (8 dígitos), nombre, apellido, semestre (1–6), carrera, sección.

Incluye soft delete mediante el campo Activo.

- Libro

Representa los libros disponibles.

Campos: título, autor, cantidad disponible, carátula.

Incluye soft delete mediante el campo Activo.

- Prestamo

Representa un préstamo realizado a un estudiante.

Campos: estudiante, tipo (interno/externo), fechas en UTC, observaciones, detalles, flag Devuelto.

- PrestamoDetalle

Relaciona un préstamo con los libros y cantidades solicitadas.

- Devolucion

Representa la devolución de un préstamo.

Campos: relación con préstamo, fecha en UTC, observaciones.

\---

\## 🔧 Servicios

- EstudianteService

CRUD completo con validación de cédula única y soft delete.

Permite buscar estudiantes por cédula o nombre.

- LibroService

CRUD de libros con búsqueda por título o autor.

Soft delete para evitar pérdida de datos.

Gestión de carátulas almacenadas en wwwroot/caratulas.

- PrestamoService
- Regla de negocio: máximo 3 libros por préstamo.
- Control de stock para evitar préstamos superiores a la cantidad disponible.
- Métodos para crear, actualizar y listar préstamos pendientes.

- DevolucionService
- Registro de devoluciones con idempotencia.
- Actualización automática del stock.
- Listado del historial de devoluciones.

\---

\## 🎨 Interfaz de usuario

\### Layout

- Sidebar con navegación: Inicio, Libros, Estudiantes, Préstamos, Devoluciones.
- Encabezado institucional: IUJO Biblioteca.
- Información de integrantes y datos académicos.
- Colores institucionales: rojo #b71c1c, amarillo #ffeb3b.

\### Pages

- Inicio (/) → pantalla de bienvenida con imagen institucional de fondo.
- Libros (/libros) → CRUD de libros con subida de carátulas y búsqueda.
- Estudiantes (/estudiantes) → CRUD de estudiantes con validaciones y búsqueda.
- Préstamos (/prestamos) → gestión de préstamos, regla de máximo 3 libros, tabla de pendientes.
- Devoluciones (/devoluciones) → registro de devoluciones con observaciones y historial.
- Error (/Error) → página de manejo de errores en modo Development.

\---

\## 🌐 Recursos estáticos (wwwroot)

- caratulas/ → carátulas de libros subidas por usuarios.
- img/ → imagen institucional de fondo (iujo-barquisimeto.jpg).
- lib/ → librerías externas (Bootstrap).
- icono.jpg → favicon institucional.
- app.css → estilos globales personalizados (tipografía, botones, validaciones, sidebar).

\---

\## ⚙️ Configuración de ejecución (Properties/launchSettings.json)

- Perfiles:
- http → http://localhost:5221
- https → https://localhost:7026
- Ambiente: ASPNETCORE\_ENVIRONMENT = Development.

\---

\## 📦 Dependencias

- Npgsql.PostgreSQL 9.0.0 → proveedor EF Core para PostgreSQL.
- Microsoft.EntityFrameworkCore 9.0.0 → ORM principal.
- Microsoft.EntityFrameworkCore.Design 9.0.0 → herramientas de migración y diseño.

\---

\## 👥 Créditos

Proyecto desarrollado por:

- Cristofer Aranguren

Instituto Universitario Jesús Obrero – Barquisimeto

Semestre 4, Sección A
