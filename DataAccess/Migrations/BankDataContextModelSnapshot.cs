﻿// <auto-generated />
using System;
using B1Task2.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace B1Task2.DataAccess.Migrations
{
    [DbContext(typeof(BankDataContext))]
    partial class BankDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("B1Task2.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountCode")
                        .HasColumnType("integer")
                        .HasColumnName("accountcode");

                    b.Property<int>("ClassId")
                        .HasColumnType("integer")
                        .HasColumnName("classid");

                    b.HasKey("Id")
                        .HasName("account_pkey");

                    b.HasIndex("ClassId");

                    b.ToTable("account", (string)null);
                });

            modelBuilder.Entity("B1Task2.Models.AccountClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ClassCode")
                        .HasColumnType("integer")
                        .HasColumnName("classcode");

                    b.Property<string>("ClassName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("classname");

                    b.Property<int>("SourceId")
                        .HasColumnType("integer");

                    b.HasKey("Id")
                        .HasName("accountclass_pkey");

                    b.HasIndex("SourceId");

                    b.ToTable("accountclass", (string)null);
                });

            modelBuilder.Entity("B1Task2.Models.AccountSource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("SourceType")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("sourcetype");

                    b.Property<DateTime?>("UploadDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("uploaddate")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("Id")
                        .HasName("accountsource_pkey");

                    b.ToTable("accountsource", (string)null);
                });

            modelBuilder.Entity("B1Task2.Models.Element", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Accountid")
                        .HasColumnType("integer")
                        .HasColumnName("accountid");

                    b.Property<int>("Elementtypeid")
                        .HasColumnType("integer")
                        .HasColumnName("elementtypeid");

                    b.Property<decimal>("Value")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("element_pkey");

                    b.HasIndex("Accountid");

                    b.HasIndex("Elementtypeid");

                    b.ToTable("element", (string)null);
                });

            modelBuilder.Entity("B1Task2.Models.ElementsType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("det_pkey");

                    b.ToTable("det", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Входящее сальдо актив",
                            Name = "IN_BALANCE_A"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Входящее сальдо пассив",
                            Name = "IN_BALANCE_P"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Обороты дебет",
                            Name = "TURNOVER_D"
                        },
                        new
                        {
                            Id = 4,
                            Description = "Обороты кредит",
                            Name = "TURNOVER_K"
                        },
                        new
                        {
                            Id = 5,
                            Description = "Исходящее сальдо актив",
                            Name = "OUT_BALANCE_A"
                        },
                        new
                        {
                            Id = 6,
                            Description = "Исходящее сальдо пассив",
                            Name = "OUT_BALANCE_P"
                        });
                });

            modelBuilder.Entity("B1Task2.Models.Account", b =>
                {
                    b.HasOne("B1Task2.Models.AccountClass", "Class")
                        .WithMany("Accounts")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("account_classid_fkey");

                    b.Navigation("Class");
                });

            modelBuilder.Entity("B1Task2.Models.AccountClass", b =>
                {
                    b.HasOne("B1Task2.Models.AccountSource", "Source")
                        .WithMany("AccountClasses")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("accountclass_sourceid_fkey");

                    b.Navigation("Source");
                });

            modelBuilder.Entity("B1Task2.Models.Element", b =>
                {
                    b.HasOne("B1Task2.Models.Account", "Account")
                        .WithMany("Elements")
                        .HasForeignKey("Accountid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("element_accountid_fkey");

                    b.HasOne("B1Task2.Models.ElementsType", "ElementType")
                        .WithMany("Elements")
                        .HasForeignKey("Elementtypeid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("element_elementtypeid_fkey");

                    b.Navigation("Account");

                    b.Navigation("ElementType");
                });

            modelBuilder.Entity("B1Task2.Models.Account", b =>
                {
                    b.Navigation("Elements");
                });

            modelBuilder.Entity("B1Task2.Models.AccountClass", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("B1Task2.Models.AccountSource", b =>
                {
                    b.Navigation("AccountClasses");
                });

            modelBuilder.Entity("B1Task2.Models.ElementsType", b =>
                {
                    b.Navigation("Elements");
                });
#pragma warning restore 612, 618
        }
    }
}