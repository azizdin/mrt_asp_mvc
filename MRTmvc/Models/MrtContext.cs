using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MRTmvc.Models
{
    public class MrtContext : DbContext
    {
       
        public DbSet<Register> Registers { get; set; }
       // public DbSet <Login> Logins { get; set; }
        public DbSet<Route> Routes { get; set; }

        //public System.Data.Entity.DbSet<MRTmvc.Models.Route> Routes { get; set; }
    }
}