using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServicioCargasBatch.Services;
using ServicioCargasBatch.Workers;

var builder = Host.CreateApplicationBuilder(args);

// ======================================
// 👉 Ejecutar como Windows Service
// ======================================
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "ServicioCargasBatch";
});

// ======================================
// 👉 HttpClient para consumir APIs
// ======================================
builder.Services.AddHttpClient();

// ======================================
// 👉 Inyección de dependencias
// ======================================
builder.Services.AddSingleton<ApiService>();
builder.Services.AddSingleton<FileLogger>(); // Logger que crea log al iniciar

// ======================================
// 👉 Worker principal
// ======================================
builder.Services.AddHostedService<TransaccionesWorker>();

// ======================================
// 👉 Construir y ejecutar
// ======================================
var host = builder.Build();
host.Run();
