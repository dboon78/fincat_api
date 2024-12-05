using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext:IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
        :base(dbContextOptions){
            
        }
       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure composite key for Portfolio
            modelBuilder.Entity<Portfolio>()
                .HasKey(p => p.PortfolioId);

            // Configure primary key for Holding
            modelBuilder.Entity<Holding>()
                .HasKey(h => h.HoldingId);

            modelBuilder.Entity<HistoricPrice>()
                .HasKey(hp=>hp.Id);
                
            modelBuilder.Entity<PriceChange>()
                .HasKey(hp=>hp.PriceChangeId);
            modelBuilder.Entity<StockExchange>()
                .HasKey(se=>se.ExchangeId);
            modelBuilder.Entity<CoinPaprikaCoin>()
                .HasKey(coin=>coin.id);
            modelBuilder.Entity<Currency>()
                .HasKey(c=>c.Code);
                
            // If a stock is deleted, delete all the comments that go with it
            modelBuilder.Entity<Stock>()
                .HasMany(s => s.Comments)
                .WithOne(c => c.Stock)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<StockExchange>()
                .HasMany(s => s.Stocks)
                .WithOne(se=>se.Exchange)
                .HasForeignKey(x=>x.ExchangeId)
                .HasConstraintName("FK_Stocks_StockExchange_ExchangeId");

            modelBuilder.Entity<Stock>()
                .HasOne(x=>x.Exchange)
                .WithMany(s => s.Stocks)
                .HasForeignKey(x=>x.ExchangeId)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<Stock>()
                .HasMany(s=>s.HistoricPrices)
                .WithOne(hp=>hp.Stock)
                .HasForeignKey(x=>x.StockId)
                .OnDelete(DeleteBehavior.Cascade);

            // If a portfolio is deleted, delete all the holdings that go with it
            modelBuilder.Entity<Portfolio>()
                .HasMany(p => p.Holdings)
                .WithOne(h => h.Portfolio)
                .HasForeignKey(x=>x.PortfolioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the Stock-Holding relationship
            modelBuilder.Entity<Stock>()
                .HasMany(s => s.Holdings)
                .WithOne(h => h.Stock)
                .HasForeignKey(h => h.StockId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the one-to-one relationship between Stock and PriceChange
            modelBuilder.Entity<Stock>()
                .HasOne(s => s.PriceChange)
                .WithOne(pc => pc.Stock)
                .HasForeignKey<PriceChange>(pc => pc.StockId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Stock>()
                .HasMany(s=>s.HistoricPrices)
                .WithOne(hp=>hp.Stock)
                .HasForeignKey(hp=>hp.StockId)
                .OnDelete(DeleteBehavior.Cascade);

            // modelBuilder.Entity<HistoricPrice>()
            //     .HasOne(hp=>hp.Stock)
            //     .WithMany(s=>s.HistoricPrices)
            //     .HasForeignKey(s=>s.StockId)
            //     .OnDelete(DeleteBehavior.NoAction);

            // Configure foreign key for Portfolio in Holding
            modelBuilder.Entity<Holding>()
                .HasOne(h => h.Portfolio)
                .WithMany(p => p.Holdings)
                .HasForeignKey(h => h.PortfolioId)
                .OnDelete(DeleteBehavior.Cascade);

                
            modelBuilder.Entity<AppUser>(entity => { 
                // Configure the Currency relationship 
                entity.Property(e => e.CurrencyCode).HasDefaultValue("USD");
                entity.HasOne(u => u.UserCurrency) 
                    .WithMany() 
                    .HasForeignKey(u => u.CurrencyCode) 
                    .HasPrincipalKey(c => c.Code)
                    .OnDelete(DeleteBehavior.Cascade); 
                    // If you have other configurations for AppUser, include them here 
                }
            );

            modelBuilder.Entity<Portfolio>()
                .HasOne(p => p.AppUser)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.AppUserId);

            modelBuilder.Entity<Holding>()
                .HasOne(h => h.Stock)
                .WithMany(s => s.Holdings)
                .HasForeignKey(h => h.StockId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HistoricPrice>()
                .HasIndex(h => new { h.StockId,h.Date })
                .HasDatabaseName("IX_HistoricPrice_Exchange_Symbol_Date")
                .IsUnique();

            // Seed roles
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
            };
            
            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }



        public DbSet<Stock> Stocks { get;set;}
        public DbSet<Comment> Comments { get;set;}
        public DbSet<Portfolio> Portfolios{get;set;}
        public DbSet<Holding> Holdings{ get;set;}

        public DbSet<PriceChange> PriceChanges{ get;set;}
        public DbSet<HistoricPrice> HistoricPrices { get; set; }
        public DbSet<StockExchange> StockExchanges { get; set;}

        public DbSet<CoinPaprikaCoin> CoinPaprikaCoins{ get; set; }
        
        public DbSet<Currency> Currencies{ get; set; }
    }
}