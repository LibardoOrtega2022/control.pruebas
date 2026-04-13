using Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        // Usar la cadena de conexión a LocalDB
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BibliografiasDb;Trusted_Connection=true;");
        
        return new AppDbContext(optionsBuilder.Options);
    }
}
