using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Scripts
{
    [Puzzle(50, "tests")]
    internal class Bende : IMyPuzzle
    {
        public void Setup()
        {
            var input = File.ReadAllLines("Inputs/TEMP.txt")
                    .Select(ParseOp)
                    .ToList();

            Part1(input);
            Part2(input);
        }

        public object? SolveFirst()
        {
            return null;
        }

        public object? SolveSecond()
        {
            return null;
        }


        class Vm
        {
            public int Clock { get; set; }
            public int RegX { get; set; }

            public Vm(int clock, int regX)
            {
                Clock = clock;
                RegX = regX;
            }
        }

        enum Op
        {
            Addx,
            Noop
        }


        static List<Vm> Eval(Op op, Vm vm)
        {
            switch (op)
            {
                case Op.Addx:
                    return new List<Vm>
                    {
                        new Vm(vm.Clock + 1, vm.RegX),
                        new Vm(vm.Clock + 2, vm.RegX + 1)
                    };
                case Op.Noop:
                    return new List<Vm>
                    {
                        new Vm(vm.Clock + 1, vm.RegX)
                    };
                default:
                    throw new ArgumentException($"Unknown operation '{op}'");
            }
        }

        static Op ParseOp(string s)
        {
            if (s.StartsWith("addx"))
            {
                return Op.Addx;
            }
            else if (s.StartsWith("noop"))
            {
                return Op.Noop;
            }
            else
            {
                throw new ArgumentException($"Unknown operation '{s}'");
            }
        }

        static void Part1(List<Op> input)
        {
            Console.WriteLine("Part 1");

            var vm = new Vm(0, 1);

            var result = input
                .SelectMany(op => Eval(op, vm))
                .Where(v => (v.Clock - 20) % 40 == 0)
                .Sum(v => v.RegX * v.Clock);

            Console.WriteLine(result);
        }

        static void Part2(List<Op> input)
        {
            Console.WriteLine("Part 2");

            var vm = new Vm(0, 1);

            var result = input
                .SelectMany(op => Eval(op, vm))
                .Select((v, i) => Math.Abs(v.RegX - (i % 40)) <= 1 ? '#' : ' ')
                .ToArray();

            var output = Enumerable
                .Range(0, result.Length / 6)
                .Select(i => result.Skip(i * 6).Take(6).ToArray())
                .Select(row => new string(row))
                .ToArray();

            Console.WriteLine(string.Join("\n", output));
        }
    }
}




