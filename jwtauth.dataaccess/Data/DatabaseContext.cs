using Microsoft.EntityFrameworkCore;

namespace jwtauth.dataaccess.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(){}
        public DatabaseContext(DbContextOptions options) : base(options){}
    }
}