using TickSystem;
using AdventOfCode2022.Scripts;
using System.Reflection;

namespace AdventOfCode2022
{
    internal class Application : Tickable
    {
        Type[] puzzles;
        IMyPuzzle? puzzle = null;
        bool solvedPuzzle = false;

        public Application()
        {
            if (!File.Exists("Settings.config"))
                FileManager.Write("Settings.config", "1", true);
            Console.Title = "Advent of Code 2022";

        }
        
        public void TitleText()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Advent of Code 2022");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\tBy Winter Rose");
            Console.ForegroundColor = ConsoleColor.White;
        }
        
        public void Setup()
        {
            

            Console.WriteLine();
            Console.WriteLine("Loading puzzles...");
            Console.WriteLine();
            puzzles = TypeWorker.FindTypesWithAttribute<PuzzleAttribute>();
        }

        public override void Start(params string[] args)
        {
            Setup();
        }

        public void SolvePuzzle()
        {
            Console.Clear();
            TitleText();
            
            solvedPuzzle = true;
            puzzle!.Setup();
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
            if (puzzle is null)
            {
                string settings = FileManager.Read("Settings.config")!;
                int day = TypeWorker.CastPrimitive<int>(settings);
                GetPuzzle(day);
            }
            if (Input.GetKey(ConsoleKey.Escape, true, false))
                Exit();
            if (Input.GetKey(ConsoleKey.Spacebar, true, false))
            {
                Console.WriteLine("Give the number of the day you wish to visit:");
                Console.Write("\tDay: ");
                string input = Console.ReadLine() ?? "1";
                int day = TypeWorker.CastPrimitive<int>(input);

                GetPuzzle(day);
            }
            if (!solvedPuzzle)
                SolvePuzzle();
        }

        public void GetPuzzle(int day)
        {
            foreach (Type puzzle in puzzles)
            {
                PuzzleAttribute p = puzzle.GetCustomAttribute<PuzzleAttribute>()!;
                if (p.day == day)
                {
                    FileManager.Write("Settings.config", day.ToString(), true);

                    MethodInfo activator = typeof(ActivatorExtra).GetMethod("CreateInstance", 1, Type.EmptyTypes)!.MakeGenericMethod(puzzle);
                    this.puzzle = (IMyPuzzle)activator.Invoke(null, null);
                    solvedPuzzle = false;
                    return;
                }
            }
            throw new Exception($"No puzzle found for day {day}.");
        }

        public override void OnExit()
        {
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
