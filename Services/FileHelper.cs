namespace ServicioCargasBatch.Services;

public static class FileHelper
{
    public static void MoverArchivo(string archivoOrigen, string carpetaDestino)
    {
        Directory.CreateDirectory(carpetaDestino);
        var destino = Path.Combine(carpetaDestino, Path.GetFileName(archivoOrigen));
        if (File.Exists(destino)) File.Delete(destino);
        File.Move(archivoOrigen, destino);
    }
}
