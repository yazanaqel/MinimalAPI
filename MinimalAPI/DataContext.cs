namespace MinimalAPI;

public class DataContext:DbContext
{
    public DataContext(DbContextOptions<DataContext>option):base(option)
    {
        
    }

    public DbSet<Book> Books { get; set; }
}
