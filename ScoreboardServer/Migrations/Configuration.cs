namespace ScoreboardServer.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ScoreboardServer.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ScoreboardServer.Models.ScoreboardContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ScoreboardContext context)
        {
            // This method will be called after migrating to the latest version.

           //   You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            
            context.Players.AddOrUpdate(
              p => p.FirstName,
              new Player { FirstName = "MikeTron"},
              new Player { FirstName = "John"},
              new Player { FirstName = "Alex"}
            );
            
        }
    }
}
