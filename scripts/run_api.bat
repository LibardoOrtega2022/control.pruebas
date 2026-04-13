@echo off
REM Script para ejecutar API Backend en Windows CMD
REM Uso: run_api.bat

cls
echo.
echo ╔════════════════════════════════════════╗
echo ║   🔌 BIBLIOTECA - API Backend        ║
echo ╚════════════════════════════════════════╝
echo.

cd Api
echo 🔨 Compilando API...
dotnet build --configuration Release
if errorlevel 1 (
    echo ❌ Error en compilacion
    pause
    exit /b 1
)

echo.
echo 🚀 Iniciando API Backend...
echo 📍 URL: http://localhost:5088
echo 📖 Swagger: http://localhost:5088/swagger
echo.
echo Presiona Ctrl+C para detener
echo.

dotnet run --configuration Release
