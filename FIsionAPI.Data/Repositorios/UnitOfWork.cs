using FIsionAPI.Business.Interfaces;
using FIsionAPI.Data.Contexto;
using System.Threading.Tasks;

namespace FIsionAPI.Data.Repositorios;

public class UnitOfWork : IUnitOfWork
{
    private readonly FisionContext _context;

    public UnitOfWork(FisionContext context)
    {
        _context = context;
    }

    public async Task<bool> Commit()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
