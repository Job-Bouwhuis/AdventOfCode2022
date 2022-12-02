using WinterRose.FileManagement;

namespace AdventOfCode2022.Scripts;

public class Day1Puzzle : IMyPuzzle
{
    private List<int> elves = new();

    public object? SolveFirst()
    {
        string? input = FileManager.Read("Inputs/Day1.txt");
        if (input == null)
            return "Could not solve.";

        string[] collection = input.Split("\r\n");

        int elfIndex = 0;
        int calories = 0;

        foreach(int lineIndex in collection.Length)
        {
            string item = collection[lineIndex];
            if (string.IsNullOrWhiteSpace(item))
            {
                elves.Add(calories);
                calories = 0;
                elfIndex++;
                continue;
            }
            int cals = TypeWorker.CastPrimitive<int>(item);
            calories += cals;
        }

        return elves.Max();
    }

    public object? SolveSecond() 
    {
        var collection = elves.OrderByDescending(x => x);

        var top3 = collection.Take(3);

        int sum = top3.Sum();

        return sum;
    }
}
