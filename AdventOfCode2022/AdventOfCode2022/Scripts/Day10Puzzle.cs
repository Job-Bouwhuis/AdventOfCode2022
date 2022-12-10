using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterRose.Serialization;

namespace AdventOfCode2022.Scripts
{
    [Puzzle(10, "communication array got wet lol")]
    internal class Day10Puzzle : IMyPuzzle
    {
        string[] input;

        public void Setup()
        {
            input = FileManager.ReadAllLines("Inputs/Day10.txt").ToStringArray();
        }

        public object? SolveFirst()
        {
            int x = 1;
            int cycles = 1;
            int result = 0;
            foreach (string line in input)
            {
                if (line.StartsWith("noop"))
                {
                    TickPart1();
                }
                if (line.StartsWith("addx"))
                {
                    TickPart1();
                    TickPart1();
                    x += TypeWorker.CastPrimitive<int>(line[5..]);
                }
            }
            return result;

            void TickPart1()
            {
                if ((cycles - 20) % 40 == 0)
                {
                    result += x * cycles;
                }
                cycles++;
            }
        }
        

        public object? SolveSecond()
        {
            //List<string> instructions = input.Where(x => x.StartsWith("addx")).ToList();
            var instructionCount = 0;
            var X = 1;
            var screen = new char[6][];
            screen[0] = new char[40];

            int row = 0;
            bool shouldAdd = false;
            foreach (int cycle in 1..240)
            {
                if (cycle is 41 or 81 or 121 or 161 or 201 or 241)
                {
                    row++;
                    screen[row] = new char[40];
                }

                var pos = (cycle - 1) % 40;
                if (pos == X || pos + 1 == X || pos - 1 == X)
                    screen[row][pos] = '#';
                else
                    screen[row][pos] = '.';

                if (shouldAdd)
                {
                    shouldAdd = false;
                    X += TypeWorker.CastPrimitive<int>(input[instructionCount][5..]);
                    instructionCount++;
                }
                else if (input[instructionCount][0] == 'a')
                    shouldAdd = true;
                else
                    instructionCount++;
            }
            MutableString result = new();
            foreach (int y in screen.Length)
            {
                result += '\n';
                foreach (int x in screen[y].Length)
                    result.Append(screen[y][x]);
            }
            return result;
        }
    }
}
