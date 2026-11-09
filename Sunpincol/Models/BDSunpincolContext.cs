using System;
using System.Collections.Generic;
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

/*    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\BDSunpincol.mdf; Integrated Security=True");
*/
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producto__3214EC0784313386");

            entity.ToTable("Producto");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Categoria)
                .HasMaxLength(50)
                .HasColumnName("categoria");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .HasColumnName("Descripcion");

            entity.Property(e => e.guia)
                .HasMaxLength(50)
                .HasColumnName("guia");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio).HasColumnName("precio");
            entity.Property(e => e.Stock).HasColumnName("stock");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
