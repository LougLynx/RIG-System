using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Manage_Receive_Issues_Goods.Models;

public partial class RigContext : IdentityDbContext
{
    public RigContext()
    {
    }

    public RigContext(DbContextOptions<RigContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Actualdetailtlip> Actualdetailtlips { get; set; }

    public virtual DbSet<Actualreceivedtlip> Actualreceivedtlips { get; set; }

    public virtual DbSet<Actualsissuestlip> Actualsissuestlips { get; set; }

    public virtual DbSet<Actualsreceivedenso> Actualsreceivedensos { get; set; }

    public virtual DbSet<Aspnetrole> Aspnetroles { get; set; }

    public virtual DbSet<Aspnetroleclaim> Aspnetroleclaims { get; set; }

    public virtual DbSet<Aspnetuser> Aspnetusers { get; set; }

    public virtual DbSet<Aspnetuserclaim> Aspnetuserclaims { get; set; }

    public virtual DbSet<Aspnetuserlogin> Aspnetuserlogins { get; set; }

    public virtual DbSet<Aspnetusertoken> Aspnetusertokens { get; set; }

    public virtual DbSet<Efmigrationshistory> Efmigrationshistories { get; set; }

    public virtual DbSet<Historyplanreceivedtlip> Historyplanreceivedtlips { get; set; }

    public virtual DbSet<Plandetailreceivedtlip> Plandetailreceivedtlips { get; set; }

    public virtual DbSet<Planreceivetlip> Planreceivetlips { get; set; }

    public virtual DbSet<Planritd> Planritds { get; set; }

    public virtual DbSet<Planritddetail> Planritddetails { get; set; }

    public virtual DbSet<Statusesritd> Statusesritds { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Weekday> Weekdays { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        IConfiguration configuration = builder.Build();
        optionsBuilder.UseMySql(configuration.GetConnectionString("DefaultConnection"),
            new MySqlServerVersion(new Version(8, 0, 21))); // Thay đổi phiên bản MySQL phù hợp
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Actualdetailtlip>(entity =>
        {
            entity.HasKey(e => e.ActualDetailId).HasName("PRIMARY");

            entity.HasOne(d => d.ActualReceived).WithMany(p => p.Actualdetailtlips)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("actualdetailtlip_ibfk_1");
        });

        modelBuilder.Entity<Actualreceivedtlip>(entity =>
        {
            entity.HasKey(e => e.ActualReceivedId).HasName("PRIMARY");

            entity.HasOne(d => d.SupplierCodeNavigation).WithMany(p => p.Actualreceivedtlips)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("actualreceivedtlip_ibfk_1");
        });

        modelBuilder.Entity<Actualsissuestlip>(entity =>
        {
            entity.HasKey(e => e.ActualId).HasName("PRIMARY");

            entity.HasOne(d => d.PlanDetail).WithMany(p => p.Actualsissuestlips)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("actualsissuestlip_ibfk_1");
        });

        modelBuilder.Entity<Actualsreceivedenso>(entity =>
        {
            entity.HasKey(e => e.ActualId).HasName("PRIMARY");

            entity.HasOne(d => d.PlanDetail).WithMany(p => p.Actualsreceivedensos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("actualsreceivedenso_ibfk_1");
        });

        modelBuilder.Entity<Aspnetrole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        modelBuilder.Entity<Aspnetroleclaim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Role).WithMany(p => p.Aspnetroleclaims).HasConstraintName("FK_AspNetRoleClaims_AspNetRoles_RoleId");
        });

        modelBuilder.Entity<Aspnetuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "Aspnetuserrole",
                    r => r.HasOne<Aspnetrole>().WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_AspNetUserRoles_AspNetRoles_RoleId"),
                    l => l.HasOne<Aspnetuser>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_AspNetUserRoles_AspNetUsers_UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("aspnetuserroles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<Aspnetuserclaim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.User).WithMany(p => p.Aspnetuserclaims).HasConstraintName("FK_AspNetUserClaims_AspNetUsers_UserId");
        });

        modelBuilder.Entity<Aspnetuserlogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.HasOne(d => d.User).WithMany(p => p.Aspnetuserlogins).HasConstraintName("FK_AspNetUserLogins_AspNetUsers_UserId");
        });

        modelBuilder.Entity<Aspnetusertoken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.HasOne(d => d.User).WithMany(p => p.Aspnetusertokens).HasConstraintName("FK_AspNetUserTokens_AspNetUsers_UserId");
        });

        modelBuilder.Entity<Efmigrationshistory>(entity =>
        {
            entity.HasKey(e => e.MigrationId).HasName("PRIMARY");
        });

        modelBuilder.Entity<Historyplanreceivedtlip>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PRIMARY");

            entity.HasOne(d => d.ActualReceived).WithMany(p => p.Historyplanreceivedtlips).HasConstraintName("historyplanreceivedtlip_ibfk_2");

            entity.HasOne(d => d.PlanDetail).WithMany(p => p.Historyplanreceivedtlips).HasConstraintName("historyplanreceivedtlip_ibfk_1");
        });

        modelBuilder.Entity<Plandetailreceivedtlip>(entity =>
        {
            entity.HasKey(e => e.PlanDetailId).HasName("PRIMARY");

            entity.HasOne(d => d.Plan).WithMany(p => p.Plandetailreceivedtlips)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plandetailreceivedtlip_ibfk_3");

            entity.HasOne(d => d.SupplierCodeNavigation).WithMany(p => p.Plandetailreceivedtlips)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plandetailreceivedtlip_ibfk_1");

            entity.HasOne(d => d.Weekday).WithMany(p => p.Plandetailreceivedtlips)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plandetailreceivedtlip_ibfk_2");
        });

        modelBuilder.Entity<Planreceivetlip>(entity =>
        {
            entity.HasKey(e => e.PlanId).HasName("PRIMARY");
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

            entity.HasOne(d => d.StatusIssue).WithMany(p => p.PlanritddetailStatusIssues).HasConstraintName("planritddetails_ibfk_3");

            entity.HasOne(d => d.StatusReceive).WithMany(p => p.PlanritddetailStatusReceives).HasConstraintName("planritddetails_ibfk_2");
        });

        modelBuilder.Entity<Statusesritd>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PRIMARY");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierCode).HasName("PRIMARY");
        });

        modelBuilder.Entity<Weekday>(entity =>
        {
            entity.HasKey(e => e.WeekdayId).HasName("PRIMARY");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
