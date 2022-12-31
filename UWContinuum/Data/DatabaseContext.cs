using Microsoft.EntityFrameworkCore;
using UWContinuum.Models;

namespace UWContinuum.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<WebEmailsModel>? WebEmails { get; set; }

    }
}
