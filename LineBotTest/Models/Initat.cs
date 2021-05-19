using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace LineBotTest.Models
{
    public class DbCoNtext : DbContext
    {
        public DbCoNtext() : base("Database1")
        { }
        public DbSet<Account> Accounts { get; set; }

    }
}