using Microsoft.EntityFrameworkCore;
using filtro.Models;

namespace filtro.Data
{
    public class  DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options){}
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Employ> Employees { get; set; }
        
    }
}