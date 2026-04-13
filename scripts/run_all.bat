@echo off
REM Script para ejecutar API + Frontend en paralelo en Windows CMD
REM Uso: run_all.bat

cls
echo.
echo ╔════════════════════════════════════════╗
echo ║   ▶️  BIBLIOTECA - Ejecutar Todo      ║
echo ╚════════════════════════════════════════╝
echo.

if not exist "run_api.bat" (
    echo ❌ run_api.bat no encontrado
    pause
    exit /b 1
)

if not exist "run_frontend.bat" (
    echo ❌ run_frontend.bat no encontrado
    pause
    exit /b 1
)

echo 🚀 Iniciando servicios en paralelo...
echo.

REM Iniciar API en ventana nueva
echo 1️⃣  Iniciando API Backend...
start cmd /k call run_api.bat

REM Esperar un poco
timeout /t 3 /nobreak

REM Iniciar Frontend en otra ventana
echo 2️⃣  Iniciando Frontend Blazor...
start cmd /k call run_frontend.bat

timeout /t 2 /nobreak

cls
echo.
echo ╔════════════════════════════════════════╗
echo ║   Ambos servicios iniciados ✅         ║
echo ╚════════════════════════════════════════╝
echo.
echo 📍 URLs disponibles:
echo    Frontend:  http://localhost:5098
echo    API:       http://localhost:5088
echo    Swagger:   http://localhost:5088/swagger
echo.
echo 💡 Verifica las dos ventanas nuevas de CMD
echo    Para detener: Cierra ambas ventanas
echo.
