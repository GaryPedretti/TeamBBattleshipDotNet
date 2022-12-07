using System.ComponentModel;
using System.Diagnostics.Contracts;
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
        public string Name { get; set; }

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
        public void AddPosition(string input)
        {
            if (Positions == null)
            {
                Positions = new List<Position>();
            }

            var letter = (Letters)Enum.Parse(typeof(Letters), input.ToUpper().Substring(0, 1));
            var number = int.Parse(input.Substring(1, 1));
            Positions.Add(new Position { Column = letter, Row = number });
        }

        public bool IsValidPosition(string? input)
        {
            if (input == null || input.Length < 2)
                return false;
            
            var letter = input.ToUpper().Substring(0, 1);
            if (letter.Any(x => !char.IsLetter(x)) || !Enum.TryParse(letter, out Letters _))
            {
                return false;
            }

            var number = input.Length> 2 ? input.Substring(1, 2) : input.Substring(1,1);
            int numeric;
            if (!int.TryParse(number, out numeric))
            {
                return false;
            }
            if(numeric < 0 || numeric > 9)
            {
                return false;
            }

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
            get { return Positions.TrueForAll((x => x.IsHit));  }
        }
        #endregion
    }
}