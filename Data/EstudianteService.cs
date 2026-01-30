using BibliotecaBlazor.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaBlazor.Data
{
  public class EstudianteService
  {
    private readonly BibliotecaContext _db;

    public EstudianteService(BibliotecaContext db)
    {
      _db = db;
    }

    // 🔹 Obtener todos los estudiantes activos
    public async Task<List<Estudiante>> ObtenerTodosAsync()
        => await _db.Estudiantes
                    .Where(e => e.Activo)
                    .AsNoTracking()
                    .OrderBy(e => e.Nombre)
                    .ToListAsync();

    // 🔹 Buscar estudiantes activos por cédula o nombre
    public async Task<List<Estudiante>> BuscarAsync(string? termino)
    {
      termino = termino?.Trim().ToLower();
      return await _db.Estudiantes
          .Where(e => e.Activo &&
                      (string.IsNullOrEmpty(termino) ||
                       e.Cedula.ToLower().Contains(termino!) ||
                       e.Nombre.ToLower().Contains(termino!) ||
                       e.Apellido.ToLower().Contains(termino!)))
          .AsNoTracking()
          .OrderBy(e => e.Nombre)
          .ToListAsync();
    }

    // 🔹 Buscar estudiante por cédula (para validar unicidad)
    public async Task<Estudiante?> BuscarPorCedulaAsync(string cedula)
    {
      return await _db.Estudiantes
                      .AsNoTracking()
                      .FirstOrDefaultAsync(e => e.Activo && e.Cedula == cedula);
    }

    // 🔹 Crear estudiante
    public async Task CrearAsync(Estudiante estudiante)
    {
      estudiante.Activo = true;
      _db.Estudiantes.Add(estudiante);
      await _db.SaveChangesAsync();
    }

    // 🔹 Actualizar estudiante
    public async Task ActualizarAsync(Estudiante estudiante)
    {
      var existente = await _db.Estudiantes.FindAsync(estudiante.Id);
      if (existente is null) return;

      existente.Cedula = estudiante.Cedula;
      existente.Nombre = estudiante.Nombre;
      existente.Apellido = estudiante.Apellido;
      existente.Semestre = estudiante.Semestre;
      existente.Carrera = estudiante.Carrera;
      existente.Seccion = estudiante.Seccion;

      await _db.SaveChangesAsync();
    }

    // 🔹 Soft delete estudiante
    public async Task EliminarAsync(int id)
    {
      var estudiante = await _db.Estudiantes.FindAsync(id);
      if (estudiante is null) return;

      estudiante.Activo = false;
      await _db.SaveChangesAsync();
    }
  }
}
