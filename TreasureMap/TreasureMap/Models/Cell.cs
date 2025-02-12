using TreasureMap.Constant;

namespace TreasureMap.Models
{
    public class Cell
    {
        public CellType Type { get; set; }
        public int TreasureCount { get; set; }
        public bool HasAdventurer { get; set; }
       

        public Cell(CellType type = CellType.Default, int treasureCount = 0, bool hasAdventurer = false)
        {
            Type = type;
            TreasureCount = treasureCount;
            HasAdventurer = hasAdventurer;
        }
    }

}
