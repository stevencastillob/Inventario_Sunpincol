
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

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Bodeguero> Bodegueros { get; set; }
    public virtual DbSet<Historial> Historiales { get; set; }

    public virtual DbSet<GuiaDespacho> GuiasDespacho { get; set; }
    public virtual DbSet<Recepcion> Recepciones { get; set; }









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

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido);

            entity.ToTable("Pedido");

            entity.Property(e => e.IdPedido).HasColumnName("idPedido");
            entity.Property(e => e.IdSedeSolicitante).HasColumnName("idSedeSolicitante");
            entity.Property(e => e.IdEmpleadoSolicitante).HasColumnName("idEmpleadoSolicitante");
            entity.Property(e => e.CodigoProducto).HasColumnName("codigoProducto").HasMaxLength(30).IsUnicode(false);
            entity.Property(e => e.CantidadSolicitada).HasColumnName("cantidadSolicitada");
            entity.Property(e => e.EstadoPedido).HasColumnName("estadoPedido").HasMaxLength(30).IsUnicode(false);
            entity.Property(e => e.FechaSolicitud).HasColumnName("fechaSolicitud");

            entity.HasOne<Sede>().WithMany().HasForeignKey(e => e.IdSedeSolicitante);
            entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleadoSolicitante);
            entity.HasOne<Producto>().WithMany().HasForeignKey(e => e.CodigoProducto);
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado);
            entity.ToTable("Empleado");
            entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");
            entity.Property(e => e.IdSedeAsignada).HasColumnName("idSedeAsignada");
            entity.Property(e => e.Cargo).HasColumnName("cargo").HasMaxLength(50).IsUnicode(false);
            entity.Property(e => e.Turno).HasColumnName("turno").HasMaxLength(30).IsUnicode(false);
            entity.HasOne<Usuario>().WithMany().HasForeignKey(e => e.IdEmpleado);
            entity.HasOne<Sede>().WithMany().HasForeignKey(e => e.IdSedeAsignada);
        });

        modelBuilder.Entity<Bodeguero>(entity =>
        {
            entity.HasKey(e => e.IdBodeguero);
            entity.ToTable("Bodeguero");
            entity.Property(e => e.IdBodeguero).HasColumnName("idBodeguero");
            entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");
            entity.Property(e => e.IdBodegaAsignada).HasColumnName("idBodegaAsignada");
            entity.Property(e => e.ZonaAlmacenamiento).HasColumnName("zonaAlmacenamiento").HasMaxLength(60).IsUnicode(false);
            entity.Property(e => e.Turno).HasColumnName("turno").HasMaxLength(30).IsUnicode(false);
            entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
            entity.HasOne<Sede>().WithMany().HasForeignKey(e => e.IdBodegaAsignada);
        });

        modelBuilder.Entity<Historial>(entity =>
        {
            entity.HasKey(e => e.IdHistorial);
            entity.ToTable("Historial");
            entity.Property(e => e.IdHistorial).HasColumnName("idHistorial");
            entity.Property(e => e.IdPedido).HasColumnName("idPedido");
            entity.Property(e => e.IdUsuarioResponsable).HasColumnName("idUsuarioResponsable");
            entity.Property(e => e.EstadoAnterior).HasColumnName("estadoAnterior").HasMaxLength(30).IsUnicode(false);
            entity.Property(e => e.EstadoActual).HasColumnName("estadoActual").HasMaxLength(30).IsUnicode(false);
            entity.Property(e => e.FechaRegistro).HasColumnName("fechaRegistro");
            entity.HasOne<Pedido>().WithMany().HasForeignKey(e => e.IdPedido);
            entity.HasOne<Usuario>().WithMany().HasForeignKey(e => e.IdUsuarioResponsable);
        });

        modelBuilder.Entity<GuiaDespacho>(entity =>
        {
            entity.HasKey(e => e.IdGuiaDespacho);
            entity.ToTable("GuiaDespacho");
            entity.Property(e => e.IdGuiaDespacho).HasColumnName("idGuiaDespacho");
            entity.Property(e => e.IdPedido).HasColumnName("idPedido");
            entity.Property(e => e.IdBodegueroEmisor).HasColumnName("idBodegueroEmisor");
            entity.Property(e => e.NumeroGuia).HasColumnName("numeroGuia").HasMaxLength(50).IsUnicode(false);
            entity.Property(e => e.FechaEmision).HasColumnName("fechaEmision");
            entity.Property(e => e.EstadoEntrega).HasColumnName("estadoEntrega").HasMaxLength(30).IsUnicode(false);

            entity.HasOne<Pedido>().WithMany().HasForeignKey(e => e.IdPedido);
            entity.HasOne<Bodeguero>().WithMany().HasForeignKey(e => e.IdBodegueroEmisor);
        });

        modelBuilder.Entity<Recepcion>(entity =>
        {
            entity.HasKey(e => e.IdRecepcion);
            entity.ToTable("Recepcion");
            entity.Property(e => e.IdRecepcion).HasColumnName("idRecepcion");
            entity.Property(e => e.IdPedido).HasColumnName("idPedido");
            entity.Property(e => e.IdGuiaDespacho).HasColumnName("idGuiaDespacho");
            entity.Property(e => e.IdEmpleadoReceptor).HasColumnName("idEmpleadoReceptor");
            entity.Property(e => e.CantidadRecibida).HasColumnName("cantidadRecibida");
            entity.Property(e => e.Conformidad).HasColumnName("conformidad");
            entity.Property(e => e.FechaRecepcion).HasColumnName("fechaRecepcion");

            entity.HasOne<Pedido>().WithMany().HasForeignKey(e => e.IdPedido);
            entity.HasOne<GuiaDespacho>().WithMany().HasForeignKey(e => e.IdGuiaDespacho);
            entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleadoReceptor);
        });














        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
