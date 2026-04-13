# Script para ejecutar Frontend Blazor
# Uso: .\run_frontend.ps1
# Descripción: Compila y ejecuta el frontend en http://localhost:5098

Write-Host ""
Write-Host "BIBLIOTECA - Frontend Blazor" -ForegroundColor Cyan
Write-Host ""

$FrontendPath = ".\Infrastructure\Frontend"

if (-not (Test-Path $FrontendPath)) {
    Write-Host "Carpeta Infrastructure\Frontend no encontrada en directorio actual" -ForegroundColor Red
    exit 1
}

Write-Host "Compilando Frontend..." -ForegroundColor Yellow
cd $FrontendPath

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
Write-Host "Iniciando Frontend Blazor..." -ForegroundColor Green
Write-Host "URL: http://localhost:5098" -ForegroundColor Cyan
Write-Host ""
Write-Host "Presiona Ctrl+C para detener" -ForegroundColor Yellow
Write-Host "Asegurate que en otra terminal este corriendo: .\run_api.ps1" -ForegroundColor Gray
Write-Host ""

dotnet run --configuration Release
