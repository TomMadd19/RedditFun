using Microsoft.EntityFrameworkCore;
using RedditFun.Services.Domain.Models;

namespace RedditFun.Services.Domain.Contexts
{
    public class RedditDataContext : DbContext
    {
        public DbSet<RedditData> RedditData { get; set; }

        public RedditDataContext(DbContextOptions<RedditDataContext> options) : base(options)
        {
        }

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			// in memory database used for simplicity, change to a real db for production applications
			options.UseInMemoryDatabase("RedditDb");
		}

		protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RedditData>().ToTable("RedditData");
            builder.Entity<RedditData>().HasKey(d => d.FullName);
            builder.Entity<RedditData>().Property(d => d.Id).IsRequired();
            builder.Entity<RedditData>().Property(d => d.SubReddit).IsRequired();
			builder.Entity<RedditData>().Property(d => d.FullName).IsRequired().HasMaxLength(50);
			builder.Entity<RedditData>().Property(d => d.Title).IsRequired(); 
            builder.Entity<RedditData>().Property(d => d.UpVotes).IsRequired();
            builder.Entity<RedditData>().Property(d => d.DownVotes).IsRequired();
            builder.Entity<RedditData>().Property(d => d.PostedBy).IsRequired().HasMaxLength(50);
            builder.Entity<RedditData>().Property(d => d.Score).IsRequired();
			
		}
    }
}
