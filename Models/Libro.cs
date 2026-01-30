namespace BibliotecaBlazor.Models;

using System.ComponentModel.DataAnnotations;

public class Libro
{
  public int Id { get; set; }

  [Required, StringLength(200)]
  public string TituloLibro { get; set; } = default!;

  [Required, StringLength(200)]
  public string Autor { get; set; } = default!;

  [Range(0, int.MaxValue)]
  public int CantidadDisponible { get; set; }

  [StringLength(500)]
  public string? CaratulaUrl { get; set; }

  // 🔹 Soft delete
  public bool Activo { get; set; } = true;

  // 🔹 Navegación inversa: un libro puede estar en muchos préstamos
  public ICollection<PrestamoDetalle> PrestamoDetalles { get; set; } = new List<PrestamoDetalle>();
}
