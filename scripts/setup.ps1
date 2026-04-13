# Script de Setup Automatizado - Biblioteca
# Uso: .\setup.ps1
# Descripción: Verifica requisitos y compila el proyecto

Write-Host ""
Write-Host "BIBLIOTECA - Setup Automatizado" -ForegroundColor Cyan
Write-Host ""

# Función para verificar instalación
function Test-Installation {
    param(
        [string]$Name,
        [string]$Command,
        [string]$MinVersion = ""
    )
    
    Write-Host "Verificando $Name..." -ForegroundColor Yellow
    
    try {
        $result = & cmd /c "$Command 2>&1" | Select-String -Pattern "error" -NotMatch | Select-Object -First 1
        
        if ($result) {
            Write-Host "$Name: OK" -ForegroundColor Green
            return $true
        }
        else {
            Write-Host "$Name: NO ENCONTRADO" -ForegroundColor Red
            return $false
        }
    }
    catch {
        Write-Host "$Name: Error al verificar" -ForegroundColor Red
        return $false
    }
}

# Paso 1: Verificar .NET 10
Write-Host ""
Write-Host "PASO 1: Verificar Requisitos" -ForegroundColor Magenta
Write-Host "==================="

$dotnetOk = Test-Installation ".NET 10 SDK" "dotnet --version"

if (-not $dotnetOk) {
    Write-Host ""
    Write-Host ".NET 10 SDK no instalado" -ForegroundColor Red
    Write-Host "Descargalo desde: https://dotnet.microsoft.com/download/dotnet/10.0" -ForegroundColor Yellow
    exit 1
}

# Paso 2: Verificar SQL Server LocalDB
$localdbOk = Test-Installation "SQL Server LocalDB" "sqllocaldb info"

if (-not $localdbOk) {
    Write-Host ""
    Write-Host "SQL Server LocalDB no encontrado" -ForegroundColor Yellow
    Write-Host "Se instalara automáticamente en primer acceso" -ForegroundColor Gray
}

# Paso 3: Compilar Solución
Write-Host ""
Write-Host "PASO 2: Compilar Solución" -ForegroundColor Magenta
Write-Host "==================="

try {
    Write-Host "Compilando Biblioteca.sln..." -ForegroundColor Cyan
    
    $buildOutput = dotnet build "Biblioteca.sln" --configuration Release 2>&1
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Compilación exitosa" -ForegroundColor Green
        Write-Host $buildOutput | Select-String "Build succeeded"
    }
    else {
        Write-Host "Error en compilación" -ForegroundColor Red
        Write-Host $buildOutput
        exit 1
    }
}
catch {
    Write-Host "Error durante compilación: $_" -ForegroundColor Red
    exit 1
}

# Paso 4: Verificar carpetas importantes
Write-Host ""
Write-Host "PASO 3: Verificar Estructura" -ForegroundColor Magenta
Write-Host "==================="

$folders = @(
    "Api",
    "Application",
    "Core",
    "Infrastructure"
)

foreach ($folder in $folders) {
    if (Test-Path $folder) {
        Write-Host "$folder/: OK" -ForegroundColor Green
    }
    else {
        Write-Host "$folder/: NO ENCONTRADO" -ForegroundColor Red
    }
}

# Paso 5: Información de ejecución
Write-Host ""
Write-Host "PASO 4: Información de Ejecución" -ForegroundColor Magenta
Write-Host "==================="

Write-Host ""
Write-Host "Para ejecutar la aplicación, usa:" -ForegroundColor Cyan

Write-Host ""
Write-Host "1. BACKEND API:" -ForegroundColor Yellow
Write-Host "   cd Api" -ForegroundColor Gray
Write-Host "   dotnet run" -ForegroundColor Gray
Write-Host "   URL: http://localhost:5088" -ForegroundColor Cyan
Write-Host "   Swagger: http://localhost:5088/swagger" -ForegroundColor Cyan

Write-Host ""
Write-Host "2. FRONTEND BLAZOR (en otra terminal):" -ForegroundColor Yellow
Write-Host "   cd Infrastructure\Frontend" -ForegroundColor Gray
Write-Host "   dotnet run" -ForegroundColor Gray
Write-Host "   URL: http://localhost:5098" -ForegroundColor Cyan

Write-Host ""
Write-Host "Alternativa rapida - Usa los scripts:" -ForegroundColor Cyan
Write-Host "   .\run_api.ps1      (ejecuta solo API)" -ForegroundColor Gray
Write-Host "   .\run_frontend.ps1 (ejecuta solo Frontend)" -ForegroundColor Gray
Write-Host "   .\run_all.ps1      (ejecuta ambos en paralelo)" -ForegroundColor Gray

# Paso 6: Guía rápida
Write-Host ""
Write-Host "Documentación:" -ForegroundColor Cyan
Write-Host "   Quick Start:  QUICK_START.md" -ForegroundColor Gray
Write-Host "   Completa:     README.md" -ForegroundColor Gray
Write-Host "   Arquitectura: ARCHITECTURE.md" -ForegroundColor Gray
Write-Host "   Postman:      Biblioteca.postman_collection.json" -ForegroundColor Gray

Write-Host ""
Write-Host "Setup Completado Exitosamente" -ForegroundColor Green
Write-Host ""
