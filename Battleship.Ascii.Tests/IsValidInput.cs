// This file is part of the Battleship project.
namespace Battleship.Ascii.Tests
{
   using Battleship.GameController.Contracts;
   using Microsoft.VisualStudio.TestTools.UnitTesting;
    [TestClass]
    public class IsValidInputTest
    {
        [TestMethod]
        public void IsValidInput_ReturnsExpectedValue()
        {
            // Arrange
            var input = "A1";
            var expected = true;

            // Act
            var result = Program.IsValidInput(input);

            // Assert
            Assert.AreEqual(expected, result);
        }

                [TestMethod]
        public void IsValidInput_InvalidInput()
        {
            // Arrange
            var input = "T13";
            var expected = false;

            // Act
            var result = Program.IsValidInput(input);

            // Assert
            Assert.AreEqual(expected, result);
        }

    }
}