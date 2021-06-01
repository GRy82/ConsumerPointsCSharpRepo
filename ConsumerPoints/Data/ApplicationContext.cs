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

            modelBuilder.Entity<SpendingMarker>()
                .HasData(
                    new SpendingMarker
                    {
                        Id=1,
                        LastSpentDate=DateTime.MinValue,
                        LastWasPartiallySpent=false,
                        Remainder=0
                    });
        }

        public DbSet<SpendingMarker> SpendingMarkers { get; set; }
        public DbSet<PayerPoints> PayerPoints { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
