namespace TodoApi.Data
{
    using Microsoft.EntityFrameworkCore;
    using TodoApi.Models;

    public class DataContext: DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Configuration.GetConnectionString("TodoApiDatabase"));
        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
