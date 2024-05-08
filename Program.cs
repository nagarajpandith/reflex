﻿namespace ReflexGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // retrieve stored scores from text file
            string path = Environment.CurrentDirectory + @"\scores.txt";
            List<double> scores = new List<double>();
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "0");
            }
            var lines = File.ReadAllLines(path);
            for (var i = 0; i < lines.Length; i += 1)
            {
                if (lines[i] != "")
                {
                    var line = Double.Parse(lines[i]);
                    scores.Add(line);
                }
            }
            Random r = new();
            Console.WriteLine("WAIT FOR IT...\n");
            // get random int N from 1 to 10s
            int N = r.Next(1000, 10000);
            // sleep for N
            System.Threading.Thread.Sleep(N);
            // print GO
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("LET'S GOOOO !!");
            Console.ResetColor();
            // start timer
            var watch = System.Diagnostics.Stopwatch.StartNew();
            // read space from user
            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Spacebar)) { }
            // stop timer
            watch.Stop();
            TimeSpan ts = watch.Elapsed;
            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}.{1:00}", ts.Seconds, ts.Milliseconds / 10);
            double parsedValue = Double.Parse(elapsedTime);
            // handle cheat
            if (parsedValue == 00.00)
            {
                Console.WriteLine("You probably cheated. Try again!");
            }
            else
            {
                Console.WriteLine($"You took {elapsedTime}s to complete.");
                // handle empty txt file case
                double highScore = Double.MaxValue;
                if (scores.Count > 0)
                {
                    highScore = scores.Min();
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                if (parsedValue > highScore)
                {
                    Console.WriteLine("You couldn't beat the lowest SpeedTime: " + highScore + "s.");
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
    }
}
