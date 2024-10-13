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
	/*#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
				=> optionsBuilder.UseMySql("server=localhost;port=3306;database=rig;user=root;password=sa123", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.39-mysql"));
	*/
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
		modelBuilder
			.UseCollation("utf8mb4_0900_ai_ci")
			.HasCharSet("utf8mb4");

		modelBuilder.Entity<Actualreceived>(entity =>
		{
			entity.HasKey(e => e.ActualReceivedId).HasName("PRIMARY");

			entity.ToTable("actualreceived");

			entity.HasIndex(e => e.ScheduleId, "ScheduleID");

			entity.Property(e => e.ActualReceivedId).HasColumnName("ActualReceivedID");
			entity.Property(e => e.ActualDeliveryTime).HasColumnType("datetime");
			entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");

			entity.HasOne(d => d.Schedule).WithMany(p => p.Actualreceiveds)
				.HasForeignKey(d => d.ScheduleId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("actualreceived_ibfk_1");
		});

		modelBuilder.Entity<Actualsritd>(entity =>
		{
			entity.HasKey(e => e.ActualId).HasName("PRIMARY");

			entity.ToTable("actualsritd");

			entity.HasIndex(e => e.PlanDetailId, "PlanDetailID");

			entity.Property(e => e.ActualId).HasColumnName("ActualID");
			entity.Property(e => e.ActualTime).HasColumnType("datetime");
			entity.Property(e => e.PlanDetailId).HasColumnName("PlanDetailID");

			entity.HasOne(d => d.PlanDetail).WithMany(p => p.Actualsritds)
				.HasForeignKey(d => d.PlanDetailId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("actualsritd_ibfk_1");
		});

		modelBuilder.Entity<Planritd>(entity =>
		{
			entity.HasKey(e => e.PlanId).HasName("PRIMARY");

			entity.ToTable("planritd");

			entity.Property(e => e.PlanId).HasColumnName("PlanID");
			entity.Property(e => e.PlanName).HasMaxLength(100);
			entity.Property(e => e.PlanType).HasColumnType("enum('Received','Issued')");
		});

		modelBuilder.Entity<Planritddetail>(entity =>
		{
			entity.HasKey(e => e.PlanDetailId).HasName("PRIMARY");

			entity.ToTable("planritddetails");

			entity.HasIndex(e => e.PlanId, "PlanID");

			entity.Property(e => e.PlanDetailId).HasColumnName("PlanDetailID");
			entity.Property(e => e.PlanDetailName).HasMaxLength(100);
			entity.Property(e => e.PlanId).HasColumnName("PlanID");
			entity.Property(e => e.PlanTime).HasColumnType("time");

			entity.HasOne(d => d.Plan).WithMany(p => p.Planritddetails)
				.HasForeignKey(d => d.PlanId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("planritddetails_ibfk_1");
		});

		modelBuilder.Entity<Schedulereceived>(entity =>
		{
			entity.HasKey(e => e.ScheduleId).HasName("PRIMARY");

			entity.ToTable("schedulereceived");

			entity.HasIndex(e => e.DeliveryTimeId, "DeliveryTimeID");

			entity.HasIndex(e => e.SupplierId, "SupplierID");

			entity.HasIndex(e => e.WeekdayId, "WeekdayID");

			entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
			entity.Property(e => e.DeliveryTimeId).HasColumnName("DeliveryTimeID");
			entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
			entity.Property(e => e.WeekdayId).HasColumnName("WeekdayID");

			entity.HasOne(d => d.DeliveryTime).WithMany(p => p.Schedulereceiveds)
				.HasForeignKey(d => d.DeliveryTimeId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("schedulereceived_ibfk_2");

			entity.HasOne(d => d.Supplier).WithMany(p => p.Schedulereceiveds)
				.HasForeignKey(d => d.SupplierId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("schedulereceived_ibfk_1");

			entity.HasOne(d => d.Weekday).WithMany(p => p.Schedulereceiveds)
				.HasForeignKey(d => d.WeekdayId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("schedulereceived_ibfk_3");
		});

		modelBuilder.Entity<Status>(entity =>
		{
			entity.HasKey(e => e.StatusId).HasName("PRIMARY");

			entity.ToTable("statuses");

			entity.HasIndex(e => e.PlanDetailId, "PlanDetailID");

			entity.Property(e => e.StatusId).HasColumnName("StatusID");
			entity.Property(e => e.PlanDetailId).HasColumnName("PlanDetailID");
			entity.Property(e => e.Status1)
				.HasColumnType("enum('Pending','In Transit','Delivered')")
				.HasColumnName("Status");

			entity.HasOne(d => d.PlanDetail).WithMany(p => p.Statuses)
				.HasForeignKey(d => d.PlanDetailId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("statuses_ibfk_1");
		});

		modelBuilder.Entity<Supplier>(entity =>
		{
			entity.HasKey(e => e.SupplierId).HasName("PRIMARY");

			entity.ToTable("supplier");

			entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
			entity.Property(e => e.SupplierName).HasMaxLength(100);
		});

		modelBuilder.Entity<Time>(entity =>
		{
			entity.HasKey(e => e.TimeId).HasName("PRIMARY");

			entity.ToTable("time");

			entity.Property(e => e.TimeId).HasColumnName("TimeID");
			entity.Property(e => e.Time1)
				.HasColumnType("time")
				.HasColumnName("Time");
		});

		modelBuilder.Entity<Weekday>(entity =>
		{
			entity.HasKey(e => e.WeekdayId).HasName("PRIMARY");

			entity.ToTable("weekday");

			entity.Property(e => e.WeekdayId).HasColumnName("WeekdayID");
			entity.Property(e => e.DayName).HasMaxLength(50);
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
