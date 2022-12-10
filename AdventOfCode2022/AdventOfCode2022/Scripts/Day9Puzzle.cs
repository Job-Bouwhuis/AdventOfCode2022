using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterRose.Vectors;

namespace AdventOfCode2022.Scripts
{
    [Puzzle(9, "Rope physics")]
    internal class Day9Puzzle : IMyPuzzle
    {
        string[] input;

        public void Setup()
        {
            input = FileManager.ReadAllLines("Inputs/Day9.txt").ToStringArray();
        }

        public object? SolveFirst()
        {
            Vector2 StartPos = new(0, 0);
            Vector2 prefHeadPos = StartPos;
            Vector2 headPos = StartPos;
            Vector2 tailPos = StartPos;
            List<Vector2> tailPositions = new() { StartPos };

            foreach (string line in input)
            {
                string[] strings = line.Split(' ');
                Direction dir = Enum.Parse<Direction>(strings[0]);
                int count = TypeWorker.CastPrimitive<int>(strings[1]);

                foreach (int i in count)
                {
                    headPos = MoveHead(headPos, dir);
                    tailPos = MoveTail(headPos, tailPos);
                    AddIfCoolConditionMatches(tailPositions, tailPos);
                    prefHeadPos = headPos;
                }
            }

            return tailPositions.Count;
        }

        public object? SolveSecond()
        {
            Vector2 StartPos = new(0, 0);
            Vector2 prefHeadPos = StartPos;
            Vector2 headPos = StartPos;
            Vector2 tailPos = StartPos;
            List<Vector2> tailPositions = new() { StartPos };

            List<Vector2> tails = new();
            SnowUtils.Repeat(() => tails.Add(StartPos), 9);

            foreach (string line in input)
            {
                string[] strings = line.Split(' ');
                Direction dir = Enum.Parse<Direction>(strings[0]);
                int count = TypeWorker.CastPrimitive<int>(strings[1]);

                foreach(int i in count)
                {
                    headPos = MoveHead(headPos, dir);

                    Vector2 lastEvaluated = headPos;
                    foreach(int tail in tails.Count)
                    {
                        tails[tail] = MoveTail(lastEvaluated, tails[tail]);
                        lastEvaluated = tails[tail];
                    }
                    AddIfCoolConditionMatches(tailPositions, tails[^1]);
                }
            }
            return tailPositions.Count;
        }

        void AddIfCoolConditionMatches(List<Vector2> buffer, Vector2 itemToAdd)
        {
            if (buffer.Where(x => x.x == itemToAdd.x && x.y == itemToAdd.y).ToList().Count == 0)
                buffer.Add(itemToAdd);
        }

        Vector2 MoveHead(Vector2 headPos, Direction dir)
        {
            switch (dir)
            {
                case Direction.U:
                    headPos.y += 1;
                    break;
                case Direction.D:
                    headPos.y -= 1;
                    break;
                case Direction.L:
                    headPos.x -= 1;
                    break;
                case Direction.R:
                    headPos.x += 1;
                    break;
            }
            return headPos;
        }

        Vector2 MoveTail(Vector2 head, Vector2 tail)
        {
            var distance = Vector2.Distance(head, tail);
            if (distance <= 1) return tail;

            //get the angle from which the head moved from the tail
            var angle = Math.Atan2(tail.x - head.x, tail.y - head.y) * (180 / Math.PI);

            // apply change in position 
            if (angle is < 0 and > -180) tail.x++;
            if (angle is > 0 and < 180) tail.x--;
            if (angle is > 90 or < -90) tail.y++;
            if (angle is < 90 and > -90) tail.y--;

            return tail;
        }

        enum Direction
        {
            U,
            D,
            L,
            R
        }
    }
}
