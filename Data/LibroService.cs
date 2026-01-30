using BibliotecaBlazor.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaBlazor.Data
{
  public class LibroService
  {
    private readonly BibliotecaContext _db;

    public LibroService(BibliotecaContext db)
    {
      _db = db;
    }

    // 🔹 Obtener todos los libros activos
    public async Task<List<Libro>> ObtenerTodosAsync()
    {
      return await _db.Libros
          .Where(l => l.Activo)
          .AsNoTracking()
          .OrderBy(l => l.TituloLibro)
          .ToListAsync();
    }

    // 🔹 Buscar libros activos por título o autor
    public async Task<List<Libro>> BuscarAsync(string? term)
    {
      var query = _db.Libros.Where(l => l.Activo).AsQueryable();

      if (!string.IsNullOrWhiteSpace(term))
      {
        query = query.Where(l =>
            l.TituloLibro.ToLower().Contains(term.ToLower()) ||
            l.Autor.ToLower().Contains(term.ToLower()));
      }

      return await query
          .AsNoTracking()
          .OrderBy(l => l.TituloLibro)
          .ToListAsync();
    }

    // 🔹 Crear libro
    public async Task CrearAsync(Libro libro)
    {
      libro.Activo = true;
      _db.Libros.Add(libro);
      await _db.SaveChangesAsync();
    }

    // 🔹 Actualizar libro
    public async Task ActualizarAsync(Libro libro)
    {
      var existente = await _db.Libros.FindAsync(libro.Id);
      if (existente is null) return;

      existente.TituloLibro = libro.TituloLibro;
      existente.Autor = libro.Autor;
      existente.CantidadDisponible = libro.CantidadDisponible;
      existente.CaratulaUrl = libro.CaratulaUrl;

      await _db.SaveChangesAsync();
    }

    // 🔹 Soft delete libro
    public async Task EliminarAsync(int id)
    {
      var libro = await _db.Libros.FindAsync(id);
      if (libro is not null)
      {
        libro.Activo = false;
        await _db.SaveChangesAsync();
      }
    }

    // 🔹 Guardar carátula en wwwroot/caratulas
    public async Task<string> GuardarCaratulaAsync(Stream stream, string fileName)
    {
      var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "caratulas");

      if (!Directory.Exists(folderPath))
      {
        Directory.CreateDirectory(folderPath);
      }

      var safeFileName = Path.GetFileName(fileName);
      var filePath = Path.Combine(folderPath, safeFileName);

      using (var fileStream = new FileStream(filePath, FileMode.Create))
      {
        await stream.CopyToAsync(fileStream);
      }

      return $"/caratulas/{safeFileName}";
    }
  }
}
