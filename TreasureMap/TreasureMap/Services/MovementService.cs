using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreasureMap.Constant;
using TreasureMap.Interfaces;
using TreasureMap.Models;

namespace TreasureMap.Services
{
    public class MovementService : IMovementService
    {
        public bool IsValidMovement(Adventurer adventurer, Cell[,] map)
        {
            var (nextCol, nextRow) = GetNextCell(adventurer.Orientation, adventurer.Col, adventurer.Row);
                   // Check if we don't go outside the map
            return nextCol >= 0 && nextCol < map.GetLength(1) &&  
                   nextRow >= 0 && nextRow < map.GetLength(0) &&
                   // Check we don't hit a mountain 
                   map[nextRow, nextCol].Type != CellType.Mountain &&
                   // Check we move to a cell that does not have an adventurer
                   !map[nextRow, nextCol].HasAdventurer;        

        }

        public (int, int) GetNextCell(string orientation, int col, int row)
        {
            return orientation switch
            {
                "N" => (col, row - 1),
                "S" => (col, row + 1),
                "E" => (col + 1, row),
                "O" => (col - 1, row),
                _ => (col, row)
            };
        }

        public void UpdateCells(Adventurer adventurer, Cell[,] map)
        {
            map[adventurer.Row, adventurer.Col].HasAdventurer = false;

            var (nextCol, nextRow) = GetNextCell(adventurer.Orientation, adventurer.Col, adventurer.Row);
            map[nextRow, nextCol].HasAdventurer = true;

            if (map[nextRow, nextCol].Type == CellType.Treasure && map[nextRow, nextCol].TreasureCount > 0)
            {
                adventurer.TotalTreasure++;
                map[nextRow, nextCol].TreasureCount--;
            }

            adventurer.Row = nextRow;
            adventurer.Col = nextCol;
        }
    }
}
