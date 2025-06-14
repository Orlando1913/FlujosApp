﻿// <auto-generated />
using FlujosApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FlujosApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250610031527_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.36")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("FlujosApp.Entities.Campo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PasoId")
                        .HasColumnType("int");

                    b.Property<string>("Valor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("YaProcesado")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("PasoId");

                    b.ToTable("Campos");
                });

            modelBuilder.Entity("FlujosApp.Entities.Flujo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Flujos");
                });

            modelBuilder.Entity("FlujosApp.Entities.Paso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("FlujoId")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Orden")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FlujoId");

                    b.ToTable("Pasos");
                });

            modelBuilder.Entity("FlujosApp.Entities.PasoDependencia", b =>
                {
                    b.Property<int>("PasoId")
                        .HasColumnType("int");

                    b.Property<int>("DependeDePasoId")
                        .HasColumnType("int");

                    b.HasKey("PasoId", "DependeDePasoId");

                    b.HasIndex("DependeDePasoId");

                    b.ToTable("PasoDependencias");
                });

            modelBuilder.Entity("FlujosApp.Entities.Campo", b =>
                {
                    b.HasOne("FlujosApp.Entities.Paso", "Paso")
                        .WithMany("Campos")
                        .HasForeignKey("PasoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Paso");
                });

            modelBuilder.Entity("FlujosApp.Entities.Paso", b =>
                {
                    b.HasOne("FlujosApp.Entities.Flujo", "Flujo")
                        .WithMany("Pasos")
                        .HasForeignKey("FlujoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Flujo");
                });

            modelBuilder.Entity("FlujosApp.Entities.PasoDependencia", b =>
                {
                    b.HasOne("FlujosApp.Entities.Paso", "DependeDePaso")
                        .WithMany("DependeDeEste")
                        .HasForeignKey("DependeDePasoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FlujosApp.Entities.Paso", "Paso")
                        .WithMany("Dependencias")
                        .HasForeignKey("PasoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("DependeDePaso");

                    b.Navigation("Paso");
                });

            modelBuilder.Entity("FlujosApp.Entities.Flujo", b =>
                {
                    b.Navigation("Pasos");
                });

            modelBuilder.Entity("FlujosApp.Entities.Paso", b =>
                {
                    b.Navigation("Campos");

                    b.Navigation("DependeDeEste");

                    b.Navigation("Dependencias");
                });
#pragma warning restore 612, 618
        }
    }
}
