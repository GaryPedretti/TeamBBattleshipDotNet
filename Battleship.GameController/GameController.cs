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
                        position.Hit();
                        
                        return true;
                    }
                }
            }

            return false;
        }

        public static Ship GetShotShip(IEnumerable<Ship> ships, Position shot)
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
                        return ship;
                    }
                }
            }

            return null;
        }
        /// <summary>
        /// Checks the ship sunk condition.
        /// </summary>
        /// <param name="ship">
        /// The ship.
        /// </param>
        /// <returns>
        /// True if sunk, else false
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// ships
        ///     or
        ///     shot
        /// </exception>
        public static bool CheckIsSunk(Ship ship)
        {
            if (ship == null)
            {
                throw new ArgumentNullException("ship");
            }

            foreach (var position in ship.Positions)
            {
                if (!position.IsHit())
                {
                    return false;
                }
            }
            ship.IsSunk = true;
            return true;
        }

        public static IEnumerable<string> SunkShipNames(IEnumerable<Ship> ships)
        {
            if (ships == null)
            {
                throw new ArgumentNullException("ships");
            }

            return ships.Where(ship => ship.IsSunk).Select(ship => ship.Name);
        }

        public static bool AllShipsSunk(IEnumerable<Ship> ships)
        {
            if (ships == null)
            {
                throw new ArgumentNullException("ships");
            }


            foreach (var ship in ships)
            {

                    if (!ship.IsSunk)
                    {
                        return false;
                    }
                
            }

            return true;
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
            return ship.Positions.Count == ship.Size;
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