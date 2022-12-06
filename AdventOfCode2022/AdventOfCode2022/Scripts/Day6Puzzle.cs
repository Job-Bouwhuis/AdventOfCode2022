using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Scripts;

internal class Day6Puzzle : IMyPuzzle
{
    string input;
    public Day6Puzzle()
    {
        input = FileManager.Read("Inputs/Day6.txt")!;
    }
    public object? SolveFirst()
    {
        int result;
        if ((result = GetAnswer(4)) == -1)
            return "No signal key found ";
        else return result;
    }

    public object? SolveSecond()
    {
        int result;
        if ((result = GetAnswer(14)) == -1)
            return "No message found ";
        else return result;
    }

    int GetAnswer(int data)
    {
        int count = 0;
        foreach (char c in input)
        {
            char[] chars = input.ToCharArray().Take(count..(count + data)).ToArray();
            if (chars.Any(x => x == '\0'))
                break;
            if (AreUnequeCharacters(chars))
            {
                return count + data;
            }
            count++;
        }
        return -1;
    }

    bool AreUnequeCharacters(params char[] chars)
    {
        int count = 0;
        foreach (char c in chars)
            foreach (char cc in chars)
            {
                if (c == cc)
                    count++;
            }
        return count == chars.Length;
    }
}
