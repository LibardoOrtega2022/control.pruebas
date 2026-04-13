using Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistences;


public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    #region DbSets 
    public DbSet<BookEntity> BooksEntities => Set<BookEntity>();
    public DbSet<AuthorEntity> AuthorEntities => Set<AuthorEntity>();
    public DbSet<LoanEntity> Loans => Set<LoanEntity>();
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure LoanEntity
        modelBuilder.Entity<LoanEntity>(e =>
        {
            e.ToTable("Loans");
            e.HasKey(x => x.Id);
            e.Property(x => x.BorrowerName).HasMaxLength(200).IsRequired();
            e.Property(x => x.Status).HasMaxLength(50).IsRequired();
            e.Property(x => x.LoanDate).IsRequired();
            e.Property(x => x.DueDate).IsRequired();
            e.Property(x => x.ReturnDate);
            e.Property(x => x.CreatedDate).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            e.Property(x => x.UpdatedDate);
            e.HasOne(x => x.Book).WithMany().HasForeignKey(x => x.BookId);
            
            // Índice para búsquedas: "Dame todos los préstamos activos de este libro"
            e.HasIndex(x => new { x.BookId, x.ReturnDate });
            e.HasIndex(x => x.Status);
        });
        
        // Configure BookEntity
        modelBuilder.Entity<BookEntity>(e =>
        {
            e.ToTable("Books");
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).HasMaxLength(200).IsRequired();
            e.Property(x => x.Genre).HasMaxLength(100).IsRequired();
            e.Property(x => x.ISBN).HasMaxLength(13);
            e.Property(x => x.NumberOfPages).IsRequired();
            e.Property(x => x.PublishedDate).IsRequired();
            e.Property(x => x.CreatedDate).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            e.Property(x => x.UpdatedDate);
            e.Property(x => x.IsDeleted).HasDefaultValue(false);
            e.HasOne(x => x.Author).WithMany(x => x.Books).HasForeignKey(x => x.AuthorId);
            
            // Índice para búsquedas frecuentes
            e.HasIndex(x => new { x.AuthorId, x.IsDeleted });
            e.HasIndex(x => x.Title);
        });
        
        // Configure AuthorEntity
        modelBuilder.Entity<AuthorEntity>(e =>
        {
            e.ToTable("Author");
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
            e.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            e.Property(x => x.Country).HasMaxLength(50);
            e.Property(x => x.Biography).HasMaxLength(1000);
            e.Property(x => x.BirthDate).IsRequired();
            e.Property(x => x.CreatedDate).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            e.Property(x => x.UpdatedDate);
            e.Property(x => x.IsDeleted).HasDefaultValue(false);
            e.HasMany(x => x.Books).WithOne(x => x.Author).HasForeignKey(x => x.AuthorId);
        });
    }
}