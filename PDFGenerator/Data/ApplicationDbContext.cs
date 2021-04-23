using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using PDFGenerator.Models.ClientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientFirmRelation>()
                .HasKey(k => new { k.ClientID, k.FirmID });
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Fix> Fixes { get; set; }
        public DbSet<Firm> Firms { get; set; }
        public DbSet<Accesory> Accesories { get; set; }
        public DbSet<ClientFirmRelation> ClientFirmRelations { get; set; }
    }

    public class ApplicationDbContextFactory
            : IDesignTimeDbContextFactory<ApplicationDbContext>
    {

        public ApplicationDbContext CreateDbContext(string[] args) =>
            Program.BuildWebHost(args).Services
                .GetRequiredService<ApplicationDbContext>();

    }
}

