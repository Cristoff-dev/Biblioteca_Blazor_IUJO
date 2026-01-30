namespace BibliotecaBlazor.Models
{
  public class PrestamoDetalle
  {
    public int Id { get; set; }
    public int PrestamoId { get; set; }
    public Prestamo Prestamo { get; set; } = null!;
    public int LibroId { get; set; }
    public Libro Libro { get; set; } = null!;
    public int Cantidad { get; set; }
  }
}
