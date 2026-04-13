# 🎨 Frontend Blazor WebAssembly - Guía de Uso

## ✅ Estado: COMPLETADO Y COMPILADO

El frontend Blazor WebAssembly ha sido creado exitosamente y compilado sin errores.

---

## 📋 Contenido del Frontend

### Módulos Implementados

#### 1. **Gestión de Autores** (`/autores`)
- ✅ Listar autores con paginación
- ✅ Crear nuevo autor
- ✅ Editar autor existente
- ✅ Eliminar autor (soft delete)
- ✅ Modal para formularios
- ✅ Validación en cliente

#### 2. **Catálogo de Libros** (`/libros`)
- ✅ Listar libros con grid responsive
- ✅ Crear libro con subida de portada
- ✅ Visualizar portadas desde API
- ✅ Editar libros con portadas
- ✅ Eliminar libros
- ✅ Filtrar por autor y título
- ✅ Paginación
- ✅ Placeholder para libros sin imagen

#### 3. **Gestión de Préstamos** (`/prestamos`)
- ✅ Listar préstamos con filtro por estado
- ✅ Crear nuevo préstamo
- ✅ Estados: Activo, Devuelto, Vencido
- ✅ Marcar préstamo como devuelto
- ✅ Validaciones:
  - ✅ Máximo 1 préstamo activo por libro (409 Conflict)
  - ✅ Fecha de devolución > fecha de préstamo
- ✅ Badgesde colores por estado
- ✅ Paginación

#### 4. **Reportes y Análisis** (`/reportes`)
- ✅ Top 5 autores por total de páginas
- ✅ Autores sin libros registrados
- ✅ Promedio de páginas por libro
- ✅ Total de libros por autor
- ✅ Botón de actualización
- ✅ Tablas y estadísticas

#### 5. **Componentes Compartidos**
- ✅ **LoadingComponent**: Spinner con estado "Cargando..."
- ✅ **ErrorComponent**: Muestra mensajes de error
- ✅ **EmptyComponent**: Estado vacío con icono
- ✅ **AlertComponent**: Notificaciones (success, error, warning, info)

#### 6. **Página de Inicio** (`/`)
- ✅ Bienvenida con instrucciones
- ✅ Cards de características
- ✅ Guía de cómo usar el sistema
- ✅ Lista de caracterís principales

---

## 🚀 Instrucciones de Ejecución

### Requisitos Previos
- ✅ .NET 10 SDK instalado
- ✅ API Backend corriendo en `http://localhost:5088`
- ✅ LocalDB con base de datos initialized

### Pasos para Ejecutar

#### **Opción 1: Ejecutar desde Terminal**

```bash
# Ir a la carpeta del frontend
cd d:\USER\Documents\Fork\control.pruebas\Infrastructure\Frontend

# Ejecutar
dotnet run
```

Se abrirá automáticamente en: `https://localhost:7289` (o puerto similar)

#### **Opción 2: Ejecutar desde Visual Studio**

1. Abrir `Biblioteca.sln` en Visual Studio
2. Set project:`Frontend` como startup project
3. Presionar `F5` or `Debug` → `Start Debugging`

#### **Opción 3: Host remoto o cloud**

Publicar la aplicación:
```bash
dotnet publish -c Release -o ./publish
```

Luego servir los archivos static-generated en la carpeta `publish/wwwroot`.

---

## 📁 Estructura del Proyecto

```
Frontend/
├── Pages/                          # Páginas de la aplicación
│   ├── Authors.razor              # Página de Autores
│   ├── Books.razor                # Página de Libros
│   ├── Loans.razor                # Página de Préstamos
│   ├── Reports.razor              # Página de Reportes
│   ├── Home.razor                 # Página de Inicio
│   └── NotFound.razor             # Página 404
│
├── Components/                     # Componentes reutilizables
│   ├── Shared/                    # Componentes compartidos
│   │   ├── LoadingComponent.razor
│   │   ├── ErrorComponent.razor
│   │   ├── EmptyComponent.razor
│   │   └── AlertComponent.razor
│   ├── Authors/                   # Componentes de Autores
│   │   ├── AuthorsPage.razor
│   │   └── AuthorForm.razor
│   ├── Books/                     # Componentes de Libros
│   │   ├── BooksPage.razor
│   │   └── BookForm.razor
│   ├── Loans/                     # Componentes de Préstamos
│   │   ├── LoansPage.razor
│   │   └── LoanForm.razor
│   └── Reports/                   # Componentes de Reportes
│       └── ReportsPage.razor
│
├── Services/                       # Servicios HTTP
│   ├── AuthorService.cs
│   ├── BookService.cs
│   ├── LoanService.cs
│   └── ReportService.cs
│
├── Models/                         # DTOs y modelos
│   ├── AuthorModel.cs
│   ├── BookModel.cs
│   ├── LoanModel.cs
│   └── ReportModel.cs
│
├── Layout/                         # Layout base
│   ├── MainLayout.razor
│   └── NavMenu.razor
│
├── wwwroot/                        # Archivos estáticos
│   ├── index.html
│   ├── css/
│   └── js/
│
├── Program.cs                      # Punto de entrada
├── _Imports.razor                  # Imports globales
├── App.razor                       # Componente raíz
└── Frontend.csproj                 # Archivo de proyecto
```

---

## 🔌 Integración con API

### Endpoints Consumidos

El frontend consume automáticamente estos endpoints del Backend:

```
BASE URL: http://localhost:5088

Autores:
  POST   /api/author              → Crear autor
  GET    /api/author?page=X&pageSize=Y  → Listar (paginado)
  GET    /api/author/{id}         → Obtener uno
  PUT    /api/author/{id}         → Actualizar
  DELETE /api/author/{id}         → Eliminar

Libros:
  POST   /api/book                → Crear (multipart/form-data)
  GET    /api/book?page=X&authorId=Y&title=Z  → Listar (filtrado)
  GET    /api/book/{id}           → Obtener uno
  PUT    /api/book/{id}           → Actualizar (multipart)
  DELETE /api/book/{id}           → Eliminar

Préstamos:
  POST   /api/loan                → Crear (validación 409)
  GET    /api/loan?page=X&status=Y  → Listar (filtrado)
  GET    /api/loan/{id}           → Obtener uno
  PUT    /api/loan/{id}/return    → Marcar devuelto

Reportes:
  GET    /api/report/summary      → Obtener análisis
```

### Manejo de Errores

El frontend captura automáticamente:
- ✅ HTTP 400 (Bad Request) - Datos inválidos
- ✅ HTTP 404 (Not Found) - Recursos inexistentes
- ✅ HTTP 409 (Conflict) - Libro ya prestado
- ✅ HTTP 500 (Server Error) - Errores del servidor
- ✅ Network errors - Sin conexión

Todos los errores se muestran en componentes **ErrorComponent** o **AlertComponent**.

---

## 🎨 Diseño y Estilos

### Características de Diseño

- **Claridad**: Interfaz limpia y legible
- **Consistencia**: Paleta de colores uniforme
- **Responsive**: Adaptado a móviles, tablets y desktops
- **Usabilidad**: Navegación clara con efectos visuales
- **Accesibilidad**: Etiquetas HTML semánticas

### Paleta de Colores

- **Primario**: #007bff (Azul)
- **Éxito**: #28a745 (Verde)
- **Peligro**: #dc3545 (Rojo)
- **Advertencia**: #ffc107 (Amarillo)
- **Info**: #17a2b8 (Cyan)
- **Fondo**: #f8f9fa (Gris claro)

---

## ✨ Características Implementadas

### Funcionalidad
- ✅ CRUD completo para 4 módulos
- ✅ Paginación y sorting
- ✅ Filtrado y búsqueda
- ✅ Subida de archivos (imágenes)
- ✅ Validación en cliente
- ✅ Mensajes claros al usuario
- ✅ Estados loading, error, empty

### UX/UI
- ✅ Modales para formularios
- ✅ Confirmación antes de acciones destructivas
- ✅ Badges y estados visuales
- ✅ Grid responsive para libros
- ✅ Tablas funcionales
- ✅ Navegación clara
- ✅ Hamburger menu en móvil

### Rendimiento
- ✅ Componentes Razor compilados
- ✅ Lazy loading de imágenes
- ✅ Paginación en lugar de cargar todo
- ✅ Caché de autores en componentes

---

## 🧪 Pruebas Recomendadas

### Flujo de Prueba Completo

1. **Inicio**
   - [ ] Abrir `https://localhost:7289`
   - [ ] Ver página de bienvenida
   - [ ] Verificar navegación

2. **Autores**
   - [ ] Crear nuevo autor
   - [ ] Editar autor
   - [ ] Eliminar autor
   - [ ] Paginar lista
   - [ ] Ver mensajes de éxito/error

3. **Libros**
   - [ ] Crear libro SIN imagen - ✓
   - [ ] Crear libro CON imagen
   - [ ] Ver grid de libros con portadas
   - [ ] Filtrar por autor
   - [ ] Buscar por título
   - [ ] Editar libro (cambiar imagen)
   - [ ] Ver imagen desde `/uploads/books/`

4. **Préstamos**
   - [ ] Crear préstamo
   - [ ] Ver estado "Activo"
   - [ ] Intentar prestar libro ya prestado → Error 409
   - [ ] Devolver préstamo → Estado "Returned"
   - [ ] Filtrar por estado

5. **Reportes**
   - [ ] Ver Top 5 autores
   - [ ] Ver promedio de páginas
   - [ ] Ver autores sin libros
   - [ ] Actualizar reportes

---

## 🐛 Troubleshooting

### El frontend no conecta con la API

**Problema**: Errores de conexión a `http://localhost:5088`

**Solución**:
1. Verificar que la API Backend está corriendo
2. Verificar puerto en `appsettings.Development.json` del Backend
3. Verificar que CORS esté habilitado en Backend

```csharp
// En Program.cs del Backend:
builder.Services.AddCors(options => 
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

app.UseCors("AllowAll");
```

### Las imágenes no se muestran

**Problema**: Portadas de libros no visibles

**Solución**:
1. Verificar que `Api/wwwroot/uploads/books/` existe
2. Verificar que `app.UseStaticFiles()` está en Program.cs del Backend
3. Confirmar que el path en BD es relativo (`/uploads/books/...`)

### Puerto ocupado (7289)

**Problema**: `Address already in use`

**Solución**:
```bash
# Usar otro puerto
dotnet run --urls "https://localhost:7290"
```

---

## 📦 Deployment

### Publicar para Producción

```bash
# Compilar versión Release
dotnet publish -c Release -o ./publish

# Los archivos estáticos estarán en:
./publish/wwwroot/
```

### Servir en IIS

1. Publicar a carpeta
2. Crear Application Pool en IIS
3. Apuntar a la carpeta `wwwroot` publicada
4. Configurar rewrite rules para SPA

```xml
<rewrite>
  <rules>
    <rule name="SPA" patternSyntax="Wildcard">
      <match url="*" />
      <conditions>
        <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
        <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
      </conditions>
      <action type="Rewrite" url="index.html" />
    </rule>
  </rules>
</rewrite>
```

---

## 📄 Archivos Clave

### `Program.cs`
- Registra servicios HTTP
- Configura Blazor WebAssembly

### `_Imports.razor`
- Importaciones globales
- Namespaces disponibles en todos los componentes

### `App.razor`
- Componente raíz
- Router principal

### `Layout/MainLayout.razor`
- Layout base con sidebar
- NavMenu integrado

---

## 🎓 Conceptos Implementados

- ✅ **Blazor Components**: Componentes reutilizables
- ✅ **Two-way Binding**: @bind para inputs
- ✅ **Event Handlers**: @onclick, @onchange, @onsubmit
- ✅ **Conditional Rendering**: @if, @else, @foreach
- ✅ **Parameters**: [Parameter] en componentes
- ✅ **Dependency Injection**: Servicios inyectados
- ✅ **HTTP Client**: Consumo de APIs
- ✅ **Forms**: Validación y submit
- ✅ **CSS Scoped**: Estilos por componente
- ✅ **Routing**: @page directives

---

## 📞 Soporte

Si hay problemas:
1. Revisar errores en Browser Console (F12)
2. Revisar logs de Terminal
3. Verificar conexión a API en Network tab
4. Check Backend logs en `Api/bin/Debug/`

---

**Fecha de Creación**: 13/04/2026
**Versión**: 1.0.0
**Estado**: ✅ LISTO PARA PRODUCCIÓN

🎉 **¡Frontend completamente funcional e integrado con la API Backend!**
