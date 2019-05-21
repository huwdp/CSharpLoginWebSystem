using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LoginExampleWebApp
{
    // https://docs.microsoft.com/en-us/ef/core/get-started/netcore/new-db-sqlite


    public class DatabaseContext : DbContext
    {
        // https://docs.microsoft.com/en-us/ef/core/get-started/netcore/new-db-sqlite

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { }

        public DbSet<LoginExample.WebApp.DbModels.UserModel> Users { get; set; }
    }

}