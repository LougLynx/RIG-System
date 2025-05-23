
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

var builder = WebApplication.CreateBuilder(args);

// Trỏ đến file cấu hình chung trong thư mục SharedConfig
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    var env = hostingContext.HostingEnvironment;

    // Tìm đường dẫn tới SharedConfig
    var sharedConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SharedConfig", "sharedappsettings.json");

    if (!File.Exists(sharedConfigPath))
    {
        throw new FileNotFoundException($"Configuration file '{sharedConfigPath}' not found.");
    }

    // Thêm file cấu hình từ SharedConfig
    config
        .AddJsonFile(sharedConfigPath, optional: false, reloadOnChange: true)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

    // Load các biến môi trường (nếu có)
    config.AddEnvironmentVariables();
});

// Đăng ký SignalR và HTTP client
builder.Services.AddSignalR();
builder.Services.AddHttpClient<TLIPWarehouseController>();

// Thêm cấu hình để tắt chuyển đổi camelCase
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.PropertyNamingPolicy = null);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Đăng ký DbContext
builder.Services.AddDbContext<RigContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6; // Tối thiểu 6 ký tự
})
.AddRoles<IdentityRole>() // Role
.AddEntityFrameworkStores<RigContext>()
.AddDefaultUI()
.AddDefaultTokenProviders();// Token để xác nhận email, mật khẩu

// Cấu hình Authentication
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.LoginPath = "/Identity/Account/Login"; // Trang đăng nhập
    options.LogoutPath = "/Identity/Account/Logout"; // Trang đăng xuất
    options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // Trang từ chối truy cập
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Thời gian session tồn tại
    options.SlidingExpiration = true;
});

// Đăng ký DI Repository và Service
builder.Services.AddScoped<IScheduleReceivedDensoRepository, ScheduleReceivedDensoRepository>();
builder.Services.AddScoped<ISchedulereceivedTLIPRepository, SchedulereceivedTLIPRepository>();
builder.Services.AddScoped<IScheduleIssuedTLIPRepository, ScheduleIssuedTLIPRepository>();
builder.Services.AddScoped<I_RDTDRepository, RDTDRepository>();

builder.Services.AddScoped<IScheduleReceivedDensoService, ScheduleReceivedDensoService>();
builder.Services.AddScoped<ISchedulereceivedTLIPService, SchedulereceivedTLIPService>();
builder.Services.AddScoped<IScheduleIssuedTLIPService, ScheduleIssuedTLIPService>();
builder.Services.AddScoped<I_RDTDService, RDTDService>();

//phân quyền
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Config", "AdminOnly");
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});


// Cấu hình Logging
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});

//Cấu hình Razor page
builder.Services.AddRazorPages();

var app = builder.Build();

// Cấu hình ứng dụng
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<UpdateReceiveDensoHub>("/updateReceiveDensoHub");
app.MapHub<UpdateIssueTLIPHub>("/updateIssueTLIPHub");
app.MapHub<UpdateReceiveTLIPHub>("/updateReceiveTLIPHub");

app.MapRazorPages();

app.Run();