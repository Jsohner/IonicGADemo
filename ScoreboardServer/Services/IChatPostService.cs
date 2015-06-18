using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardServer.Services
{
    interface IChatPostService
    {
        void PostToSlack(string textToPost);
    }

    class FakeChatPostService : IChatPostService
    {
        public void PostToSlack(string textToPost)
        {
            return;
        }
    }
}
