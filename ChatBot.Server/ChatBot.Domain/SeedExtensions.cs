using ChatBot.Domain.Models;
using Microsoft.EntityFrameworkCore;

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
        }
    }
}