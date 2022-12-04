using System.Runtime.CompilerServices;
using static AdventOfCode2022.Scripts.Day4Puzzle;

namespace AdventOfCode2022.Scripts;

public class Day4Puzzle : IMyPuzzle
{
    string[] input;

    public Day4Puzzle()
    {
        input = FileManager.ReadAllLines("Inputs/Day4.txt").ToStringArray();
    }

    public object? SolveFirst()
    {
        int completeOverlaps = 0;
        foreach (string line in input)
        {
            string[] splitted = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
            string[] STRINGrange1 = splitted[0].Split('-', StringSplitOptions.RemoveEmptyEntries);
            string[] STRINGrange2 = splitted[1].Split('-', StringSplitOptions.RemoveEmptyEntries);
            Range<int> range1 = new(TypeWorker.CastPrimitive<int>(STRINGrange1[0]), TypeWorker.CastPrimitive<int>(STRINGrange1[1]));
            Range<int> range2 = new(TypeWorker.CastPrimitive<int>(STRINGrange2[0]), TypeWorker.CastPrimitive<int>(STRINGrange2[1]));

            bool check1 = range1.Min >= range2.Min;
            bool check2 = range1.Max <= range2.Max;
            bool check3 = check1 || check2;
            if (check3)
                completeOverlaps++;
            else
            {
                bool check4 = range1.Min <= range2.Min;
                bool check5 = range1.Max >= range2.Max;
                bool check6 = check4 || check5;
                if (check6)
                    completeOverlaps++;
            }

        }
        return completeOverlaps;
    }

    public object? SolveSecond()
    {
        int overlaps = 0;
        foreach (string line in input)
        {
            string[] splitted = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
            string[] STRINGrange1 = splitted[0].Split('-', StringSplitOptions.RemoveEmptyEntries);
            string[] STRINGrange2 = splitted[1].Split('-', StringSplitOptions.RemoveEmptyEntries);
            Range area1 = new(TypeWorker.CastPrimitive<int>(STRINGrange1[0]), TypeWorker.CastPrimitive<int>(STRINGrange1[1]));
            Range area2 = new(TypeWorker.CastPrimitive<int>(STRINGrange2[0]), TypeWorker.CastPrimitive<int>(STRINGrange2[1]));

            foreach (int i in area1.Min..(area1.Max+1))
            {
                foreach (int j in area2.Min..(area2.Max+1))
                {
                    if (i == j)
                    {
                        overlaps++;
                        goto EndLoop;
                    }
                        
                }
            }
        EndLoop:;
            
        }

        return overlaps;
    }

    public class Range
    {
        public readonly int Min;
        public readonly int Max;

        public Range(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }
}
