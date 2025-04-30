
namespace Battleship.Ascii
{
    using System;
    using System.Collections.Generic;
    using System.Formats.Asn1;
    using System.Linq;
    using Battleship.Ascii.TelemetryClient;
    using Battleship.GameController;
    using Battleship.GameController.Contracts;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;

    public class Program
    {
        private static List<Ship> myFleet;

        private static List<Ship> enemyFleet;

        private static int[,] gameBoard;

        private static ITelemetryClient telemetryClient;
        static void Main()
        {
            telemetryClient = new ApplicationInsightsTelemetryClient();
            telemetryClient.TrackEvent("ApplicationStarted", new Dictionary<string, string> { { "Technology", ".NET" } });

            try
            {
                Console.Title = "Battleship";
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
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
        private static void PrintBattleshipGrid(int[,] grid)
        {
            int rows = grid.GetLength(0); // Rows will now be letters (A-H)
            int cols = grid.GetLength(1); // Columns will be numbers (1-8)

            // Print column headers (numbers)
            Console.Write("   ");
            for (int col = 0; col < cols; col++)
            {
                Console.Write($" {col + 1}");
            }
            Console.WriteLine();

            // Print each row with letter label
            for (int row = 0; row < rows; row++)
            {
                char rowLetter = (char)('A' + row);
                Console.Write($" {rowLetter} ");
                for (int col = 0; col < cols; col++)
                {
                    Console.Write($" {grid[row, col]}");
                }
                Console.WriteLine();
            }
        }
        private static void StartGame()
        {
            gameBoard = new int[8, 8];
            bool quit = false;

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
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
            Console.ResetColor();
            bool isGoodPosition = false;
            var position = ParsePosition("a0");
            do
            {
                PrintBattleshipGrid(gameBoard);
                //SAM
                do
                {
                    Console.WriteLine();
                    Console.WriteLine("Player, it's your turn");
                    Console.WriteLine("Enter coordinates for your shot, m to show the grid, or exit to quit:");

                    string input = Console.ReadLine().Trim();

                    if (input.Contains("exit"))
                    {
                        Console.WriteLine("Thanks for Playing!");
                        quit = true;
                    }
                    else if (input.Contains("m"))
                    {
                        PrintBattleshipGrid(gameBoard);  
                    }
                    else
                    {

                        var letter = (Letters)Enum.Parse(typeof(Letters), input.ToUpper().Substring(0, 1));
                        var number = int.Parse(input.Substring(1, 1)) - 1;

                        var sanitizedInput = $"{letter}{number}";
                        position = ParsePosition(sanitizedInput); 
                        for (int i = 0; i < gameBoard.GetLength(0); i++)
                        {
                            char tempLetter = NumberToLetter(i);
                            for (int j = 0; j < gameBoard.GetLength(1); j++)
                            {
                                 if (gameBoard[i, j] == 0 && ArePositionsEqual(ParsePosition($"{tempLetter}{j}"), position))
                                {
                                    isGoodPosition = true;
                                    // 1 is hit 
                                    // 2 is miss
                                    gameBoard[i, j-1] = (GameController.CheckIsHit(enemyFleet, position) ? 1 : 2);
                                }
                            }
                        }
                        if(!isGoodPosition){
                        Console.WriteLine("BAD POSITION, have already guessed, try again or invalid");
                        }
                    }
                    
                }while(!isGoodPosition && !quit);
                isGoodPosition = false;
                
                if (!quit)
                {
                    var isHit = GameController.CheckIsHit(enemyFleet, position);
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
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine();
                        Console.WriteLine("Yeah! Nice Hit !");

                    } else {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("    *     .     *   ");
                        Console.WriteLine("        *   .       ");
                        Console.WriteLine("   SPLASH! You missed!");
                        Console.WriteLine("        ~~~~~~       ");
                        Console.WriteLine("     ~~~~  ~~~~      ");
                        Console.WriteLine("  ~~~~      ~~~~     ");
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine();
                        Console.WriteLine("Miss");
                        Console.ResetColor();
                    }
                    position = GetRandomPosition();
                    isHit = GameController.CheckIsHit(myFleet, position);
                    telemetryClient.TrackEvent("Computer_ShootPosition", new Dictionary<string, string>() { { "Position", position.ToString() }, { "IsHit", isHit.ToString() } });
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine();
                    Console.WriteLine("Computer shot in {0}{1}", position.Column, position.Row);
                    Console.ResetColor();
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
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine();
                        Console.WriteLine("They've hit your ship !");

                    } else {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("    *     .     *   ");
                        Console.WriteLine("        *   .       ");
                        Console.WriteLine("   SPLASH! They missed!");
                        Console.WriteLine("        ~~~~~~       ");
                        Console.WriteLine("     ~~~~  ~~~~      ");
                        Console.WriteLine("  ~~~~      ~~~~     ");
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine();
                        Console.WriteLine("Miss");
                        Console.ResetColor();
                    }
                }
            }while (quit == false);
        }


        private static char NumberToLetter(int number)
        {
            return (char)('A' + number);
        }

        public static Position ParsePosition(string input)
        {
            var letter = (Letters)Enum.Parse(typeof(Letters), input.ToUpper().Substring(0, 1));
            var number = int.Parse(input.Substring(1, 1));
            return new Position(letter, number);
        }

        private static bool ArePositionsEqual(Position a, Position b)
        {
            return a.Column == b.Column && a.Row == b.Row;
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

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Please position your fleet (Game board size is from A to H and 1 to 8) :");

            foreach (var ship in myFleet)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Please enter the positions for the {0} (size: {1})", ship.Name, ship.Size);
                for (var i = 1; i <= ship.Size; i++)
                {
                    Console.WriteLine("Enter position {0} of {1} (i.e A3):", i, ship.Size);
                    var position = Console.ReadLine();
                    ship.AddPosition(position);
                    telemetryClient.TrackEvent("Player_PlaceShipPosition", new Dictionary<string, string>() { { "Position", position }, { "Ship", ship.Name }, { "PositionInShip", i.ToString() } });
                }
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
