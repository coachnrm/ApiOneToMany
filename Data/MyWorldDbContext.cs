using Microsoft.EntityFrameworkCore;
using ApiCrud.Data.Entities;

namespace ApiCrud.Data
{
    public class MyWorldDbContext : DbContext
    {
        public MyWorldDbContext(DbContextOptions<MyWorldDbContext> options):base(options)
        {

        }

        public DbSet<Customer> Customer {get; set;}
        public DbSet<CustomerAddresses> CustomerAddresses {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerAddresses>()
            .HasOne(_ => _.Customer)
            .WithMany(_ => _.CustomerAddresses)
            .HasForeignKey(_ => _.CustomerId);
        }
    }
}