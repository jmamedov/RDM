using Microsoft.EntityFrameworkCore;
using RDM.Core.Entities;
using RDM.Infrastructure.Data;

namespace RDM.Blazor.Services
{


    // Node Bill of Material Service
    public interface INodeBillOfMaterialService
{
    Task<List<NodeBillOfMaterial>> GetAllAsync(long? parentNodeId = null);
    Task<NodeBillOfMaterial> CreateAsync(NodeBillOfMaterial bom);
    Task DeleteAsync(long hierarchyTypeId, long parentNodeId, long childNodeId);
}

public class NodeBillOfMaterialService : INodeBillOfMaterialService
{
    private readonly RdmDbContext _context;

    public NodeBillOfMaterialService(RdmDbContext context)
    {
        _context = context;
    }

    public async Task<List<NodeBillOfMaterial>> GetAllAsync(long? parentNodeId = null)
    {
        var query = _context.NodeBillOfMaterials
            .Include(b => b.HierarchyType)
            .Include(b => b.ParentNode)
            .Include(b => b.ChildNode)
            .AsQueryable();

        if (parentNodeId.HasValue)
            query = query.Where(b => b.ParentNodeId == parentNodeId.Value);

        return await query.ToListAsync();
    }

    public async Task<NodeBillOfMaterial> CreateAsync(NodeBillOfMaterial bom)
    {
        _context.NodeBillOfMaterials.Add(bom);
        await _context.SaveChangesAsync();
        return bom;
    }

    public async Task DeleteAsync(long hierarchyTypeId, long parentNodeId, long childNodeId)
    {
        var bom = await _context.NodeBillOfMaterials
            .FirstOrDefaultAsync(b => b.HierarchyTypeId == hierarchyTypeId &&
                                     b.ParentNodeId == parentNodeId &&
                                     b.ChildNodeId == childNodeId);
        if (bom != null)
        {
            _context.NodeBillOfMaterials.Remove(bom);
            await _context.SaveChangesAsync();
        }
    }
}
}