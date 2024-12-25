/*using Manage_Receive_Issues_Goods.Repositories.Implementations;
using Manage_Receive_Issues_Goods.Services;
using Manage_Receive_Issues_Goods.Models;
using Microsoft.EntityFrameworkCore;
using DataFetchingWorkerService;
using Manage_Receive_Issues_Goods.Repository;
using Microsoft.Extensions.Hosting.WindowsServices;
using log4net;
using log4net.Config;
using System.IO;

// Cấu hình log4net
var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

// Tạo logger
ILog log = LogManager.GetLogger(typeof(Program));

try
{
    // Log bắt đầu khởi tạo service
    log.Info("Starting Worker Service setup...");

    var host = Host.CreateDefaultBuilder(args)
        .UseWindowsService()
        .ConfigureAppConfiguration((context, config) =>
        {
            try
            {
                // Đọc file cấu hình workerappsettings.json từ thư mục chạy
                var exePath = AppContext.BaseDirectory;
                var configFilePath = Path.Combine(exePath, "workerappsettings.json");

                if (!File.Exists(configFilePath))
                {
                    log.Error($"Configuration file '{configFilePath}' not found.");
                    throw new FileNotFoundException($"Configuration file '{configFilePath}' not found.");
                }

                log.Info("Configuration file found and loaded.");
                config.AddJsonFile(configFilePath, optional: false, reloadOnChange: true);
            }
            catch (Exception ex)
            {
                log.Error("Error loading configuration file.", ex);
                throw;
            }
        })
        .ConfigureServices((context, services) =>
        {
            try
            {
                // Đăng ký chuỗi kết nối từ cấu hình
                var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrEmpty(connectionString))
                {
                    log.Error("Connection string 'DefaultConnection' is missing in the configuration.");
                    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
                }

                log.Info("Connection string loaded successfully.");

                // Đăng ký DbContext
                services.AddDbContext<RigContext>(options =>
                {
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                    log.Info("Database context configured successfully.");
                });

                // Đăng ký HttpClient
                services.AddHttpClient("MyApiClient", client =>
                {
                    client.BaseAddress = new Uri("http:/10.73.131.39");
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                });

                log.Info("HttpClient configured successfully.");

                // Đăng ký Repository và Service
                services.AddScoped<ISchedulereceivedTLIPRepository, SchedulereceivedTLIPRepository>();
                services.AddScoped<ISchedulereceivedTLIPService, SchedulereceivedTLIPService>();
                log.Info("Repositories and services registered successfully.");

                // Đăng ký Worker Service
                services.AddHostedService<DataTLIPWorker>();
                log.Info("Worker service registered successfully.");

                // Đăng ký Logging
                services.AddLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                });

                log.Info("Logging configured successfully.");
            }
            catch (Exception ex)
            {
                log.Error("Error during service configuration.", ex);
                throw;
            }
        })
        .Build();

    log.Info("Worker Service setup completed. Starting the service...");
    await host.RunAsync();
    log.Info("Worker Service started successfully.");
}
catch (Exception ex)
{
    log.Fatal("Worker Service failed to start.", ex);
}
*/
using Manage_Receive_Issues_Goods.Repositories.Implementations;
using Manage_Receive_Issues_Goods.Services;
using Manage_Receive_Issues_Goods.Models;
using Microsoft.EntityFrameworkCore;
using DataFetchingWorkerService;
using Manage_Receive_Issues_Goods.Repository;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using log4net;
using log4net.Config;
using Serilog;
using System.IO;

// Cấu hình log4net
var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

ILog log = LogManager.GetLogger(typeof(Program));

try
{
    // Cấu hình Serilog
    Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning) // Bỏ log SQL
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Infrastructure", Serilog.Events.LogEventLevel.Warning) // Bỏ log cơ sở hạ tầng EF Core
    .WriteTo.Console()
    .WriteTo.File(
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RIGDataFetchingService.log"),
        rollingInterval: RollingInterval.Day
    )
    .CreateLogger();

    log.Info("Starting Worker Service setup...");

    var builder = Host.CreateDefaultBuilder(args)
        .UseWindowsService(options =>
        {
            // Tên của Windows Service
            options.ServiceName = "RIGDataFetchingService";
        })
        .UseSerilog() // Tích hợp Serilog vào hệ thống logging
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            var env = hostingContext.HostingEnvironment;

            // Đường dẫn đến SharedConfig
            var sharedConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SharedConfig", "sharedappsettings.json");

            if (!File.Exists(sharedConfigPath))
            {
                throw new FileNotFoundException($"Configuration file '{sharedConfigPath}' not found.");
            }

            // Thêm các file cấu hình
            config
                .AddJsonFile(sharedConfigPath, optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        })
        .ConfigureServices((context, services) =>
        {
            // Chuỗi kết nối
            var connectionString = context.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<RigContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // Đăng ký repository và service
            services.AddHttpClient();
            services.AddScoped<ISchedulereceivedTLIPRepository, SchedulereceivedTLIPRepository>();
            services.AddScoped<ISchedulereceivedTLIPService, SchedulereceivedTLIPService>();

            // Đăng ký Worker Service
            services.AddHostedService<DataTLIPWorker>();
        })
        .ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddSerilog(); // Thêm Serilog vào hệ thống logging
            logging.AddConsole();
        });

    var host = builder.Build();

    log.Info("Worker Service is starting...");
    Log.Information("Starting RIGDataFetchingService..."); // Serilog log
    await host.RunAsync();
    log.Info("Worker Service started successfully.");
    Log.Information("RIGDataFetchingService started successfully.");
}
catch (Exception ex)
{
    log.Fatal("Worker Service failed to start.", ex);
    Log.Fatal(ex, "RIGDataFetchingService failed to start.");
    throw;
}
finally
{
    // Đảm bảo Serilog được đóng khi ứng dụng kết thúc
    Log.CloseAndFlush();
}