using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BackEndDebian.Model;

public partial class DbinventoryContext : DbContext
{
    public DbinventoryContext()
    {
    }

    public DbinventoryContext(DbContextOptions<DbinventoryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Barcode> Barcodes { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Device> Devices { get; set; }

    public virtual DbSet<Inventorynumber> Inventorynumbers { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Person> Persons { get; set; }

    public virtual DbSet<Personrole> Personroles { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=193.104.57.148;Port=5432;Database=dbinventory;Username=debianone;Password=toor");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Barcode>(entity =>
        {
            entity.HasKey(e => e.Barcodeid).HasName("barcodes_pkey");

            entity.ToTable("barcodes");

            entity.Property(e => e.Barcodeid).HasColumnName("barcodeid");
            entity.Property(e => e.Barcodevalue)
                .HasMaxLength(255)
                .HasColumnName("barcodevalue");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Deviceid).HasColumnName("deviceid");

            entity.HasOne(d => d.Device).WithMany(p => p.Barcodes)
                .HasForeignKey(d => d.Deviceid)
                .HasConstraintName("barcodes_deviceid_fkey");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasKey(e => e.Deviceid).HasName("device_pkey");

            entity.ToTable("device");

            entity.Property(e => e.Deviceid).HasColumnName("deviceid");
            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Locationid).HasColumnName("locationid");
            entity.Property(e => e.Manufacturerid).HasColumnName("manufacturerid");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasOne(d => d.Category).WithMany(p => p.Devices)
                .HasForeignKey(d => d.Categoryid)
                .HasConstraintName("device_categoryid_fkey");

            entity.HasOne(d => d.Location).WithMany(p => p.Devices)
                .HasForeignKey(d => d.Locationid)
                .HasConstraintName("device_locationid_fkey");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Devices)
                .HasForeignKey(d => d.Manufacturerid)
                .HasConstraintName("device_manufacturerid_fkey");
        });

        modelBuilder.Entity<Inventorynumber>(entity =>
        {
            entity.HasKey(e => e.Inventorynumberid).HasName("inventorynumbers_pkey");

            entity.ToTable("inventorynumbers");

            entity.Property(e => e.Inventorynumberid).HasColumnName("inventorynumberid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Deviceid).HasColumnName("deviceid");
            entity.Property(e => e.Number)
                .HasMaxLength(255)
                .HasColumnName("number");

            entity.HasOne(d => d.Device).WithMany(p => p.Inventorynumbers)
                .HasForeignKey(d => d.Deviceid)
                .HasConstraintName("inventorynumbers_deviceid_fkey");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Locationid).HasName("locations_pkey");

            entity.ToTable("locations");

            entity.Property(e => e.Locationid).HasColumnName("locationid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Manufacturerid).HasName("manufacturers_pkey");

            entity.ToTable("manufacturers");

            entity.Property(e => e.Manufacturerid).HasColumnName("manufacturerid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Personid).HasName("persons_pkey");

            entity.ToTable("persons");

            entity.HasIndex(e => e.Personname, "persons_personname_key").IsUnique();

            entity.Property(e => e.Personid).HasColumnName("personid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Passwordhash)
                .HasMaxLength(255)
                .HasColumnName("passwordhash");
            entity.Property(e => e.Personname)
                .HasMaxLength(255)
                .HasColumnName("personname");
            entity.Property(e => e.Salt)
                .HasMaxLength(255)
                .HasColumnName("salt");
        });

        modelBuilder.Entity<Personrole>(entity =>
        {
            entity.HasKey(e => e.Personroleid).HasName("personroles_pkey");

            entity.ToTable("personroles");

            entity.HasIndex(e => new { e.Personid, e.Roleid }, "personroles_personid_roleid_key").IsUnique();

            entity.Property(e => e.Personroleid).HasColumnName("personroleid");
            entity.Property(e => e.Personid).HasColumnName("personid");
            entity.Property(e => e.Roleid).HasColumnName("roleid");

            entity.HasOne(d => d.Person).WithMany(p => p.Personroles)
                .HasForeignKey(d => d.Personid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("personroles_personid_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Personroles)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("personroles_roleid_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Rolename, "roles_rolename_key").IsUnique();

            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Rolename)
                .HasMaxLength(255)
                .HasColumnName("rolename");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
