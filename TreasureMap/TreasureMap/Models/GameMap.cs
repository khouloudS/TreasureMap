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
        public GameMap(Cell[,] maps, List<Adventurer> adventurers)
        {
            Map = maps;
            Adventurers = adventurers;
        }
    }

}
