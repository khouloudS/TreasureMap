using System;
using System.IO;
using TreasureMap.Constant;
using TreasureMap.Models;

class Program
{

    static Dictionary<string, Dictionary<string, string>> directionLookup = new Dictionary<string, Dictionary<string, string>>()
    {
        { "S", new Dictionary<string, string> { { "G", "E" }, { "D", "O" } } }, // Sud + gauche = Est, Sud + droite = Ouest
        { "E", new Dictionary<string, string> { { "G", "N" }, { "D", "S" } } }, // Est + gauche = Nord, Est + droite = Sud
        { "N", new Dictionary<string, string> { { "G", "O" }, { "D", "E" } } }, // Nord + gauche = Ouest, Nord + droite = Est
        { "O", new Dictionary<string, string> { { "G", "S" }, { "D", "N" } } }  // Ouest + gauche = Sud, Ouest + droite = Nord
    };


    static void Main(string[] args)
    {
        string filePath = "";

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: The file '{filePath}' does not exist.");
            return;
        }

        GameMap gameData = ReadFile(filePath);

        if (gameData != null)
        {
            DisplayMap(gameData.Map);

            foreach (var adventurer in gameData.Adventurers)
            {
                Console.WriteLine($"{adventurer.Name} , ({adventurer.Row}, {adventurer.Col}), {adventurer.TotalTreasure}");
            }
        }
    }

    /// <summary>
    /// Reads the file and processes the lines to construct the map.
    /// </summary>
    static GameMap ReadFile(string filePath)
    {
        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                Cell[,] map = null;
                
                List<Adventurer> adventurersList = new List<Adventurer>();
                int col = 0;
                int row = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    var t = line[0];
                    if (line[0] == '#') continue;
                    string[] parts = line.Split(" - ");

                    switch (parts[0])
                    {
                        case "C":
                            col = int.Parse(parts[1]);
                            row = int.Parse(parts[2]);
                            map = InitializeMap(row, col);
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
                                adventurersList.Add(adventurer);
                            }
                            break;
                        default:
                            break;
                    }
                }

                return new GameMap(map, adventurersList);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Initializes a map with Default cells.
    /// </summary>
    static Cell[,] InitializeMap(int rows, int cols)
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


    /// <summary>
    /// Checks if there is at least one adventurer in the map.
    /// </summary>
    static bool HasAdventurer(Cell[,] map)
    {
        foreach (var cell in map)
        {
            if (cell.HasAdventurer)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Displays the map.
    /// </summary>
    static void DisplayMap(Cell[,] map)
    {
        for (int row = 0; row < map.GetLength(0); row++)
        {
            for (int col = 0; col < map.GetLength(1); col++)
            {
                Console.Write($"{map[row, col]} ");
            }
            Console.WriteLine();
        }
    }
    
}
