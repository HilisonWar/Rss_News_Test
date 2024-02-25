using NewsRSS_TestExersice.Models;
using Microsoft.EntityFrameworkCore;
using NewsRSS_TestExersice.Helpers;
using Microsoft.Extensions.Caching.Distributed;
using NewsRSS_TestExersice.Services;
using Rss_Application.Models;

var builder = WebApplication.CreateBuilder(args);

//Scaffold-DbContext "Host=127.0.0.1:3658;Database=postgres;Username=postgres;Password=postgres" Npgsql.EntityFrameworkCore.PostgreSQL -OutputDir Models -f

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen();

builder.Services.AddTransient<CacheService>();

builder.Services.AddTransient<NewsService>();

builder.Services.AddTransient<RssService>();

builder.Services.AddDbContext<PostgresContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreDb")));



builder.Services.AddStackExchangeRedisCache(options =>
{
	options.Configuration = builder.Configuration.GetConnectionString("RedisCache");
});

var app = builder.Build();




//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSwagger();

app.UseSwaggerUI();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
