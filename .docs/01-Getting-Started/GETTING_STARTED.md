# 🚀 COMENZAR EN 3 PASOS

**Documento más simple y rápido para ejecutar el proyecto**

---

## ✅ Verificar Requisitos

```bash
# 1. Verificar .NET 10
dotnet --version

# 2. Verificar LocalDB
sqllocaldb info
```

Si alguno falta:
- **.NET 10**: https://dotnet.microsoft.com/download/dotnet/10.0
- **LocalDB**: Se incluye con Visual Studio 2022

---

## ▶️ OPCIÓN A: Ejecución Automática (RECOMENDADO)

### Windows PowerShell

```powershell
# Paso 1: Setup inicial (verifica todo, compila proyecto)
.\setup.ps1

# Paso 2: Ejecutar TODO con un comando
.\run_all.ps1
```

### Windows CMD

```batch
# Paso 1: Setup inicial
setup.bat

# Paso 2: Ejecutar TODO
run_all.bat
```

✅ **Listo.** Ambos servicios corriendo en 2 clics.

---

## ▶️ OPCIÓN B: Manual en DOS Terminales

```bash
# TERMINAL 1: Backend API
cd Api
dotnet run
```

```bash
# TERMINAL 2: Frontend (mientras Terminal 1 sigue ejecutando)
cd Frontend
dotnet run
```

---

## 🌐 Accede a la Aplicación

| Componente | URL |
|-----------|-----|
| **Frontend** | http://localhost:5098 |
| **API** | http://localhost:5088 |
| **Documentación** | http://localhost:5088/swagger |

---

## 📚 ¿Necesitas más ayuda?

| Pregunta | Documento |
|----------|-----------|
| Instalación detallada | [README.md](./README.md) |
| Quick start (5 min) | [QUICK_START.md](./QUICK_START.md) |
| Troubleshooting | [README.md#troubleshooting](./README.md#troubleshooting) |

---

**¡Eso es todo!** 🎉 Sistema ejecutándose.
