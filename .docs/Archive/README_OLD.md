# 📚 Biblioteca - Sistema de Gestión de Biblioteca

## ⚡ TL;DR - Comienza Aquí

**¿Prisa?** Only 2 commands:

```powershell
.\setup.ps1      # One time setup
.\run_all.ps1    # Run everything
```

**Luego abre**: http://localhost:5098

**Documentación rápida:**
- 🚀 [START_HERE.txt](./START_HERE.txt) - Visual + quick instructions
- 3️⃣ [GETTING_STARTED.md](./GETTING_STARTED.md) - 3 steps
- ⏱️ [QUICK_START.md](./QUICK_START.md) - 5 minutes
- 📖 [EJECUCION_GUIA.md](./EJECUCION_GUIA.md) - Complete reference

---

## Descripción General

Sistema completo de gestión de biblioteca construido con **.NET 10** y **Blazor WebAssembly**, incluyendo API Rest backend y frontend interactivo. Permite administrar autores, libros, préstamos y generar reportes de análisis.

**Versión**: 1.0.0  
**Fecha de Actualización**: Abril 13, 2026  
**Estado**: ✅ Producción Lista  
**Documentación**: [ARCHITECTURE.md](./ARCHITECTURE.md) | [BUGS_ARREGLADOS.md](./BUGS_ARREGLADOS.md) | [EXPERIENCIA_EJECUCION.md](./EXPERIENCIA_EJECUCION.md)

---

## 📋 Tabla de Contenidos

1. [Requisitos](#requisitos)
2. [Instalación](#instalación)
3. [Configuración](#configuración)
4. [Ejecutar Aplicación](#ejecutar-aplicación)
5. [API Documentation](#api-documentation)
6. [Endpoints Especiales](#endpoints-especiales)
7. [Migración de Base de Datos](#migración-de-base-de-datos)
8. [Estructura del Proyecto](#estructura-del-proyecto)
9. [Testing](#testing)
10. [Troubleshooting](#troubleshooting)

---

## � Requisitos

### Software Requerido

- ✅ **.NET 10 SDK** - [Descargar](https://dotnet.microsoft.com/download/dotnet/10.0)
- ✅ **SQL Server LocalDB** - Incluido con Visual Studio 2022 (o [instalación independiente](https://learn.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb))
- ✅ **Visual Studio 2022** (Recomendado) O **VS Code** + extensiones C#/Blazor
- ✅ **Postman** (Opcional, para testing de API)

### Verificar Instalación

```bash
# Verificar .NET 10
dotnet --version
# Esperado: 10.0.x

# Verificar SQL Server LocalDB
sqllocaldb info
# Esperado: MSSQLLocalDB disponible
```

---

## 💻 Instalación

### 1. Descargar/Clonar Proyecto

```bash
cd d:\USER\Documents\Fork\control.pruebas
```

### 2. Restaurar Dependencias

```bash
# Restaurar paquetes NuGet para toda la solución
dotnet restore Biblioteca.sln
```

### 3. Verificar Estructura

```bash
# Verificar que todos los proyecto existen
ls -la
# Esperado: Api/, Core/, Application/, Infrastructure/
```

---

## ⚙️ Configuración

### Connection String - LocalDB

**Archivo:** `Api/appsettings.json` y `Infrastructure/appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BibliotecaDB;Trusted_Connection=true;"
  }
}
```

**Componentes:**
- `Server=(localdb)\mssqllocaldb` → Base de datos local
- `Database=BibliotecaDB` → Nombre de la BD
- `Trusted_Connection=true` → Autenticación Windows (sin usuario/contraseña)

### Alternativas de Connection String

**SQL Server Remoto (Azure):**
```
Server=mi-servidor.database.windows.net;Database=BibliotecaDB;User Id=usuario;Password=contraseña;TrustServerCertificate=true;
```

**Docker SQL Server:**
```
Server=localhost,1433;Database=BibliotecaDB;User Id=sa;Password=YourPassword123;Encrypt=false;
```

**Cambiar Connection String:**
1. Abrir `Api/appsettings.json`
2. Reemplazar valor en `ConnectionStrings.DefaultConnection`
3. Lo mismo en `Infrastructure/appsettings.json`
4. Aplicar migraciones (ver abajo)

---

## 🔄 Migración de Base de Datos

### ✅ Pre-requisitos

- Connection string configurada ✅
- SQL Server LocalDB/SQL Server ejecutándose ✅

### Paso 1: Aplicar Migraciones

```bash
cd d:\USER\Documents\Fork\control.pruebas\Infrastructure

# Aplicar todas las migraciones (crea/actualiza BD)
dotnet ef database update

# Se crearán las tablas:
# - [dbo].[Author]
# - [dbo].[Books]  
# - [dbo].[Loans]
# - [dbo].[__EFMigrationsHistory]
```

### Paso 2: Verificar Creación

**Opción A - SQL Server Management Studio:**
1. Conectar a `(localdb)\mssqllocaldb`
2. Base de datos → BibliotecaDB
3. Verificar tablas: Author, Books, Loans

**Opción B - Azure Data Studio:**
```bash
# Conectar a (localdb)\mssqllocaldb
# Query: SELECT * FROM Author
```

**Opción C - PowerShell:**
```powershell
$connection = New-Object System.Data.SqlClient.SqlConnection
$connection.ConnectionString = "Server=(localdb)\mssqllocaldb;Database=BibliotecaDB;Integrated Security=true;"
$connection.Open()

$command = $connection.CreateCommand()
$command.CommandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'"
$command.ExecuteReader().ReadAll() | Format-Table
```

### Paso 3: Datos Iniciales (Opcional)

```bash
# Crear seeder (opcional para demo)
# Actualmente: BD comienza vacía
```

---

## 🚀 Ejecutar Aplicación

### ⚡ Opción 1: Scripts PowerShell (MÁS FÁCIL)

**Setup inicial (una sola vez):**
```powershell
# En PowerShell, desde la carpeta raíz del proyecto
.\setup.ps1
```

**Ejecutar ambos servicios:**
```powershell
.\run_all.ps1
```

**O ejecutar por separado:**
```powershell
# Terminal 1 - Solo API
.\run_api.ps1

# Terminal 2 - Solo Frontend
.\run_frontend.ps1
```

### ⚡ Opción 2: Terminal Separadas

```bash
# Terminal 1 - Backend (API)
cd Api
dotnet run
# Escuchar en: http://localhost:5088 (HTTP)
```

```bash
# Terminal 2 - Frontend (Blazor)
cd Infrastructure\Frontend
dotnet run
# Escuchar en: http://localhost:5098
```

### 🎨 Opción 3: Visual Studio 2022

1. Abrir `Biblioteca.sln` en Visual Studio
2. **Project** → **Set Startup Projects**
3. Seleccionar:
   - ✅ Api (Action: Start)
   - ✅ Frontend (Action: Start)
4. Presionar `F5` o **Debug** → **Start Debugging**

**Resultado:**
- API abierto en `http://localhost:5088`
- Frontend abierto en `http://localhost:5098`

### 📱 Opción 4: VS Code

```bash
# Abrir carpeta raíz en VS Code
code .

# Terminal integrada (Ctrl+`)
# Terminal 1: cd Api && dotnet run
# Terminal 2: cd Infrastructure\Frontend && dotnet run
```

---

## 📖 API Documentation

### 🔗 Acceder a Swagger UI

Una vez que compiles y ejecutes el backend:

```
🌐 http://localhost:5088/swagger
```

**Ruta exacta:** `http://localhost:5088/swagger/index.html`

**Especificación OpenAPI:** `http://localhost:5088/swagger/v1/swagger.json`

### ✨ Características en Swagger

- ✅ Listado de todos los endpoints
- ✅ Schemas request/response completos
- ✅ Códigos HTTP documentados (200, 201, 400, 404, 409, 500)
- ✅ Botón "Try it out" - Ejecutar requests directamente
- ✅ Ejemplos de request/response
- ✅ Parámetros y validaciones

### 📮 Postman Collection

Para facilitar el testing de la API, una **colección Postman completa** está incluida:

**Archivo:** `Biblioteca.postman_collection.json`

**Cómo usar:**

1. **Descargar Postman** - [postman.com](https://www.postman.com/downloads)
2. **Importar colección:**
   - Abre Postman → "Import" button
   - Selecciona `Biblioteca.postman_collection.json`
3. **Usar endpoints:**
   - Todos los 18+ endpoints están organizados por recurso (Autores, Libros, Préstamos, Reportes)
   - Variables pre-configuradas para URL base (`{{base_url}}`)
   - Ejemplos de request body incluidos

**Endpoints Disponibles:**

- ✅ **Autores**: Crear, Listar, Obtener, Actualizar, Eliminar
- ✅ **Libros**: Crear con upload, Listar, Obtener, Actualizar, Eliminar
- ✅ **Préstamos**: Crear, Listar, Obtener, Devolver (PUT /return)
- ✅ **Reportes**: Resumen de biblioteca

### 📍 Base URL de Endpoints

| Recurso | URL |
|---------|-----|
| **Autores** | `http://localhost:5088/api/author` |
| **Libros** | `http://localhost:5088/api/book` |
| **Préstamos** | `http://localhost:5088/api/loan` |
| **Reportes** | `http://localhost:5088/api/report` |

---

## 🎯 Endpoints Especiales

### 1️⃣ Crear Libro - Upload de Portada (Multipart/Form-Data)

Este endpoint permite subir un libro con su portada.

**Endpoint:** `POST /api/book`

**Content-Type:** `multipart/form-data`

**Campos del Form:**

| Campo | Tipo | Obligatorio | Descripción |
|-------|------|-------------|-------------|
| `title` | string | ✅ | Título del libro |
| `numberOfPages` | integer | ✅ | Cantidad de páginas |
| `authorId` | integer | ✅ | ID del autor (debe existir en BD) |
| `genre` | string | ✅ | Género literario |
| `publishedDate` | date | ✅ | Fecha de publicación (formato: YYYY-MM-DD) |
| `isbn` | string | ❌ | ISBN opcional |
| `file` | file | ❌ | Portada (jpg, png, webp) |

**Ejemplo cURL:**

```bash
curl -X POST http://localhost:5088/api/book \
  -F "title=Cien años de soledad" \
  -F "numberOfPages=417" \
  -F "authorId=1" \
  -F "genre=Realismo mágico" \
  -F "publishedDate=1967-05-30" \
  -F "isbn=978-0060883287" \
  -F "file=@portada.jpg"
```

**Respuesta (201 Created):**

```json
{
  "id": 5,
  "title": "Cien años de soledad",
  "genre": "Realismo mágico",
  "numberOfPages": 417,
  "isbn": "978-0060883287",
  "publishedDate": "1967-05-30T00:00:00",
  "coverImagePath": "/uploads/books/libro-5-portada.jpg",
  "authorId": 1,
  "createdDate": "2026-04-13T10:30:45.123456Z",
  "updatedDate": null
}
```

**Códigos Respuesta:**

| Código | Descripción |
|--------|-------------|
| `201 Created` | ✅ Libro creado exitosamente |
| `400 Bad Request` | ❌ Datos inválidos o formato de archivo no soportado |
| `404 Not Found` | ❌ Author ID no existe en BD |
| `500 Server Error` | ❌ Error al guardar archivo o en BD |

---

### 2️⃣ Devolver Préstamo - `PUT /api/loan/{id}/return`

Marca un préstamo como devuelto. Calcula automáticamente si está vencido (overdue).

**Endpoint:** `PUT /api/loan/{id}/return`

**Parámetros:**

| Parámetro | Ubicación | Tipo | Obligatorio |
|-----------|----------|------|------------|
| `id` | URL Path | integer | ✅ |

**Body:** Vacío (no requiere JSON)

**Ejemplo cURL:**

```bash
curl -X PUT http://localhost:5088/api/loan/3/return \
  -H "Content-Type: application/json"
```

**Respuesta (200 OK):**

```json
{
  "id": 3,
  "bookId": 5,
  "borrowerName": "Juan Perez",
  "loanDate": "2026-04-01T00:00:00",
  "dueDate": "2026-04-15T00:00:00",
  "returnDate": "2026-04-13T14:25:10.456789Z",
  "status": "Returned",
  "createdDate": "2026-04-01T10:00:00Z",
  "updatedDate": "2026-04-13T14:25:10.456789Z"
}
```

**Lógica de Estado:**

```
IF returnDate ≤ dueDate:
   status = "Returned" 
   ✅ Libro devuelto a tiempo

IF returnDate > dueDate:
   status = "Overdue"
   ⚠️  Libro devuelto atrasado
```

**Códigos Respuesta:**

| Código | Descripción |
|--------|-------------|
| `200 OK` | ✅ Préstamo devuelto exitosamente |
| `400 Bad Request` | ❌ Préstamo ya fue devuelto (no se puede devolver 2 veces) |
| `404 Not Found` | ❌ Préstamo con ese ID no existe en BD |
| `500 Server Error` | ❌ Error en la BD |

**Ejemplo - Devolución a Tiempo:**

```json
{
  "loanDate": "2026-04-01",
  "dueDate": "2026-04-15",
  "returnDate": "2026-04-13",
  "status": "Returned"
}
// 2026-04-13 ≤ 2026-04-15 → ✅ A tiempo
```

**Ejemplo - Devolución Vencida:**

```json
{
  "loanDate": "2026-04-01",
  "dueDate": "2026-04-15",
  "returnDate": "2026-04-20",
  "status": "Overdue"
}
// 2026-04-20 > 2026-04-15 → ⚠️ Vencido
```

---

### 3️⃣ Validación - Un Préstamo Activo por Libro

**Regla de Negocio:** Un libro solo puede tener UN préstamo activo (sin devolver).

Si intenta prestar un libro que ya está prestado, el API retorna **409 Conflict**.

**Endpoint:** `POST /api/loan`

**Request Body:**

```json
{
  "bookId": 5,
  "borrowerName": "Maria Lopez",
  "loanDate": "2026-04-13",
  "dueDate": "2026-04-27"
}
```

**Respuesta (409 Conflict):**

```json
{
  "message": "El libro con ID 5 ya tiene un préstamo activo. Debe devolver el préstamo anterior antes de crear uno nuevo."
}
```

**Códigos Respuesta:**

| Código | Descripción |
|--------|-------------|
| `201 Created` | ✅ Préstamo creado |
| `400 Bad Request` | ❌ Datos inválidos (ej: dueDate < loanDate) |
| `404 Not Found` | ❌ Libro no existe en BD |
| `409 Conflict` | ❌ Libro ya tiene préstamo activo |
| `500 Server Error` | ❌ Error del servidor |

---

## 📊 DTOs Principales

### AuthorResponse

```json
{
  "id": 1,
  "name": "Gabriel",
  "lastName": "García Márquez",
  "birthDate": "1927-03-06T00:00:00",
  "country": "Colombia",
  "biography": "Escritor de Cien Años de Soledad",
  "createdDate": "2026-04-13T10:00:00Z",
  "updatedDate": null,
  "bookCount": 7
}
```

### BookResponse

```json
{
  "id": 1,
  "title": "Cien años de soledad",
  "genre": "Realismo mágico",
  "numberOfPages": 417,
  "isbn": "978-0060883287",
  "publishedDate": "1967-05-30T00:00:00",
  "coverImagePath": "/uploads/books/libro-1-portada.jpg",
  "authorId": 1,
  "createdDate": "2026-04-13T10:15:00Z",
  "updatedDate": null
}
```

### LoanResponse

```json
{
  "id": 3,
  "bookId": 5,
  "borrowerName": "Juan Perez",
  "loanDate": "2026-04-01T00:00:00",
  "dueDate": "2026-04-15T00:00:00",
  "returnDate": "2026-04-13T14:25:10.456789Z",
  "status": "Returned",
  "createdDate": "2026-04-01T10:00:00Z",
  "updatedDate": "2026-04-13T14:25:10.456789Z"
}
```

### ReportSummaryResponse

```json
{
  "top5AuthorsByPages": [
    {
      "authorId": 1,
      "authorName": "García Márquez",
      "totalPages": 3500,
      "bookCount": 7
    }
  ],
  "authorsWithoutBooks": ["Autor Solitario"],
  "averagePages": {
    "averagePages": 340.5,
    "totalBooks": 250
  },
  "totalBooksByAuthor": [
    {"authorName": "García Márquez", "totalBooks": 7}
  ]
}
```

---

## 📊 Otros Endpoints

### GET - Listar Autores

```
GET /api/author?page=1&pageSize=10&sortBy=CreatedDate DESC
```

**Response:**
```json
{
  "page": 1,
  "pageSize": 10,
  "data": [...],
  "count": 2
}
```

### GET - Listar Libros

```
GET /api/book?page=1&pageSize=10&authorId=1&title=Don&sortBy=b.Title ASC
```

**Parámetros Opcionales:**
- `authorId`: Filtrar por autor
- `title`: Buscar por título
- `sortBy`: Ordenar resultado

### GET - Listar Préstamos

```
GET /api/loan?page=1&pageSize=10&status=Active&bookId=1
```

**Parámetros Opcionales:**
- `status`: "Active", "Returned", "Overdue"
- `bookId`: Filtrar por libro

### GET - Reportes

```
GET /api/report/summary
```

Retorna resumen completo: Top autores, promedio páginas, etc.

---

## 🧪 Testing

### 📬 Postman Collection

Se incluye `Biblioteca.postman_collection.json` con todos los endpoints preconfigurados.

**Importar en Postman:**

1. Abrir Postman
2. **File** → **Import**
3. Seleccionar `Biblioteca.postman_collection.json`
4. Colección importada lista para usar
5. Todos los endpoints preconfigurados con ejemplos

**Variables de Entorno (Postman):**

```json
{
  "base_url": "http://localhost:5088",
  "api_version": "v1"
}
```

### 🧪 Swagger UI (Recomendado)

1. Acceder a `http://localhost:5088/swagger`
2. Expandir endpoint
3. Click "Try it out"
4. Completar parámetros
5. Click "Execute"

---

## 🏗️ Estructura del Proyecto

```
Biblioteca.sln                                  # Solución .NET 10
│
├── 📂 Api/                                     # Backend REST API
│   ├── Controllers/
│   │   ├── AuthorController.cs                # CRUD Autores
│   │   ├── BookController.cs                  # CRUD Libros + multipart  
│   │   ├── LoanController.cs                  # CRUD Préstamos + /return
│   │   └── ReportController.cs                # Analytics
│   ├── Services/
│   │   └── ImageService.cs                    # Manejo de uploads
│   ├── Program.cs                             # Swagger + DI
│   ├── wwwroot/uploads/books/                 # Almacén de portadas
│   └── Api.csproj
│
├── 📂 Core/                                    # Lógica de Negocio
│   ├── Domains/
│   │   ├── Authors/                           # Domain Services Autores
│   │   ├── Books/                             # Domain Services Libros
│   │   ├── Loans/                             # Domain Services Préstamos
│   │   └── Reports/                           # Domain Services Analytics
│   └── Core.csproj
│
├── 📂 Application/                             # Abstracciones & DTOs
│   ├── Abstractions/                          # Interfaces
│   │   ├── Authors/IAuthorRepository.cs
│   │   ├── Authors/IAuthorQueries.cs
│   │   └── Persistence/IUnitOfWork.cs
│   ├── DTOs/
│   │   ├── Authors/
│   │   ├── Books/
│   │   └── Loans/
│   ├── Entities/
│   │   ├── AuthorEntity.cs
│   │   ├── BookEntity.cs
│   │   └── LoanEntity.cs
│   └── Application.csproj
│
├── 📂 Infrastructure/                          # Persistencia (BD)
│   ├── Persistences/
│   │   ├── AppDbContext.cs                    # EF Core DbContext
│   │   └── UnitOfWork.cs                      # Transacciones ACID
│   ├── Repositories/
│   │   └── Authors/AuthorRepository.cs        # Implementación Repos
│   ├── Queries/
│   │   ├── Authors/AuthorQueries.cs           # Queries optimizadas (Dapper)
│   │   └── Reports/ReportQueries.cs           # Analytics queries
│   ├── Migrations/
│   │   ├── 20260127173931_InitDataBase.cs     # Migración inicial
│   │   ├── 20260127173931_InitDataBase.Designer.cs
│   │   └── AppDbContextModelSnapshot.cs
│   ├── DependencyInjection.cs                 # Registro servicios
│   └── Infrastructure.csproj
│
├── 📂 Infrastructure/Frontend/                 # Blazor WASM Frontend
│   ├── Components/
│   │   ├── Shared/
│   │   │   ├── LoadingComponent.razor
│   │   │   ├── ErrorComponent.razor
│   │   │   ├── EmptyComponent.razor
│   │   │   └── AlertComponent.razor
│   │   ├── Authors/
│   │   ├── Books/
│   │   ├── Loans/
│   │   └── Reports/
│   ├── Services/
│   │   ├── AuthorService.cs
│   │   ├── BookService.cs
│   │   ├── LoanService.cs
│   │   └── ReportService.cs
│   ├── Models/
│   │   ├── AuthorModel.cs
│   │   ├── BookModel.cs
│   │   ├── LoanModel.cs
│   │   └── ReportModel.cs
│   ├── Layout/
│   │   ├── MainLayout.razor
│   │   └── NavMenu.razor
│   ├── Program.cs                             # Configuración WASM
│   ├── App.razor                              # Componente raíz
│   ├── _Imports.razor                         # Imports globales
│   ├── wwwroot/                               # Assets estáticos
│   └── Frontend.csproj
│
├── 📝 Documentación/
│   ├── README.md                              # Este archivo
│   ├── ARCHITECTURE.md                        # Decisiones de diseño
│   ├── BUGS_ARREGLADOS.md                     # Análisis de bugs y fixes
│   ├── TESTING_DEBUGGING_REPORTE_FINAL.md     # Resultados testing
│   └── BUGS_SUMMARY.txt                       # Resumen rápido
│
└── Biblioteca.sln                             # Solución Visual Studio
```

---

## 🧪 Testing

### Opción A: Swagger UI (Recomendado)

1. **Acceder:** `http://localhost:5088/swagger`
2. **Usuarios:** Versión más intuitiva
3. **Advantage:** Puede ver documentación y respuestas en tiempo real

### Opción B: Postman

1. **Importar:** `Biblioteca.postman_collection.json`
2. **Ventajas:** Historia, variables de entorno, automatización
3. **Desventaja:** Requiere instalar app

### Opción C: cURL

**Crear autor:**
```bash
curl -X POST http://localhost:5088/api/author \
  -H "Content-Type: application/json" \
  -d '{
    "name":"Jorge Luis",
    "lastName":"Borges",
    "birthDate":"1899-08-24",
    "country":"Argentina",
    "biography":"Escritor"
  }'
```

**Listar autores:**
```bash
curl http://localhost:5088/api/author?page=1&pageSize=10
```

**Crear libro con imagen:**
```bash
curl -X POST http://localhost:5088/api/book \
  -F "title=Ficciones" \
  -F "numberOfPages=156" \
  -F "authorId=1" \
  -F "genre=Ficción" \
  -F "publishedDate=1944-08-15" \
  -F "file=@portada.jpg"
```

**Devolver préstamo:**
```bash
curl -X PUT http://localhost:5088/api/loan/1/return
```

---

## 🔧 Troubleshooting

### ❌ Error: "Cannot open server '.'"

**Causa:** SQL Server LocalDB no está ejecutándose

**Solución:**

```bash
# Opción 1: Iniciar LocalDB
sqllocaldb start mssqllocaldb

# Opción 2: Abrir SQL Server Management Studio
# Query: SELECT @@VERSION
```

### ❌ Error: "Database 'BibliotecaDB' does not exist"

**Causa:** Migraciones no aplicadas

**Solución:**

```bash
cd d:\USER\Documents\Fork\control.pruebas\Infrastructure
dotnet ef database update
```

### ❌ Imagen no se carga (404)

**Causas posibles:**

1. Carpeta no existe: `Api/wwwroot/uploads/books/`
2. `app.UseStaticFiles();` falta en `Program.cs`
3. Path incorrecto en respuesta API

**Solución:**

```bash
# Crear carpeta manualmente
mkdir Api/wwwroot/uploads/books

# Verificar Program.cs contiene:
# app.UseStaticFiles();
```

### ❌ Puerto 5088 ya está en uso

```bash
# Opción 1: Matar proceso
netstat -ano | findstr :5088
taskkill /PID <PID> /F

# Opción 2: Cambiar puerto en launchSettings.json
# "applicationUrl": "http://localhost:5089"
```

### ❌ Listado de autores retorna lista vacía

**Ver:** [BUG 1 - BUGS_ARREGLADOS.md](./BUGS_ARREGLADOS.md)

**Solución:** Compillar e verificar `UnitOfWork.CommitAsync()` incluye `SaveChangesAsync()`

### ❌ Swagger retorna 404

**Ver:** [BUG 3 - BUGS_ARREGLADOS.md](./BUGS_ARREGLADOS.md)

**Solución:** Verificar `Program.cs` tiene:
```csharp
app.UseSwagger();
app.UseSwaggerUI(...);
```

---

## 📚 Recursos Adicionales

| Archivo | Contenido |
|---------|----------|
| [ARCHITECTURE.md](./ARCHITECTURE.md) | Decisiones de diseño, Clean Architecture, patrones |
| [BUGS_ARREGLADOS.md](./BUGS_ARREGLADOS.md) | Análisis de 3 bugs críticos y soluciones |
| [TESTING_DEBUGGING_REPORTE_FINAL.md](./TESTING_DEBUGGING_REPORTE_FINAL.md) | Resultados y validación de testing |

---

## 📋 Checklist Pre-Producción

Antes de deployar a producción:

- [ ] Connection string de producción configurada
- [ ] Swagger deshabilitado (solo en Development)
- [ ] Validaciones de input reforzadas
- [ ] Unit tests creados y pasando (xUnit)
- [ ] Integration tests completados
- [ ] Performance testing (1000+ registros)
- [ ] Backup strategy definida
- [ ] Logging centralizado configurado
- [ ] HTTPS certificados instalados
- [ ] Rate limiting configurado
- [ ] CORS configurado adecuadamente

---

## 📞 Soporte

**Problemas comunes:** Ver [Troubleshooting](#troubleshooting)

**Bugs conocidos:** [BUGS_ARREGLADOS.md](./BUGS_ARREGLADOS.md)

**Documentación completa:** [ARCHITECTURE.md](./ARCHITECTURE.md)

---

## 📄 Licencia

Proyecto educativo. Libre para usar y modificar.

---

## 👥 Autores  

Sistema de Gestión de Biblioteca - Abril 2026

---

**Última actualización:** 13/04/2026  
**Versión:** 1.0.0  
**Status:** ✅ Production Ready

