using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DallalCore.Models;

namespace DallalCore.Data
{
    public class DallalCoreContext : DbContext
    {
        public DallalCoreContext (DbContextOptions<DallalCoreContext> options)
            : base(options)
        {
        }

        public DbSet<DallalCore.Models.Product> Product { get; set; }

        public DbSet<DallalCore.Models.User> User { get; set; }

        public DbSet<DallalCore.Models.Address> Address { get; set; }

        public DbSet<DallalCore.Models.Category> Category { get; set; }

        public DbSet<DallalCore.Models.Report> Report { get; set; }

        public DbSet<DallalCore.Models.Rate> Rate { get; set; }
    }
}
