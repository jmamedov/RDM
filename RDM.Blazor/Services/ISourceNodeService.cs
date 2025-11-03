using Microsoft.EntityFrameworkCore;
using RDM.Core.Entities;
using RDM.Infrastructure.Data;

namespace RDM.Blazor.Services
{
    // Source Node Service
    public interface ISourceNodeService
    {
        Task<List<SourceNode>> GetAllAsync(long? sourceId = null, long? entityTypeId = null, string? localeCode = null);
        Task<SourceNode?> GetByIdAsync(long id);
        Task<SourceNode> CreateAsync(SourceNode node);
        Task UpdateAsync(SourceNode node);
        Task DeleteAsync(long id);
    }

    public class SourceNodeService : ISourceNodeService
    {
        private readonly RdmDbContext _context;

        public SourceNodeService(RdmDbContext context)
        {
            _context = context;
        }

        public async Task<List<SourceNode>> GetAllAsync(long? sourceId = null, long? entityTypeId = null, string? localeCode = null)
        {
            var query = _context.SourceNodes
                .Include(n => n.SourceSystem)
                .Include(n => n.EntityType)
                .Include(n => n.Locale)
                .AsQueryable();

            if (sourceId.HasValue)
                query = query.Where(n => n.SourceId == sourceId.Value);

            if (entityTypeId.HasValue)
                query = query.Where(n => n.EntityTypeId == entityTypeId.Value);

            if (!string.IsNullOrEmpty(localeCode))
                query = query.Where(n => n.LocaleCode == localeCode);

            return await query.OrderBy(n => n.Name).ToListAsync();
        }

        public async Task<SourceNode?> GetByIdAsync(long id) =>
            await _context.SourceNodes
                .Include(n => n.SourceSystem)
                .Include(n => n.EntityType)
                .Include(n => n.Locale)
                .FirstOrDefaultAsync(n => n.Id == id);

        public async Task<SourceNode> CreateAsync(SourceNode node)
        {
            _context.SourceNodes.Add(node);
            await _context.SaveChangesAsync();
            return node;
        }

        public async Task UpdateAsync(SourceNode node)
        {
            _context.SourceNodes.Update(node);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var node = await _context.SourceNodes.FindAsync(id);
            if (node != null)
            {
                _context.SourceNodes.Remove(node);
                await _context.SaveChangesAsync();
            }
        }
    }
}

