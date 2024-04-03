using Microsoft.EntityFrameworkCore;

namespace EBid.Models
{
    public class EBidDbContext:DbContext
    {
        public EBidDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Client> clients {  get; set; }
        public DbSet<BankAccount> bankAccounts { get; set; }
        public DbSet<Customer> customers { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<ProductDetails> productsDetails { get; set; }
        public DbSet<Photo> photos { get; set; }
        public DbSet<Bid> bids { get; set; }
        public DbSet<Auction> auctions { get; set; }
        public DbSet<User> users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasOne(e => e.BankAccount)
                .WithOne(e => e.Client)
                .HasForeignKey<BankAccount>(e => e.ClientId);

            modelBuilder.Entity<BankAccount>()
                .HasOne(e => e.Client)
                .WithOne(e => e.BankAccount)
                .HasForeignKey<BankAccount>(e => e.ClientId);

            modelBuilder.Entity<Auction>()
                .HasOne(e => e.Product) 
                .WithOne(e => e.Auction)
                .HasForeignKey<Auction>(e => e.ProductId);


            modelBuilder.Entity<Product>()
                .HasOne(e => e.Client)
                .WithMany(e => e.Products)
                .HasForeignKey(e => e.ClientId);

            modelBuilder.Entity<Client>()
                .HasMany(e => e.Products)
                .WithOne(e => e.Client)
                .HasForeignKey(e => e.ClientId)
                .OnDelete(DeleteBehavior.ClientNoAction);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Bids)
                .WithOne(e => e.Product)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Auction>()
                .HasOne(e => e.Client)
                .WithMany(e => e.Auctions)
                .HasForeignKey(e => e.ClientId);


        }

    }
}
