using Microsoft.EntityFrameworkCore;
using RDM.Core.Entities;
using RDM.Infrastructure.Data;

namespace RDM.Blazor.Services
{
    // Hierarchy Type Service
    public interface IHierarchyTypeService
    {
        Task<List<HierarchyType>> GetAllAsync();
        Task<HierarchyType?> GetByIdAsync(int id);
        Task<HierarchyType> CreateAsync(HierarchyType hierarchyType);
        Task UpdateAsync(HierarchyType hierarchyType);
        Task DeleteAsync(int id);
    }

    public class HierarchyTypeService : IHierarchyTypeService
    {
        private readonly RdmDbContext _context;

        public HierarchyTypeService(RdmDbContext context)
        {
            _context = context;
        }

        public async Task<List<HierarchyType>> GetAllAsync() =>
            await _context.HierarchyTypes.OrderBy(t => t.Name).ToListAsync();

        public async Task<HierarchyType?> GetByIdAsync(int id) =>
            await _context.HierarchyTypes.FindAsync(id);

        public async Task<HierarchyType> CreateAsync(HierarchyType hierarchyType)
        {
            _context.HierarchyTypes.Add(hierarchyType);
            await _context.SaveChangesAsync();
            return hierarchyType;
        }

        public async Task UpdateAsync(HierarchyType hierarchyType)
        {
            _context.HierarchyTypes.Update(hierarchyType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var hierarchyType = await _context.HierarchyTypes.FindAsync(id);
            if (hierarchyType != null)
            {
                _context.HierarchyTypes.Remove(hierarchyType);
                await _context.SaveChangesAsync();
            }
        }
    }
}