using Microsoft.EntityFrameworkCore;
using RDM.Infrastructure.Data;
using RDM.Blazor.Services;
using RDM.Blazor.Components;

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

//app.UseHttpsRedirection();
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

app.Run();