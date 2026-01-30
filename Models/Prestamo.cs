namespace BibliotecaBlazor.Models
{
  public class Prestamo
  {
    public int Id { get; set; }
    public int EstudianteId { get; set; }
    public Estudiante Estudiante { get; set; } = null!;
    public bool EsInterno { get; set; }

    private DateTime _fechaSalida;
    public DateTime FechaSalida
    {
      get => _fechaSalida;
      set => _fechaSalida = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }

    private DateTime? _fechaDevolucion;
    public DateTime? FechaDevolucion
    {
      get => _fechaDevolucion;
      set => _fechaDevolucion = value.HasValue
          ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)
          : null;
    }

    public List<PrestamoDetalle> Detalles { get; set; } = new();

    // 🔹 Nuevo: observaciones opcionales al crear préstamo
    public string? Observaciones { get; set; }

    // 🔹 Nuevo: flag para saber si ya fue devuelto
    public bool Devuelto { get; set; } = false;
  }
}
