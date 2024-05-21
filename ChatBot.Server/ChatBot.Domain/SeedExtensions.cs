using ChatBot.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace ChatBot.Domain
{
    public static partial class SeedExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            SeedVendors(modelBuilder);
        }

        private static void SeedVendors(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Test>()
                .HasData(
                    new Test
                    {
                        Id = 1,
                        Field = "Test1"
                    },
                    new Test
                    {
                        Id = 2,
                        Field = "Test2"
                    },
                    new Test
                    {
                        Id = 3,
                        Field = "Test3"
                    });
        }
    }
}