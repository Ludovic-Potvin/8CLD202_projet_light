using Microsoft.EntityFrameworkCore;
using MVC.Models;
using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.FeatureManagement;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var cosmosConfig = builder.Configuration.GetRequiredSection("Cosmos");
var accountEndpoint = cosmosConfig["AccountEndpoint"] ?? throw new InvalidOperationException("Cosmos AccountEndpoint is missing");
var accountKey = cosmosConfig["AccountKey"] ?? throw new InvalidOperationException("Cosmos AccountKey is missing");
var databaseName = cosmosConfig["DatabaseName"] ?? throw new InvalidOperationException("Cosmos DatabaseName is missing");

// Ajouter la BD
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseCosmos(accountEndpoint, accountKey, databaseName));

var app = builder.Build();

// Ensure Cosmos DB is created
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
    await dbContext.SeedDataAsync();
}

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