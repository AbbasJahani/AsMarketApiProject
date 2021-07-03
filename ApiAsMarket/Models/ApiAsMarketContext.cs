using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ApiAsMarket.Models;

namespace ApiAsMarket.Models
{
    public partial class ApiAsMarketContext : DbContext
    {
        public ApiAsMarketContext()
        {
        }

        public ApiAsMarketContext(DbContextOptions<ApiAsMarketContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdminInfo> AdminInfo { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<FavoriteProduct> FavoriteProduct { get; set; }
        public virtual DbSet<File> File { get; set; }
        public virtual DbSet<Oprator> Oprator { get; set; }
        public virtual DbSet<Poster> Poster { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductCategory> ProductCategory { get; set; }
        public virtual DbSet<Sale> Sale { get; set; }
        public virtual DbSet<SaleDetail> SaleDetails { get; set; }
        public virtual DbSet<Seller> Seller { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }
        public virtual DbSet<TiketReciver> TiketReciver { get; set; }
        public virtual DbSet<SaleFormSms> SaleFormSms { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=.;Database=ApiAsMarket;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminInfo>(entity =>
            {
                entity.Property(e => e.FullName).HasMaxLength(200);

                entity.Property(e => e.ImageUrl);

                entity.Property(e => e.Mobile).HasMaxLength(11);

                entity.Property(e => e.Password).HasMaxLength(150);

                entity.Property(e => e.Token).HasMaxLength(300);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(300);

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Family).HasMaxLength(50);

                entity.Property(e => e.Image);

                entity.Property(e => e.Mobile).HasMaxLength(11);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.NationalCode).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(150);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.PostalCode).HasMaxLength(10);

                entity.Property(e => e.Token).HasMaxLength(300);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.Entity<FavoriteProduct>(entity =>
            {
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.FavoriteProduct)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Favorite_ProductId_Product_Id");
            });

            modelBuilder.Entity<File>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Link).HasMaxLength(1000);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Description);

                entity.Property(e => e.Image);
            });

            modelBuilder.Entity<Oprator>(entity =>
            {
                entity.Property(e => e.Family).HasMaxLength(500);

                entity.Property(e => e.Image);

                entity.Property(e => e.Mobile).HasMaxLength(11);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.NationalCode).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.Token).HasMaxLength(300);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.Entity<Poster>(entity =>
            {
                entity.Property(e => e.Image1);

                entity.Property(e => e.Image2);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Caption).HasMaxLength(500);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Poster)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Poster_ProductId_Product_Id");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Attribute);

                entity.Property(e => e.Code).HasMaxLength(100);

                entity.Property(e => e.Image1);

                entity.Property(e => e.Image2);

                entity.Property(e => e.Image3);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Product_CategoryId_Category_Id");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Image1).HasMaxLength(512);
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.Property(e => e.SaleDate).HasColumnType("datetime");

                entity.Property(e => e.TrackingCode).HasMaxLength(50);

                entity.Property(e => e.TotalPrice);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Sale)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Sale_CustomerId_Customer_Id");

                entity.HasOne(d => d.Operator)
                    .WithMany(p => p.Sale)
                    .HasForeignKey(d => d.OperatorId)
                    .HasConstraintName("FK_Sale_OperatorId_Operator_Id");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.Sale)
                    .HasForeignKey(d => d.SellerId)
                    .HasConstraintName("FK_Sale_SellerId_Seller_Id");

            });

            modelBuilder.Entity<SaleDetail>(entity =>
            {
                entity.HasOne(d => d.Sale)
                    .WithMany(p => p.SaleDetails)
                    .HasForeignKey(d => d.SaleId)
                    .HasConstraintName("FK_SaleDetail_SaleId_Sale_Id")
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.SaleDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_SaleDetail_ProductId_Product_Id");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.SaleDetails)
                    .HasForeignKey(d => d.SellerId)
                    .HasConstraintName("FK_SaleDetail_SellerId_Seller_Id");

            });

            modelBuilder.Entity<Seller>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(300);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Image);

                entity.Property(e => e.Mobile).HasMaxLength(11);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.NationalCode).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(150);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.Token).HasMaxLength(300);

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.Property(e => e.Money).HasMaxLength(500);

                entity.Property(e => e.Family).HasMaxLength(500);
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.Property(e => e.Mobile).HasMaxLength(11);

                entity.Property(e => e.SendDate).HasColumnType("datetime");

                entity.Property(e => e.Subject).HasMaxLength(50);

                entity.Property(e => e.Text).HasMaxLength(500);
            });

            modelBuilder.Entity<SaleFormSms>(entity =>
            {
                entity.Property(e => e.ProductId);
                entity.Property(e => e.CustomerMobile);
                entity.Property(e => e.OperatorId);
                entity.Property(e => e.SellerId);
                entity.Property(e => e.State);
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.Property(e => e.Name);
            });
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.Date);
                entity.Property(e => e.Amount);
                entity.Property(e => e.Authority);
                entity.Property(e => e.Status);

            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

      
    }
}
