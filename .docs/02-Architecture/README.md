================================================================================
                    02-ARCHITECTURE - DISEÑO DEL SISTEMA
================================================================================

Esta sección contiene la documentación completa de la arquitectura,
estructura de carpetas y diseño del sistema Biblioteca.

📁 Archivos en esta sección:
------ ------
• PROJECT_ANALYSIS.md             - Análisis detallado archivo por archivo
• STRUCTURE_QUICK_REFERENCE.md   - Vista rápida (1 página)
• API_COLLECTION.json             - Endpoints para Postman

================================================================================
                          QUICK REFERENCE
================================================================================

Arquitectura: Clean Architecture (4 capas)
────────────────────────────────────────

Frontend (Blazor WASM)
        ↓
    API Layer (ASP.NET Controllers)
        ↓
    Core Layer (Business Logic)
        ↓
    Application Layer (DTOs, Interfaces)
        ↓
    Infrastructure (EF Core + Dapper + EF Migrations)
        ↓
    Database (SQL Server LocalDB)

Ventajas:
✅ Sin acoplamiento entre capas
✅ Fácil de probar
✅ Escalable independientemente

================================================================================
                        MÓDULOS PRINCIPALES (4)
================================================================================

👥 AUTORES
────────
• Create, Read, Update, Delete
• Búsqueda y paginación
• Soft delete
• Endpoints: 5

📖 LIBROS
────────
• CRUD con imágenes
• Asociación con autores
• Géneros y búsqueda
• Endpoints: 5

🔄 PRÉSTAMOS
───────────
• Crear y gestionar préstamos
• MAX 1 préstamo activo por libro
• Devoluciones con cálculo de multas
• Endpoints: 4

📊 REPORTES
──────────
• Resumen de biblioteca
• Estadísticas
• Análisis de géneros
• Endpoints: 2

TOTAL: 16 endpoints

================================================================================
                      PATRONES DE DISEÑO USADOS
================================================================================

✅ Repository Pattern
   Permite abstracción de la base de datos

✅ CQRS Pattern (Queries + Commands)
   Separación lectura/escritura

✅ Unit Of Work Pattern
   Manejo transaccional

✅ Dependency Injection
   Inyección de dependencias en program.cs

✅ DTOs (Data Transfer Objects)
   Separación entre DB entities y API responses

✅ Soft Delete
   Eliminación lógica de datos

================================================================================
                      ARCHIVOS POR CAPA
================================================================================

API Layer (Api/)
────────────
• Controllers/ - 4 controladores (Authors, Books, Loans, Reports)
• Services/ - Servicios de imagen, serialización
• appsettings.json - Configuración

Core Layer (Core/)
──────────────
• Domains/ - Lógica de negocio pura
  ├── Authors/ (5 domain services)
  ├── Books/ (5 domain services)
  ├── Loans/ (4 domain services)
  └── Reports/ (1 domain service)

Application Layer (Application/)
────────────────────
• Abstractions/ - Interfaces y contratos
• DTOs/ - Objetos de transferencia
• Entities/ - Modelos de dominio

Infrastructure Layer (Infrastructure/)
──────────────────
• Persistences/ - EF Core DbContext
• Repositories/ - Implementación de interfaces
• Queries/ - Dapper queries (lectura)
• Migrations/ - Historial de BD

================================================================================
                    DATOS PERSISTIDOS
================================================================================

AuthorEntity:
  • Id, Name, LastName
  • BirthDate, Country, Biography
  • CreatedDate, UpdatedDate, IsDeleted

BookEntity:
  • Id, Title, ISBN, Genre
  • NumberOfPages, PublishedDate
  • AuthorId (FK), CoverImagePath
  • CreatedDate, UpdatedDate, IsDeleted

LoanEntity:
  • Id, BookId (FK), BorrowerName
  • LoanDate, ExpectedReturnDate, ReturnDate
  • Status (Active/Returned), Penalty
  • CreatedDate, UpdatedDate, IsDeleted

================================================================================
                        BASE DE DATOS
================================================================================

Tipo:           SQL Server LocalDB
Estrategia:     Entity Framework Core
Schema:         Auto-migración
Pattern:        Code First
Migrations:     20260127173931_InitDataBase

Características:
✅ Foreign keys configuradas
✅ Índices de búsqueda
✅ Columnas de auditoría (CreatedDate, UpdatedDate)
✅ Soft delete (IsDeleted)

Para ver estructura BD:
  $ cd Infrastructure
  $ dotnet ef database update
  (Se crea automáticamente si no existe)

================================================================================
                        ENDPOINTS API
================================================================================

Authors:
  • GET    /api/authors             - Listar con paginación
  • GET    /api/authors/{id}        - Obtener por ID
  • POST   /api/authors             - Crear
  • PUT    /api/authors/{id}        - Modificar
  • DELETE /api/authors/{id}        - Eliminar

Books:
  • GET    /api/books               - Listar
  • GET    /api/books/{id}          - Obtener detalle
  • POST   /api/books               - Crear (con imagen)
  • PUT    /api/books/{id}          - Modificar
  • DELETE /api/books/{id}          - Eliminar

Loans:
  • GET    /api/loans               - Listar
  • GET    /api/loans/{id}          - Obtener
  • POST   /api/loans               - Crear préstamo
  • PUT    /api/loans/{id}/return   - Devolver libro

Reports:
  • GET    /api/reports/summary     - Resumen
  • GET    /api/reports/genre-stats - Estadísticas

Ver más detalles: Swagger en http://localhost:5088/swagger/index.html

================================================================================
                      CONFIGURACIÓN IMPORTANTE
================================================================================

Connection String (appsettings.json):
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BibliotecaDb;..."

Perfiles de ejecución (launchSettings.json):
  • http          - Puerto 5088
  • https         - Puerto 5089

Logging:
  • Console - Información en terminal
  • File - Registro en archivo

CORS:
  • Frontend autorizado en localhost:5089

================================================================================
                        TESTING
================================================================================

Framework: xUnit + Moq + EF InMemory
Ubicación: Tests/
Tests: 12 (100% passing)

Categorías:
  • Unit Tests - Lógica de dominio (6 tests)
  • Integration - Queries con BD (4 tests)
  • Serialization - JSON (2 tests)

Ejecutar:
  $ dotnet test Tests/

Resultado esperado:
  Total: 12; Passed: 12; Failed: 0 ✅

================================================================================
                          PRÓXIMOS PASOS
================================================================================

1. Lee PROJECT_ANALYSIS.md para detalles completos
2. Importa API_COLLECTION.json en Postman
3. Ejecuta la aplicación desde 03-Development/
4. Escribe tests para nuevas features
5. Mantén sincronizada esta documentación

================================================================================
              Para información detallada: PROJECT_ANALYSIS.md
              Para guía visual: STRUCTURE_QUICK_REFERENCE.md
================================================================================
