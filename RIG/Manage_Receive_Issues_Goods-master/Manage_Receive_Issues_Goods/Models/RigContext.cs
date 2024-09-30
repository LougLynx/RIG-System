using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Manage_Receive_Issues_Goods.Models;

public partial class RigContext : DbContext
{
    public RigContext()
    {
    }

    public RigContext(DbContextOptions<RigContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Actualreceived> Actualreceiveds { get; set; }

    public virtual DbSet<Schedulereceived> Schedulereceiveds { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Time> Times { get; set; }

    public virtual DbSet<Weekday> Weekdays { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    /*#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=sa123;database=rig", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.36-mysql"));*/
    {
        var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        IConfiguration configuration = builder.Build();

        // Cần thêm phiên bản của MySQL
        optionsBuilder.UseMySql(
            configuration.GetConnectionString("DefaultConnection"),
            ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Actualreceived>(entity =>
        {
            entity.HasKey(e => e.ActualReceivedId).HasName("PRIMARY");

            entity.HasOne(d => d.Schedule).WithMany(p => p.Actualreceiveds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("actualreceived_ibfk_1");
        });

        modelBuilder.Entity<Schedulereceived>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PRIMARY");

            entity.HasOne(d => d.DeliveryTime).WithMany(p => p.Schedulereceiveds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("schedulereceived_ibfk_2");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Schedulereceiveds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("schedulereceived_ibfk_1");

            entity.HasOne(d => d.Weekday).WithMany(p => p.Schedulereceiveds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("schedulereceived_ibfk_3");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PRIMARY");
        });

        modelBuilder.Entity<Time>(entity =>
        {
            entity.HasKey(e => e.TimeId).HasName("PRIMARY");
        });

        modelBuilder.Entity<Weekday>(entity =>
        {
            entity.HasKey(e => e.WeekdayId).HasName("PRIMARY");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
