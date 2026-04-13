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
        if (file == null || file.Length == 0)
            return null;

        // Validar extensión
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(fileExtension))
            throw new BadImageFormatException("Solo se permiten archivos JPG, PNG o WebP");

        // Crear carpeta si no existe
        var uploadPath = Path.Combine(env.WebRootPath, uploadFolder);
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        // Generar nombre único (evitar conflictos)
        var fileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(uploadPath, fileName);

        // Guardar archivo
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Devolver ruta relativa que se puede usar en <img src="" />
        return $"/{uploadFolder}/{fileName}";
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
