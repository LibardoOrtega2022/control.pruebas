@echo off
REM Script para ejecutar Frontend Blazor en Windows CMD
REM Uso: run_frontend.bat

cls
echo.
echo ╔════════════════════════════════════════╗
echo ║   🌐 BIBLIOTECA - Frontend Blazor    ║
echo ╚════════════════════════════════════════╝
echo.

cd Infrastructure\Frontend
echo 🔨 Compilando Frontend...
dotnet build --configuration Release
if errorlevel 1 (
    echo ❌ Error en compilacion
    pause
    exit /b 1
)

echo.
echo 🚀 Iniciando Frontend Blazor...
echo 📍 URL: http://localhost:5098
echo.
echo Presiona Ctrl+C para detener
echo 💡 Asegurate que en otra terminal este corriendo: run_api.bat
echo.

dotnet run --configuration Release
