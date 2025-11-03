using Microsoft.EntityFrameworkCore;
using RDM.Core.Entities;
using RDM.Infrastructure.Data;

namespace RDM.Blazor.Services
{
    // Product Entity Type Service
    public interface IProductEntityTypeService
    {
        Task<List<ProductEntityType>> GetAllAsync();
        Task<ProductEntityType?> GetByIdAsync(long id);
        Task<ProductEntityType> CreateAsync(ProductEntityType entityType);
        Task UpdateAsync(ProductEntityType entityType);
        Task DeleteAsync(long id);
    }

    public class ProductEntityTypeService : IProductEntityTypeService
    {
        private readonly RdmDbContext _context;

        public ProductEntityTypeService(RdmDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductEntityType>> GetAllAsync() =>
            await _context.ProductEntityTypes.OrderBy(t => t.Name).ToListAsync();

        public async Task<ProductEntityType?> GetByIdAsync(long id) =>
            await _context.ProductEntityTypes.FindAsync(id);

        public async Task<ProductEntityType> CreateAsync(ProductEntityType entityType)
        {
            _context.ProductEntityTypes.Add(entityType);
            await _context.SaveChangesAsync();
            return entityType;
        }

        public async Task UpdateAsync(ProductEntityType entityType)
        {
            _context.ProductEntityTypes.Update(entityType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var entityType = await _context.ProductEntityTypes.FindAsync(id);
            if (entityType != null)
            {
                _context.ProductEntityTypes.Remove(entityType);
                await _context.SaveChangesAsync();
            }
        }
    }
}
