using Manage_Receive_Issues_Goods.Data;
using Manage_Receive_Issues_Goods.Controllers;
using Manage_Receive_Issues_Goods.Hubs;
using Manage_Receive_Issues_Goods.Models;
using Manage_Receive_Issues_Goods.Repositories.Implementations;
using Manage_Receive_Issues_Goods.Repository;
using Manage_Receive_Issues_Goods.Repository.Implementations;
using Manage_Receive_Issues_Goods.Service;
using Manage_Receive_Issues_Goods.Service.Implementations;
using Manage_Receive_Issues_Goods.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
//builder.Services.AddHttpClient();
builder.Services.AddHttpClient<TLIPWarehouseController>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllers().AddJsonOptions(options =>
options.JsonSerializerOptions.PropertyNamingPolicy = null);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<RigContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
    .AddRoles<IdentityRole>() 
    .AddEntityFrameworkStores<RigContext>();


// Configure cookie settings for HTTP
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None; // Allow cookies over HTTP
    //options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});


// Register repositories
builder.Services.AddScoped<IScheduleReceivedDensoRepository, ScheduleReceivedDensoRepository>();
builder.Services.AddScoped<ISchedulereceivedTLIPRepository, SchedulereceivedTLIPRepository>();

// Register services
builder.Services.AddScoped<IScheduleReceivedDensoService, ScheduleReceivedDensoService>();
builder.Services.AddScoped<ISchedulereceivedTLIPService, SchedulereceivedTLIPService>();

// Register background services
builder.Services.AddHostedService<DataFetchingBackgroundService>();

builder.Services.AddScoped<TLIPWarehouseController>();

builder.Services.AddScoped<RoleController>();

// Register ILogger
/*builder.Services.AddLogging();*/

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<UpdateReceiveDensoHub>("/updateReceiveDensoHub");
app.MapHub<UpdateReceiveTLIPHub>("/updateReceiveTLIPHub");
app.MapRazorPages();


app.Run();
