using Amazon.S3;
using cloudfilestorage.Database;
using cloudfilestorage.Models;
using cloudfilestorage.Repositories;
using cloudfilestorage.Repositories.Interface;
using cloudfilestorage.Services.Implementation;
using cloudfilestorage.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<IAuthRepository, AuthRepository>();
builder.Services.AddTransient<AppDbContext>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.Configure<AWSCredentials>(
    builder.Configuration.GetSection(AWSCredentials.Position));

// Add Redis cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "FileStorage.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(20);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Shared/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
