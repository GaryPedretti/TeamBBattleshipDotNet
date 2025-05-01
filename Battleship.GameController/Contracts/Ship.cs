using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Battleship.GameController.Contracts
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The ship.
    /// </summary>
    public class Ship
    {
        private bool isPlaced;
        private bool isSunk;

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Ship"/> class.
        /// </summary>
        public Ship()
        {
            Positions = new List<Position>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets the positions.
        /// </summary>
        public List<Position> Positions { get; set; }

        /// <summary>
        /// The color of the ship
        /// </summary>
        public ConsoleColor Color { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        public int Size { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add position.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        public bool AddPosition(string input)
        {
            if (Positions == null)
            {
                Positions = new List<Position>();
            }

            var letter = (Letters)Enum.Parse(typeof(Letters), input.ToUpper().Substring(0, 1));
            var number = int.Parse(input.Substring(1, 1));
            if (IsAdjacentAndLinear(letter, number))
            {
                Positions.Add(new Position { Column = letter, Row = number });
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsAdjacentAndLinear(Letters letter, int number)
        {
            if (Positions == null || Positions.Count == 0)
                return true; // First position is always valid

            var last = Positions.Last();

            int letterDist = Math.Abs((int)letter - (int)last.Column);
            int numberDist = Math.Abs(number - last.Row);

            bool isAdjacent = (letterDist == 1 && numberDist == 0) || (letterDist == 0 && numberDist == 1);
            if (!isAdjacent)
                return false;

            // If this is only the second position, accept any straight-line direction
            if (Positions.Count == 1)
                return true;

            // Determine intended direction from first two positions
            var first = Positions.First();
            bool isVertical = first.Column == Positions[1].Column;
            bool isHorizontal = first.Row == Positions[1].Row;

            // Enforce consistent direction
            if (isVertical && letter != first.Column)
                return false;
            if (isHorizontal && number != first.Row)
                return false;

            return true;
        }

        public bool IsPlaced
        {
            get { return isPlaced; }
            set
            {
                if (value.Equals(isPlaced)) return;
                isPlaced = value;
            }
        }

        public bool IsSunk
        {
            get { return isSunk; }
            set
            {
                if (value.Equals(isSunk)) return;
                isSunk = value;
            }
        }
        #endregion
    }
}