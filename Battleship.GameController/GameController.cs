using System.Linq;

namespace Battleship.GameController
{
    using System;
    using System.Collections.Generic;

    using Battleship.GameController.Contracts;

    /// <summary>
    ///     The game controller.
    /// </summary>
    public class GameController
    {
        /// <summary>
        /// Checks the is hit.
        /// </summary>
        /// <param name="ships">
        /// The ships.
        /// </param>
        /// <param name="shot">
        /// The shot.
        /// </param>
        /// <returns>
        /// True if hit, else false
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// ships
        ///     or
        ///     shot
        /// </exception>
        public static bool CheckIsHit(IEnumerable<Ship> ships, Position shot)
        {
            if (ships == null)
            {
                throw new ArgumentNullException("ships");
            }

            if (shot == null)
            {
                throw new ArgumentNullException("shot");
            }

            foreach (var ship in ships)
            {
                foreach (var position in ship.Positions)
                {
                    if (position.Equals(shot))
                    {
                        position.IsHit = true;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///     The initialize ships.
        /// </summary>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        public static IEnumerable<Ship> InitializeShips()
        {
            return new List<Ship>()
                       {
                           new Ship() { Name = "Aircraft Carrier", Size = 5, Color = ConsoleColor.Blue }, 
                           new Ship() { Name = "Battleship", Size = 4, Color = ConsoleColor.Red }, 
                           new Ship() { Name = "Submarine", Size = 3, Color = ConsoleColor.Gray }, 
                           new Ship() { Name = "Destroyer", Size = 3, Color = ConsoleColor.Yellow }, 
                           new Ship() { Name = "Patrol Boat", Size = 2, Color = ConsoleColor.Green }
                       };
        }

        /// <summary>
        /// The is ships valid.
        /// </summary>
        /// <param name="ship">
        /// The ship.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsShipValid(Ship ship)
        {
            if (ship.Positions.Count != ship.Size)
                return false;
                
            // Sort positions

            // figure out if vertical or horizontal
            int shipFirstRow = ship.Positions[0].Row;
            bool isHorizontal = ship.Positions.Count(x => x.Row == shipFirstRow) > 1;

            if (isHorizontal) {
                ship.Positions.Sort((x,y) => x.Column.CompareTo(y.Column));

                var previousCol = ship.Positions[0].Column;
                foreach (var position in ship.Positions) {
                    if (previousCol != ship.Positions[0].Column && (previousCol + 1 != position.Column)) {
                        return false;
                    }
                    previousCol = position.Column;
                }

            } else {
                ship.Positions.Sort((x,y) => x.Row.CompareTo(y.Row));

                var previousRow = ship.Positions[0].Row;
                foreach (var position in ship.Positions) {
                    if (previousRow != ship.Positions[0].Row && (previousRow + 1 != position.Row)) {
                        return false;
                    }
                    previousRow = position.Row;
                }
            }

            return ship.Positions.Count == ship.Size;
        }

        public static bool AreShipPlacementsValid(Ship ship, IEnumerable<Ship> fleet) {
            var isShipValid = IsShipValid(ship);
            bool isFleetValid = false;

            if (isShipValid) {

                
                isFleetValid = true;
            }
            return isShipValid && isFleetValid;
        }

        public static Position GetRandomPosition(int size)
        {
            var random = new Random();
            var letter = (Letters)random.Next(size);
            var number = random.Next(size);
            var position = new Position(letter, number);
            return position;
        }
     }
}