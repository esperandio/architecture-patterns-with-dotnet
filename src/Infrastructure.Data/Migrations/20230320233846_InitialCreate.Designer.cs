﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230320233846_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Domain.Batch", b =>
                {
                    b.Property<string>("Reference")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("Eta")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("PurchasedQuantity")
                        .HasColumnType("int");

                    b.Property<string>("Sku")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Reference");

                    b.ToTable("Batches");
                });

            modelBuilder.Entity("Domain.Batch", b =>
                {
                    b.OwnsMany("Domain.OrderLine", "Allocations", b1 =>
                        {
                            b1.Property<string>("BatchReference")
                                .HasColumnType("varchar(255)");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            b1.Property<string>("OrderId")
                                .IsRequired()
                                .HasColumnType("longtext");

                            b1.Property<int>("Quantity")
                                .HasColumnType("int");

                            b1.Property<string>("Sku")
                                .IsRequired()
                                .HasColumnType("longtext");

                            b1.HasKey("BatchReference", "Id");

                            b1.ToTable("OrderLine");

                            b1.WithOwner()
                                .HasForeignKey("BatchReference");
                        });

                    b.Navigation("Allocations");
                });
#pragma warning restore 612, 618
        }
    }
}