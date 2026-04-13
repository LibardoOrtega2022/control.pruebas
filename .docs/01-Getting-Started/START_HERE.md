╔═══════════════════════════════════════════════════════════════════════════╗
║                   👋 BIENVENIDO - SISTEMA BIBLIOTECA                       ║
║                                                                             ║
║  📚 Gestión de Biblioteca con .NET 10 + Blazor WebAssembly                 ║
╚═══════════════════════════════════════════════════════════════════════════╝

🎯 PARA EMPEZAR RÁPIDO (5 minutos):

   OPCIÓN A: PowerShell (Recomendado)
   ────────────────────────────────
   1. Abre PowerShell en esta carpeta
   2. Ejecuta: .\setup.ps1
   3. Luego: .\run_all.ps1

   ✅ Sistema completo funcionando en 2 comandos


   OPCIÓN B: Windows CMD
   ─────────────────────
   1. Abre CMD/Símbolo del sistema en esta carpeta
   2. Ejecuta: setup.bat
   3. Luego: run_all.bat

   ✅ Sistema completo funcionando en 2 comandos


📋 DOCUMENTACIÓN:

   Principiante                      Experimentado
   ├─ GETTING_STARTED.md            ├─ README.md
   ├─ QUICK_START.md                ├─ ARCHITECTURE.md
   └─ setup.ps1 (automático)        └─ DOCUMENTACION_INDICE.md

   Testing
   ├─ Postman: Biblioteca.postman_collection.json
   └─ Swagger: http://localhost:5088/swagger


🚀 URLS IMPORTANTES (una vez ejecutando):

   Frontend:  http://localhost:5098
   API:       http://localhost:5088
   Swagger:   http://localhost:5088/swagger


⚡ COMANDOS RÁPIDOS:

   Setup inicial (una sola vez):
   ├─ PowerShell: .\setup.ps1
   └─ CMD:        setup.bat

   Ejecutar TODO en paralelo:
   ├─ PowerShell: .\run_all.ps1
   └─ CMD:        run_all.bat

   Ejecutar por separado:
   ├─ PowerShell API:      .\run_api.ps1      (Terminal 1)
   ├─ PowerShell Frontend: .\run_frontend.ps1 (Terminal 2)
   ├─ CMD API:             run_api.bat        (Terminal 1)
   └─ CMD Frontend:        run_frontend.bat   (Terminal 2)


❓ ¿PROBLEMAS?

   LocalDB no instalado:     Ver README.md → Requisitos
   Puertos en uso:           Ver QUICK_START.md → Troubleshooting
   Compilación fallida:      Ver README.md → Troubleshooting


✨ ARQUITECTURA DEL PROYECTO:

   Api/              Backend REST API (C#, .NET 10)
   Frontend/         Frontend Blazor WASM
   Application/      Lógica de negocio (DTOs, Use Cases)
   Core/             Domain Models & Services
   Infrastructure/   Data Access (EF Core, Dapper)


📊 ESTADO DEL PROYECTO: ✅ LISTO PARA PRODUCCIÓN

   ✅ Backend compilado
   ✅ Frontend compilado
   ✅ Swagger documentado
   ✅ 3 bugs arreglados
   ✅ Testing completado
   ✅ Documentación integral


👉 SIGUIENTE PASO:

   1. Abre PowerShell aquí
   2. Escribe: .\setup.ps1
   3. Presiona Enter
   4. Luego: .\run_all.ps1

   ¡Listo! Sistema funcionando.

═══════════════════════════════════════════════════════════════════════════════
