using System;
using System.Collections;
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
   
    public class JokeGenerator : IJokeGenerator
    {
        public string GenerateJoke(Player submittingPlayer, string submittedText)
        {
            //"\"" + yellText.toUpperCase() + "!!!\" " + yellVerb + " " + player.FirstName +" "+ yellEnding
            return string.Format("\"{0}!!!\" {1} {2} {3}", submittedText.ToUpper(), GetYellVerb(), submittingPlayer.FirstName, GetYellEnding(submittingPlayer.FirstName))
            ;
        }

        private string GetYellEnding(string firstName)
        {
            var possesiveGenderPronoun = IsFemale(firstName) ? "her" : "his";
            var genderPronoun = IsFemale(firstName) ? "she" : "he";
            
            var endings = new List<string>()
            {
                "like a howling banshee.",
                "as " + genderPronoun + " loosened " + possesiveGenderPronoun + " necktie.",
                "at Jared.",
                "in a fit of rage.",
                "in a thick Australian accent.",
                "just before jumping out of the boat.",
                "as " + genderPronoun + " breathed " + possesiveGenderPronoun + " last.",
                "into a supermarket.",
                "from under " + possesiveGenderPronoun + " bed.",
                "as " + genderPronoun + " dumped several large salt-water fish from " + possesiveGenderPronoun + " briefcase.",
                "enthusiastically",
                "with a hollow tone of remorse",
                "before " + genderPronoun + " realized " + genderPronoun + " was dreaming.",
                "before " + genderPronoun + " started to weep.",
                "looking Voldemort right in the eyes"

            };
            var randomGenerator = new Random(Guid.NewGuid().GetHashCode());
            var randomIndex = randomGenerator.Next(0, endings.Count - 1);
            return endings[randomIndex];
        }

        private bool IsFemale(string firstName)
        {
            firstName = firstName.ToLower();
            return (firstName == "katherine" || firstName == "kat");
        }


        private string GetYellVerb()
        {
            var verbs = new List<string>()
            {
                 "shouted",
                 "screamed",
                 "roared",
                 "shrieked",
                 "groaned",
                 "sang",
                 "muttered",
                 "coughed",
                 "danced",
                 "mouthed",
                 "chirped"

            };

            var randomGenerator = new Random(Guid.NewGuid().GetHashCode());
            var randomIndex = randomGenerator.Next(0, verbs.Count - 1);
            return verbs[randomIndex];

        }
    }
}