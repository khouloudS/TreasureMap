namespace TreasureMap.Helper
{
    public static class DirectionHelper
    {
        public static readonly Dictionary<string, Dictionary<string, string>> DirectionLookup = new()
        {
            { "S", new Dictionary<string, string> { { "G", "E" }, { "D", "O" } } }, // Sud + gauche = Est, Sud + droite = Ouest
            { "E", new Dictionary<string, string> { { "G", "N" }, { "D", "S" } } }, // Est + gauche = Nord, Est + droite = Sud
            { "N", new Dictionary<string, string> { { "G", "O" }, { "D", "E" } } }, // Nord + gauche = Ouest, Nord + droite = Est
            { "O", new Dictionary<string, string> { { "G", "S" }, { "D", "N" } } }  // Ouest + gauche = Sud, Ouest + droite = Nord
        };

        public static string GetNewOrientation(string currentOrientation, string turnDirection)
        {
            if (DirectionLookup.ContainsKey(currentOrientation) && DirectionLookup[currentOrientation].ContainsKey(turnDirection))
            {
                return DirectionLookup[currentOrientation][turnDirection];
            }
            return "Invalid direction";
        }
    }
}
