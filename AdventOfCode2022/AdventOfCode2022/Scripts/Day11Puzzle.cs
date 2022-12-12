using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Scripts
{
    [Puzzle(11, "Monkey prediction")]
    internal class Day11Puzzle : IMyPuzzle
    {
        List<Monkee> monkeys = new(8);
        public void Setup()
        {
            monkeys.Clear();
            string textInput = FileManager.Read("Inputs/Day11.txt")!;
            string[] lines = textInput.Split($"\r\n\r\n");
            foreach (string data in lines)
            {
                monkeys.Add(new Monkee(data));
            }
        }

        public object? SolveFirst()
        {
            foreach (int i in 20)
            {
                foreach (Monkee monkey in monkeys)
                {
                    foreach (int j in monkey.inventory.Count)
                    {
                        monkey.ThrowPart1(monkeys);
                    }
                    Console.WriteLine();
                }
            }

            ulong result = 0;
            monkeys = monkeys.OrderByDescending(x => x.inspectedItems).ToList();
            result = monkeys[0].inspectedItems * monkeys[1].inspectedItems;
            return result;
        }

        public object? SolveSecond()
        {
            Setup();
            Console.Clear();

            int lcd = monkeys.Select(m => m.testValue).Aggregate(1, (x, y) => x * y);
            foreach (int i in 10000)
            {
                Console.WriteLine(i);
                foreach (Monkee monkey in monkeys)
                {
                    foreach (int j in monkey.inventory.Count)
                    {
                        monkey.ThrowPart2(monkeys, lcd);
                    }
                }
            }
            
            ulong result = 0;
            monkeys = monkeys.OrderByDescending(x => x.inspectedItems).ToList();
            result = monkeys[0].inspectedItems * monkeys[1].inspectedItems;
            return result;
        }
    }

    public class Monkee
    {
        public int monkeyID;
        public List<long> inventory = new();
        public ulong inspectedItems = 0;
        public int testValue;
        
        string operation;
        int toMonkeyIfTrue;
        int toMonkeyIfFalse;
        string rawOperation;

        public Monkee(string unparsedData)
        {
            string[] lines = unparsedData.Split("\r\n");

            string line = lines[0];
            {
                string[] data = line.Split(" ");
                monkeyID = TypeWorker.CastPrimitive<int>(data[1].TrimEnd(':'));
            }
            line = lines[1];
            {
                string[] data = line.Split(" ")[4..];
                foreach (string item in data)
                    inventory.Add(TypeWorker.CastPrimitive<long>(item.TrimEnd(',')));
            }
            line = lines[2];
            rawOperation = line[23..];
            line = lines[3];
            testValue = TypeWorker.CastPrimitive<int>(line[21..]);
            line = lines[4];
            toMonkeyIfTrue = TypeWorker.CastPrimitive<int>(line[^1]);
            line = lines[5];
            toMonkeyIfFalse = TypeWorker.CastPrimitive<int>(line[^1]);
        }


        public void ThrowPart1(List<Monkee> monkeys)
        {
            var data = EvaluateConditionPart1();
            Console.WriteLine($"monkey: {monkeyID}, data: {data.result}");
            inspectedItems++;

            if (data.condition)
                monkeys[toMonkeyIfTrue].inventory.Add(data.result);
            else
                monkeys[toMonkeyIfFalse].inventory.Add(data.result);
            inventory.RemoveAt(0);
        }
        public (long result, bool condition) EvaluateConditionPart1()
        {
            string[] operationData = GetNewestOperation().Split(' ');
            long value = 0;
            switch (operationData[1])
            {
                case "*":
                    value = TypeWorker.CastPrimitive<long>(operationData[0]) * TypeWorker.CastPrimitive<long>(operationData[2]);
                    break;
                case "+":
                    value = TypeWorker.CastPrimitive<long>(operationData[0]) + TypeWorker.CastPrimitive<long>(operationData[2]);
                    break;
            }
            value /= 3;
            return (value, value % testValue == 0);
        }


        public void ThrowPart2(List<Monkee> monkeys, int lcd)
        {

            var data = EvaluateConditionPart2(lcd);
            inspectedItems++;

            if (data.condition)
                monkeys[toMonkeyIfTrue].inventory.Add(data.result);
            else
                monkeys[toMonkeyIfFalse].inventory.Add(data.result);
            inventory.RemoveAt(0);
        }
        public (long result, bool condition) EvaluateConditionPart2(int lcd)
        {
            string[] operationData = GetNewestOperation().Split(' ');
            long value = 0;
            switch (operationData[1])
            {
                case "*":
                    value = TypeWorker.CastPrimitive<long>(operationData[0]) * TypeWorker.CastPrimitive<long>(operationData[2]);
                    break;
                case "+":
                    value = TypeWorker.CastPrimitive<long>(operationData[0]) + TypeWorker.CastPrimitive<long>(operationData[2]);
                    break;
            }

            value %= lcd;
            return (value, value % testValue == 0);
        }

        
        string GetNewestOperation()
        {
            string[] operationData = rawOperation.Split(' ');
            string lastOperationItem = operationData[1] == "old" ? inventory[0].ToString() : operationData[1];
            operation = $"{inventory[0]} {operationData[0]} {lastOperationItem}";
            return operation;
        }
    }
}