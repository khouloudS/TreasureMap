using TreasureMap.Models;

namespace TreasureMap.Interfaces
{
    public interface IFileReader
    {
        GameMap ReadFile(string filePath);
    }

}
