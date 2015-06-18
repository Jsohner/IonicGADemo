using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ScoreboardServer.Models;
using ScoreboardServer.Services;

namespace ScoreboardServer.Controllers
{
    public class RegisterScoreController : ApiController
    {
        private IVotePersister _votePersister;
        private ScoreboardContext _ScoreboardContext;

        public RegisterScoreController()
        {
            _ScoreboardContext = new ScoreboardContext();
            _votePersister = new VotePersister(_ScoreboardContext);
        }

        [Route("Vote/{playerId}")]
        public string Get(int playerId)
        {
            var player = _votePersister.FindPlayerBy(playerId);
            return _votePersister.GetCountBy(player).ToString();
        }

        [Route("Vote/{playerId}")]
        public void Post(int playerId)
        {
            var player = _votePersister.FindPlayerBy(playerId);
            _votePersister.PersistVote(player);

            var cps = new ChatPostService();     //TODO proper dependency-injection
            var jg = new JokeGenerator();
            cps.PostToSlack(jg.GenerateJoke(player, "point for me"), player);      //TODO currently it will post to slack everytime the button is clicked
                                                                                   //I know the original intent was to periodically show a scoreboard for this, but we didn't have time to make that happen yet
                                                                                   //comment out the PostToSlack call if you just want to add to the score

        }

        [Route("Word/{playerId}/{yellText}")]
        public void Post(int playerId, string yellText)
        {
            var player = _votePersister.FindPlayerBy(playerId);
            _votePersister.PersistVote(player);

            var cps = new ChatPostService();
            var jg = new JokeGenerator();
            cps.PostToSlack(jg.GenerateJoke(player, "point for me"), player);


        }

        [Route("Players/")]
        public List<Player> GetPlayers()
        {
            var players =  _votePersister.GetAllPlayers();

           return players;
        }

        [Route("Scoreboard/")]
        public List<Player> GetScoreboard()
        {
            var players = _votePersister.GetAllPlayers();

            players.ForEach(p =>
            {
                var validVotes = p.Votes.Where(v => v.CreatedAt >= DateTime.UtcNow.AddMinutes(-5)).ToList();
                p.Votes = (ICollection<Vote>) validVotes;
            });

            return players;
        }
         
    }
}
