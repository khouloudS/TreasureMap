using TreasureMap.Models;

namespace TreasureMap.Interfaces
{
    public interface IMovementService
    {
        bool IsValidMovement(Adventurer adventurer, Cell[,] map);
        (int, int) GetNextCell(string orientation, int col, int row);
        void UpdateCells(Adventurer adventurer, Cell[,] map);
    }
}
