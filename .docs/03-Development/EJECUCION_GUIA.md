# 📋 Guía Completa de Ejecución - Sistema Biblioteca

**Actualizado**: Abril 13, 2026  
**Versión**: 1.0

---

## 🎯 Objetivo

Este documento explica cómo ejecutar el Sistema de Gestión de Biblioteca de forma fácil y rápida.

---

## 📁 Archivos de Ejecución Disponibles

### 🟢 **Automatizados (RECOMENDADO)**

| Archivo | SO | Descripción | Usa |
|---------|-----|-------------|-----|
| `setup.ps1` | Windows | Setup inicial (verifica requisitos, compila) | PowerShell |
| `setup.bat` | Windows | Setup inicial | CMD |
| `run_all.ps1` | Windows | Ejecuta API + Frontend en paralelo | PowerShell |
| `run_all.bat` | Windows | Ejecuta API + Frontend en paralelo | CMD |

### 🟡 **Individuales**

| Archivo | SO | Descripción |
|---------|-----|-------------|
| `run_api.ps1` | Windows | Solo API Backend |
| `run_api.bat` | Windows | Solo API Backend |
| `run_frontend.ps1` | Windows | Solo Frontend Blazor |
| `run_frontend.bat` | Windows | Solo Frontend Blazor |

---

## 🚀 Cómo Ejecutar

### Paso 1: Verificar Requisitos

```bash
# .NET 10
dotnet --version
# Esperado: 10.0.x

# SQL Server LocalDB
sqllocaldb info
# Esperado: MSSQLLocalDB disponible
```

Si falta algo:
- **.NET 10**: https://dotnet.microsoft.com/download/dotnet/10.0
- **LocalDB**: Incluido con Visual Studio 2022

### Paso 2: Setup Inicial (Una sola vez)

#### PowerShell:
```powershell
.\setup.ps1
```

#### CMD:
```batch
setup.bat
```

**¿Qué hace?**
- ✅ Verifica .NET 10
- ✅ Verifica LocalDB
- ✅ Compila solución completamente
- ✅ Valida estructura del proyecto

### Paso 3: Ejecutar Aplicación

#### Opción A: TODO en una ventana (RECOMENDADO)

**PowerShell:**
```powershell
.\run_all.ps1
```

**CMD:**
```batch
run_all.bat
```

✅ Esto abre 2 ventanas automáticamente:
- Ventana 1: API Backend (http://localhost:5088)
- Ventana 2: Frontend Blazor (http://localhost:5098)

#### Opción B: Por Separado (en 2 terminales)

**PowerShell - Terminal 1 (API):**
```powershell
.\run_api.ps1
```

**PowerShell - Terminal 2 (Frontend):**
```powershell
.\run_frontend.ps1
```

---

**CMD - Terminal 1 (API):**
```batch
run_api.bat
```

**CMD - Terminal 2 (Frontend):**
```batch
run_frontend.bat
```

#### Opción C: Manual (Comando convencional)

**Terminal 1 - API:**
```bash
cd Api
dotnet run
```

**Terminal 2 - Frontend:**
```bash
cd Infrastructure\Frontend
dotnet run
```

---

## 🌐 Acceder a la Aplicación

Una vez ejecutando, accede desde cualquier navegador:

| Componente | URL | Descripción |
|-----------|-----|-------------|
| **Frontend** | http://localhost:5098 | Interfaz de usuario (Blazor WASM) |
| **API Backend** | http://localhost:5088 | REST API endpoints |
| **Swagger UI** | http://localhost:5088/swagger | Documentación interactiva |
| **Swagger JSON** | http://localhost:5088/swagger/v1/swagger.json | Especificación OpenAPI |

---

## ✅ Verificación

### Paso 1: Frontend Cargó

Deberías ver en http://localhost:5098:
```
Biblioteca (logo)
├── Inicio
├── Autores  
├── Libros
├── Préstamos
└── Reportes
```

### Paso 2: API Responde

Abre otra terminal y ejecuta:
```bash
curl -X GET "http://localhost:5088/api/author?page=1&pageSize=10"
```

Esperado:
```json
{
  "data": [...],
  "count": 2
}
```

### Paso 3: Swagger Carga

Abre http://localhost:5088/swagger en navegador.

Deberías ver interfaz de Swagger con todos los endpoints documentados.

---

## 🎯 Flujo Típico

1. **Primer día:**
   ```powershell
   # Una sola vez
   .\setup.ps1
   ```

2. **Diario - Ejecutar aplicación:**
   ```powershell
   # Cada vez que quieras correr
   .\run_all.ps1
   ```

3. **Abrir aplicación:**
   - Frontend: http://localhost:5098
   - Swagger API: http://localhost:5088/swagger

4. **Detener:**
   - Cierra ambas ventanas de Terminal

---

## 🐛 Solución de Problemas

### ❌ Error: "No se reconoce run_all.ps1"

**Solución:**
```powershell
# Ejecutar desde la carpeta raíz del proyecto
cd C:\ruta\al\proyecto
.\run_all.ps1
```

### ❌ Error: "Puerto 5088 en uso"

**Solución 1:** Usar otro puerto
```bash
cd Api
dotnet run --urls "http://localhost:5170"
```

**Solución 2:** Liberar puerto
```bash
# Encontrar proceso en puerto
netstat -ano | findstr ":5088"

# Matar proceso (reemplazar PID)
taskkill /PID <PID> /F
```

### ❌ Error: "LocalDB no encontrado"

**Solución:**
```bash
# Verificar instalación
sqllocaldb info

# Si no aparece, instalar desde Visual Studio Installer
# o descargar: https://learn.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb
```

### ❌ Error: "Compilación fallida"

**Solución:**
```bash
# Limpiar build anterior
dotnet clean

# Reconstruir
dotnet build

# O ejecutar setup nuevamente
.\setup.ps1
```

---

## 📊 Estructura de Ejecución

```
setup.ps1 / setup.bat
    ↓
    ├─ Verifica .NET 10
    ├─ Verifica LocalDB
    ├─ Compila Api/
    ├─ Compila Application/
    ├─ Compila Core/
    ├─ Compila Infrastructure/
    └─ Compila Infrastructure/Frontend/
        ↓
    ✅ Listo para ejecutar

run_all.ps1 / run_all.bat
    ↓
    ├─ Terminal 1: cd Api && dotnet run
    │   └─ http://localhost:5088
    │
    └─ Terminal 2: cd Infrastructure\Frontend && dotnet run
        └─ http://localhost:5098
```

---

## 💡 Pro Tips

### Compilación más rápida
```bash
dotnet build --parallel
```

### Ejecutar en Release (rendimiento)
```bash
dotnet run --configuration Release
```

### Ver logs detallado
```bash
dotnet run --verbosity detailed
```

### Ejecutar sin compilar
```bash
dotnet run --no-build
```

---

## 📚 Documentación Relacionada

| Necesitas | Archivo |
|-----------|---------|
| Más explicación de setup | [GETTING_STARTED.md](./GETTING_STARTED.md) |
| Guía de 5 minutos | [QUICK_START.md](./QUICK_START.md) |
| Instalación detallada | [README.md](./README.md) |
| Arquitectura del proyecto | [ARCHITECTURE.md](./ARCHITECTURE.md) |
| Índice completo | [DOCUMENTACION_INDICE.md](./DOCUMENTACION_INDICE.md) |
| Bugs arreglados | [BUGS_ARREGLADOS.md](./BUGS_ARREGLADOS.md) |

---

## ✨ Estado del Sistema

| Componente | Estado |
|-----------|--------|
| Backend API | ✅ Compilado y listo |
| Frontend Blazor | ✅ Compilado y listo |
| Database (LocalDB) | ✅ Automático |
| Swagger UI | ✅ Documentado y accesible |
| Postman Collection | ✅ Disponible |

---

## 🎉 ¡Listo!

Sistema completamente funcional en 3 pasos:
1. `.\setup.ps1`
2. `.\run_all.ps1`
3. Abre http://localhost:5098

**¡Disfruta!** 🚀

---

**Última actualización**: Abril 13, 2026
