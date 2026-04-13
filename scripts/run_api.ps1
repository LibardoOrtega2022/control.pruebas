# Script para ejecutar API Backend
# Uso: .\run_api.ps1
# Descripción: Compila y ejecuta el backend en http://localhost:5088

Write-Host ""
Write-Host "BIBLIOTECA - API Backend" -ForegroundColor Cyan
Write-Host ""

$ApiPath = ".\Api"

if (-not (Test-Path $ApiPath)) {
    Write-Host "Carpeta Api/ no encontrada en directorio actual" -ForegroundColor Red
    exit 1
}

Write-Host "Compilando API..." -ForegroundColor Yellow
cd $ApiPath

try {
    $buildOutput = dotnet build --configuration Release 2>&1
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Error en compilacion:" -ForegroundColor Red
        Write-Host $buildOutput
        exit 1
    }
    
    Write-Host "Compilacion exitosa" -ForegroundColor Green
}
catch {
    Write-Host "Error: $_" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "Iniciando API Backend..." -ForegroundColor Green
Write-Host "URL: http://localhost:5088" -ForegroundColor Cyan
Write-Host "Swagger: http://localhost:5088/swagger" -ForegroundColor Cyan
Write-Host ""
Write-Host "Presiona Ctrl+C para detener" -ForegroundColor Yellow
Write-Host ""

dotnet run --configuration Release
