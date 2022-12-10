using TickSystem;
using AdventOfCode2022.Scripts;
using System.Reflection;
using System.Diagnostics;

namespace AdventOfCode2022
{
    internal class Application : Tickable
    {
        Type[] puzzles;
        IMyPuzzle? puzzle = null;
        bool solvedPuzzle = false;
        bool wait = true;
        bool forceShowPuzzles = false;
        int lastEnteredDayNumber = 0;

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
            Console.Write("\nLoading puzzles");
            Task e = "................".StringAnimationChar(50).ForeachAsync(x => Console.Write(x));
            puzzles = TypeWorker.FindTypesWithAttribute<PuzzleAttribute>();
            puzzles = puzzles.OrderBy(x => x.GetCustomAttribute<PuzzleAttribute>()!.day).ToArray();
            e.Wait();
            Console.WriteLine();
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
            Stopwatch setupTime = Stopwatch.StartNew();
            puzzle!.Setup();
            setupTime.Stop();
            Stopwatch firstSolve = Stopwatch.StartNew();
            object? resultFirst = puzzle.SolveFirst();
            firstSolve.Stop();
            Stopwatch secondSolve = Stopwatch.StartNew();
            object? resultSecond = puzzle.SolveSecond();
            secondSolve.Stop();

            Console.WriteLine($"\n\tPuzzle {puzzle.GetType().Name} results:\n");
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
            Console.WriteLine("\n\n");
            Console.WriteLine($"\tSetup took: {setupTime.ElapsedMilliseconds}ms");
            Console.WriteLine($"\tFirst solve took: {firstSolve.ElapsedMilliseconds}ms");
            Console.WriteLine($"\tSecond solve took: {secondSolve.ElapsedMilliseconds}ms");
            Console.WriteLine("\n\n\tPress 'SPACE' to solve another puzzle, or 'ESC' to exit.");
        }


        public override void OnTick(Tick tick)
        {
            if (puzzle is null)
            {
                string settings = FileManager.Read("Settings.config")!;
                if (TypeWorker.TryCastPrimitive(settings.Trim(), out int day))
                    GetPuzzle(day);
                else
                    throw new InvalidDataException("The settings file is corrupt. Please delete it and restart the application.");
            }
            if (!solvedPuzzle)
                SolvePuzzle();


            ConsoleKey? key = null;
            if (!forceShowPuzzles)
                if (wait)
                    key = Console.ReadKey().Key;

            if (key is not null && key == ConsoleKey.Escape)
                Exit();
            if (!wait || key == ConsoleKey.Spacebar)
            {
                Console.WriteLine("Give the number of the day you wish to visit:");
                Console.Write("\tDay: ");
                string input = Console.ReadLine() ?? "1";
                int day = TypeWorker.CastPrimitive<int>(input);

                GetPuzzle(day);
                wait = true;
            }
            if (forceShowPuzzles || key is not null && key == ConsoleKey.Enter)
            {
                Console.Clear();
                TitleText();
                Console.WriteLine("\n\n");
                if (forceShowPuzzles)
                    Console.WriteLine($"There is no available puzzle for day {lastEnteredDayNumber}");
                Console.WriteLine("Available Puzzles: ");

                for (int i = 0; i < puzzles.Length; i++)
                {
                    Type puzzle = puzzles[i];
                    if (i % 2 == 0)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else
                        Console.ForegroundColor = ConsoleColor.Green;
                    
                    PuzzleAttribute p = puzzle.GetCustomAttribute<PuzzleAttribute>()!;
                    Console.WriteLine($"\t{p.day} - {p.description}");
                }
                Console.ForegroundColor = ConsoleColor.White;
                wait = false;
            }
        }

        public void GetPuzzle(int day)
        {
            lastEnteredDayNumber = day;
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
            forceShowPuzzles = true;
        }

        public override void OnExit()
        {
        }
    }
}
