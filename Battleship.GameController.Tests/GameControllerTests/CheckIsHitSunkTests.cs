namespace Battleship.GameController.Tests.GameControllerTests
{
    using System;

    using Battleship.GameController.Contracts;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The game controller tests.
    /// </summary>
    [TestClass]
    public class GameControllerTests
    {
        /// <summary>
        /// The should hit the ship.
        /// </summary>
        [TestMethod]
        public void ShouldHitTheShip()
        {
            var ships = GameController.InitializeShips();

            var counter = 0;
            foreach (var ship in ships)
            {
                var letter = (Letters)counter;
                for (int i = 0; i < ship.Size; i++)
                {
                    ship.Positions.Add(new Position(letter, i));
                }

                counter++;
            }

            var result = GameController.CheckIsHit(ships, new Position(Letters.A, 1));
            Assert.IsTrue(result);
        }

        /// <summary>
        /// The should not hit the ship.
        /// </summary>
        [TestMethod]
        public void ShouldNotHitTheShip()
        {
            var ships = GameController.InitializeShips();

            var counter = 0;
            foreach (var ship in ships)
            {
                var letter = (Letters)counter;
                for (int i = 0; i < ship.Size; i++)
                {
                    ship.Positions.Add(new Position(letter, i));
                }

                counter++;
            }

            var result = GameController.CheckIsHit(ships, new Position(Letters.H, 1));
            Assert.IsFalse(result);
        }

        /// <summary>
        /// The throw exception if positstion is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowExceptionIfPositstionIsNull()
        {
            GameController.CheckIsHit(GameController.InitializeShips(), null);
        }

        /// <summary>
        /// The throw exception if ship is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowExceptionIfShipIsNull()
        {
            GameController.CheckIsHit(null, new Position(Letters.H, 1));
        }
        [TestMethod]
        public void ShouldReturnTrueIfShipIsSunk()
        {
            var ship = new Ship { Size = 3 };
            ship.Positions.Add(new Position(Letters.A, 1) );
            ship.Positions.Add(new Position(Letters.A, 2) );
            ship.Positions.Add(new Position(Letters.A, 3));
            GameController.EraseHits();
            GameController.RecordHit(ship.Positions[0]);
            GameController.RecordHit(ship.Positions[1]);
            GameController.RecordHit(ship.Positions[2]);
            var result = GameController.IsShipSunk(ship);
            Assert.IsTrue(result);
        }

        /// <summary>
        /// The should return false if ship is not sunk.
        /// </summary>
        [TestMethod]
        public void ShouldReturnFalseIfShipIsNotSunk()
        {
            var ship = new Ship { Size = 3 };
            ship.Positions.Add(new Position(Letters.A, 1));
            ship.Positions.Add(new Position(Letters.A, 2));
            ship.Positions.Add(new Position(Letters.A, 3));
            GameController.EraseHits();
            GameController.RecordHit(ship.Positions[0]);
            GameController.RecordHit(ship.Positions[2]);

            var result = GameController.IsShipSunk(ship);
            Assert.IsFalse(result);
        }

        /// <summary>
        /// The throw exception if ship is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowExceptionIfShipSunkcCheckIsNull()
        {
            GameController.IsShipSunk(null);
        }      
    }
    

}