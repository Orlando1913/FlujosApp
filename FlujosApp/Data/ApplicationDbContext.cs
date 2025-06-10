using FlujosApp.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FlujosApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Flujo> Flujos => Set<Flujo>();
        public DbSet<Paso> Pasos => Set<Paso>();
        public DbSet<Campo> Campos => Set<Campo>();
        public DbSet<PasoDependencia> PasoDependencias => Set<PasoDependencia>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Flujo>()
                .HasMany(f => f.Pasos)
                .WithOne(p => p.Flujo)
                .HasForeignKey(p => p.FlujoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Paso>()
                .HasMany(p => p.Campos)
                .WithOne(c => c.Paso)
                .HasForeignKey(c => c.PasoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PasoDependencia>()
                .HasKey(pd => new { pd.PasoId, pd.DependeDePasoId });

            modelBuilder.Entity<PasoDependencia>()
                .HasOne(pd => pd.Paso)
                .WithMany(p => p.Dependencias)
                .HasForeignKey(pd => pd.PasoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PasoDependencia>()
                .HasOne(pd => pd.DependeDePaso)
                .WithMany(p => p.DependeDeEste)
                .HasForeignKey(pd => pd.DependeDePasoId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
