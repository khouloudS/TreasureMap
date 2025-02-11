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
            Console.WriteLine($"{gameData.MaxPath}");

            while (gameData.MaxPath != 0)
            {
                foreach (var adventurer in gameData.Adventurers)
                {
                    if (adventurer.Path.Length > 0)
                    {
                        var move = adventurer.Path[0].ToString();
                        adventurer.Path= adventurer.Path.Substring(1); // the first char was already used
                        if (move == "A")
                        {
                            if (IsValidMovement(adventurer, gameData.Map))
                            {
                                UpdateCells(adventurer, gameData.Map);
                            }
                        }
                        else
                        {
                            adventurer.Orientation = directionLookup[adventurer.Orientation][move];
                        }
                    }
                }

                gameData.MaxPath--;
            }

            foreach (var adventurer in gameData.Adventurers)
            {
                Console.WriteLine($"A - {adventurer.Name} - {adventurer.Col} - {adventurer.Row} - {adventurer.Orientation} - {adventurer.TotalTreasure}");
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


    static bool IsValidMovement(Adventurer adventurer, Cell[,] map)
    {
        // check if we don't go outside the map

        var (nextCol, nextRow) = GetNextCell(adventurer.Orientation, adventurer.Col, adventurer.Row);

        if (nextCol < 0 || nextCol >= map.GetLength(1) || nextRow < 0 || nextRow >= map.GetLength(0)) return false;
        // check we don't hit a mountain 
        if (map[nextRow, nextCol].Type == CellType.Mountain) return false;
        // check we move to a cell that has an adventurer
        if (map[nextRow, nextCol].HasAdventurer) return false;
        return true;
    }

    static (int, int) GetNextCell(string orientation, int col, int row)
    {
        int nextCol = 0;
        int nextRow = 0;

        if (orientation == "N")
        {
            nextCol = col;
            nextRow = row-1;
        }
        else if (orientation == "S")
        {
            nextCol = col;
            nextRow = row+1;
        }
        else if (orientation == "E")
        {
            nextCol = col+1;
            nextRow = row;
        }
        else
        {
            nextCol = col-1;
            nextRow = row;
        }
        return (nextCol, nextRow);
    }

    static void UpdateCells(Adventurer adventurer, Cell[,] map)
    {
        // Update current cell state
        map[adventurer.Row, adventurer.Col].HasAdventurer = false;

        var (nextCol, nextRow) = GetNextCell(adventurer.Orientation, adventurer.Col, adventurer.Row);
        // Update destination cell state
        map[nextRow, nextCol].HasAdventurer = true;

        // In case of treasure cell, update adventurer and cell
        if (map[nextRow, nextCol].Type == CellType.Treasure && map[nextRow, nextCol].TreasureCount > 0)
        {
            adventurer.TotalTreasure++;
            map[nextRow, nextCol].TreasureCount--;
        }
        // update adventurer position
        adventurer.Row = nextRow;
        adventurer.Col = nextCol;
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
