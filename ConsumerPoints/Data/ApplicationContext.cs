using Microsoft.EntityFrameworkCore;
using ConsumerPoints.Models;
using System;

namespace ConsumerPoints.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options)
            : base(options)
        {

        }

        public ApplicationContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Seed data - needs migration

            modelBuilder.Entity<Transaction>()
                .HasData(
                    new Transaction
                    {
                        Payer = "CVS Pharmacy",
                        Points = 40,
                        Timestamp = new DateTime(2021, 01, 30)

                    });
        }

        public DbSet<Transaction> Transactions { get; set; }
    }
}
