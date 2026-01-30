using BibliotecaBlazor.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaBlazor.Data
{
  public class PrestamoService
  {
    private readonly BibliotecaContext _db;

    public PrestamoService(BibliotecaContext db)
    {
      _db = db;
    }

    // 🔹 Validación: máximo 3 libros por préstamo
    private void ValidarCantidadMaxima(List<PrestamoDetalle> detalles)
    {
      int total = detalles.Sum(d => d.Cantidad);
      if (total > 3)
        throw new Exception("No se pueden prestar más de 3 libros en total.");
    }

    // Crear préstamo
    public async Task CrearAsync(Prestamo prestamo)
    {
      prestamo.FechaSalida = DateTime.SpecifyKind(prestamo.FechaSalida, DateTimeKind.Utc);

      if (!prestamo.EsInterno && prestamo.FechaDevolucion.HasValue)
      {
        var fecha = prestamo.FechaDevolucion.Value;
        prestamo.FechaDevolucion = fecha == DateTime.MinValue
            ? null
            : DateTime.SpecifyKind(fecha, DateTimeKind.Utc);
      }
      else
      {
        prestamo.FechaDevolucion = null;
      }

      ValidarCantidadMaxima(prestamo.Detalles); // 🔹 aplicar regla

      foreach (var d in prestamo.Detalles)
      {
        var libro = await _db.Libros.FindAsync(d.LibroId);
        if (libro is null) throw new Exception("Libro no encontrado.");
        if (d.Cantidad <= 0) throw new Exception("Cantidad inválida.");
        if (d.Cantidad > libro.CantidadDisponible) throw new Exception("Stock insuficiente.");

        libro.CantidadDisponible -= d.Cantidad;
      }

      _db.Prestamos.Add(prestamo);
      await _db.SaveChangesAsync();
    }

    // Actualizar préstamo
    public async Task ActualizarAsync(Prestamo prestamo)
    {
      var existente = await _db.Prestamos
          .Include(p => p.Detalles)
          .FirstOrDefaultAsync(p => p.Id == prestamo.Id);

      if (existente is null) return;

      existente.EstudianteId = prestamo.EstudianteId;
      existente.EsInterno = prestamo.EsInterno;
      existente.Observaciones = prestamo.Observaciones;

      existente.FechaSalida = DateTime.SpecifyKind(prestamo.FechaSalida, DateTimeKind.Utc);
      if (!prestamo.EsInterno && prestamo.FechaDevolucion.HasValue)
      {
        var fecha = prestamo.FechaDevolucion.Value;
        existente.FechaDevolucion = fecha == DateTime.MinValue
            ? null
            : DateTime.SpecifyKind(fecha, DateTimeKind.Utc);
      }
      else
      {
        existente.FechaDevolucion = null;
      }

      ValidarCantidadMaxima(prestamo.Detalles); // 🔹 aplicar regla

      // Restituir stock de detalles anteriores
      foreach (var d in existente.Detalles)
      {
        var libro = await _db.Libros.FindAsync(d.LibroId);
        if (libro is not null)
          libro.CantidadDisponible += d.Cantidad;
      }

      // Reemplazar detalles y descontar stock
      existente.Detalles.Clear();
      foreach (var d in prestamo.Detalles)
      {
        var libro = await _db.Libros.FindAsync(d.LibroId);
        if (libro is null) throw new Exception("Libro no encontrado.");
        if (d.Cantidad <= 0) throw new Exception("Cantidad inválida.");
        if (d.Cantidad > libro.CantidadDisponible) throw new Exception("Stock insuficiente.");

        existente.Detalles.Add(new PrestamoDetalle
        {
          PrestamoId = existente.Id,
          LibroId = d.LibroId,
          Cantidad = d.Cantidad
        });

        libro.CantidadDisponible -= d.Cantidad;
      }

      await _db.SaveChangesAsync();
    }

    // Obtener préstamos pendientes
    public async Task<List<Prestamo>> ObtenerPendientesAsync()
        => await _db.Prestamos
                    .Include(p => p.Estudiante)
                    .Include(p => p.Detalles)
                    .ThenInclude(d => d.Libro)
                    .Where(p => !p.Devuelto)
                    .OrderByDescending(p => p.FechaSalida)
                    .AsNoTracking()
                    .ToListAsync();
  }
}
