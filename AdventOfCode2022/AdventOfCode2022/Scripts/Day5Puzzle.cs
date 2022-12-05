using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Scripts
{
    internal class Day5Puzzle : IMyPuzzle
    {
        readonly List<List<char>> stacks = new();
        readonly string[] instructions;

        public Day5Puzzle()
        {
            GetStacks();
            instructions = FileManager.ReadAllLines("Inputs/Day5.txt").ToStringArray();
        }

        void GetStacks()
        {
            /*
                    [Q] [B]         [H]        
                [F] [W] [D] [Q]     [S]        
                [D] [C] [N] [S] [G] [F]        
                [R] [D] [L] [C] [N] [Q]     [R]
            [V] [W] [L] [M] [P] [S] [M]     [M]
            [J] [B] [F] [P] [B] [B] [P] [F] [F]
            [B] [V] [G] [J] [N] [D] [B] [L] [V]
            [D] [P] [R] [W] [H] [R] [Z] [W] [S]
             1   2   3   4   5   6   7   8   9 
             */
            stacks.Add(new() { 'D', 'B', 'J', 'V' });
            stacks.Add(new() { 'P', 'V', 'B', 'W', 'R', 'D', 'F' });
            stacks.Add(new() { 'R', 'G', 'F', 'L', 'D', 'C', 'W', 'Q' });
            stacks.Add(new() { 'W', 'J', 'P', 'M', 'L', 'N', 'D', 'B' });
            stacks.Add(new() { 'H', 'N', 'B', 'P', 'P', 'C', 'S', 'Q' });
            stacks.Add(new() { 'R', 'D', 'B', 'S', 'N', 'G' });
            stacks.Add(new() { 'Z', 'B', 'P', 'M', 'Q', 'F', 'S', 'H' });
            stacks.Add(new() { 'W', 'L', 'F' });
            stacks.Add(new() { 'S', 'V', 'F', 'M', 'R' });
        }


        public object? SolveFirst()
        {
            foreach (string line in instructions)
            {
                string[] strings = line.Split(' ');
                int count = TypeWorker.CastPrimitive<int>(strings[1]);
                int from = TypeWorker.CastPrimitive<int>(strings[3]);
                int destination = TypeWorker.CastPrimitive<int>(strings[5]);

                foreach (int i in count)
                    MoveItem(from - 1, destination - 1);
            }

            List<char> result = new();
            foreach (var stack in stacks)
            {
                result.Add(stack.Last());
            }
            return new string(result.ToArray());

            void MoveItem(int from, int destination)
            {
                List<char> l1 = stacks[from];
                List<char> l2 = stacks[destination];

                char c = l1.Last();
                l1.RemoveAt(l1.Count - 1);
                l2.Add(c);

                stacks[from] = l1;
                stacks[destination] = l2;
            }
        }

        public object? SolveSecond()
        {
            //reset the stacks to what they were in the beginning so puzzle 2 can be solved correctly
            stacks.Clear();
            GetStacks();

            foreach (string line in instructions)
            {
                string[] strings = line.Split(' ');
                int count = TypeWorker.CastPrimitive<int>(strings[1]);
                int from = TypeWorker.CastPrimitive<int>(strings[3]);
                int destination = TypeWorker.CastPrimitive<int>(strings[5]);
                MoveItems(count, from - 1, destination - 1);
            }

            List<char> result = new();
            foreach (var stack in stacks)
            {
                result.Add(stack.Last());
            }
            return new string(result.ToArray());

            void MoveItems(int count, int from, int destination)
            {
                List<char> l1 = stacks[from];
                List<char> l2 = stacks[destination];

                char[] chars = l1.Take((l1.Count - count)..).ToArray();
                SnowUtils.Repeat(() => l1.RemoveAt(l1.Count - 1), count);
                chars.Foreach(x => l2.Add(x));

                stacks[from] = l1;
                stacks[destination] = l2;
            }
        }
    }
}
