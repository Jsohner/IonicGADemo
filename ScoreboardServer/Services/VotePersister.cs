using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.EnterpriseServices;
using System.Linq;
using ScoreboardServer.Models;

namespace ScoreboardServer.Services
{
    public class VotePersister : IVotePersister
    {
        private ScoreboardContext _scoreboardContext;

        public VotePersister(ScoreboardContext scoreboardContext)
        {
            _scoreboardContext = scoreboardContext;
        }

        public Player FindPlayerBy(int playerId)
        {
            return _scoreboardContext.Players.Find(playerId);
        }

        public void PersistVote(Player player)
        {
            _scoreboardContext.Votes.Add(new Vote
            {
                Player = player,
                CreatedAt = DateTime.UtcNow // TODO Figure out how to do this with EF once you have internet access
            });

            _scoreboardContext.SaveChanges();
        }

        public int GetCountBy(Player player)
        {
            return _scoreboardContext.Votes.Count(x => x.Player.PlayerId == player.PlayerId);
        }

        public List<Player> GetAllPlayers()
        {
            _scoreboardContext.Configuration.ProxyCreationEnabled = false;
            _scoreboardContext.Configuration.LazyLoadingEnabled = false;

            var playersQuery = _scoreboardContext.Players.Include(p => p.Votes);

            return playersQuery.ToList();
        }
    }
}