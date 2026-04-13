@echo off
REM Script de Setup para Windows CMD (alternativa a PowerShell)
REM Uso: setup.bat

cls
echo.
echo ╔════════════════════════════════════════╗
echo ║   BIBLIOTECA - Setup Automatizado     ║
echo ╚════════════════════════════════════════╝
echo.

REM Verificar .NET 10
echo 🔍 Verificando .NET 10...
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo ❌ .NET 10 SDK no instalado
    echo    Descargalo desde: https://dotnet.microsoft.com/download/dotnet/10.0
    pause
    exit /b 1
)
echo ✅ .NET 10: Instalado

REM Verificar SQL Server LocalDB
echo 🔍 Verificando SQL Server LocalDB...
sqllocaldb info >nul 2>&1
if errorlevel 1 (
    echo ⚠️  SQL Server LocalDB no encontrado (se intentará instalar automáticamente)
) else (
    echo ✅ SQL Server LocalDB: Disponible
)

REM Compilar solución
echo.
echo 🔨 Compilando solución...
dotnet build "Biblioteca.sln" --configuration Release
if errorlevel 1 (
    echo ❌ Error en compilacion
    pause
    exit /b 1
)
echo ✅ Compilación exitosa

REM Información final
echo.
echo ╔════════════════════════════════════════╗
echo ║   ✅ Setup Completado Exitosamente    ║
echo ╚════════════════════════════════════════╝
echo.
echo Para ejecutar la aplicacion, usa:
echo.
echo 1️⃣  OPCION A - Ejecutar TODO en paralelo:
echo    run_all.bat
echo.
echo 2️⃣  OPCION B - Ejecutar solo API:
echo    run_api.bat
echo.
echo 3️⃣  OPCION C - Ejecutar solo Frontend:
echo    run_frontend.bat
echo.
pause
