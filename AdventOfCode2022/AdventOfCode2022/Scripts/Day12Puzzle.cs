using System;
using System.Diagnostics;
using System.Drawing;

namespace AdventOfCode2022.Scripts
{
    [Puzzle(12, "Path Finding")]
    internal class Day12Puzzle : IMyPuzzle
    {
        const int STEP_COST = 1;
        const int DESTINATION_HEIGHT_VALUE = 'E';
        static Tile[,] map;
        static Point inputSize;
        string[] input;
        public void Setup()
        {
            input = FileManager.ReadAllLines("Inputs/Day12.txt").ToStringArray();
            inputSize = new(input[1].Length, input.Length);

            map = new Tile[input.Length, input[0].Length];
            foreach (int y in input.Length)
                foreach (int x in input[y].Length)
                    if (input[y][x] == 'E')
                        map[y, x] = new(x, y, DESTINATION_HEIGHT_VALUE);
                    else
                        map[y, x] = new(x, y, input[y][x]);
        }

        Tile GetDestinationTile()
        {
            foreach (int y in inputSize.Y)
            {
                foreach (int x in inputSize.X)
                {
                    if (map[y, x].h == DESTINATION_HEIGHT_VALUE)
                        return map[y, x];
                }
            }
            throw new Exception("Destination not found");
        }

        public object? SolveFirst()
        {
            Tile goal = GetDestinationTile();
            Stack<Tile> frontier = new();
            frontier.Push(map[0, 0]);
            List<Tile> explored = new();
            Dictionary<Tile, Tile> cameFrom = new();
            
            while (frontier.Count > 0)
            {
                Tile current = frontier.Pop();
                explored.Add(current);
                if(current == goal)
                {
                    break;
                }
                foreach (Tile neighbor in current.GetNeighbors())
                {
                    if (explored.Contains(neighbor))
                        continue;
                    if (!frontier.Contains(neighbor))
                    {
                        neighbor.h = current.h + STEP_COST;
                        cameFrom.Add(neighbor, current);
                        frontier.Push(neighbor);
                    }
                    else
                    {
                        if (neighbor.h >= current.h + STEP_COST)
                        {
                            //neighbor.h = current.h + STEP_COST;
                            cameFrom.Add(neighbor, current);
                        }
                    }
                }
            }
            
            Tile PathCurrent = goal;
            List<Tile> path = new();
            while (PathCurrent != map[0, 0])
            {
                path.Add(PathCurrent);
                PathCurrent = cameFrom[PathCurrent];
            }   
            return path.Count;
        }

        public object? SolveSecond()
        {
            return null;
        }

        public record Tile(int x, int y)
        { 
            public int h { get; set; }
            public Tile[] GetNeighbors()
            {
                int lengthX = inputSize.X;
                int lengthY = inputSize.Y;
                var result = new List<Tile>();
                
                //top left
                if (x > 0 && y > 0)
                    result.Add(map[y - 1, x - 1]);
                
                //top right
                if (x + 1 < lengthX && y > 0)
                    result.Add(map[y - 1, x + 1]);
                
                //bottom right
                if (x + 1 < lengthX && y + 1 < lengthY)
                    result.Add(map[y + 1, x + 1]);
                
                //bottom left
                if (x > 0 && y + 1 < lengthY)
                    result.Add(map[y + 1, x - 1]);
                
                //right
                if (x + 1 < lengthX && y < lengthY)
                    result.Add(map[y, x + 1]);
                
                //left
                if (x > 0 && y < lengthY)
                    result.Add(map[y, x - 1]);
                
                //top
                if (y + 1 < lengthY && x < lengthX)
                    result.Add(map[y + 1, x]);
                
                //bottom
                if (y > 0 && x < lengthX)
                    result.Add(map[y - 1, x]);
                return result.ToArray();
            }
            public Tile(int x, int y, int h) : this(x, y)
            {
                this.h = h;
            }
        }
    }
}