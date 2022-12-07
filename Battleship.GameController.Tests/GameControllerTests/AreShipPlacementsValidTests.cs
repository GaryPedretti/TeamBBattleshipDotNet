namespace Battleship.GameController.Tests.GameControllerTests
{
    using System.Collections.Generic;

    using Battleship.GameController.Contracts;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The is ship valid tests.
    /// </summary>
    [TestClass]
    public class AreShipPlacementsValidTests
    {
        /// <summary>
        /// The ship placements are valid.
        /// </summary>
        [TestMethod]
        public void ShipPlacementsAreValid()
        {
            var ship = new Ship { Name = "TestShip", Size = 3 };

            ship.AddPosition("A1");
            ship.AddPosition("A2");
            ship.AddPosition("A3");

            var ship2 = new Ship { Name = "TestShip2", Size = 2 };

            ship2.AddPosition("B1");
            ship2.AddPosition("B2");

            var testFleet = new List<Ship> { ship, ship2 };
            var result = GameController.AreShipPlacementsValid(ship, testFleet);

            Assert.IsTrue(result);

            result = GameController.AreShipPlacementsValid(ship2, testFleet);

            Assert.IsTrue(result);
        }

        /// <summary>
        /// The ship placements are not valid.
        /// </summary>
        [TestMethod]
        public void ShipPlacementsAreNotValid()
        {
            var ship = new Ship { Name = "TestShip", Size = 3 };

            ship.AddPosition("A1");
            ship.AddPosition("A2");
            ship.AddPosition("A3");

            var ship2 = new Ship { Name = "TestShip2", Size = 2 };

            ship2.AddPosition("A3");
            ship2.AddPosition("B3");

            var testFleet = new List<Ship> { ship, ship2 };
            var result = GameController.AreShipPlacementsValid(ship2, testFleet);

            Assert.IsFalse(result);
        }
    }
}