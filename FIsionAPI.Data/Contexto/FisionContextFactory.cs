using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FIsionAPI.Data.Contexto;

public class FisionContextFactory : IDesignTimeDbContextFactory<FisionContext>
{
    public FisionContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<FisionContext>()
            .UseSqlServer("Server=ALDO-PC\\SQLEXPRESS;Database=fisionDb;Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        return new FisionContext(options);
    }
}
