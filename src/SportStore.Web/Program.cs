using Microsoft.EntityFrameworkCore;
using SportStore.Web.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("DefaultConnection is required.")));

var app = builder.Build();
if (args.Contains("--initialize-database"))
{
    using var scope = app.Services.CreateScope();
    await DatabaseInitializer.InitializeAsync(scope.ServiceProvider);
    return;
}

app.UseStaticFiles();
app.UseRouting();
app.MapGet("/", () => "SPORTLINE - Etap 1");
app.Run();

