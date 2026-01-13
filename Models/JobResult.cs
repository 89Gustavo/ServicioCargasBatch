namespace ServicioCargasBatch.Models;

public class JobResult
{
    public string Endpoint { get; set; } = "";
    public DateTime Fecha { get; set; }
    public bool Exitoso { get; set; }
    public string Mensaje { get; set; } = "";
}
