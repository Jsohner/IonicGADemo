using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls.WebParts;
using ScoreboardServer.Models;

namespace ScoreboardServer.Services
{
    public class StubJokeGenerator : IJokeGenerator
    {
        public string GenerateJoke(Player submittingPlayer, string submittedText)
        {
            return String.Format("{0} {1} says {2}", submittingPlayer.FirstName, submittingPlayer.LastName, submittedText);

        }
    }
}