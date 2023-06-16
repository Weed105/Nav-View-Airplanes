using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Nav_View_Airplanes.Models;

namespace Nav_View_Airplanes;

public partial class FlightContext : DbContext
{
    public FlightContext()
    {
    }

    public FlightContext(DbContextOptions<FlightContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Airport> Airports { get; set; }

    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<Intermediate> Intermediates { get; set; }

    public virtual DbSet<Plane> Planes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=1234;database=flight", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Airport>(entity =>
        {
            entity.HasKey(e => e.Idairport).HasName("PRIMARY");

            entity.ToTable("airport");

            entity.Property(e => e.Idairport)
                .ValueGeneratedNever()
                .HasColumnName("idairport");
            entity.Property(e => e.City)
                .HasMaxLength(45)
                .HasColumnName("city");
            entity.Property(e => e.X).HasColumnName("x");
            entity.Property(e => e.Y).HasColumnName("y");
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(e => e.Idflight).HasName("PRIMARY");

            entity.ToTable("flight");

            entity.HasIndex(e => e.ArrivalAirport, "fk_arrival_airport_idx");

            entity.HasIndex(e => e.DepartureAirport, "fk_departure_airport_idx");

            entity.HasIndex(e => e.Idplane, "fk_idplane_idx");

            entity.Property(e => e.Idflight)
                .ValueGeneratedNever()
                .HasColumnName("idflight");
            entity.Property(e => e.ArrivalAirport).HasColumnName("arrival_airport");
            entity.Property(e => e.ArrivalTime)
                .HasColumnType("datetime")
                .HasColumnName("arrival_time");
            entity.Property(e => e.DepartureAirport).HasColumnName("departure_airport");
            entity.Property(e => e.DepartureTime)
                .HasColumnType("datetime")
                .HasColumnName("departure_time");
            entity.Property(e => e.Idplane).HasColumnName("idplane");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.ArrivalAirportNavigation).WithMany(p => p.FlightArrivalAirportNavigations)
                .HasForeignKey(d => d.ArrivalAirport)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_arrival_airport");

            entity.HasOne(d => d.DepartureAirportNavigation).WithMany(p => p.FlightDepartureAirportNavigations)
                .HasForeignKey(d => d.DepartureAirport)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_departure_airport");

            entity.HasOne(d => d.IdplaneNavigation).WithMany(p => p.Flights)
                .HasForeignKey(d => d.Idplane)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_idplane");
        });

        modelBuilder.Entity<Intermediate>(entity =>
        {
            entity.HasKey(e => e.Idflight).HasName("PRIMARY");

            entity.ToTable("intermediates");

            entity.HasIndex(e => e.Idairport, "fk_idairport_idx");

            entity.Property(e => e.Idflight)
                .ValueGeneratedNever()
                .HasColumnName("idflight");
            entity.Property(e => e.Idairport).HasColumnName("idairport");

            entity.HasOne(d => d.IdairportNavigation).WithMany(p => p.Intermediates)
                .HasForeignKey(d => d.Idairport)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_inter_airport");

            entity.HasOne(d => d.IdflightNavigation).WithOne(p => p.Intermediate)
                .HasForeignKey<Intermediate>(d => d.Idflight)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_idflight");
        });

        modelBuilder.Entity<Plane>(entity =>
        {
            entity.HasKey(e => e.Idplane).HasName("PRIMARY");

            entity.ToTable("plane");

            entity.HasIndex(e => e.Idairport, "fk_idairport_idx");

            entity.Property(e => e.Idplane)
                .ValueGeneratedNever()
                .HasColumnName("idplane");
            entity.Property(e => e.Idairport).HasColumnName("idairport");
            entity.Property(e => e.Model)
                .HasMaxLength(45)
                .HasColumnName("model");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.X).HasColumnName("x");
            entity.Property(e => e.Y).HasColumnName("y");

            entity.HasOne(d => d.IdairportNavigation).WithMany(p => p.Planes)
                .HasForeignKey(d => d.Idairport)
                .HasConstraintName("fk_idairport");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Iduser).HasName("PRIMARY");

            entity.ToTable("user");

            entity.Property(e => e.Iduser)
                .ValueGeneratedNever()
                .HasColumnName("iduser");
            entity.Property(e => e.Indicator).HasColumnName("indicator");
            entity.Property(e => e.Login)
                .HasMaxLength(45)
                .HasColumnName("login");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(45)
                .HasColumnName("password");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(45)
                .HasColumnName("patronymic");
            entity.Property(e => e.Surname)
                .HasMaxLength(45)
                .HasColumnName("surname");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
