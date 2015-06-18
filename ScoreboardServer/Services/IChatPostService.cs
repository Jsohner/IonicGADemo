using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScoreboardServer.Models;

namespace ScoreboardServer.Services
{
    interface IChatPostService
    {
        void PostToSlack(string textToPost, Player submittingPlayer);
    }

    class FakeChatPostService : IChatPostService
    {
        public void PostToSlack(string textToPost, Player submittingPlayer)
        {
            return;
        }
    }
}
