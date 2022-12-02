using TickSystem;
using AdventOfCode2022.Scripts;

namespace AdventOfCode2022
{
    internal class Application : Tickable
    {
        public override void Start(params string[] args)
        {
            DayTwoPuzzle puzzle = new();
            object? resultFirst = puzzle.SolveFirst();
            object? resultSecond = puzzle.SolveSecond();

            Console.WriteLine($"\n\tPuzzle: {puzzle.GetType().Name}\n");
            Console.WriteLine($"\tResult of puzzle 1:\n\t{resultFirst}\n\n" ?? "Null was returned for the first result.\n\n");
            Console.WriteLine($"\tResult of puzzle 2:\n\t{resultSecond}\n\n" ?? "Null was returned for the second resut.\n\n");
        }

        public override void OnTick(Tick tick)
        {
            //Implement your application loop here.
        }
    }
}
