using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using ServicioCargasBatch.Models;
using ServicioCargasBatch.Services;

namespace ServicioCargasBatch.Workers;

public class TransaccionesWorker : BackgroundService
{
    private readonly IConfiguration _config;
    private readonly ApiService _api;
    private readonly FileLogger _logger;

    public TransaccionesWorker(IConfiguration config, ApiService api, FileLogger logger)
    {
        _config = config;
        _api = api;
        _logger = logger;

        // Log de inicio del servicio
        _logger.Log("Worker iniciado y listo para procesar archivos");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await EjecutarJobs();
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task EjecutarJobs()
    {
        var endpoints = _config.GetSection("Endpoints").Get<List<EndpointConfig>>();
        if (endpoints == null || endpoints.Count == 0) return;

        var ahora = DateTime.Now;
        var diaActual = ahora.DayOfWeek.ToString();
        var horaActual = ahora.ToString("HH:mm");

        foreach (var ep in endpoints)
        {
            if (!ep.Dias.Contains(diaActual)) continue;
            if (!ep.Horas.Contains(horaActual)) continue;

            // Crear carpetas si no existen
            Directory.CreateDirectory(ep.RutaBase);
            var procesados = Path.Combine(ep.RutaBase, "procesados");
            var error = Path.Combine(ep.RutaBase, "error");
            Directory.CreateDirectory(procesados);
            Directory.CreateDirectory(error);

            var archivo = BuscarArchivoDiaAnterior(ep);
            if (archivo == null)
            {
                _logger.Log($"[{ep.Nombre}] No se encontró archivo para el día anterior");
                continue;
            }

            // Enviar archivo al API
            var resultado = await _api.EnviarArchivoAsync(ep, archivo);
            _logger.Log(resultado);

            // Log del archivo procesado con endpoint
            _logger.LogArchivoProcesado(archivo, ep.Nombre);

            // Mover archivo según resultado
            if (resultado.Exitoso)
                FileHelper.MoverArchivo(archivo, procesados);
            else
                FileHelper.MoverArchivo(archivo, error);
        }
    }

    private string? BuscarArchivoDiaAnterior(EndpointConfig ep)
    {
        var fechaBuscada = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

        var archivos = Directory.GetFiles(ep.RutaBase, $"{fechaBuscada}_*{ep.Nombre}*.csv");

        var procesadosPath = Path.Combine(ep.RutaBase, "procesados");

        // Filtrar archivos que ya fueron procesados
        var archivosFiltrados = archivos
            .Where(a => !File.Exists(Path.Combine(procesadosPath, Path.GetFileName(a))))
            .ToArray();

        return archivosFiltrados.FirstOrDefault();
    }
}
