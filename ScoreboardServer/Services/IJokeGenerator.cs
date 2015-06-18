using ScoreboardServer.Models;

namespace ScoreboardServer.Services
{
    public interface IJokeGenerator
    {
        string GenerateJoke(Player submittingPlayer, string submittedText);
    }
}