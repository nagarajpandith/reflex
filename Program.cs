namespace ReflexGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new();
            game.Start();
        }
    }

    class Game
    {
        private readonly static Dictionary<string, ConsoleKey> _keyMap =
               new()
               {
                    { "Escape", ConsoleKey.Escape },
                    { "Spacebar", ConsoleKey.Spacebar },
                    { "Backspace", ConsoleKey.Backspace },
                    { "Tab", ConsoleKey.Tab },
                    { "RightArrow", ConsoleKey.RightArrow },
                    { "LeftArrow", ConsoleKey.LeftArrow }
               };

        private readonly static string path = Environment.CurrentDirectory + @"\scores.txt";

        private List<double> scores = [];

        private List<string> sequence = [];

        private Dictionary<string, ConsoleKey> randomMap = [];

        public void Start()
        {
            ShowWelcomeMessage();
            ShowInstructions();
            Controls();

            bool restartLoop = true;
            while (restartLoop)
            {
                scores = LoadScores();
                Random r = new();
                GenerateMap(r);

                Console.WriteLine("\n");
                Console.WriteLine("WAIT FOR IT...\n");
                int N = r.Next(1000, 10000);
                Thread.Sleep(N);

                GenerateSequence();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("LET'S GOOOO !!");
                Console.ResetColor();
                // start timer
                var watch = System.Diagnostics.Stopwatch.StartNew();
                // read space from use
                List<string> enteredValues = [];
                int count = 0;
                while (count != 10)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                        if (keyInfo.Key == ConsoleKey.Q) // If Q is pressed, break the loop
                        {
                            restartLoop = false;
                            break;
                        }
                        else if (keyInfo.Key == ConsoleKey.Enter) // If Enter is pressed, restart the loop
                        {
                            restartLoop = true;
                            break;
                        }
                        else if (keyInfo.KeyChar == 'S' || keyInfo.KeyChar == 's')
                        {
                            ScoreBoard();
                            restartLoop = true;
                            break;
                        }
                        else if (keyInfo.KeyChar == 'C' || keyInfo.KeyChar == 'c')
                        {
                            ClearScores();
                            restartLoop = true;
                            break;
                        }
                        string enteredKey = keyInfo.Key.ToString();
                        enteredValues.Add(enteredKey);
                        count++;
                        // Console.WriteLine(enteredKey, sequence[count - 1]);
                        // Console.WriteLine(enteredKey.Equals(sequence[count - 1]));
                        if (enteredKey.Equals(sequence[count - 1]))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        Console.Write(enteredKey);
                        Console.Write(" ");
                        Console.ResetColor();
                    }
                }
                Console.WriteLine("\n");
                if (!restartLoop)
                {
                    break;
                }
                bool sequenceCorrect = sequence.SequenceEqual(enteredValues);
                if (sequenceCorrect)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Correct Sequence.");
                }
                Console.ResetColor();
                // stop timer
                watch.Stop();
                TimeSpan ts = watch.Elapsed;
                // Format and display the TimeSpan value.
                string elapsedTime = string.Format(
                    "{0:00}.{1:00}",
                    ts.Seconds,
                    ts.Milliseconds / 10
                );
                double parsedValue = Math.Round(double.Parse(elapsedTime), 2);
                if (!sequenceCorrect)
                {
                    if (!restartLoop)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You lost. Try again!");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("Restarting...  ");
                        ConsoleUtility c = new();
                        c.WriteProgress(0);
                        for (var i = 0; i <= 40; ++i)
                        {
                            c.WriteProgress(i, true);
                            Thread.Sleep(50);
                        }
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"You took {elapsedTime}s to complete.");
                    Console.ResetColor();
                    // handle empty txt file case
                    double highScore = double.MaxValue;
                    if (scores.Count > 0)
                    {
                        highScore = scores.Min();
                    }
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    if (parsedValue > highScore)
                    {
                        Console.WriteLine(
                            "You couldn't beat the lowest SpeedTime: " + highScore + "s."
                        );
                    }
                    else
                    {
                        Console.WriteLine("Congrats! You have set a new SpeedTime.");
                    }
                    Console.ResetColor();
                    // Save score to text file
                    string txt = parsedValue + Environment.NewLine;
                    using StreamWriter sw = File.AppendText(path);
                    sw.WriteLine(txt);
                }
            }
            End();

        }

        public static void ShowWelcomeMessage()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(
                @" 
                    WELCOME TO
             ____       __ _           
            |  _ \ ___ / _| | _____  __
            | |_) / _ \ |_| |/ _ \ \/ /
            |  _ <  __/  _| |  __/>  < 
            |_| \_\___|_| |_|\___/_/\_\
            "
            );
            Console.ResetColor();
        }

        public static void ShowInstructions()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(
                "INSTRUCTIONS\n1. Press the keys in sequence.\n2. Sequence is always 10 keys long.\n3. Sequence always contains any of the 6 keys: ESCAPE, SPACE, RIGHTARROW, LEFTARROW, TAB & BACKSPACE.\n4. Game might randomly start anytime within 10 seconds from now.\n"
            );
            Console.ResetColor();
            LoadingProgress();
            Console.WriteLine("\n");
            Console.ResetColor();
        }

        private static void LoadingProgress()
        {
            ConsoleUtility consoleUtility = new();
            consoleUtility.WriteProgressBar(0);
            for (var i = 0; i <= 100; ++i)
            {
                consoleUtility.WriteProgressBar(i, true);
                Thread.Sleep(50);
            }
        }

        private static void Controls()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("----------------------------------");
            Console.WriteLine("|            CONTROLS            |");
            Console.WriteLine("----------------------------------");
            Console.WriteLine("|          Q - Quit Game         |");
            Console.WriteLine("|        R - Restart Game        |");
            Console.WriteLine("|        S - Show Scoreboard     |");
            Console.WriteLine("|          C - Clear Scores      |");
            Console.WriteLine("|--------------------------------|");
            Console.ResetColor();
        }

        private void ScoreBoard()
        {
            var top10 = scores.OrderBy(o => o).Take(10);
            Console.WriteLine("----------------------------------");
            Console.WriteLine("|        TOP 10 SCOREBOARD       |");
            Console.WriteLine("----------------------------------");
            foreach (var item in top10)
            {
                Console.WriteLine($"|              {item}              |");
            }
            Console.WriteLine("|--------------------------------|");
        }

        private void ClearScores()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nAre you sure you want to clear all Scores? (Y/N)");
            char res = Console.ReadKey(true).KeyChar;
            if (res == 'Y' || res == 'y')
            {
                Console.WriteLine("\nClearing Scores...\n");
                File.WriteAllText(path, String.Empty);
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Scores cleared!\n");
                Console.ResetColor();
            }
        }

        private void GenerateSequence()
        {
            sequence.Clear();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            foreach (var item in randomMap.Keys.Select((value, i) => new { i, value }))
            {
                string seqItem = item.value.Remove(item.value.Length - 1);
                Console.Write(seqItem.ToUpper());
                sequence.Add(seqItem);
                if (item.i != randomMap.Keys.Count - 1)
                {
                    Console.Write("--");
                }
            }
            Console.WriteLine("\n");
            Console.ResetColor();
        }

        private List<double> LoadScores()
        {
            List<double> scores = [];
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "0");
            }
            var lines = File.ReadAllLines(path);
            for (var i = 0; i < lines.Length; i += 1)
            {
                if (lines[i] != "")
                {
                    var line = double.Parse(lines[i]);
                    scores.Add(line);
                }
            }
            return scores;
        }

        private void GenerateMap(Random r)
        {
            randomMap.Clear();
            for (int i = 0; i < 10; i++)
            {
                int j = r.Next(0, 6);
                randomMap.Add($"{_keyMap.Keys.ElementAt(j)}{i}", _keyMap.Values.ElementAt(j));
            }
        }

        private static void End()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(
                @"
                    Thank you for playing Reflex! ^_^
             _   _                 _                      
            | | | |               | |                     
            | |_| |__   __ _ _ __ | | ___   _  ___  _   _ 
            | __| '_ \ / _` | '_ \| |/ / | | |/ _ \| | | |
            | |_| | | | (_| | | | |   <| |_| | (_) | |_| |
            \__|_| |_|\__,_|_| |_|_|\_\\__, |\___/ \__,_|
                                        __/ |            
                                        |___/    
            "
            );
            Console.ResetColor();
        }
    }
}