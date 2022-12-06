using TickSystem;
using AdventOfCode2022.Scripts;

namespace AdventOfCode2022
{
    internal class Application : Tickable
    {
        public override void Start(params string[] args)
        {
            IMyPuzzle puzzle = new Day6Puzzle();
            object? resultFirst = puzzle.SolveFirst();
            object? resultSecond = puzzle.SolveSecond();

            Console.WriteLine($"\n\tPuzzle: {puzzle.GetType().Name}\n");
            {
                if (resultFirst is IEnumerable list and not string)
                {
                    Console.WriteLine("\tResult 1 was a list of items:\n");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    foreach (var item in list)
                        Console.WriteLine($"\t{item}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                }
                else
                    Console.WriteLine($"\tResult of puzzle 1:\n\t{resultFirst}\n\n" ?? "Null was returned for the first result.\n\n");
            }

            {
                if (resultSecond is IEnumerable list and not string)
                {
                    Console.WriteLine("\tResult 2 was a list of items:\n");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    foreach (var item in list)
                        Console.WriteLine($"\t{item}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                    Console.WriteLine($"\tResult of puzzle 2:\n\t{resultSecond}\n\n" ?? "Null was returned for the second resut.\n\n");
            }

        }

        public override void OnTick(Tick tick)
        {
            if (Input.GetKey(ConsoleKey.Escape))
                Exit();
        }

        public override void OnExit()
        {
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
