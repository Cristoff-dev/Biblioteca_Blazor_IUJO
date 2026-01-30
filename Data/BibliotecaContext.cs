using BibliotecaBlazor.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaBlazor.Data
{
  public class BibliotecaContext : DbContext
  {
    public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options) { }

    public DbSet<Libro> Libros => Set<Libro>();
    public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
    public DbSet<Prestamo> Prestamos => Set<Prestamo>();
    public DbSet<PrestamoDetalle> PrestamosDetalles => Set<PrestamoDetalle>();
    public DbSet<Devolucion> Devoluciones => Set<Devolucion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // Prestamo → Estudiante (un estudiante tiene muchos préstamos)
      modelBuilder.Entity<Prestamo>()
          .HasOne(p => p.Estudiante)
          .WithMany(e => e.Prestamos)   // 🔹 usa la colección en Estudiante
          .HasForeignKey(p => p.EstudianteId)
          .OnDelete(DeleteBehavior.Restrict);

      // Prestamo → Detalles (un préstamo tiene muchos detalles)
      modelBuilder.Entity<Prestamo>()
          .HasMany(p => p.Detalles)
          .WithOne(d => d.Prestamo)
          .HasForeignKey(d => d.PrestamoId)
          .OnDelete(DeleteBehavior.Cascade);

      // PrestamoDetalle → Libro (un libro puede estar en muchos detalles)
      modelBuilder.Entity<PrestamoDetalle>()
          .HasOne(d => d.Libro)
          .WithMany(l => l.PrestamoDetalles)   // 🔹 usa la colección en Libro
          .HasForeignKey(d => d.LibroId)
          .OnDelete(DeleteBehavior.Restrict);

      // Devolucion → Prestamo (si no tienes colección en Prestamo, puedes dejarlo así)
      modelBuilder.Entity<Devolucion>()
          .HasOne(d => d.Prestamo)
          .WithMany()   // 🔹 no hay colección en Prestamo, está bien
          .HasForeignKey(d => d.PrestamoId)
          .OnDelete(DeleteBehavior.Cascade);

      // 🔹 Convención: todo en minúsculas
      foreach (var entity in modelBuilder.Model.GetEntityTypes())
      {
        entity.SetTableName(entity.GetTableName()!.ToLower());

        foreach (var property in entity.GetProperties())
          property.SetColumnName(property.GetColumnName()!.ToLower());

        foreach (var key in entity.GetKeys())
          key.SetName(key.GetName()!.ToLower());

        foreach (var index in entity.GetIndexes())
          index.SetDatabaseName(index.GetDatabaseName()!.ToLower());
      }
    }
  }
}
