using TickSystem;
using AdventOfCode2022.Scripts;

namespace AdventOfCode2022
{
    internal class Application : Tickable
    {
        public override void Start(params string[] args)
        {
            Day4Puzzle puzzle = new();
            object? resultFirst = puzzle.SolveFirst();
            object? resultSecond = puzzle.SolveSecond();

            Console.WriteLine($"\n\tPuzzle: {puzzle.GetType().Name}\n");
            Console.WriteLine($"\tResult of puzzle 1:\n\t{resultFirst}\n\n" ?? "Null was returned for the first result.\n\n");
            Console.WriteLine($"\tResult of puzzle 2:\n\t{resultSecond}\n\n" ?? "Null was returned for the second resut.\n\n");
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
