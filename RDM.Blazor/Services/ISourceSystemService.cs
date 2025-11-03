using Microsoft.EntityFrameworkCore;
using RDM.Core.Entities;
using RDM.Infrastructure.Data;

namespace RDM.Blazor.Services
{

    // Source System Service
    public interface ISourceSystemService
{
    Task<List<SourceSystem>> GetAllAsync();
    Task<List<SourceSystem>> GetHierarchyAsync();
    Task<SourceSystem?> GetByIdAsync(long id);
    Task<SourceSystem> CreateAsync(SourceSystem system);
    Task UpdateAsync(SourceSystem system);
    Task DeleteAsync(long id);
}

public class SourceSystemService : ISourceSystemService
{
    private readonly RdmDbContext _context;

    public SourceSystemService(RdmDbContext context)
    {
        _context = context;
    }

    public async Task<List<SourceSystem>> GetAllAsync() =>
        await _context.SourceSystems.OrderBy(s => s.Name).ToListAsync();

    public async Task<List<SourceSystem>> GetHierarchyAsync() =>
        await _context.SourceSystems
            .Include(s => s.Children)
            .Where(s => s.ParentId == null)
            .Where(s => s.Id > -1)
            .ToListAsync();

    public async Task<SourceSystem?> GetByIdAsync(long id) =>
        await _context.SourceSystems
            .Include(s => s.Children)
            .FirstOrDefaultAsync(s => s.Id == id);

    public async Task<SourceSystem> CreateAsync(SourceSystem system)
    {
        _context.SourceSystems.Add(system);
        await _context.SaveChangesAsync();
        return system;
    }

    public async Task UpdateAsync(SourceSystem system)
    {
        _context.SourceSystems.Update(system);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var system = await _context.SourceSystems.FindAsync(id);
        if (system != null)
        {
            _context.SourceSystems.Remove(system);
            await _context.SaveChangesAsync();
        }
    }
}
}
