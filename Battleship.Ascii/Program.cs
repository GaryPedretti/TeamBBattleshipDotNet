
using System.Drawing;

namespace Battleship.Ascii
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Battleship.Ascii.TelemetryClient;
    using Battleship.GameController;
    using Battleship.GameController.Contracts;

    public class Program
    {
        private static List<Ship> myFleet;

        private static List<Ship> enemyFleet;

        private static ITelemetryClient telemetryClient;

        static void Main()
        {
            telemetryClient = new ApplicationInsightsTelemetryClient();
            telemetryClient.TrackEvent("ApplicationStarted", new Dictionary<string, string> { { "Technology", ".NET"} });

            try
            {
                Console.Title = "Battleship";
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();

                Console.WriteLine("                                     |__");
                Console.WriteLine(@"                                     |\/");
                Console.WriteLine("                                     ---");
                Console.WriteLine("                                     / | [");
                Console.WriteLine("                              !      | |||");
                Console.WriteLine("                            _/|     _/|-++'");
                Console.WriteLine("                        +  +--|    |--|--|_ |-");
                Console.WriteLine(@"                     { /|__|  |/\__|  |--- |||__/");
                Console.WriteLine(@"                    +---------------___[}-_===_.'____                 /\");
                Console.WriteLine(@"                ____`-' ||___-{]_| _[}-  |     |_[___\==--            \/   _");
                Console.WriteLine(@" __..._____--==/___]_|__|_____________________________[___\==--____,------' .7");
                Console.WriteLine(@"|                        Welcome to Battleship                         BB-61/");
                Console.WriteLine(@" \_________________________________________________________________________|");
                Console.WriteLine();

                InitializeGame();

                StartGame();
            }
            catch (Exception e)
            {
                Console.WriteLine("A serious problem occured. The application cannot continue and will be closed.");
                telemetryClient.TrackException(e);
                Console.WriteLine("");
                Console.WriteLine("Error details:");      
                throw new Exception("Fatal error", e);
            }

        }

        private static void StartGame()
        {
            Console.Clear();
            Console.WriteLine("                  __");
            Console.WriteLine(@"                 /  \");
            Console.WriteLine("           .-.  |    |");
            Console.WriteLine(@"   *    _.-'  \  \__/");
            Console.WriteLine(@"    \.-'       \");
            Console.WriteLine("   /          _/");
            Console.WriteLine(@"  |      _  /""");
            Console.WriteLine(@"  |     /_\'");
            Console.WriteLine(@"   \    \_/");
            Console.WriteLine(@"    """"""""");

            do
            {
                var standardForegroundColor = Console.ForegroundColor;
                var standardBackgroundColor = Console.BackgroundColor;
                
                Console.WriteLine();
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.WriteLine("================================================");
                Console.BackgroundColor = standardBackgroundColor;
                Console.WriteLine();
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Player, it's your turn");
                Console.WriteLine("Enter coordinates for your shot :");
                Console.ForegroundColor = standardForegroundColor;
                var userEnty = Console.ReadLine();
                var position = ParsePosition(userEnty);
                var isHit = false;
                var valid = enemyFleet[0].IsValidPosition(userEnty);
                if (valid)
                {
                    isHit = GameController.CheckIsHit(enemyFleet, position);
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("Position is outside of playing field!");
                    Console.ResetColor();
                    }

                telemetryClient.TrackEvent("Player_ShootPosition", new Dictionary<string, string>() { { "Position", position.ToString() }, { "IsHit", isHit.ToString() } });
                if (isHit)
                {
                    Console.Beep();
                    Console.ForegroundColor = ConsoleColor.Red;
                    
                    Console.WriteLine(@"                \         .  ./");
                    Console.WriteLine(@"              \      .:"";'.:..""   /");
                    Console.WriteLine(@"                  (M^^.^~~:.'"").");
                    Console.WriteLine(@"            -   (/  .    . . \ \)  -");
                    Console.WriteLine(@"               ((| :. ~ ^  :. .|))");
                    Console.WriteLine(@"            -   (\- |  \ /  |  /)  -");
                    Console.WriteLine(@"                 -\  \     /  /-");
                    Console.WriteLine(@"                   \  \   /  /");
                    
                    Console.ForegroundColor = standardForegroundColor;
                }
                
                if (isHit)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(isHit ? "Yeah ! Nice hit !" : "You Missed !");
                Console.ForegroundColor = standardForegroundColor;
                
                Console.WriteLine();
                Console.WriteLine("Enemy Fleet Status");
                PrintFleetStatus(enemyFleet);
                Console.WriteLine();
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.WriteLine("================================================");
                Console.BackgroundColor = standardBackgroundColor;
                Console.WriteLine();

                if(enemyFleet.TrueForAll((x => x.IsSunk))) {
                    Console.WriteLine("You are the winner!");
                    break;
                }

                position = GetRandomPosition();
                isHit = GameController.CheckIsHit(myFleet, position);
                telemetryClient.TrackEvent("Computer_ShootPosition", new Dictionary<string, string>() { { "Position", position.ToString() }, { "IsHit", isHit.ToString() } });
                Console.BackgroundColor = isHit ? ConsoleColor.Red : ConsoleColor.Blue;
                Console.Write("Computer shot in {0}{1} and {2}", position.Column, position.Row, isHit ? "has hit your ship !" : "missed");
                Console.BackgroundColor = standardBackgroundColor;
                Console.WriteLine();
                if (isHit)
                {
                    Console.Beep();
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine(@"                \         .  ./");
                    Console.WriteLine(@"              \      .:"";'.:..""   /");
                    Console.WriteLine(@"                  (M^^.^~~:.'"").");
                    Console.WriteLine(@"            -   (/  .    . . \ \)  -");
                    Console.WriteLine(@"               ((| :. ~ ^  :. .|))");
                    Console.WriteLine(@"            -   (\- |  \ /  |  /)  -");
                    Console.WriteLine(@"                 -\  \     /  /-");
                    Console.WriteLine(@"                   \  \   /  /");
                    Console.BackgroundColor = standardBackgroundColor;
                }

                if(enemyFleet.TrueForAll((x => x.IsSunk))) {
                    Console.WriteLine("You are the winner!");
                    break;
                }

                if(myFleet.TrueForAll((x => x.IsSunk))) {
                    Console.WriteLine("You lost!");
                    break;
                }
            }
            while (true);
        }

        public static Position ParsePosition(string input)
        {
            var letter = (Letters)Enum.Parse(typeof(Letters), input.ToUpper().Substring(0, 1));
            var number = int.Parse(input.Substring(1, 1));
            return new Position(letter, number);
        }

        private static Position GetRandomPosition()
        {
            int rows = 8;
            int lines = 8;
            var random = new Random();
            var letter = (Letters)random.Next(lines);
            var number = random.Next(rows);
            var position = new Position(letter, number);
            return position;
        }

        private static void InitializeGame()
        {
            InitializeMyFleet();

            InitializeEnemyFleet();
        }

        private static void InitializeMyFleet()
        {
            myFleet = GameController.InitializeShips().ToList();

            Console.WriteLine("Please position your fleet (Game board size is from A to H and 1 to 8) :");

            var standardBackgroundColor = Console.BackgroundColor;

            foreach (var ship in myFleet)
            {
                Console.WriteLine();
                Console.WriteLine("Please enter the positions for the {0} (size: {1})", ship.Name, ship.Size);
              
                bool areShipPlacementsValid = false;
                do {
                    Console.WriteLine();
                    Console.WriteLine("Please enter the positions for the {0} (size: {1})", ship.Name, ship.Size);
                    for (var i = 1; i <= ship.Size;)
                    {
                        Console.WriteLine("Enter position {0} of {1} (i.e A3):", i, ship.Size);
                        var position = Console.ReadLine();
                        if (ship.IsValidPosition(position))
                        {
                            ship.AddPosition(position);
                            telemetryClient.TrackEvent("Player_PlaceShipPosition",
                                new Dictionary<string, string>()
                                {
                                    { "Position", position }, { "Ship", ship.Name }, { "PositionInShip", i.ToString() }
                                });
                            i++;
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.WriteLine("Position is outside of playing field!");
                            Console.ResetColor();
                        }
                    }

                    areShipPlacementsValid = GameController.AreShipPlacementsValid(ship, myFleet);

                    if (!areShipPlacementsValid) {
                        Console.WriteLine();
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid {0} location! Please try again.", ship.Name);
                        Console.BackgroundColor = standardBackgroundColor;
                        Console.WriteLine();
                        
                        ship.Positions = null;
                    }
                } while (!areShipPlacementsValid);
            }
        }

        private static void PrintFleetStatus(IEnumerable<Ship> ships) {
            Console.WriteLine();
            foreach(var ship in ships) {
                Console.WriteLine("{0} sunk: {1}", ship.Name, ship.IsSunk.ToString());
            }
        }

        private static void InitializeEnemyFleet()
        {
            enemyFleet = GameController.InitializeShips().ToList();

            enemyFleet[0].Positions.Add(new Position { Column = Letters.B, Row = 4 });
            enemyFleet[0].Positions.Add(new Position { Column = Letters.B, Row = 5 });
            enemyFleet[0].Positions.Add(new Position { Column = Letters.B, Row = 6 });
            enemyFleet[0].Positions.Add(new Position { Column = Letters.B, Row = 7 });
            enemyFleet[0].Positions.Add(new Position { Column = Letters.B, Row = 8 });

            enemyFleet[1].Positions.Add(new Position { Column = Letters.E, Row = 6 });
            enemyFleet[1].Positions.Add(new Position { Column = Letters.E, Row = 7 });
            enemyFleet[1].Positions.Add(new Position { Column = Letters.E, Row = 8 });
            enemyFleet[1].Positions.Add(new Position { Column = Letters.E, Row = 9 });

            enemyFleet[2].Positions.Add(new Position { Column = Letters.A, Row = 3 });
            enemyFleet[2].Positions.Add(new Position { Column = Letters.B, Row = 3 });
            enemyFleet[2].Positions.Add(new Position { Column = Letters.C, Row = 3 });

            enemyFleet[3].Positions.Add(new Position { Column = Letters.F, Row = 8 });
            enemyFleet[3].Positions.Add(new Position { Column = Letters.G, Row = 8 });
            enemyFleet[3].Positions.Add(new Position { Column = Letters.H, Row = 8 });

            enemyFleet[4].Positions.Add(new Position { Column = Letters.C, Row = 5 });
            enemyFleet[4].Positions.Add(new Position { Column = Letters.C, Row = 6 });
        }
    }
}
