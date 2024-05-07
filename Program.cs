using static ReflexGame.ConsoleUtility;

namespace ReflexGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("WAIT FOR IT...\n");
            // get random int N from 1 to 10s
            int N = r.Next(1000, 10000);
            // sleep for N
            Thread.Sleep(N);
            // print GO
            Console.WriteLine("GO!");
        }
    }
}
