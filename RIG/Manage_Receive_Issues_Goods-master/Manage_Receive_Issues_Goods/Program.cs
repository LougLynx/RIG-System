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


// Register repositories
builder.Services.AddScoped<IScheduleReceivedDensoRepository, ScheduleReceivedDensoRepository>();
builder.Services.AddScoped<ISchedulereceivedTLIPRepository, SchedulereceivedTLIPRepository>();

// Register services
builder.Services.AddScoped<IScheduleReceivedDensoService, ScheduleReceivedDensoService>();
builder.Services.AddScoped<ISchedulereceivedTLIPService, SchedulereceivedTLIPService>();

// Register ILogger
   builder.Services.AddLogging();

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

app.UseHttpsRedirection();
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

/*var serviceProvider = app.Services;
var timer = new Timer(async _ =>
{
    var controller = serviceProvider.GetService<TLIPWarehouseController>();
    await controller.FetchData();
}, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));*/

app.Run();
