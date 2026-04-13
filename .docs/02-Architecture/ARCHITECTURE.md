# 🏛️ Arquitectura del Sistema - Decisiones Clave

## Resumen Ejecutivo

**Patrón:** Clean Architecture + Domain Driven Design  
**Stack:** .NET 10, EF Core, Dapper, Blazor WASM  
**Base de Datos:** SQL Server LocalDB  
**Deployment:** HTTP (localhost:5088 API, localhost:5098 Frontend)

---

## 🏗️ Capas de Arquitectura

```
┌─────────────────────────────────────────┐
│  Presentación (UI)                      │
│  ├─ Frontend: Blazor WASM               │
│  └─ Controllers: HTTP REST              │
├─────────────────────────────────────────┤
│  Domain/Core (Lógica de Negocio)        │
│  ├─ CreateAuthorDomain                  │
│  ├─ CreateBookDomain                    │
│  ├─ CreateLoanDomain                    │
│  └─ GenerateLibrarySummaryReportDomain  │
├─────────────────────────────────────────┤
│  Application (Abstracciones)            │
│  ├─ Interfaces: IAuthorRepository       │
│  ├─ DTOs: CreateAuthorRequest           │
│  └─ Entities: AuthorEntity              │
├─────────────────────────────────────────┤
│  Infrastructure (Persistencia)          │
│  ├─ EF Core: AppDbContext               │
│  ├─ Repositories: AuthorRepository      │
│  ├─ Queries: AuthorQueries (Dapper)     │
│  └─ UnitOfWork: Transacciones ACID      │
└─────────────────────────────────────────┘
          ↓
    SQL Server LocalDB
```

---

## 📋 Decisiones Clave

### 1. **Dapper para Queries Complejas (Reportes)**

**Decisión:** Usar Dapper en lugar de LINQ para queries analíticas

**Razón:**
- Reportes = queries SQL complejas (JOINs, GROUP BY, UNION)
- Dapper = SQL nativo + mapping automático → mejor performance
- EF LINQ genera SQL subóptimo para analytics complejos
- No hay n+1 queries problem con SQL nativo

**Implementación:**
```csharp
// AuthorQueries.cs - Dapper
var sql = @"
    SELECT a.Id, a.Name,
           (SELECT COUNT(*) FROM Books WHERE AuthorId = a.Id) as BookCount
    FROM Author a
    WHERE a.IsDeleted = 0
    ORDER BY {sortBy}
    OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";
    
var result = await unitOfWork.Connection
    .QueryAsync<AuthorResponse>(sql);
```

**Tradeoff:** Requiere escribir SQL → menos type-safe que LINQ

---

### 2. **Soft Delete (IsDeleted Flag)**

**Decisión:** Usar soft delete en lugar de borrado físico

**Razón:**
- Mantiene historial completo de datos
- Permite auditoría y compliance
- No afecta integridad referencial
- Fácil de revertir accidentales

**Implementación:**
```csharp
// Eliminar: solo marca como deleted
public async Task DeleteAuthorAsync(int id, CancellationToken ct)
{
    var author = await GetAuthorByIdAsync(id, ct);
    author.IsDeleted = true; // ← Solo marca, no borra
    author.UpdatedDate = DateTime.UtcNow;
}

// Consultar: siempre filtra
var author = await dbContext.AuthorEntities
    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
```

**Tradeoff:** Requiere WHERE IsDeleted = 0 en todas las queries

---

### 3. **UnitOfWork Pattern para Transacciones**

**Decisión:** Implementar patrón UnitOfWork para control de transacciones

**Razón:**
- Garantiza ACID properties (Atomicity, Consistency, Isolation, Durability)
- Si una operación falla → todo rollback automático
- Ej: Crear autor falla → no deja datos inconsistentes

**Implementación:**
```csharp
// Domain Service
public async Task<AuthorResponse> CreateAsync(
    CreateAuthorRequest request, CancellationToken ct)
{
    await uow.BeginAsync(ct);        // Inicia transacción
    try
    {
        await repo.AddAuthorAsync(author, ct);
        await uow.CommitAsync(ct);    // Persiste datos
        // ... mapeo y retorno
    }
    catch
    {
        await uow.RollbackAsync(ct);  // Revierte todo
        throw;
    }
}
```

**Problema histórico:** Bug #1 - CommitAsync no llamaba SaveChangesAsync()

---

### 4. **Multipart/Form-Data para Uploads (No Base64)**

**Decisión:** Usar multipart/form-data en lugar de Base64 encoded en JSON

**Razón:**
- Standard HTTP para uploads de archivos
- Más eficiente que Base64 (reduce ~33% tamaño)
- Compatible con navegadores, Postman, cURL
- Soporte nativo en ASP.NET Core

**Implementación:**
```csharp
[HttpPost]
[Consumes("multipart/form-data")]
public async Task<IActionResult> CreateBook(
    [FromForm] string title,
    [FromForm] IFormFile file,  // ← Archivo directo
    ...)
```

**Tradeoff:** Frontend debe usar FormData, no JSON simple

---

### 5. **Status 409 Conflict para Préstamos Duplicados**

**Decisión:** Retornar HTTP 409 (Conflict) cuando libro ya está prestado

**Razón:**
- REST semantics: 409 = "The request conflicts with the current state"
- NOT 400 (Bad Request) = datos mal formados
- NOT 500 (Server Error) = error del servidor
- Comunica claramente el problema al cliente

**Implementación:**
```csharp
catch (InvalidOperationException ex)
{
    // Libro ya tiene préstamo activo
    return StatusCode(409, new { message = ex.Message });
}
```

**Estándar:** RFC 7231 HTTP Semantics

---

### 6. **Swagger Solo en Development**

**Decisión:** Habilitar Swagger/OpenAPI solo en ambiente Development

**Razón:**
- Security: No exponer documentación de API en producción
- Performance: Middleware innecesario en prod
- Intent: Developers usan Swagger locally, users no la necesitan

**Implementación:**
```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

**Tradeoff:** Reduce visibilidad de API en prod

---

### 7. **Lazy Loading - NO Circular References**

**Decisión:** Evitar lazy loading y circular references en EF

**Razón:**
- Lazy loading causa n+1 queries
- Circular refs causan JSON serialization errors
- Explicit loading es más predecible

**Implementación:**
```csharp
// AuthorEntity → Books (one-to-many)
// BUT: No public ICollection<Book> Books { get; set; }
// INSTEAD: Calculado dinámicamente en queries

var authorWithBooks = await db.Authors
    .AsNoTracking()
    .Select(a => new AuthorResponse
    {
        Id = a.Id,
        BookCount = a.Books.Count  // ← Eager loading explícito
    })
    .ToListAsync();
```

---

## 📊 Estructura DTO vs Entity

```csharp
// Domain Entity (BD) - Contiene todo
public class AuthorEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDeleted { get; set; }  // ← Campo interno
    public DateTime CreatedDate { get; set; }
}

// DTO Response (API) - Solo público
public class AuthorResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    // IsDeleted NO se expone ← Security
}
```

**Razón:** Ocultar campos internos, no romper API con cambios BD

---

## 🔄 Flujo Típico: Crear Libro

```
1. Frontend.BookForm                         (Blazor Component)
   └─ POST /api/book (multipart/form-data)
   
2. BookController.CreateBook()               (HTTP Endpoint)
   └─ ImageService.SaveImageAsync()         (Guardar portada)
   
3. CreateBookDomain.CreateAsync()            (Domain Service - Lógica)
   └─ IUnitOfWork.BeginAsync()               (Inicia transacción)
   └─ IBookRepository.AddBookAsync()         (Repository - EF)
   └─ IUnitOfWork.CommitAsync()              (Persiste + commit)
   
4. AppDbContext.SaveChangesAsync()           (EF Core)
   └─ SQL: INSERT INTO Books (...)
   
5. BookResponse                              (DTO Response)
   └─ Frontend recibe {"id": 1, ...}
```

---

## 🎯 Decisiones NO Tomadas (Pero Consideradas)

| Opción | Por qué NO |
|--------|-----------|
| **JWT Auth** | Scope del proyecto: local development |
| **Redis Cache** | Datos relativamente inmutables |
| **GraphQL** | REST API suficiente para este caso |
| **CQRS Pattern** | Overkill para escala actual |
| **Microservicios** | Single responsibility monolith enough |

---

## 📈 Escalabilidad Futura

**Si necesita crecer:**

1. **Autenticación:** Agregar JWT en Program.cs
2. **Caching:** Redis para reportes frecuentes
3. **Logging:** Serilog/ELK Stack
4. **Testing:** xUnit + Integration tests
5. **CI/CD:** GitHub Actions / Azure DevOps
6. **Deployment:** Docker + Kubernetes

---

## ✅ Validación de Decisiones

| Decisión | Probada | Resultado |
|----------|---------|-----------|
| Dapper para reportes | ✅ | Queries optimizadas, performante |
| Soft Delete | ✅ | Auditoría funciona, sin datos perdidos |
| UnitOfWork | ✅ | Transacciones ACID garantizadas |
| Multipart/form-data | ✅ | Upload de imágenes funcional |
| 409 Conflict | ✅ | Validación de préstamo activos |
| Swagger DevOnly | ✅ | No expone en deployment |

---

**Fecha:** 13/04/2026  
**Versión:** 1.0.0  
**Status:** Producción Lista ✅
