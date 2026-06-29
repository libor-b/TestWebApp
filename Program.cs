using Microsoft.EntityFrameworkCore;
using PersonApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Výběr databáze:
//  - lokálně (Development) => SQLite
//  - na serveru (jiné prostředí) => MySQL
// Lze přepsat konfigurací "DatabaseProvider" (Sqlite | MySql).
var provider = builder.Configuration["DatabaseProvider"]
    ?? (builder.Environment.IsDevelopment() ? "Sqlite" : "MySql");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (provider.Equals("MySql", StringComparison.OrdinalIgnoreCase))
    {
        var conn = builder.Configuration.GetConnectionString("MySql")
            ?? throw new InvalidOperationException("Chybí connection string 'MySql'.");
        options.UseMySql(conn, ServerVersion.AutoDetect(conn));
    }
    else
    {
        var conn = builder.Configuration.GetConnectionString("Sqlite")
            ?? "Data Source=people.db";
        options.UseSqlite(conn);
    }
});

var app = builder.Build();

// Vytvoření DB / migrace a naplnění demo daty.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbSeeder.Seed(db);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
