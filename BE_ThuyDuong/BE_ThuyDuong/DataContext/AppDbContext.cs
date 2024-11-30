using BE_ThuyDuong.Entities;
using Microsoft.EntityFrameworkCore;
using BE_ThuyDuong.Entities;
using System.Data;

namespace BE_ThuyDuong.DataContext
{
    public class AppDbContext:DbContext
    {
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Card> cards { get; set; }
        public DbSet<ComfirmEmail> comfirmEmails { get; set; }
        public DbSet<HistorryPay> historryPays { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<ProductType> productTypes { get; set; }
        public DbSet<RefreshToken> refreshTokens { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<Trademark> trademarks { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Bill> bills { get; set; }
        public DbSet<Feedback> feedbacks { get; set; }
    


    }
}
