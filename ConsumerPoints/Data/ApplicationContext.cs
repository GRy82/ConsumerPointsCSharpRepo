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
                        Points = 400,
                        Timestamp = new DateTime(2021, 01, 30)

                    },
                    new Transaction{
                        Payer = "CVS Pharmacy",
                        Points = 200,
                        Timestamp = new DateTime(2021, 02, 04)

                    });

            modelBuilder.Entity<PayerPoints>()
                .HasData(
                    new PayerPoints
                    {
                        Payer = "cvs pharmacy",
                        Points = 600
                    });
        }

        public DbSet<PayerPoints> PayerPoints { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
