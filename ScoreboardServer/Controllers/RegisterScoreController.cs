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


            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                   { "token", "xoxp-6261658387-6261658403-6272659622-7f1736" },
                   { "channel", "C067PK2U8" },
                   { "text", "this is coming from the c# ```And this is in code```"},
                   { "username", player.FirstName},
                   { "icon_url", "http://store.bbcomcdn.com/images/store/prodimage/prod_prod910018/image_prodprod910018_largeImage_X_450_white.jpg"},
                   { "mrkdwn", "true"},
                   { "mrkdwn_in", "text"}

                };

                var content = new FormUrlEncodedContent(values);

                var response =  client.PostAsync("https://slack.com/api/chat.postMessage", content);

                var responseString = response.Result.Content.ReadAsStringAsync();
            }

        }

        [Route("Word/{playerId}")]
        public void Post(int playerId, string yellText)
        {
            var player = _votePersister.FindPlayerBy(playerId);
            _votePersister.PersistVote(player);


            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                   { "token", "xoxp-6261658387-6261658403-6272659622-7f1736" },
                   { "channel", "C067PK2U8" },
                   { "text", yellText},
                   { "username", player.FirstName},
                   { "icon_url", ":" + yellText + ":"},
                   
                };

                var content = new FormUrlEncodedContent(values);

                var response = client.PostAsync("https://slack.com/api/chat.postMessage", content);

                var responseString = response.Result.Content.ReadAsStringAsync();
            }

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
