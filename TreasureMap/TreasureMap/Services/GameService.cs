using TreasureMap.Constant;
using TreasureMap.Helper;
using TreasureMap.Interfaces;
using TreasureMap.Models;

namespace TreasureMap.Services
{

    public class GameService: IGameService
    {
        private readonly IMovementService _movementService;
        private readonly IMapService _mapService;

        public GameService(IMovementService movementService, IMapService mapService)
        {
            _movementService = movementService;
            _mapService = mapService;
        }

        public void StartGame()
        {
            Console.Write("Enter the path to the map file: ");
            string filePath = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(filePath))
            {
                Console.WriteLine("Invalid file path.");
                return;
            }

            GameMap gameData = ReadFile(filePath);
            if (gameData == null)
            {
                Console.WriteLine("Error loading the game map.");
                return;
            }

            PlayGame(gameData);
            string outputPath = "game_results.txt";
            DisplayResults(gameData, outputPath);
        }

        public GameMap ReadFile(string filePath)
        {
            try
            {
                using StreamReader reader = new(filePath);
                string line;
                Cell[,] map = null;
                int maxPath = 0;
                List<Adventurer> adventurersList = new();

                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (string.IsNullOrEmpty(line) || line[0] == '#') continue;

                    string[] parts = line.Split(" - ");
                    switch (parts[0])
                    {
                        case "C":
                            map = _mapService.InitializeMap(int.Parse(parts[2]), int.Parse(parts[1]));
                            break;
                        case "M":
                            map[int.Parse(parts[2]), int.Parse(parts[1])] = new Cell(CellType.Mountain);
                            break;
                        case "T":
                            map[int.Parse(parts[2]), int.Parse(parts[1])] = new Cell(CellType.Treasure, int.Parse(parts[3]));
                            break;
                        case "A":
                            Adventurer adventurer = new()
                            {
                                Name = parts[1],
                                Col = int.Parse(parts[2]),
                                Row = int.Parse(parts[3]),
                                Orientation = parts[4],
                                Path = parts[5]
                            };

                            if (map != null && !map[adventurer.Row, adventurer.Col].HasAdventurer
                                && map[adventurer.Row, adventurer.Col].Type != CellType.Mountain)
                            {
                                map[adventurer.Row, adventurer.Col].HasAdventurer = true;
                                if (map[adventurer.Row, adventurer.Col].Type == CellType.Treasure && map[adventurer.Row, adventurer.Col].TreasureCount > 0)
                                {
                                    adventurer.TotalTreasure++;
                                    map[adventurer.Row, adventurer.Col].TreasureCount--;
                                }
                                maxPath = Math.Max(maxPath, adventurer.Path.Length);
                                adventurersList.Add(adventurer);
                            }
                            break;
                    }
                }

                return new GameMap(map, adventurersList, maxPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                return null;
            }
        }

        public void PlayGame(GameMap gameData)
        {
            while (gameData.MaxPath > 0)
            {
                foreach (var adventurer in gameData.Adventurers)
                {
                    if (adventurer.Path.Length > 0)
                    {
                        string move = adventurer.Path[0].ToString();
                        adventurer.Path = adventurer.Path.Substring(1);

                        if (move == "A")
                        {
                            if (_movementService.IsValidMovement(adventurer, gameData.Map))
                            {
                                _movementService.UpdateCells(adventurer, gameData.Map);
                            }
                        }
                        else
                        {
                            adventurer.Orientation = DirectionHelper.GetNewOrientation(adventurer.Orientation, move);
                        }
                    }
                }

                gameData.MaxPath--;
            }
        }

        public void DisplayResults(GameMap gameData, string outputPath)
        {
            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                writer.WriteLine($"C - {gameData.Map.GetLength(1)} - {gameData.Map.GetLength(0)}");
                for (int row = 0; row < gameData.Map.GetLength(0); row++)
                {
                    for (int col = 0; col < gameData.Map.GetLength(1); col++)
                    {
                        if (gameData.Map[row, col].Type == CellType.Mountain)
                        {
                            writer.WriteLine($"M - {col} - {row}");
                        }
                        if (gameData.Map[row, col].Type == CellType.Treasure && gameData.Map[row, col].TreasureCount > 0)
                        {
                            writer.WriteLine($"# {{T comme Trésor}} - {{Axe horizontal}} - {{Axe vertical}} - {{Nb. de trésors\r\nrestants}}");

                            writer.WriteLine($"T - {col} - {row} - {gameData.Map[row, col].TreasureCount}");
                        }

                    }
                }
                writer.WriteLine("# {A comme Aventurier} - {Nom de l’aventurier} - {Axe horizontal} - {Axe vertical} - {Orientation} - {Nb. trésors ramassés}");

                foreach (var adventurer in gameData.Adventurers)
                {
                    writer.WriteLine($"A - {adventurer.Name} - {adventurer.Col} - {adventurer.Row} - {adventurer.Orientation} - {adventurer.TotalTreasure}");
                }
            }

            Console.WriteLine($"Results saved to: {outputPath}");
        }

    }

}
