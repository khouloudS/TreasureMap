using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreasureMap.Models
{
    public class Adventurer
    {
        public string Name { get; set; }
        public int Col { get; set; }
        public int Row { get; set; }
        public string Orientation { get; set; }
        public string Path { get; set; }
        public int TotalTreasure { get; set; }
    }

}
