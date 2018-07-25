using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace IntlOps.Data
{
    //Main Identity Interfaces
    public class Applications
    {
        public int ApplicationId { get; set; }
        public int ClientId { get; set; }
        public string ApplicationStatus { get; set; }
        public DateTime ApplicationDate { get; set; }
        public decimal Income { get; set; }    
        public decimal CreditRequested { get; set; }
        public ApplicationUser Client { get; set; }
    }
    public class ApplicationUser : IdentityUser<int>
    {
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string AccountName { get; set; }
        [Required]
        public string JobTitle { get; set; }
        [Required]
        public string JobType { get; set; }
        [Required]
        public string Birthdate { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string MaritalStatus { get; set; }
        [Required]
        public string Street1 { get; set; }
        [Required]
        public string Street2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Zipcode { get; set; }
        public IEnumerable<Applications> Applications { get; internal set; }
    }
    public class ApplicationRole : IdentityRole<int>
    {
        public ApplicationRole()
        {
        }
        public ApplicationRole(string roleName) : base(roleName)
        {
        }
    }
    public class ApplicationUserClaim<T> : IdentityUserClaim<int>
    {
    }
    public class ApplicationUserLogin<T> : IdentityUserLogin<int>
    {
    }
    public class ApplicationRoleClaim<T> : IdentityRoleClaim<int>
    {
    }
    public class ApplicationUserRole<T> : IdentityUserRole<int>
    {
    }
    public class ApplicationUserToken<T> : IdentityUserToken<int>
    {
    }
    public class States
    {
        public int StateId { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
    }

    //Primary Implementation
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public virtual DbSet<Applications> Application { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUser { get; set; }
        public virtual DbSet<ApplicationRole> ApplicationRole { get; set; }
        public virtual DbSet<ApplicationUserClaim<int>> ApplicationUserClaim { get; set; }
        public virtual DbSet<ApplicationRoleClaim<int>> ApplicationRoleClaim { get; set; }
        public virtual DbSet<ApplicationUserLogin<int>> ApplicationUserLogin { get; set; }
        public virtual DbSet<ApplicationUserRole<int>> ApplicationUserRole { get; set; }
        public virtual DbSet<ApplicationUserToken<int>> ApplicationUserToken { get; set; }
        public virtual DbSet<States> States { get; set; }

        public ApplicationDbContext(){}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=MSI\SQLEXPRESS;Initial Catalog=intlops;Integrated Security=True;Pooling=False;");
            }
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options){}

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Applications>(entity =>
            {
                entity.HasKey(e => e.ApplicationId);

                entity.Property(e => e.ApplicationId).HasColumnName("Application_ID");

                entity.Property(e => e.ApplicationDate).HasColumnType("date");

                entity.Property(e => e.ApplicationStatus)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClientId).HasColumnName("Client_ID");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Applications_Users");
            });
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.AccountName).IsRequired();

                entity.Property(e => e.Birthdate).HasMaxLength(50);

                entity.Property(e => e.City).IsRequired();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Firstname).IsRequired();

                entity.Property(e => e.Gender).HasColumnType("nchar(10)");

                entity.Property(e => e.JobTitle).HasMaxLength(256);

                entity.Property(e => e.JobType).HasMaxLength(50);

                entity.Property(e => e.Lastname).IsRequired();

                entity.Property(e => e.MaritalStatus).HasMaxLength(50);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.State).IsRequired();

                entity.Property(e => e.Street1).IsRequired();

                entity.Property(e => e.Street2).IsRequired();

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.Property(e => e.Zipcode).IsRequired();

            });
            builder.Entity<ApplicationRole>(entity =>
            {
                entity.ToTable("Role");
            });
            builder.Entity<IdentityUserClaim<int>>(entity =>
            {
                entity.ToTable("UserClaim");
            });
            builder.Entity<IdentityUserLogin<int>>(entity =>
            {
                entity.ToTable("UserLogin");
            });
            builder.Entity<IdentityRoleClaim<int>>(entity =>
            {
                entity.ToTable("RoleClaim");
            });
            builder.Entity<IdentityUserRole<int>>(entity =>
            {
                entity.ToTable("UserRole");
            });
            builder.Entity<IdentityUserToken<int>>(entity =>
            {
                entity.ToTable("UserToken");
            });
            builder.Entity<States>(entity =>
            {
                entity.HasKey(e => e.StateId);

                entity.Property(e => e.StateId).HasColumnName("stateID");

                entity.Property(e => e.StateCode)
                    .IsRequired()
                    .HasColumnName("stateCode")
                    .HasColumnType("nchar(2)");

                entity.Property(e => e.StateName)
                    .IsRequired()
                    .HasColumnName("stateName")
                    .HasMaxLength(128);
            });
        }
    }
}
