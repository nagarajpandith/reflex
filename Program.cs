using static ReflexGame.ConsoleUtility;

namespace ReflexGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, ConsoleKey> keyMap =
                new()
                {
                    { "Escape", ConsoleKey.Escape },
                    { "Spacebar", ConsoleKey.Spacebar },
                    { "Backspace", ConsoleKey.Backspace },
                    { "Tab", ConsoleKey.Tab },
                    { "RightArrow", ConsoleKey.RightArrow },
                    { "LeftArrow", ConsoleKey.LeftArrow }
                };

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

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(
                "INSTRUCTIONS\n1. Press the keys in sequence.\n2. Sequence is always 10 keys long.\n3. Sequence always contains any of the 6 keys: ESCAPE, SPACE, RIGHTARROW, LEFTARROW, TAB & BACKSPACE.\n4. Game might randomly start anytime within 10 seconds from now.\n"
            );
            WriteProgressBar(0);
            for (var i = 0; i <= 100; ++i)
            {
                WriteProgressBar(i, true);
                Thread.Sleep(50);
            }
            Console.WriteLine("\n");
            Console.ResetColor();

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

            bool restartLoop = true;
            while (restartLoop)
            {
                // retrieve stored scores from text file
                string path = Environment.CurrentDirectory + @"\scores.txt";
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

                Dictionary<string, ConsoleKey> randomMap = new Dictionary<string, ConsoleKey>(); // Initialize the dictionary properly
                Random r = new();

                for (int i = 0; i < 10; i++)
                {
                    int j = r.Next(0, 6);
                    randomMap.Add($"{keyMap.Keys.ElementAt(j)}{i}", keyMap.Values.ElementAt(j));
                }

                Console.WriteLine("\n");
                Console.WriteLine("WAIT FOR IT...\n");
                // get random int N from 1 to 10s
                int N = r.Next(1000, 10000);
                // sleep for N
                Thread.Sleep(N);
                // print GO
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                List<string> sequence = [];
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
                            var top10 = scores.OrderBy(o => o).Take(10);
                            Console.WriteLine("----------------------------------");
                            Console.WriteLine("|        TOP 10 SCOREBOARD       |");
                            Console.WriteLine("----------------------------------");
                            foreach (var item in top10)
                            {
                                Console.WriteLine($"|              {item}              |");
                            }
                            Console.WriteLine("|--------------------------------|");
                            restartLoop = true;
                            break;
                        }
                        else if (keyInfo.KeyChar == 'C' || keyInfo.KeyChar == 'c')
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
                            restartLoop = true;
                            break;
                            Console.ResetColor();
                        }
                        string enteredKey = keyInfo.Key.ToString();
                        enteredValues.Add(enteredKey);
                        count++;
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
                        WriteProgress(0);
                        for (var i = 0; i <= 40; ++i)
                        {
                            WriteProgress(i, true);
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