# Script para ejecutar API + Frontend en paralelo
# Uso: .\run_all.ps1
# Descripción: Inicia ambos servicios en ventanas separadas

Write-Host ""
Write-Host "BIBLIOTECA - Ejecutar Todo" -ForegroundColor Cyan
Write-Host ""

$currentDir = Get-Location

# Verificar que los scripts existan
if (-not (Test-Path ".\run_api.ps1")) {
    Write-Host "run_api.ps1 no encontrado" -ForegroundColor Red
    exit 1
}

if (-not (Test-Path ".\run_frontend.ps1")) {
    Write-Host "run_frontend.ps1 no encontrado" -ForegroundColor Red
    exit 1
}

Write-Host "Iniciando servicios en paralelo..." -ForegroundColor Green
Write-Host ""

# Iniciar API en una ventana nueva
Write-Host "1. Iniciando API Backend..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$currentDir'; .\run_api.ps1"
Start-Sleep -Seconds 2

# Iniciar Frontend en otra ventana
Write-Host "2. Iniciando Frontend Blazor..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$currentDir'; .\run_frontend.ps1"
Start-Sleep -Seconds 2

Write-Host ""
Write-Host "Ambos servicios iniciados" -ForegroundColor Green
Write-Host ""

Write-Host "URLs disponibles:" -ForegroundColor Cyan
Write-Host "   Frontend:  http://localhost:5089" -ForegroundColor Yellow
Write-Host "   API:       http://localhost:5088" -ForegroundColor Yellow
Write-Host "   Swagger:   http://localhost:5088/swagger" -ForegroundColor Yellow
Write-Host ""

Write-Host "Verifica las dos ventanas nuevas de PowerShell" -ForegroundColor Gray
Write-Host "Para detener todo: Cierra ambas ventanas" -ForegroundColor Gray
Write-Host ""
