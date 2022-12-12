using System;
using System.Diagnostics;
using System.Drawing;

namespace AdventOfCode2022.Scripts
{
    [Puzzle(12, "Path Finding")]
    internal class Day12Puzzle : IMyPuzzle
    {
        public char[,] map;
        Point inputSize;
        string[] input;
        public void Setup()
        {
            input = FileManager.ReadAllLines("Inputs/Day12.txt").ToStringArray();
            inputSize = new(input.Length, input[1].Length);

            map = new char[input.Length, input[0].Length];
            foreach (int i in input.Length)
            {

                foreach (int j in input[i].Length)
                    map[i, j] = input[i][j];
            }
        }

        Tile GetPositionOfDestination()
        {
            foreach (int i in inputSize.X)
            {
                foreach (int j in inputSize.Y)
                {
                    if (map[i, j] == 'E')
                        return new Tile() { X = i, Y = j };
                }
            }
            throw new Exception("Destination not found");
        }

        public object? SolveFirst()
        {
            var start = new Tile();
            start.Y = 0;
            start.X = 0;

            var finish = GetPositionOfDestination();
            start.SetDistance(finish);

            List<Tile> activeTiles = new();
            List<Tile> visitedTiles = new();


            return null;
        }

        public object? SolveSecond()
        {
            return null;
        }

        private static List<Tile> GetWalkableTiles(List<string> map, Tile currentTile, Tile targetTile)
        {
            var Temp = new List<Tile>()
            {
                new Tile { X = currentTile.X, Y = currentTile.Y - 1, Parent = currentTile, Cost = currentTile.Cost + 1 },
                new Tile { X = currentTile.X, Y = currentTile.Y + 1, Parent = currentTile, Cost = currentTile.Cost + 1},
                new Tile { X = currentTile.X - 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
                new Tile { X = currentTile.X + 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
            };
            List<Tile> possibleTiles = new();
            foreach (Tile tile in Temp)
            {

            }

            var maxX = map.First().Length - 1;
            var maxY = map.Count - 1;

            return possibleTiles
                    .Where(tile => tile.X >= 0 && tile.X <= maxX)
                    .Where(tile => tile.Y >= 0 && tile.Y <= maxY)
                    .Where(tile => map[tile.Y][tile.X] == ' ' || map[tile.Y][tile.X] == 'B')
                    .ToList();
        }

        class Tile
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Cost { get; set; }
            public int Distance { get; set; }
            public int CostDistance => Cost + Distance;
            public Tile Parent { get; set; }

            //The distance is essentially the estimated distance, ignoring walls to our target. 
            //So how many tiles left and right, up and down, ignoring walls, to get there. 
            public void SetDistance(Tile target)
            {
                Distance = Math.Abs(target.X - X) + Math.Abs(target.Y - Y);
            }
        }
    }
}