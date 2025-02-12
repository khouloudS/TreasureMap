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
    public class MapService: IMapService
    {
        public Cell[,] InitializeMap(int rows, int cols)
        {
            Cell[,] map = new Cell[rows, cols];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    map[row, col] = new Cell();
                }
            }

            return map;
        }
    }
}
