using System;
using System.Collections.Generic;
using Lab12.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab12.Data;

public partial class VeterinariaContext : DbContext
{
    public VeterinariaContext()
    {
    }

    public VeterinariaContext(DbContextOptions<VeterinariaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Mascota> Mascotas { get; set; }

    public virtual DbSet<Propietario> Propietarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Mascota>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Mascotas__3214EC07C97AF567");

            entity.HasOne(d => d.Propietario).WithMany(p => p.Mascotas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mascotas_Propietarios");
        });

        modelBuilder.Entity<Propietario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Propieta__3214EC07CDDA9A39");

            entity.Property(e => e.Dni).IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
