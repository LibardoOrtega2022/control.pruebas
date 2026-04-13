# 🐛 Debugging & Bug Fixes Report

## Prioridad Alta - Bugs Arreglados: 3/3 ✅

---

## BUG 1: No se pueden obtener el listado de los autores

### 📋 Síntoma
- **Endpoint**: `GET /api/author?page=1&pageSize=10`
- **Comportamiento**: Retorna lista vacía `{"page":1,"pageSize":10,"data":[],"count":0}` incluso después de registrar autores
- **Impacto**: Los usuarios no pueden ver el listado de autores registrados

### 🔍 Causa Raíz
La clase `UnitOfWork.cs` en el método `CommitAsync()` NO estaba persistiendo los cambios de Entity Framework a la base de datos.

**Código problemático** (Infrastructure/Persistences/UnitOfWork.cs):
```csharp
public async Task CommitAsync(CancellationToken ct)
{
    if (Db.Database.CurrentTransaction is not null)
        await Db.Database.CurrentTransaction.CommitAsync(ct);
    // ❌ Falta hacer SaveChangesAsync() de EF
}
```

**Flujo del problema**:
1. `CreateAuthorDomain.CreateAsync()` llama `repo.AddAuthorAsync(author)` → agrega a DbContext (en memoria)
2. Luego llama `uow.CommitAsync()` → hace commit de la transacción
3. Pero EF *nunca* persistió los cambios (`SaveChangesAsync` nunca fue llamado)
4. Por lo tanto, la entidad nunca llegó a la BD
5. El listado de `AuthorQueries.GetAuthorsAsync()` ejecuta SQL SELECT que no retorna nada

### ✅ Solución Implementada
Actualizar `UnitOfWork.CommitAsync()` para llamar `SaveChangesAsync()` ANTES de hacer commit de la transacción:

**Código arreglado** (Infrastructure/Persistences/UnitOfWork.cs):
```csharp
public async Task CommitAsync(CancellationToken ct)
{
    // ✅ Guardar cambios de EF PRIMERO
    await Db.SaveChangesAsync(ct);
    
    // ✅ Hacer commit de la transacción
    if (Db.Database.CurrentTransaction is not null)
        await Db.Database.CurrentTransaction.CommitAsync(ct);
}
```

### ✔️ Verificación Post-Arreglo
```
POST /api/author → Retorna: {"id":2, "name":"TestAuto", ...}
GET /api/author?page=1 → Retorna: {"data":[{id:1,...}, {id:2,...}], "count":2}
```

---

## BUG 2: No se puede registrar el autor (ID=0)

### 📋 Síntoma
- **Endpoint**: `POST /api/author`
- **Comportamiento**: El endpoint retorna un autor con `"id": 0` en lugar del ID auto-incrementado
- **Impacto**: El frontend no puede identificar el nuevo autor porque ID es 0 (inválido)

### 🔍 Causa Raíz
Consecuencia directa del BUG 1. Como `SaveChangesAsync()` nunca era llamado en `UnitOfWork.CommitAsync()`:

**Secuencia del problema**:
1. `CreateAuthorDomain.CreateAsync()` crea `AuthorEntity { Id = 0 }`
2. Llama `repo.AddAuthorAsync(author)` → DbContext.Add(author)
3. Entity Framework no genera el ID hasta que se hace `SaveChangesAsync()`
4. Pero SaveChangesAsync NUNCA se llamaba
5. Por lo tanto, `author.Id` permanecía como 0
6. La respuesta retorna `{"id": 0, ...}` ❌

### ✅ Solución Implementada
La misma solución del BUG 1 resuelve este problema. Al agregar `SaveChangesAsync()` en `CommitAsync()`:
1. EF ejecuta la sentencia SQL INSERT
2. EF recupera el ID auto-generado de la BD
3. EF asigna el ID a la entidad local
4. El Domain retorna `author.Id` con valor correcto ✅

### ✔️ Verificación Post-Arreglo
```powershell
POST /api/author
Response: {
  "id": 2,           # ✅ ID auto-generado correcto (no 0)
  "name": "TestAuto",
  "lastName": "Author",
  "createdDate": "2026-04-13T01:53:49.7756907Z"
}
```

---

## BUG 3: Swagger UI no aparece

### 📋 Síntoma
- **Endpoint**: `GET /swagger/index.html`
- **Error**: HTTP 404 (Not Found)
- **Impacto**: Los desarrolladores no pueden usar Swagger para explorar/testar los endpoints

### 🔍 Causa Raíz
El middleware de Swagger NO estaba habilitado en `Program.cs`. 

**Código problemático** (Api/Program.cs):
```csharp
var app = builder.Build();

// Configuration para Swagger fue registrado:
builder.Services.AddSwaggerGen(c => {...}); ✅

// PERO el middleware de Swagger NUNCA fue agregado:
// app.UseSwagger();          ❌ FALTA
// app.UseSwaggerUI();        ❌ FALTA

app.UseStaticFiles();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
```

**Causa técnica**:
- `AddSwaggerGen()` → Genera la especificación OpenAPI (esquema)
- `AddEndpointsApiExplorer()` → Permite explorar endpoints del controlador
- `UseSwagger()` → **MIDDLEWARE que sirve el archivo swagger.json**
- `UseSwaggerUI()` → **MIDDLEWARE que sirve la UI (HTML/JS)**

Sin los middlewares, aunque la especificación OpenAPI se genera, NO se sirve al cliente.

### ✅ Solución Implementada
Agregar los middlewares de Swagger a `Program.cs`:

**Código arreglado** (Api/Program.cs):
```csharp
var app = builder.Build();

// ✅ Habilitar Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Biblioteca API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

**Configuración explicada**:
- `if (app.Environment.IsDevelopment())` → Solo en desarrollo (seguridad)
- `app.UseSwagger()` → Sirve `/swagger/v1/swagger.json` (especificación OpenAPI)
- `app.UseSwaggerUI()` → Sirve la interfaz HTML en `/swagger`
- `c.RoutePrefix = "swagger"` → La UI se accede en `/swagger` (no `/swagger/ui`)

### ✔️ Verificación Post-Arreglo
```
GET http://localhost:5088/swagger
Response: HTML + JavaScript (Swagger UI carga correctamente)
- Título: "Swagger UI"
- Se listan todos los endpoints
- Se puede hacer pruebas de endpoints directamente
```

---

## 📊 Resumen de Impacto

| Bug | Afectados | Severidad | Status |
|-----|-----------|-----------|--------|
| BUG 1 | Backend (GET /api/author) | 🔴 CRÍTICO | ✅ ARREGLADO |
| BUG 2 | API Response (POST /api/author) | 🔴 CRÍTICO | ✅ ARREGLADO |
| BUG 3 | Developer Experience (Swagger) | 🟡 IMPORTANTE | ✅ ARREGLADO |

---

## 🧪 Pruebas Realizadas

### Prueba 1: Crear Autor (POST)
```powershell
Body: {
  "name": "TestAuto",
  "lastName": "Author", 
  "birthDate": "1980-01-01",
  "country": "Spain",
  "biography": "Test"
}

Response: HTTP 201 Created
{
  "id": 2,                              # ✅ ID correcto (no 0)
  "name": "TestAuto",
  "createdDate": "2026-04-13T01:53:49.7756907Z"
}
```

### Prueba 2: Listar Autores (GET)
```powershell
Response: HTTP 200 OK
{
  "page": 1,
  "pageSize": 10,
  "data": [                            # ✅ Ahora contiene datos (no vacío)
    {
      "id": 2,
      "name": "TestAuto",
      "birthDate": "1980-01-01T00:00:00",
      "createdDate": "2026-04-13T01:53:49.7756907"
    },
    {
      "id": 1,
      "name": "Test",
      "birthDate": "1980-01-01T00:00:00",
      "createdDate": "2026-04-13T01:53:30.1465392"
    }
  ],
  "count": 2                           # ✅ Count correctamente actualizado
}
```

### Prueba 3: Swagger UI
```
URL: http://localhost:5088/swagger
Status: HTTP 200 OK
Content: HTML + JavaScript (Swagger UI interface)
- ✅ Página carga correctamente
- ✅ Se listan todos los endpoints
- ✅ Se pueden hacer requests de prueba directamente
```

---

## 📝 Archivos Modificados

1. **Infrastructure/Persistences/UnitOfWork.cs**
   - Línea: `CommitAsync()` method
   - Cambio: Agregar `await Db.SaveChangesAsync(ct);` antes del commit

2. **Api/Program.cs**
   - Línea: Después de `var app = builder.Build();`
   - Cambio: Agregar middleware `UseSwagger()` e `UseSwaggerUI()`

---

## 🚀 Recomendaciones Futuras

1. **Agregar Tests Unitarios**: Crear pruebas que verifiquen:
   - AuthorRepository persiste datos correctamente
   - UnitOfWork hace commit de transacciones
   - Swagger UI está habilitado en development

2. **Implementar Logging**: Registrar:
   - Cuándo se ejecuta `SaveChangesAsync()`
   - Cuándo se cometen transacciones
   - Errores de persistencia

3. **Pipeline de CI/CD**: Agregar validaciones automáticas para:
   - Middleware de Swagger habilitado (si está en Development)
   - UnitOfWork tests pasen
   - Endpoints aceptan/retornan datos correctamente

---

**Fecha de Fix**: 13/04/2026  
**Tiempo Total Debug**: ~30 minutos  
**Estado Final**: ✅ ALL BUGS FIXED - SISTEMA OPERACIONAL
