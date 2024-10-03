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

    public virtual DbSet<Actualsritd> Actualsritds { get; set; }

    public virtual DbSet<Planritd> Planritds { get; set; }

    public virtual DbSet<Planritddetail> Planritddetails { get; set; }

    public virtual DbSet<Schedulereceived> Schedulereceiveds { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Time> Times { get; set; }

    public virtual DbSet<Weekday> Weekdays { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=Pbei7955;database=rig", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.36-mysql"));

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

        modelBuilder.Entity<Actualsritd>(entity =>
        {
            entity.HasKey(e => e.ActualId).HasName("PRIMARY");

            entity.HasOne(d => d.PlanDetail).WithMany(p => p.Actualsritds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("actualsritd_ibfk_1");
        });

        modelBuilder.Entity<Planritd>(entity =>
        {
            entity.HasKey(e => e.PlanId).HasName("PRIMARY");
        });

        modelBuilder.Entity<Planritddetail>(entity =>
        {
            entity.HasKey(e => e.PlanDetailId).HasName("PRIMARY");

            entity.HasOne(d => d.Plan).WithMany(p => p.Planritddetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("planritddetails_ibfk_1");
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

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PRIMARY");

            entity.HasOne(d => d.PlanDetail).WithMany(p => p.Statuses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("statuses_ibfk_1");
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
