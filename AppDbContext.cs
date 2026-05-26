using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{

      public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
{
}
    public DbSet<User> Users { get; set; }
    public DbSet<Document> Documents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Geliştirme aşaması için lokal bir SQL Server bağlantısı kullanıyoruz. 
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MetinAnaliziDB;Trusted_Connection=True;");
    }
}