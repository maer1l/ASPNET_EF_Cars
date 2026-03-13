using System;
using System.Collections.Generic;
using ASPNET_EF_Cars.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNET_EF_Cars.Data;

public partial class AspcarsContext : DbContext
{
    public AspcarsContext()
    {
    }

    public AspcarsContext(DbContextOptions<AspcarsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.CarId).HasName("PK__cars__4C9A0DB37B4591E4");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__categori__D54EE9B4A1C61A5A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
