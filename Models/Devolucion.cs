namespace BibliotecaBlazor.Models
{
  public class Devolucion
  {
    public int Id { get; set; }

    // Relación con préstamo
    public int PrestamoId { get; set; }
    public Prestamo Prestamo { get; set; } = null!;

    private DateTime _fechaDevolucion;
    public DateTime FechaDevolucion
    {
      get => _fechaDevolucion;
      set => _fechaDevolucion = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }

    // 🔹 Observaciones independientes de la devolución
    public string? Observaciones { get; set; }
  }
}
