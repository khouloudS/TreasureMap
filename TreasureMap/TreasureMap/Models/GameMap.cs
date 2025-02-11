using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreasureMap.Models
{
    public class GameMap
    {
        public Cell[,] Map { get; set; }
        public List<Adventurer> Adventurers { get; set; }
        public int MaxPath { get; set; }
        public GameMap(Cell[,] maps, List<Adventurer> adventurers, int maxPath)
        {
            Map = maps;
            Adventurers = adventurers;
            MaxPath = maxPath;
        }
    }

}
