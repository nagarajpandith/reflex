namespace ReflexGame
{
    class Program
    {
        static void Main(string[] args)
        {
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
        }
        }
    }
}
