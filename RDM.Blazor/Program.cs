using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using RDM.Blazor.Components;
using RDM.Blazor.Services;
using RDM.Infrastructure.Data;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure Database
builder.Services.AddDbContext<RdmDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    )
);

// Register application services
builder.Services.AddScoped<IRegionalLocaleService, RegionalLocaleService>();
builder.Services.AddScoped<IProductEntityTypeService, ProductEntityTypeService>();
builder.Services.AddScoped<IHierarchyTypeService, HierarchyTypeService>();
builder.Services.AddScoped<ISourceSystemService, SourceSystemService>();
builder.Services.AddScoped<ISourceNodeService, SourceNodeService>();
builder.Services.AddScoped<INodeBillOfMaterialService, NodeBillOfMaterialService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Optional: Auto-migrate database on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RdmDbContext>();
    try
    {
        // Uncomment to automatically apply migrations on startup
        // await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.MapGet("/api/export/nodes", async (
 ISourceNodeService nodeService,
 long? sourceId,
 long? entityTypeId,
 string? localeCode,
 string? search) =>
{
 var nodes = await nodeService.GetAllAsync(sourceId, entityTypeId, localeCode);

 // Apply search filter (same as UI)
 if (!string.IsNullOrWhiteSpace(search))
 {
 var searchLower = search.ToLowerInvariant();
 nodes = nodes.Where(n =>
 (n.Code?.ToLowerInvariant().Contains(searchLower) ?? false) ||
 (n.Name?.ToLowerInvariant().Contains(searchLower) ?? false) ||
 (n.Description?.ToLowerInvariant().Contains(searchLower) ?? false) ||
 (n.EntityType?.Name?.ToLowerInvariant().Contains(searchLower) ?? false) ||
 (n.Locale?.Name?.ToLowerInvariant().Contains(searchLower) ?? false) ||
 (n.SourceSystem?.Name?.ToLowerInvariant().Contains(searchLower) ?? false)
 ).ToList();
 }

 using var workbook = new XLWorkbook();
 var worksheet = workbook.Worksheets.Add("Nodes");

 // Header
 worksheet.Cell(1,1).Value = "Code";
 worksheet.Cell(1,2).Value = "Name";
 worksheet.Cell(1,3).Value = "Description";
 worksheet.Cell(1,4).Value = "Entity Type";
 worksheet.Cell(1,5).Value = "Locale";
 worksheet.Cell(1,6).Value = "Source System";

 // Data
 int row =2;
 foreach (var n in nodes)
 {
 worksheet.Cell(row,1).Value = n.Code;
 worksheet.Cell(row,2).Value = n.Name;
 worksheet.Cell(row,3).Value = n.Description;
 worksheet.Cell(row,4).Value = n.EntityType?.Name;
 worksheet.Cell(row,5).Value = n.Locale?.Name;
 worksheet.Cell(row,6).Value = n.SourceSystem?.Name;
 row++;
 }

 using var stream = new MemoryStream();
 workbook.SaveAs(stream);
 stream.Position =0;
 return Results.File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "nodes.xlsx");
});

app.Run();