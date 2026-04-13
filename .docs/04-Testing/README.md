================================================================================
                    04-TESTING - PRUEBAS UNITARIAS
================================================================================

Esta sección contiene toda la documentación sobre testing, reports y
estrategia de pruebas del proyecto Biblioteca.

📁 Archivos en esta sección:
------ ------
• TEST_REPORT.txt                          - Reporte de ejecución (12/12 ✅)
• TESTING_DEBUGGING_REPORTE_FINAL.md      - Guía de debugging

================================================================================
                          ESTADO ACTUAL
================================================================================

Resumen:
  ✅ Tests Total:           12
  ✅ Pasadas:               12
  ❌ Fallidas:              0
  ✅ Succes Rate:           100%

Build:
  ✅ Errores:        0
  ✅ Advertencias:   0
  ✅ Status:         LIMPIO

Tiempo de ejecución: 3.2 segundos

================================================================================
                      ESTRUCTURA DE PRUEBAS
================================================================================

Tests/ (proyecto xUnit)
├── Domains/
│   ├── Authors/
│   │   └── CreateAuthorDomainTests.cs      [3 tests]
│   └── Books/
│       └── CreateBookDomainTests.cs        [3 tests]
│
├── Queries/
│   └── Authors/
│       └── AuthorQueriesTests.cs           [4 tests]
│
└── Controllers/
    └── AuthorControllerTests.cs            [2 tests]

Total: 12 Test Cases

================================================================================
                      DETALLES POR CATEGORÍA
================================================================================

1️⃣  DOMAIN SERVICE TESTS (6 tests)
    ──────────────────────────

    CreateAuthorDomainTests.cs
    ─────────────────────────
    ✅ CreateAsync_WithValidRequest_ReturnsAuthorResponse
       Valida creación de autor con datos válidos
       Simula: Repository, UnitOfWork
       Assertions: 4

    ✅ CreateAsync_WithoutBiography_ReturnsAuthorResponse
       Valida que biografía puede estar vacía
       Assertions: 3

    ✅ CreateAsync_CallsBeginAndCommit
       Verifica transacciones (Begin/Commit)
       Assertions: 2

    CreateBookDomainTests.cs
    ──────────────────────
    ✅ CreateAsync_WithValidRequest_ReturnsBookResponse
       Valida creación de libro con autor resuelto
       Mocks: AuthorRepository, UnitOfWork
       Assertions: 4

    ✅ CreateAsync_WithInvalidAuthorId_ThrowsKeyNotFoundException
       Valida excepción cuando autor no existe
       Exception: KeyNotFoundException
       Assertions: 1

    ✅ CreateAsync_WithFutureDate_ThrowsArgumentException
       Valida regla: fecha de publicación no puede ser futura
       Exception: ArgumentException
       Assertions: 1

2️⃣  INTEGRATION TESTS (4 tests)
    ────────────────────

    AuthorQueriesTests.cs
    ─────────────────────
    Database: InMemory (EF Core)
    Datos: 3 autores seed

    ✅ GetAllAuthors_WithInMemory_ReturnsAllAuthors
       Valida consulta de todos los autores
       Assertions: 3

    ✅ GetAuthorById_WithInMemory_ReturnsAuthor
       Valida búsqueda por ID
       Assertions: 3

    ✅ GetAuthorById_NotFound_ReturnsNull
       Valida que retorna null si no existe
       Assertions: 1

    ✅ GetAuthors_WithPagination_ReturnsPaginatedResults
       Valida Skip/Take (paginación)
       Assertions: 2

3️⃣  SERIALIZATION TESTS (2 tests)
    ───────────────────

    AuthorControllerTests.cs
    ────────────────────────
    Serializer: System.Text.Json

    ✅ SerializeAuthorRequest_Works
       Valida CreateAuthorRequest → JSON
       Assertions: 3

    ✅ DeserializeAuthorResponse_Works
       Valida JSON → AuthorResponse (PropertyNameCaseInsensitive)
       Assertions: 4

================================================================================
                        FRAMEWORK & DEPENDENCIAS
================================================================================

xUnit:        2.6+  (Test runner)
Moq:          4.20.72 (Mocking)
EF InMemory:  Latest (Test database)
MSTest.TestAdapter: xUnit VSTest Adapter

Lenguaje: C# 14.0
Framework: net10.0
Runtime: .NET 10.0.3

================================================================================
                      PATRONES DE PRUEBA USADOS
================================================================================

✅ AAA Pattern (Arrange-Act-Assert)
   Estructura clara: Setup → Ejecución → Verificación

✅ Moq Mocks
   Aislar dependencias (Repository, UnitOfWork)

✅ InMemory Database
   Pruebas rápidas sin BD real

✅ IDisposable Pattern
   Limpiar recursos después de pruebas

================================================================================
                          CÓMO CORRER LOS TESTS
================================================================================

Desde línea de comandos:

# Todos los tests
$ cd Tests
$ dotnet test

# Un test específico
$ dotnet test --filter "CreateAsync_WithValidRequest"

# Con salida detallada
$ dotnet test --verbosity detailed

# Con reporte de cobertura
$ dotnet test /p:CollectCoverageMetrics=true

Resultado esperado:
  Test Run Successful
  Total Tests: 12
  Passed: 12
  Failed: 0

================================================================================
                        ESCRIBIR NUEVAS PRUEBAS
================================================================================

Template básico (xUnit + Moq):

    [Fact]
    public async Task Method_Scenario_ExpectedResult()
    {
        // Arrange - Setup
        var mockRepo = new Mock<IRepository>();
        mockRepo.Setup(x => x.GetAsync(1))
            .ReturnsAsync(new Entity { Id = 1 });
        
        // Act - Execute
        var result = await _service.DoSomething(1);
        
        // Assert - Verify
        Assert.NotNull(result);
        Assert.Equal("expected", result.Value);
        mockRepo.Verify(x => x.GetAsync(1), Times.Once);
    }

Checklist:
  ☑️  Usa patrón AAA
  ☑️  Nombre claro (Method_Scenario_Result)
  ☑️  Mock de dependencias externas
  ☑️  Assertions específicas
  ☑️  Ejecuta localmente antes de commit

================================================================================
                        DEBUGGING DE TESTS
================================================================================

Si un test falla:

1. Lee el error en consola
2. Verifica mocks están configurados (ReturnsAsync)
3. Usa breakpoints en Visual Studio
4. Debug → Start/Attach Debugger
5. Revisa logs de ejecución

Errores comunes:

❌ "Cannot convert Mock<IRepository> to IRepository"
   ✅ Fix: Usa .Object → _mockRepo.Object

❌ "No exception was thrown"
   ✅ Fix: Asegúrate que dominio valida (puede ser lógico)

❌ "Relational-specific methods..."
   ✅ Fix: Usa InMemory, no Dapper queries en tests

================================================================================
                      COBERTURA DE CÓDIGO
================================================================================

Áreas cubiertas:
  ✅ Domain Services (Business Logic) - 6 tests
  ✅ Queries (Database) - 4 tests
  ✅ Serialization (JSON) - 2 tests

Áreas no cubiertas (TODO):
  ⏳ Controllers (HTTP responses)
  ⏳ Book/Loan domain services
  ⏳ Error handling en algunas rutas

Próximas pruebas a escribir:
  • CreateBookDomainTests (más casos)
  • CreateLoanDomainTests (validación de préstamos)
  • LoanQueriesTests (consultas de préstamos)
  • BookQueriesTests (consultas de libros)

================================================================================
                        INTEGRACIÓN CI/CD
================================================================================

Cuando se integre CI/CD (GitHub Actions, Azure Pipeline):

1. Execute: dotnet restore
2. Execute: dotnet build
3. Execute: dotnet test --verbosity normal
4. Report: xUnit test results
5. Fail: Si algún test falla

Configuración recomendada:
  • Run tests en cada PR
  • Require 100% tests passing para merge
  • Generar coverage reports
  • Publicar resultados

================================================================================
                          RECURSOS
================================================================================

Documentación xUnit:
  https://xunit.net/

Documentación Moq:
  https://github.com/mostafa-z/moq4

Patrón AAA:
  https://www.arrange-act-assert.com/

================================================================================
              Ver TEST_REPORT.txt para detalles completos de ejecución
================================================================================
