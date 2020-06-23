using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using CargoSenderLibrary.Models;

namespace CargoSenderLibrary.DB
{
    public class DBContext : DbContext
    {
        public DBContext() : base("DbConnectionString") { }

        public DbSet<CargoModel> ParsData { get; set; }
    }
}
