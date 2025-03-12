using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WriteAndErase_App.Models;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderproduct> Orderproducts { get; set; }

    public virtual DbSet<Pickuppoint> Pickuppoints { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Productmanufacturer> Productmanufacturers { get; set; }

    public virtual DbSet<Productsupplier> Productsuppliers { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Unitofmeasurement> Unitofmeasurements { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=postgres;Username=postgres;Password=123456");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Categoryname)
                .HasMaxLength(100)
                .HasColumnName("categoryname");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Manufacturerid).HasName("manufacturers_pkey");

            entity.ToTable("manufacturers");

            entity.Property(e => e.Manufacturerid).HasColumnName("manufacturerid");
            entity.Property(e => e.Manufacturername)
                .HasMaxLength(100)
                .HasColumnName("manufacturername");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Orderclient)
                .HasDefaultValue(1)
                .HasColumnName("orderclient");
            entity.Property(e => e.Ordercodetoreceive).HasColumnName("ordercodetoreceive");
            entity.Property(e => e.Orderdate).HasColumnName("orderdate");
            entity.Property(e => e.Orderdeliverydate).HasColumnName("orderdeliverydate");
            entity.Property(e => e.Orderpickuppoint).HasColumnName("orderpickuppoint");
            entity.Property(e => e.Orderstatus).HasColumnName("orderstatus");

            entity.HasOne(d => d.OrderclientNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Orderclient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderclient_fk");

            entity.HasOne(d => d.OrderpickuppointNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Orderpickuppoint)
                .HasConstraintName("orderpickuppoint_fk");

            entity.HasOne(d => d.OrderstatusNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Orderstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderstatus_fk");
        });

        modelBuilder.Entity<Orderproduct>(entity =>
        {
            entity.HasKey(e => e.Orderproductid).HasName("orderproduct_pkey");

            entity.ToTable("orderproduct");

            entity.Property(e => e.Orderproductid).HasColumnName("orderproductid");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Productarticlenumber)
                .HasMaxLength(100)
                .HasColumnName("productarticlenumber");
            entity.Property(e => e.Productquantity).HasColumnName("productquantity");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderproducts)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_orderproduct_fk");

            entity.HasOne(d => d.ProductarticlenumberNavigation).WithMany(p => p.Orderproducts)
                .HasForeignKey(d => d.Productarticlenumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_orderproduct_fk");
        });

        modelBuilder.Entity<Pickuppoint>(entity =>
        {
            entity.HasKey(e => e.Pickuppointid).HasName("pickuppoint_pkey");

            entity.ToTable("pickuppoint");

            entity.Property(e => e.Pickuppointid).HasColumnName("pickuppointid");
            entity.Property(e => e.Pickuppointname)
                .HasMaxLength(100)
                .HasColumnName("pickuppointname");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Productarticlenumber).HasName("product_pkey");

            entity.ToTable("product");

            entity.Property(e => e.Productarticlenumber)
                .HasMaxLength(100)
                .HasColumnName("productarticlenumber");
            entity.Property(e => e.Productcategory).HasColumnName("productcategory");
            entity.Property(e => e.Productcost).HasColumnName("productcost");
            entity.Property(e => e.Productdescription)
                .HasColumnType("character varying")
                .HasColumnName("productdescription");
            entity.Property(e => e.Productdiscountamount).HasColumnName("productdiscountamount");
            entity.Property(e => e.Productmaximumpossiblediscountamount).HasColumnName("productmaximumpossiblediscountamount");
            entity.Property(e => e.Productname)
                .HasColumnType("character varying")
                .HasColumnName("productname");
            entity.Property(e => e.Productphoto)
                .HasColumnType("character varying")
                .HasColumnName("productphoto");
            entity.Property(e => e.Productquantityinstock).HasColumnName("productquantityinstock");
            entity.Property(e => e.Productstatus)
                .HasColumnType("character varying")
                .HasColumnName("productstatus");
            entity.Property(e => e.Productunitofmeasurement).HasColumnName("productunitofmeasurement");

            entity.HasOne(d => d.ProductcategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Productcategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("productcategory_fk");

            entity.HasOne(d => d.ProductunitofmeasurementNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Productunitofmeasurement)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("productunitofmeasurement_fk");
        });

        modelBuilder.Entity<Productmanufacturer>(entity =>
        {
            entity.HasKey(e => e.Productmanufacturerid).HasName("productmanufacturer_pkey");

            entity.ToTable("productmanufacturer");

            entity.Property(e => e.Productmanufacturerid).HasColumnName("productmanufacturerid");
            entity.Property(e => e.Manufacturerid).HasColumnName("manufacturerid");
            entity.Property(e => e.Productarticlenumber)
                .HasMaxLength(100)
                .HasColumnName("productarticlenumber");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Productmanufacturers)
                .HasForeignKey(d => d.Manufacturerid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("manufacturer_productmanufacturer_fk");

            entity.HasOne(d => d.ProductarticlenumberNavigation).WithMany(p => p.Productmanufacturers)
                .HasForeignKey(d => d.Productarticlenumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_productmanufacturer_fk");
        });

        modelBuilder.Entity<Productsupplier>(entity =>
        {
            entity.HasKey(e => e.Productsupplierid).HasName("productsupplier_pkey");

            entity.ToTable("productsupplier");

            entity.Property(e => e.Productsupplierid).HasColumnName("productsupplierid");
            entity.Property(e => e.Productarticlenumber)
                .HasMaxLength(100)
                .HasColumnName("productarticlenumber");
            entity.Property(e => e.Supplierid).HasColumnName("supplierid");

            entity.HasOne(d => d.ProductarticlenumberNavigation).WithMany(p => p.Productsuppliers)
                .HasForeignKey(d => d.Productarticlenumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_productsupplier_fk");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Productsuppliers)
                .HasForeignKey(d => d.Supplierid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("supplier_productsupplier_fk");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("role_pkey");

            entity.ToTable("role");

            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Rolename)
                .HasMaxLength(100)
                .HasColumnName("rolename");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Statusid).HasName("status_pkey");

            entity.ToTable("status");

            entity.Property(e => e.Statusid).HasColumnName("statusid");
            entity.Property(e => e.Statusname)
                .HasMaxLength(100)
                .HasColumnName("statusname");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Supplierid).HasName("suppliers_pkey");

            entity.ToTable("suppliers");

            entity.Property(e => e.Supplierid).HasColumnName("supplierid");
            entity.Property(e => e.Suppliername)
                .HasMaxLength(100)
                .HasColumnName("suppliername");
        });

        modelBuilder.Entity<Unitofmeasurement>(entity =>
        {
            entity.HasKey(e => e.Unitofmeasurementid).HasName("unitofmeasurement_pkey");

            entity.ToTable("unitofmeasurement");

            entity.Property(e => e.Unitofmeasurementid).HasColumnName("unitofmeasurementid");
            entity.Property(e => e.Unitofmeasurementname)
                .HasMaxLength(100)
                .HasColumnName("unitofmeasurementname");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Userlogin, "unique_user_login").IsUnique();

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Userlogin)
                .HasMaxLength(100)
                .HasColumnName("userlogin");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
            entity.Property(e => e.Userpassword)
                .HasColumnType("character varying")
                .HasColumnName("userpassword");
            entity.Property(e => e.Userpatronymic)
                .HasMaxLength(100)
                .HasColumnName("userpatronymic");
            entity.Property(e => e.Userrole).HasColumnName("userrole");
            entity.Property(e => e.Usersurname)
                .HasMaxLength(100)
                .HasColumnName("usersurname");

            entity.HasOne(d => d.UserroleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Userrole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("userrole_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
