
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Cosmos;
using Microsoft.Identity.Client;

namespace MVC.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cosmos DB requires specifying a container
            modelBuilder.Entity<Post>()
                .ToContainer("Posts") // Set container name instead of table
                .HasPartitionKey(p => p.Id); // Define partition key (consider User or Category instead for better partitioning)

            modelBuilder.Entity<Comment>()
                .ToContainer("Comments")
                .HasPartitionKey(c => c.PostId); // Group comments by PostId
        }

        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public async Task SeedDataAsync()
        {
            if (await Posts.CountAsync() == 0)
            {
                Posts.AddRange(new List<Post>
                {
                    new Post { Id = "-2", Title = "Meme2", Category = Category.Humour, User = "Guillaume Routhier", Image = File.ReadAllBytes(AppContext.BaseDirectory + "Meme2.png") },
                    new Post { Id = "-3", Title = "Meme3", Category = Category.Humour, User = "Guillaume R.", Image = File.ReadAllBytes(AppContext.BaseDirectory + "Meme3.png") },
                    new Post { Id = "-4", Title = "Meme4", Category = Category.Humour, User = "Guillaume R", Image = File.ReadAllBytes(AppContext.BaseDirectory + "Meme4.jpg") }
                });

                await SaveChangesAsync();
            }
        }
    }
}
