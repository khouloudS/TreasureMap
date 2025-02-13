# Treasure Map Game

This project implements a simulation game where adventurers explore a map, collect treasures, and avoid obstacles like mountains. The game reads a map file, moves adventurers according to a set of instructions, and outputs the results.

## Solution Overview

1. **Main Components**:
   - **GameService**: Starts the game, reads the map from a file, and manages the game logic.
   - **MovementService**: Handles the movement of adventurers on the map, including direction changes and validation of movements.
   - **MapService**: Manages the map's initialization and updating during the game.
   - **FileReaderService**: Reads the map data from the file and creates the game map with adventurers, treasures, and obstacles.

2. **Unit Tests**:
   - Tests for movement validation, correct direction changes, and proper behavior when adventurers interact with the map.
   - Example: Checks if adventurers can move and whether they can collect treasures or hit obstacles.

3. **CI/CD with Actions**:
   - **Action Build and Release**: The application has an automatic build and release pipeline using GitHub Actions, which packages the solution into a zip file.

## Running the Game

1. Download the latest release from the **Releases** section of the project. The zip file will be named `release-treasure-map.zip`.

2. Extract the zip file, and inside the `release-treasure-map` folder, you'll find the `TreasureMap.exe` file.

3. To run the game, open **Command Prompt** (`cmd`), navigate to the folder containing `TreasureMap.exe`, and run the following command:

   ```
   release-treasure-map> TreasureMap.exe
   ```

4. The game will prompt you to enter the path to the map file, example:

   ```
   Enter the path to the map file: C:\Users\Desktop\tresor.txt
   ```

5. Once the game finishes, it will save the results to a file named `game_results.txt`.

   ```
   Results saved to: game_results.txt
   ```
