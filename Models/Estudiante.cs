namespace BibliotecaBlazor.Models;

using System.ComponentModel.DataAnnotations;

public class Estudiante
{
  public int Id { get; set; }

  [Required(ErrorMessage = "La cédula es obligatoria.")]
  [RegularExpression(@"^\d{8}$", ErrorMessage = "La cédula debe tener exactamente 8 dígitos.")]
  public string Cedula { get; set; } = default!;

  [Required, StringLength(100)]
  public string Nombre { get; set; } = default!;

  [Required, StringLength(100)]
  public string Apellido { get; set; } = default!;

  [Range(1, 6, ErrorMessage = "El semestre debe estar entre 1 y 6.")]
  public int Semestre { get; set; }

  [Required, StringLength(100)]
  public string Carrera { get; set; } = default!;

  [Required, StringLength(1)]
  public string Seccion { get; set; } = default!;

  // 🔹 Soft delete
  public bool Activo { get; set; } = true;

  // 🔹 Navegación inversa: un estudiante puede tener muchos préstamos
  public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
