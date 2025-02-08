using Microsoft.EntityFrameworkCore;
using MVC.Models;
using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.FeatureManagement;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Ajouter la BD
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!)
    .LogTo(Console.WriteLine, LogLevel.Trace)
    .EnableDetailedErrors());

var app = builder.Build();

switch (builder.Configuration.GetValue<string>("DatabaseConfiguration")
{
    case "SQL":
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContextSQL>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }
        break;
    case "NoSQL":
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContextNoSQL>();
            await dbContext.Database.EnsureCreated();
        }
}
//dbContext.Database.Migrate();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public partial class Program { }