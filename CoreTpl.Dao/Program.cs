using CoreTpl.Dao.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CoreTpl.Dao
{
    class Program
    {
        public static void Main(string[] args) 
        {
            TplDbContext dc = new DesignTimeDbContextFactory().CreateDbContext(args);
        }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TplDbContext>
    {
        public TplDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<TplDbContext>();
            builder.UseNpgsql("Host=localhost;Database=CoreTpl;Username=postgres;Password=p@ssw0rd");
            return new TplDbContext(builder.Options);
        }
    }

}
