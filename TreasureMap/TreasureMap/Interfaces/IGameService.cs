using TreasureMap.Models;

namespace TreasureMap.Interfaces
{
    public interface IGameService
    {
        GameMap ReadFile(string filePath);
        void PlayGame(GameMap gameData);
        void DisplayResults(GameMap gameMap, string outputPath);
        void StartGame();
    }
}
