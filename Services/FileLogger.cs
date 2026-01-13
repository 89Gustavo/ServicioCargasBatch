using ServicioCargasBatch.Models;

namespace ServicioCargasBatch.Services;

public class FileLogger
{
    private readonly string _path;

    public FileLogger()
    {
        _path = @"C:\Logs\log.txt";
        Directory.CreateDirectory(@"C:\Logs");

        if (!File.Exists(_path))
        {
            File.WriteAllText(_path, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | INFO | Servicio Batch iniciado{Environment.NewLine}");
        }
        else
        {
            File.AppendAllText(_path, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | INFO | Servicio Batch iniciado{Environment.NewLine}");
        }
    }

    public void Log(string mensaje)
    {
        var linea = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | INFO | {mensaje}";
        File.AppendAllText(_path, linea + Environment.NewLine);
    }

    public void Log(JobResult result)
    {
        var linea =
            $"{result.Fecha:yyyy-MM-dd HH:mm:ss} | {result.Endpoint} | {(result.Exitoso ? "OK" : "ERROR")} | {result.Mensaje}";
        File.AppendAllText(_path, linea + Environment.NewLine);
    }

    // ✅ Método para loggear archivo procesado con endpoint
    public void LogArchivoProcesado(string archivo, string endpoint)
    {
        var linea = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {endpoint} | Archivo procesado: {Path.GetFileName(archivo)}";
        File.AppendAllText(_path, linea + Environment.NewLine);
    }
}
