using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using ScoreboardServer.Models;

namespace ScoreboardServer.Services
{
    public class ChatPostService : IChatPostService
    {
        public void PostToSlack(string textToPost, Player submittingPlayer)
        {
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                   { "token", "xoxp-6261658387-6261658403-6272659622-7f1736" },  //TODO needs to be changed to desired token
                   { "channel", "C067PK2U8" }, //TODO  needs to be changed to desired channel
                   { "text", textToPost},
                   { "username", submittingPlayer.FirstName},
                    { "icon_emoji", string.Format(":{0}:", submittingPlayer.FirstName.ToLower())}
                   

                };

                var content = new FormUrlEncodedContent(values);

                var response = client.PostAsync("https://slack.com/api/chat.postMessage", content);

                var responseString = response.Result.Content.ReadAsStringAsync();
            }
        }
    }
}