using Application.Abstractions;
using Application.Abstractions.Authors;
using Application.Abstractions.Books;
using Application.Abstractions.Loans;
using Application.Abstractions.Reports;
using Infrastructure.Persistences;
using Infrastructure.Queries.Authors;
using Infrastructure.Queries.Books;
using Infrastructure.Queries.Loans;
using Infrastructure.Queries.Reports;
using Infrastructure.Repositories.Authors;
using Infrastructure.Repositories.Books;
using Infrastructure.Repositories.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(config.GetConnectionString("Default")));

        services.AddScoped<UnitOfWork>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UnitOfWork>());

        // Authors
        services.AddScoped<IAuthorQueries, AuthorQueries>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();

        // Books
        services.AddScoped<IBookQueries, BookQueries>();
        services.AddScoped<IBookRepository, BookRepository>();

        // Loans
        services.AddScoped<ILoanQueries, LoanQueries>();
        services.AddScoped<ILoanRepository, LoanRepository>();

        // Reports
        services.AddScoped<IReportQueries, ReportQueries>();

        return services;
    }
}