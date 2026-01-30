using BibliotecaBlazor.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaBlazor.Data
{
  public class DevolucionService
  {
    private readonly BibliotecaContext _db;

    public DevolucionService(BibliotecaContext db)
    {
      _db = db;
    }

    // Registrar devolución
    public async Task RegistrarAsync(int prestamoId, string? observaciones = null)
    {
      var prestamo = await _db.Prestamos
          .Include(p => p.Detalles)
          .ThenInclude(d => d.Libro)
          .FirstOrDefaultAsync(p => p.Id == prestamoId);

      if (prestamo is null) return;

      // 🔹 Idempotencia: si ya fue devuelto, no repetir
      if (prestamo.Devuelto) return;

      // Actualizar stock de libros
      foreach (var detalle in prestamo.Detalles)
      {
        detalle.Libro.CantidadDisponible += detalle.Cantidad;
      }

      // Marcar préstamo como devuelto
      prestamo.Devuelto = true;

      // Registrar devolución
      var devolucion = new Devolucion
      {
        PrestamoId = prestamo.Id,
        FechaDevolucion = DateTime.UtcNow,
        Observaciones = observaciones
      };

      _db.Devoluciones.Add(devolucion);
      await _db.SaveChangesAsync();
    }

    // Obtener todas las devoluciones
    public async Task<List<Devolucion>> ObtenerTodasAsync()
        => await _db.Devoluciones
                    .Include(d => d.Prestamo)
                    .ThenInclude(p => p.Estudiante)
                    .Include(d => d.Prestamo)
                    .ThenInclude(p => p.Detalles)
                    .ThenInclude(det => det.Libro)
                    .OrderByDescending(d => d.FechaDevolucion)
                    .AsNoTracking()
                    .ToListAsync();
  }
}
