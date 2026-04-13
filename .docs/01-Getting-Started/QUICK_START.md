# ⚡ Quick Start - 5 minutos para ejecutar

**Versión**: 1.0  
**Actualizado**: Abril 13, 2026  
**Tiempo estimado**: 5 minutos

---

## 🚀 Opción 1: Ejecución Automática (RECOMENDADO)

### Windows PowerShell

```powershell
# 1. Abre PowerShell como administrador en la carpeta del proyecto

# 2. Ejecuta el script de setup
.\setup.ps1

# 3. Una vez finalizado, el proyecto está listo para usar
```

**¿Qué hace el script?**
- ✅ Verifica .NET 10 instalado
- ✅ Verifica SQL Server LocalDB
- ✅ Compila backend (Api, Application, Core, Infrastructure)
- ✅ Compila frontend (Blazor)
- ✅ Muestra rutas de acceso

---

## 🎯 Opción 2: Ejecución Manual en 3 Pasos

### Paso 1: Compilar Backend

```bash
cd Api
dotnet build --configuration Release
cd ..
```

**Esperado:** "Build succeeded" sin errores

### Paso 2: Compilar Frontend

```bash
cd Infrastructure\Frontend
dotnet build --configuration Release
cd ..
```

**Esperado:** Compilación sin errores

### Paso 3: Ejecutar Aplicación

**Terminal 1 - API Backend:**
```bash
cd Api
dotnet run
```

**Esperado:**
```
Now listening on: http://localhost:5088
Application started
```

**Terminal 2 - Frontend Blazor:**
```bash
cd Infrastructure\Frontend
dotnet run
```

**Esperado:**
```
Now listening on: http://localhost:5089
Application started
```

---

## 📱 Acceder a la Aplicación

### Una vez corriendo ambas:

| Componente | URL | Descripción |
|-----------|-----|-------------|
| 🌐 **Frontend** | http://localhost:5089 | Interfaz Blazor WebAssembly |
| 🔌 **API Backend** | http://localhost:5088/api | REST API |
| 📖 **Swagger UI** | http://localhost:5088/swagger | Documentación interactiva |

---

## 🧪 Verificación Rápida

### ✅ Frontend cargó correctamente

Deberías ver:
```
Biblioteca (logo)
├── Inicio
├── Autores  
├── Libros
├── Préstamos
└── Reportes
```

### ✅ Backend funcionando

```bash
# En otra Terminal (o en Postman)
curl -X GET "http://localhost:5088/api/author?page=1&pageSize=10"
```

**Esperado respuesta:**
```json
{
  "data": [...],
  "count": 2
}
```

### ✅ Swagger cargando

Abre: http://localhost:5088/swagger

Deberías ver la interfaz de Swagger con todos los endpoints

---

## ⚙️ Configuración de BD

### LocalDB Automática (DEFAULT)

El proyecto usa **SQL Server LocalDB** automáticamente:
- **Connection String:** `Server=(localdb)\\mssqllocaldb;Database=BibliotecaDb;Trusted_Connection=true;`
- **Base de datos:** Se crea automáticamente en primer acceso
- **No requiere pasos adicionales**

### Cambiar a otra BD

Ver **README.md** → Sección "Configuración" para:
- Azure SQL Server
- Docker SQL Server
- Otros dialectos SQL

---

## 🐛 Troubleshooting Rápido

### ❌ Erro: .NET 10 no instalado

```bash
# Verifica versión
dotnet --version

# Descarga desde: https://dotnet.microsoft.com/download/dotnet/10.0
```

### ❌ Error: LocalDB no encontrado

```bash
# Verifica instalación
sqllocaldb info

# Si no aparece, instala desde Visual Studio 2022 Installer
# o descarga: https://learn.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb
```

### ❌ Puerto 5088 o 5089 en uso

```bash
# Usa el script con puerto custom
.\run.ps1 -ApiPort 5170 -FrontendPort 5171
```

### ❌ Compilation failed

```bash
# Limpia y reconstruye
dotnet clean
dotnet build
```

---

## 📚 Documentación Completa

| Necesitas | Accede a |
|-----------|----------|
| Instalación detallada | [README.md](./README.md) |
| Decisiones arquitectónicas | [ARCHITECTURE.md](./ARCHITECTURE.md) |
| Bugs arreglados | [BUGS_ARREGLADOS.md](./BUGS_ARREGLADOS.md) |
| Endpoints en Postman | [Biblioteca.postman_collection.json](./Biblioteca.postman_collection.json) |
| Índice completo | [DOCUMENTACION_INDICE.md](./DOCUMENTACION_INDICE.md) |

---

## ✨ Pro Tips

### 💡 Compilar en paralelo (más rápido)

```bash
dotnet build --parallel
```

### 💡 Ejecutar en Release (más rápido)

```bash
dotnet run --configuration Release
```

### 💡 Ejecutar ambos en una sola Terminal

```bash
# En PowerShell
start-process powershell -ArgumentList "cd $pwd; .\run_api.ps1"
start-process powershell -ArgumentList "cd $pwd; .\run_frontend.ps1"
```

### 💡 Testing sin frontend

Solo ejecutar API y usar Swagger:
```bash
cd Api
dotnet run
# Accede http://localhost:5088/swagger
```

---

## 📊 Checklist de Setup

- [ ] .NET 10 instalado (`dotnet --version`)
- [ ] SQL Server LocalDB disponible (`sqllocaldb info`)
- [ ] Proyecto compilado sin errores (`dotnet build`)
- [ ] API corriendo en http://localhost:5088
- [ ] Frontend corriendo en http://localhost:5089
- [ ] Swagger accesible en http://localhost:5088/swagger
- [ ] Frontend carga con navbar visible
- [ ] API responde a GET /api/author

---

**¡Listo!** 🎉 Sistema completamente funcional en 5 minutos.

❓ Si quedan dudas, ver [README.md](./README.md) o [DOCUMENTACION_INDICE.md](./DOCUMENTACION_INDICE.md)
