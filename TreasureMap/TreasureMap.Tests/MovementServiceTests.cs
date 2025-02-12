using System;
using TreasureMap.Constant;
using TreasureMap.Interfaces;
using TreasureMap.Models;
using TreasureMap.Services;
using Xunit;

namespace TreasureMap.Tests
{
    public class MovementServiceTests
    {
        private readonly MovementService _movementService;
        private readonly MapService _mapService;

        public MovementServiceTests()
        {
            _movementService = new MovementService();
            _mapService = new MapService();
        }

        [Fact]
        public void IsValidMovement_ShouldReturnTrue_WhenMovementIsValid()
        {
            // Arrange
            var map = _mapService.InitializeMap(5, 5);

            var adventurer = new Adventurer { Col = 1, Row = 1, Orientation = "N" };

            // Act
            var result = _movementService.IsValidMovement(adventurer, map);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetNextCell_ShouldReturnCorrectCoordinates_WhenOrientationIsNorth()
        {
            // Arrange
            var adventurer = new Adventurer { Col = 1, Row = 1, Orientation = "N" };

            // Act
            var (nextCol, nextRow) = _movementService.GetNextCell(adventurer.Orientation, adventurer.Col, adventurer.Row);

            // Assert
            Assert.Equal(1, nextCol);  
            Assert.Equal(0, nextRow);  
        }

        [Fact]
        public void IsNotValidMovement_ShouldReturnFalse_WhenMovementOutsideMap()
        {
            // Arrange
            var map = _mapService.InitializeMap(5, 5);

            var adventurer = new Adventurer { Col = 2, Row = 0, Orientation = "N" };

            // Act
            var result = _movementService.IsValidMovement(adventurer, map);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsNotValidMovement_ShouldNotAllowAdventurerToMoveIntoOccupiedCell()
        {
            // Arrange
            var map = _mapService.InitializeMap(5, 5);
            map[2, 2] = new Cell(CellType.Default);
            map[2, 2].HasAdventurer = true;

            var adventurer1 = new Adventurer { Col = 2, Row = 1, Orientation = "S" };

            var result = _movementService.IsValidMovement(adventurer1, map);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsNotValidMovement_ShouldReturnFalse_WhenMovingIntoMountain()
        {
            // Arrange
            var map = _mapService.InitializeMap(5, 5);

            map[2, 2] = new Cell(CellType.Mountain);

            var adventurer = new Adventurer { Col = 2, Row = 1, Orientation = "S" }; 

            // Act
            var result = _movementService.IsValidMovement(adventurer, map); 

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void UpdateCells_ShouldNotMoveAdventurer_WhenMovingIntoMountain()
        {
            // Arrange
            var map = _mapService.InitializeMap(5, 5);

            map[2, 2] = new Cell(CellType.Mountain);

            var adventurer = new Adventurer { Col = 2, Row = 1, Orientation = "S" };

            // Act
            var isValidMovement = _movementService.IsValidMovement(adventurer, map);
            if (isValidMovement)
            {
                _movementService.UpdateCells(adventurer, map);
            }

            // Assert
            
            Assert.Equal(1, adventurer.Row);  
            Assert.Equal(2, adventurer.Col);
        }

    }
}
