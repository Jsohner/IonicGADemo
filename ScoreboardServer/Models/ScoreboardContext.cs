using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Web;

namespace ScoreboardServer.Models
{
    public class ScoreboardContext : DbContext
    {
        public ScoreboardContext() : base("DefaultConnection") {}

        public DbSet<Player> Players { get; set; }
        public DbSet<Vote> Votes { get; set; }
    }
}