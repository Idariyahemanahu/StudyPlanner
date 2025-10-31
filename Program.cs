using CreateDbFromScratch.Model;
//using Microsoft.OpenApi.Models;
//using Pomelo.EntityFrameworkCore.MySql.Infrastructure;/
using Microsoft.EntityFrameworkCore;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

// Add services to the container
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRazorPages();
// Importing database configuration from appsettings.json
var dbConfig = builder.Configuration.GetSection("Database");
var server = dbConfig["Source"];
var port = dbConfig["Port"];
var database = dbConfig["DatabaseName"];
var user = dbConfig["Username"];
var password = dbConfig["Password"];

// Construct the connection string using string interpolation
var connectionString = $"server={server};port={port};database={database};user={user};password={password}";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);
// Add Session services 
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // session lifetime
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();


// Enable Swagger only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//Enable Session Middleware
app.UseSession();

app.MapRazorPages();
app.MapControllers();

app.Run();

