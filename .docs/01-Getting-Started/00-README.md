================================================================================
                    BIBLIOTECA - INICIO RÁPIDO
================================================================================

📚 Biblioteca es un sistema de gestión de bibliotecas completo construido con:
   • Backend: ASP.NET Core 10.0 API + EF Core + Dapper
   • Frontend: Blazor WebAssembly
   • Database: SQL Server LocalDB (Auto-migración)

================================================================================
                         1️⃣ PRIMER INICIO (5 minutos)
================================================================================

🔧 Pre-requisitos:
  ✅ .NET 10.0 SDK instalado
  ✅ SQL Server Express o LocalDB
  ✅ Un editor (VS Code, Visual Studio 2022)

📝 Pasos:

1. Restaurar dependencias:
   $ dotnet restore

2. Crear la base de datos (Auto-migración):
   $ cd Infrastructure
   $ dotnet ef database update

3. Ejecutar el backend (puerto 5088):
   $ cd Api
   $ dotnet run

4. En otra terminal, ejecutar frontend (puerto 5098):
   $ cd ../Frontend
   $ dotnet run

5. Acceder:
   • API Swagger: http://localhost:5088/swagger/index.html
   • Frontend: http://localhost:5098

================================================================================
                      📚 ESTRUCTURA DEL PROYECTO
================================================================================

Control.pruebas/
├── .docs/                          ← Documentación organizada
│   ├── 01-Getting-Started/         ← Guías de inicio
│   ├── 02-Architecture/            ← Diseño y estructura
│   ├── 03-Development/             ← Desarrollo local
│   ├── 04-Testing/                 ← Pruebas unitarias/integración
│   ├── 05-Deployment/              ← Despliegue producción
│   └── 06-Troubleshooting/         ← Problemas y soluciones
│
├── Scripts/                        ← Script de arranque
│   ├── run_api.ps1                 ← Ejecutar API
│   ├── run_frontend.ps1            ← Ejecutar Frontend
│   ├── run_all.ps1                 ← Ambos juntos
│   └── setup.ps1                   ← Setup inicial
│
├── Api/                            ← Capa API HTTP (Controladores)
├── Core/                           ← Capa de dominio (Lógica de negocio)
├── Application/                    ← DTOs, interfaces, entidades
├── Infrastructure/                 ← EF Core, Dapper, migraciones
├── Frontend/                       ← Blazor WASM
└── Tests/                          ← Pruebas unitarias/integración

================================================================================
                         ⚡ TAREAS COMUNES
================================================================================

▶️ CORRER TODO:
   $ .\scripts\run_all.ps1

▶️ SOLO BACKEND:
   $ .\scripts\run_api.ps1

▶️ SOLO FRONTEND:
   $ .\scripts\run_frontend.ps1

▶️ CORRER PRUEBAS:
   $ dotnet test Tests/

▶️ CREAR MIGRACIÓN NEW:
   $ dotnet ef migrations add {NombreMigracion} -p Infrastructure -s Api

▶️ VER ENDPOINTS:
   → http://localhost:5088/swagger/index.html

================================================================================
                      🏗️ ARQUITECTURA LIMPIA
================================================================================

El proyecto sigue Clean Architecture:

  [Frontend - Blazor WASM]
           ↓
  [API REST - ASP.NET]
           ↓
  [Domain Logic - Core]
           ↓
  [Use Cases - Application]
           ↓
  [Data Access - Infrastructure]

Beneficios:
✅ Sin aclopamiento entre capas
✅ Fácil de probar
✅ Fácil de mantener
✅ Escalable

================================================================================
                         📋 MÓDULOS PRINCIPALES
================================================================================

1. 👥 AUTORES (Authors)
   • Crear, leer, actualizar, eliminar autores
   • Información: nombre, apellido, fecha nacimiento, país, biografía
   • API: GET/POST/PUT/DELETE /api/authors

2. 📖 LIBROS (Books)
   • Crear, leer, actualizar, eliminar libros
   • Subir portadas (imágenes)
   • Información: título, ISBN, género, fecha publicación
   • API: GET/POST/PUT/DELETE /api/books

3. 🔄 PRÉSTAMOS (Loans)
   • Crear, gestionar, devolver préstamos
   • Un préstamo activo máximo por libro
   • Calcular multas automáticamente
   • API: GET/POST/PUT /api/loans

4. 📊 REPORTES (Reports)
   • Resumen de la biblioteca
   • Estadísticas de préstamos
   • Libros por género
   • API: GET /api/reports

================================================================================
                         🧪 PRUEBAS
================================================================================

El proyecto incluye 12 pruebas automatizadas:

Ubicación: Tests/

Tipos de pruebas:
  ✅ Unit Tests - Lógica de dominio (6)
  ✅ Integration Tests - Queries con BD (4)
  ✅ Serialization Tests - JSON (2)

Ejecutar:
  $ dotnet test Tests/

Resultado esperado:
  Total: 12; Passed: 12; Failed: 0

================================================================================
                      🐛 PROBLEMAS COMUNES
================================================================================

❌ "Database connection failed"
   → Verificar SQL Server está corriendo
   → Revisar appsettings.json connection string

❌ "Port 5088 already in use"
   → netstat -ano | findstr :5088
   → taskkill /PID {ProcessId} /F

❌ "Build error - NuGet packages"
   → dotnet nuget locals all --clear
   → dotnet restore

❌ "Test failed with Nullable warning"
   → Ya está solucionado, build limpio: 0 warnings

📖 Para más ayuda: Ver .docs/06-Troubleshooting/

================================================================================
                         📚 DOCUMENTACIÓN COMPLETA
================================================================================

Para información detallada, ver:

→ .docs/02-Architecture/        - Diseño del sistema
→ .docs/03-Development/         - Guía de desarrollo
→ .docs/04-Testing/             - Framework de pruebas
→ .docs/05-Deployment/          - Despliegue en producción

================================================================================
                     ✅ PROYECTO LISTO - PRODUCCIÓN
================================================================================

Estado del Sistema:
  ✅ Build: LIMPÍO (0 errores, 0 advertencias)
  ✅ Tests: TODO PASA (12/12 ✅)
  ✅ Architecture: CLEAN ARCHITECTURE
  ✅ Database: AUTO-MIGRACIÓN
  ✅ Frontend: BLAZOR WASM FUNCIONAL

Este proyecto está listo para producción con:
• Arquitectura limpia escalable
• Cobertura de pruebas completa
• Validación robusta
• Manejo de errores adecuado
• Logging y auditoría

================================================================================
                        Última actualización: 13 April 2026
================================================================================
