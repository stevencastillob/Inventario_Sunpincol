
using Microsoft.EntityFrameworkCore;

namespace Sunpincol.Models;

public partial class BDSunpincolContext : DbContext
{
    public BDSunpincolContext()
    {
    }

    public BDSunpincolContext(DbContextOptions<BDSunpincolContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Sede> Sedes { get; set; }

    public virtual DbSet<Inventario> Inventarios { get; set; }
    public virtual DbSet<Guia> Guias { get; set; }

    public virtual DbSet<Cambio> Cambios { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }












    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.ToTable("Producto");

            entity.HasKey(e => e.CodigoProducto);

            entity.Property(e => e.CodigoProducto)
                .HasMaxLength(30)
                .HasColumnName("codigoProducto");

            entity.Property(e => e.NombreProducto)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("nombreProducto");

            entity.Property(e => e.Categoria)
                .HasMaxLength(60)
                .IsRequired()
                .HasColumnName("categoria");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsRequired()
                .HasColumnName("descripcion");

            entity.Property(e => e.PrecioUnitario)
                .HasPrecision(12, 2)
                .HasColumnName("precioUnitario");

            entity.Property(e => e.EstadoProducto)
                .HasColumnName("estadoProducto");
        });

        modelBuilder.Entity<Sede>(entity =>
        {
            entity.ToTable("Sede");

            entity.HasKey(e => e.IdSede);

            entity.Property(e => e.IdSede)
                .HasColumnName("idSede");

            entity.Property(e => e.NombreSede)
                .HasMaxLength(80)
                .HasColumnName("nombreSede");

            entity.Property(e => e.EstadoSede)
                .HasMaxLength(20)
                .HasColumnName("estadoSede");
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.ToTable("Inventario");

            entity.HasKey(e => e.IdInventario);

            entity.Property(e => e.IdInventario)
                .HasColumnName("idInventario");

            entity.Property(e => e.IdSede)
                .HasColumnName("idSede");

            entity.Property(e => e.CodigoProducto)
                .HasMaxLength(30)
                .HasColumnName("codigoProducto");

            entity.Property(e => e.StockActual)
                .HasColumnName("stockActual");

            entity.Property(e => e.StockMinimo)
                .HasColumnName("stockMinimo");

            entity.Property(e => e.FechaUltimaActualizacion)
                .HasColumnName("fechaUltimaActualizacion");
        });

        modelBuilder.Entity<Guia>(entity =>
        {
            entity.ToTable("Guia");
            entity.HasKey(e => e.IdGuia);
            entity.Property(e => e.IdGuia).HasColumnName("idGuia");
            entity.Property(e => e.IdSedeOrigen).HasColumnName("idSedeOrigen");
            entity.Property(e => e.IdSedeDestino).HasColumnName("idSedeDestino");
            entity.Property(e => e.ReferenciaGuia).HasMaxLength(50).IsRequired().HasColumnName("referenciaGuia");
            entity.Property(e => e.FechaEmision).HasColumnName("fechaEmision");
            entity.Property(e => e.EstadoGuia).HasMaxLength(30).IsRequired().HasColumnName("estadoGuia");
        });


        modelBuilder.Entity<Cambio>(entity =>
        {
            entity.ToTable("Cambios");
            entity.HasKey(e => e.IdCambio);
            entity.Property(e => e.IdCambio).HasColumnName("idCambio");
            entity.Property(e => e.IdInventario).HasColumnName("idInventario");
            entity.Property(e => e.IdUsuarioResponsable).HasColumnName("idUsuarioResponsable");
            entity.Property(e => e.CantidadAnterior).HasColumnName("cantidadAnterior");
            entity.Property(e => e.CantidadNueva).HasColumnName("cantidadNueva");
            entity.Property(e => e.MotivoCambio).HasColumnName("motivoCambio");
            entity.Property(e => e.FechaCambio).HasColumnName("fechaCambio");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");
            entity.HasKey(e => e.IdUsuario);
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
            entity.Property(e => e.Correo).HasColumnName("correo");
            entity.Property(e => e.Contrasena).HasColumnName("contrasena");
            entity.Property(e => e.Rol).HasColumnName("rol");
            entity.Property(e => e.EstadoActivo).HasColumnName("estadoActivo");
        });










        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
