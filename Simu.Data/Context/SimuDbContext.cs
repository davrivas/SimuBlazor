using Microsoft.EntityFrameworkCore;
using Simu.Core.Entities;

namespace Simu.Data.Context;

/// <summary>
/// Entity Framework Core DbContext for Simu database
/// </summary>
public class SimuDbContext : DbContext
{
    public SimuDbContext(DbContextOptions<SimuDbContext> options) : base(options)
    {
    }

    // DbSets for all entities
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Moto> Motos { get; set; }
    public DbSet<Transaccion> Transacciones { get; set; }
    public DbSet<DetalleTransaccion> DetallesTransacciones { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Reparacion> Reparaciones { get; set; }
    public DbSet<Accesorio> Accesorios { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<Permiso> Permisos { get; set; }
    public DbSet<Marca> Marcas { get; set; }
    public DbSet<EstadoMoto> EstadoMotos { get; set; }
    public DbSet<TipoTransaccion> TiposTransaccion { get; set; }
    public DbSet<TipoProducto> TiposProducto { get; set; }
    public DbSet<TipoAccesorio> TiposAccesorio { get; set; }
    public DbSet<TipoReparacion> TiposReparacion { get; set; }
    public DbSet<TipoDocumento> TiposDocumento { get; set; }
    public DbSet<Ciudad> Ciudades { get; set; }
    public DbSet<Departamento> Departamentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure table names to match original Java schema
        modelBuilder.Entity<Moto>().ToTable("tbl_motos");
        modelBuilder.Entity<Transaccion>().ToTable("tbl_transacciones");
        modelBuilder.Entity<Producto>().ToTable("tbl_productos");
        modelBuilder.Entity<Reparacion>().ToTable("tbl_reparaciones");
        modelBuilder.Entity<Usuario>().ToTable("tbl_usuarios");
        modelBuilder.Entity<Accesorio>().ToTable("tbl_accesorios");

        // Configure decimal precision for prices
        modelBuilder.Entity<Producto>()
            .Property(p => p.Precio)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Producto>()
            .Property(p => p.PorcentajeDescuento)
            .HasPrecision(5, 2);

        modelBuilder.Entity<Transaccion>()
            .Property(t => t.Total)
            .HasPrecision(12, 2);

        modelBuilder.Entity<DetalleTransaccion>()
            .Property(d => d.PrecioUnitario)
            .HasPrecision(10, 2);

        modelBuilder.Entity<DetalleTransaccion>()
            .Property(d => d.Subtotal)
            .HasPrecision(12, 2);

        modelBuilder.Entity<Reparacion>()
            .Property(r => r.Costo)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Accesorio>()
            .Property(a => a.Precio)
            .HasPrecision(10, 2);

        // Configure relationships
        modelBuilder.Entity<Usuario>()
            .HasOne(u => u.Rol)
            .WithMany(r => r.Usuarios)
            .HasForeignKey(u => u.IdRol)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Usuario>()
            .HasOne(u => u.Ciudad)
            .WithMany(c => c.Usuarios)
            .HasForeignKey(u => u.IdCiudad)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Moto>()
            .HasOne(m => m.Marca)
            .WithMany(ma => ma.Motos)
            .HasForeignKey(m => m.IdMarca)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Moto>()
            .HasOne(m => m.EstadoMoto)
            .WithMany(e => e.Motos)
            .HasForeignKey(m => m.IdEstadoMoto)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaccion>()
            .HasOne(t => t.Moto)
            .WithMany(m => m.Transacciones)
            .HasForeignKey(t => t.IdMoto)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaccion>()
            .HasOne(t => t.Usuario)
            .WithMany(u => u.Transacciones)
            .HasForeignKey(t => t.IdUsuario)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DetalleTransaccion>()
            .HasOne(d => d.Transaccion)
            .WithMany(t => t.Detalles)
            .HasForeignKey(d => d.IdTransaccion)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DetalleTransaccion>()
            .HasOne(d => d.Producto)
            .WithMany(p => p.DetallesTransacciones)
            .HasForeignKey(d => d.IdProducto)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Reparacion>()
            .HasOne(r => r.Moto)
            .WithMany(m => m.Reparaciones)
            .HasForeignKey(r => r.IdMoto)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Reparacion>()
            .HasOne(r => r.Usuario)
            .WithMany(u => u.Reparaciones)
            .HasForeignKey(r => r.IdUsuario)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Producto>()
            .HasOne(p => p.TipoProducto)
            .WithMany(tp => tp.Productos)
            .HasForeignKey(p => p.IdTipoProducto)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Accesorio>()
            .HasOne(a => a.TipoAccesorio)
            .WithMany(ta => ta.Accesorios)
            .HasForeignKey(a => a.IdTipoAccesorio)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ciudad>()
            .HasOne(c => c.Departamento)
            .WithMany(d => d.Ciudades)
            .HasForeignKey(c => c.IdDepartamento)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Permiso>()
            .HasOne(p => p.Rol)
            .WithMany(r => r.Permisos)
            .HasForeignKey(p => p.IdRol)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
