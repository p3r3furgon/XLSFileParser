using B1Task2.Models;
using Microsoft.EntityFrameworkCore;

namespace B1Task2.DataAccess;

public partial class BankDataContext : DbContext
{
    public BankDataContext()
    {
    }

    public BankDataContext(DbContextOptions<BankDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountClass> Accountclasses { get; set; }

    public virtual DbSet<AccountSource> Accountsources { get; set; }

    public virtual DbSet<ElementsType> Dets { get; set; }

    public virtual DbSet<Element> Elements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("account_pkey");

            entity.ToTable("account");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountCode).HasColumnName("accountcode");
            entity.Property(e => e.ClassId).HasColumnName("classid");

            entity.HasOne(d => d.Class).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("account_classid_fkey");
        });

        modelBuilder.Entity<AccountClass>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("accountclass_pkey");

            entity.ToTable("accountclass");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClassCode).HasColumnName("classcode");
            entity.Property(e => e.ClassName)
                .HasMaxLength(255)
                .HasColumnName("classname");

            entity.HasOne(d => d.Source).WithMany(p => p.AccountClasses)
                .HasForeignKey(d => d.SourceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("accountclass_sourceid_fkey");
        });

        modelBuilder.Entity<AccountSource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("accountsource_pkey");

            entity.ToTable("accountsource");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SourceType)
                .HasMaxLength(100)
                .HasColumnName("sourcetype");
            entity.Property(e => e.UploadDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp with time zone")
                .HasColumnName("uploaddate");
        });

        modelBuilder.Entity<ElementsType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("det_pkey");

            entity.ToTable("det");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.HasData(
                new ElementsType
                { 
                    Id = 1,
                    Name = "IN_BALANCE_A",
                    Description = "Входящее сальдо актив"
                },
                new ElementsType
                {
                    Id = 2,
                    Name = "IN_BALANCE_P",
                    Description = "Входящее сальдо пассив"
                },
                new ElementsType
                {
                    Id = 3,
                    Name = "TURNOVER_D",
                    Description = "Обороты дебет"
                },
                new ElementsType
                {
                    Id = 4,
                    Name = "TURNOVER_K",
                    Description = "Обороты кредит"
                },
                new ElementsType
                {
                    Id = 5,
                    Name = "OUT_BALANCE_A",
                    Description = "Исходящее сальдо актив"
                },
                new ElementsType
                {
                    Id = 6,
                    Name = "OUT_BALANCE_P",
                    Description = "Исходящее сальдо пассив"
                }
                );

        });

        modelBuilder.Entity<Element>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("element_pkey");

            entity.ToTable("element");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Accountid).HasColumnName("accountid");
            entity.Property(e => e.Elementtypeid).HasColumnName("elementtypeid");
            entity.Property(e => e.Value)
                .HasPrecision(18, 2)
                .HasColumnName("value");

            entity.HasOne(d => d.Account).WithMany(p => p.Elements)
                .HasForeignKey(d => d.Accountid)
                .HasConstraintName("element_accountid_fkey");

            entity.HasOne(d => d.ElementType).WithMany(p => p.Elements)
                .HasForeignKey(d => d.Elementtypeid)
                .HasConstraintName("element_elementtypeid_fkey");
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
