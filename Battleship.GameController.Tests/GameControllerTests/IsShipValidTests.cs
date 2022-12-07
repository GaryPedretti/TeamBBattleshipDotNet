namespace Battleship.GameController.Tests.GameControllerTests
{
    using System.Collections.Generic;

    using Battleship.GameController.Contracts;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The is ship valid tests.
    /// </summary>
    [TestClass]
    public class IsShipValidTests
    {

        [TestMethod]
        public void LocationA1IsValid()
        {
            var ship = new Ship();
            var result = ship.IsValidPosition("A1");
            Assert.AreEqual(result, true);
        }
        
        [TestMethod]
        public void LocationEmptyIsInvalid()
        {
            var ship = new Ship();
            var result = ship.IsValidPosition("");
            Assert.AreEqual(result, false);
        }
        
        [TestMethod]
        public void LocationA11IsInvalid()
        {
            var ship = new Ship();
            var result = ship.IsValidPosition("A11");
            Assert.AreEqual(result, false);
        }
        
        [TestMethod]
        public void LocationP1IsInvalid()
        {
            var ship = new Ship();
            var result = ship.IsValidPosition("P1");
            Assert.AreEqual(result, false);
        }
        
        [TestMethod]
        public void Location11IsInvalid()
        {
            var ship = new Ship();
            var result = ship.IsValidPosition("11");
            Assert.AreEqual(result, false);
        }

        [TestMethod]
        public void LocationAAIsInvalid()
        {
            var ship = new Ship();
            var result = ship.IsValidPosition("AA");
            Assert.AreEqual(result, false);
        }
        
        /// <summary>
        /// The ship is not valid.
        /// </summary>
        [TestMethod]
        public void ShipIsNotValid()
        {
            var ship = new Ship { Name = "TestShip", Size = 3 };
            var result = GameController.IsShipValid(ship);

            Assert.IsFalse(result);
        }

        /// <summary>
        /// The ship is valid.
        /// </summary>
        [TestMethod]
        public void ShipIsValid()
        {
            var positions = new List<Position> { new Position(Letters.A, 1), new Position(Letters.A, 1), new Position(Letters.A, 1) };

            var ship = new Ship { Name = "TestShip", Size = 3, Positions = positions };
            var result = GameController.IsShipValid(ship);

            Assert.IsTrue(result);
        }
    }
}