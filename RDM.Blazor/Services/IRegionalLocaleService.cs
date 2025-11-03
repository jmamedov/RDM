using Microsoft.EntityFrameworkCore;
using RDM.Core.Entities;
using RDM.Infrastructure.Data;

namespace RDM.Blazor.Services
{
    // Regional Locale Service
    public interface IRegionalLocaleService
    {
        Task<List<RegionalLocale>> GetAllAsync();
        Task<RegionalLocale?> GetByCodeAsync(string code);
        Task<RegionalLocale> CreateAsync(RegionalLocale locale);
        Task UpdateAsync(RegionalLocale locale);
        Task DeleteAsync(string code);
    }
    public class RegionalLocaleService : IRegionalLocaleService
    {
        private readonly RdmDbContext _context;

        public RegionalLocaleService(RdmDbContext context)
        {
            _context = context;
        }

        public async Task<List<RegionalLocale>> GetAllAsync() =>
            await _context.RegionalLocales.OrderBy(l => l.Name).ToListAsync();

        public async Task<RegionalLocale?> GetByCodeAsync(string code) =>
            await _context.RegionalLocales.FindAsync(code);

        public async Task<RegionalLocale> CreateAsync(RegionalLocale locale)
        {
            _context.RegionalLocales.Add(locale);
            await _context.SaveChangesAsync();
            return locale;
        }

        public async Task UpdateAsync(RegionalLocale locale)
        {
            _context.RegionalLocales.Update(locale);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string code)
        {
            var locale = await _context.RegionalLocales.FindAsync(code);
            if (locale != null)
            {
                _context.RegionalLocales.Remove(locale);
                await _context.SaveChangesAsync();
            }
        }
    }
}