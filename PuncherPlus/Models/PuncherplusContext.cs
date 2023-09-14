using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PuncherPlus.Models;

public partial class PuncherplusContext : DbContext
{
    public PuncherplusContext()
    {
    }

    public PuncherplusContext(DbContextOptions<PuncherplusContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Dpointer> Dpointers { get; set; }

    public virtual DbSet<Muser> Musers { get; set; }

    
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder);
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
       //=> optionsBuilder.UseMySql("server=localhost;port=3306;database=puncherplus;uid=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.31-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Dpointer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("dpointer");

            entity.HasIndex(e => e.IdUser, "FK_dpointer_muser");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("createAT");
            entity.Property(e => e.pointerType)
                .HasMaxLength(150)
                .HasColumnName("pointerType");
            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.Motivo)
                .HasMaxLength(150)
                .HasColumnName("motivo");
            //entity.Property(e => e.Salida)
            //    .HasColumnType("time")
            //    .HasColumnName("salida");
        });

        modelBuilder.Entity<Muser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("muser");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deletedAt");
            entity.Property(e => e.FamilyName).HasMaxLength(50);
            entity.Property(e => e.GivenName).HasMaxLength(50);
            entity.Property(e => e.Nick)
                .HasMaxLength(50)
                .HasColumnName("nick");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
