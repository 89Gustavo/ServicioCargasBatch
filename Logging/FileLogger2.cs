using ServicioCargasBatch.Models;

namespace ServicioCargasBatch.Services;

public class FileLogger2
{
    private readonly string _path = @"C:\Logs\log.txt";

    public FileLogger2()
    {
        Directory.CreateDirectory(@"C:\Logs");
    }

    // ✅ LOG SIMPLE (string)
    public void Log(string mensaje)
    {
        var linea = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | INFO | {mensaje}";
        File.AppendAllText(_path, linea + Environment.NewLine);
    }

    // ✅ LOG DE RESULTADO (JobResult)
    public void Log(JobResult result)
    {
        var linea =
            $"{result.Fecha:yyyy-MM-dd HH:mm:ss} | " +
            $"{result.Endpoint} | " +
            $"{(result.Exitoso ? "OK" : "ERROR")} | " +
            $"{result.Mensaje}";

        File.AppendAllText(_path, linea + Environment.NewLine);
    }
}
