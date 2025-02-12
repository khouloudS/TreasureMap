using TreasureMap.Models;

namespace TreasureMap.Interfaces
{
    public interface IMapService
    {
        Cell[,] InitializeMap(int rows, int cols);
    }
}
