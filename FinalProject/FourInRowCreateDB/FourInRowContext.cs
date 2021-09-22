using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourInRowCreateDB {
    class FourInRowContext : DbContext {

        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=FourInRowDB;AttachDbFilename=C:\fourinrow\databases\fourinrow_Ayman_Bassam.mdf;Integrated Security=True";
        protected override void OnConfiguring(DbContextOptionsBuilder builder) {
            base.OnConfiguring(builder);
            builder.UseSqlServer(connectionString);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }

    }
}
