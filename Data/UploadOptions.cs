namespace BibliotecaBlazor.Data;

public class UploadsOptions
{
  public string CoversRelativePath { get; set; } = "uploads/covers";
  public string[] AllowedExtensions { get; set; } = new[] { ".jpg", ".jpeg", ".png" };
  public long MaxSizeBytes { get; set; } = 5 * 1024 * 1024; // 5 MB
}
