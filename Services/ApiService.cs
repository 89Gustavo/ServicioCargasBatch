using ServicioCargasBatch.Models;
using System.Net.Http.Headers;

namespace ServicioCargasBatch.Services;

public class ApiService
{
    private readonly HttpClient _http;

    public ApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<JobResult> EjecutarEndpoint(EndpointConfig ep)
    {
        try
        {
            var response = await _http.PostAsync(ep.Url, null);

            return new JobResult
            {
                Endpoint = ep.Nombre,
                Fecha = DateTime.Now,
                Exitoso = response.IsSuccessStatusCode,
                Mensaje = response.ReasonPhrase ?? "OK"
            };
        }
        catch (Exception ex)
        {
            return new JobResult
            {
                Endpoint = ep.Nombre,
                Fecha = DateTime.Now,
                Exitoso = false,
                Mensaje = ex.Message
            };
        }
    }

    public async Task<JobResult> EnviarArchivoAsync(EndpointConfig ep, string rutaArchivo)
    {
        try
        {
            using var form = new MultipartFormDataContent();
            using var stream = File.OpenRead(rutaArchivo);
            using var fileContent = new StreamContent(stream);

            fileContent.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            form.Add(fileContent, "archivo", Path.GetFileName(rutaArchivo));

            var url = $"{ep.Url}?usuarioBitacora=sistema_automatizado_gualan";
            var response = await _http.PostAsync(url, form);

            return new JobResult
            {
                Endpoint = ep.Nombre,
                Fecha = DateTime.Now,
                Exitoso = response.IsSuccessStatusCode,
                Mensaje = response.IsSuccessStatusCode
                    ? $"Archivo enviado: {Path.GetFileName(rutaArchivo)}"
                    : await response.Content.ReadAsStringAsync()
            };
        }
        catch (Exception ex)
        {
            return new JobResult
            {
                Endpoint = ep.Nombre,
                Fecha = DateTime.Now,
                Exitoso = false,
                Mensaje = ex.Message
            };
        }
    }
}
