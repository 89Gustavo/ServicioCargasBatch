namespace ServicioCargasBatch.Models;

public class EndpointConfig
{
    public string Nombre { get; set; } = "";
    public string Url { get; set; } = "";
    public List<string> Horas { get; set; } = new();
    public List<string> Dias { get; set; } = new();
    public string RutaBase { get; set; } = "";
}
