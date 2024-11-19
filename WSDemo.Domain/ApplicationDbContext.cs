using Microsoft.EntityFrameworkCore;

namespace WSDemo.Domain;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<FolderItem> FolderItems { get; set; }
}
