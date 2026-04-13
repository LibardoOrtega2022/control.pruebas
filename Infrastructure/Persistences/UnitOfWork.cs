using System.Data;
using Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Persistences;

public sealed class UnitOfWork(AppDbContext db) : IUnitOfWork
{
    private AppDbContext Db { get; } = db;

    public IDbConnection Connection => Db.Database.GetDbConnection();
    public IDbTransaction? Transaction => Db.Database.CurrentTransaction?.GetDbTransaction();

    public async Task BeginAsync(CancellationToken ct)
    {
        if (Connection.State != ConnectionState.Open)
            await Db.Database.OpenConnectionAsync(ct);

        if (Db.Database.CurrentTransaction is null)
            await Db.Database.BeginTransactionAsync(ct);
    }

    public async Task CommitAsync(CancellationToken ct)
    {
        // BUG FIX: Guardar cambios de EF antes de hacer commit de la transacción
        await Db.SaveChangesAsync(ct);
        if (Db.Database.CurrentTransaction is not null)
            await Db.Database.CurrentTransaction.CommitAsync(ct);
    }

    public async Task RollbackAsync(CancellationToken ct)
    {
        if (Db.Database.CurrentTransaction is not null)
            await Db.Database.CurrentTransaction.RollbackAsync(ct);
    }
}