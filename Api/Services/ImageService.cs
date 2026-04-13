namespace Api.Services;

public interface IImageService
{
    Task<string?> SaveImageAsync(IFormFile file, string uploadFolder);
    void DeleteImage(string? imagePath);
}

public class ImageService(IWebHostEnvironment env) : IImageService
{
    public async Task<string?> SaveImageAsync(IFormFile file, string uploadFolder)
    {
        // Debug logging
        Console.WriteLine($"[ImageService] SaveImageAsync called with file: {file?.FileName}, size: {file?.Length}");

        if (file == null || file.Length == 0)
        {
            Console.WriteLine("[ImageService] File is null or empty, returning null");
            return null;
        }

        // Validar extensión
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        Console.WriteLine($"[ImageService] File extension: {fileExtension}");

        if (!allowedExtensions.Contains(fileExtension))
            throw new BadImageFormatException("Solo se permiten archivos JPG, PNG o WebP");

        // Crear carpeta si no existe
        var uploadPath = Path.Combine(env.WebRootPath, uploadFolder);
        Console.WriteLine($"[ImageService] Upload path: {uploadPath}");
        
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
            Console.WriteLine($"[ImageService] Created directory: {uploadPath}");
        }

        // Generar nombre único (evitar conflictos)
        var fileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(uploadPath, fileName);
        Console.WriteLine($"[ImageService] Saving to: {filePath}");

        // Guardar archivo
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var relativePath = $"/{uploadFolder}/{fileName}";
        Console.WriteLine($"[ImageService] File saved successfully. Relative path: {relativePath}");

        // Devolver ruta relativa que se puede usar en <img src="" />
        return relativePath;
    }

    public void DeleteImage(string? imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
            return;

        try
        {
            var fullPath = Path.Combine(env.WebRootPath, imagePath.TrimStart('/'));
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
        catch
        {
            // Log pero no fallar
        }
    }
}
