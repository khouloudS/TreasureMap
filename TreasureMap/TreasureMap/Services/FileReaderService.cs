using TreasureMap.Constant;
using TreasureMap.Interfaces;
using TreasureMap.Models;
using TreasureMap.Services;

namespace TreasureMap.Data
{
    public class FileReaderService : IFileReader
    {
        public readonly IMapService _mapService;
        public FileReaderService(IMapService mapService)
        {
            _mapService = mapService;
        }
        public GameMap ReadFile(string filePath)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    Cell[,] map = null;
                    int maxPath = 0;

                    List<Adventurer> adventurersList = new List<Adventurer>();
                    int col = 0;
                    int row = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();

                        if (string.IsNullOrEmpty(line) || line[0] == '#')
                            continue;
                        string[] parts = line.Split(" - ");

                        switch (parts[0])
                        {
                            case "C":
                                col = int.Parse(parts[1]);
                                row = int.Parse(parts[2]);
                                map = _mapService.InitializeMap(row, col);
                                break;
                            case "M":
                                col = int.Parse(parts[1]);
                                row = int.Parse(parts[2]);
                                map[row, col] = new Cell(CellType.Mountain);
                                break;

                            case "T":
                                col = int.Parse(parts[1]);
                                row = int.Parse(parts[2]);
                                int treasureCount = int.Parse(parts[3]);
                                map[row, col] = new Cell(CellType.Treasure, treasureCount);
                                break;
                            case "A":
                                var adventurer = new Adventurer
                                {
                                    Name = parts[1],
                                    Col = int.Parse(parts[2]),
                                    Row = int.Parse(parts[3]),
                                    Orientation = parts[4],
                                    Path = parts[5]
                                };
                                if (map != null
                                    && !map[adventurer.Row, adventurer.Col].HasAdventurer
                                    && !string.Equals(map[adventurer.Row, adventurer.Col].Type, CellType.Mountain))
                                {
                                    map[adventurer.Row, adventurer.Col].HasAdventurer = true;
                                    Cell currentCell = map[adventurer.Row, adventurer.Col];
                                    if (string.Equals(currentCell.Type, CellType.Treasure) && currentCell.TreasureCount > 0)
                                    {
                                        adventurer.TotalTreasure++;   // Adventurer collects one treasure
                                        currentCell.TreasureCount--;  // Cell loses one treasure
                                    }
                                    if (maxPath < adventurer.Path.Length) maxPath = adventurer.Path.Length;
                                    adventurersList.Add(adventurer);
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    return new GameMap(map, adventurersList, maxPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                return null;
            }
        }
    }

}
