namespace AdventOfCode2022.Scripts;

[Puzzle(8, "Finding a sutable tree to build a tree house")]
internal class Day8Puzzle : IMyPuzzle
{
    string[] input;
    public void Setup()
    {
        input = FileManager.ReadAllLines("Inputs/Day8.txt").ToStringArray();
    }

    public object? SolveFirst()
    {
        Console.WriteLine("Computing first answer...");
        int visableTrees = 0;
        foreach (int i in input.Length)
        {
            string treeRow = input[i];
            Console.WriteLine();
            foreach (int j in treeRow.Length)
            {
                char tree = treeRow[j];
                if (i == 0 || j == 0 || j == treeRow.Length - 1 || i == input.Length - 1)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(tree);
                    visableTrees++;
                    continue;
                }

                int treeSize = TypeWorker.CastPrimitive<int>(tree);
                if (input[i].Take(j).All(h => h < tree) || input[i].Skip(j + 1).All(h => h < tree) || input.Take(i).All(h => h[j] < tree) || input.Skip(i + 1).All(h => h[j] < tree))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(tree);
                    visableTrees++;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(tree);
                }
            }
        }
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\n");
        return visableTrees;
    }

    public object? SolveSecond()
    {
        Console.WriteLine("Computing second answer...");
        char[,] forest = new char[input.Length, input[0].Length];
        foreach (int i in input.Length)
        {
            foreach (int j in input[0].Length)
            {
                forest[i, j] = input[i][j];
            }
        }

        List<int> result = new();

        foreach (int i in input.Length)
        {
            Console.WriteLine();
            foreach (int j in forest.GetLength(1))
            {

                char tree = forest[i, j];

                if (i == 0 || j == 0 || j == forest.GetLength(1) - 1 || i == input.Length - 1)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(tree);
                    continue;
                }

                int treeSize = TypeWorker.CastPrimitive<int>(tree);

                int counted = GetScenicScore();
                result.Add(counted);
                
                if (counted > 1)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(tree);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(tree);
                }
                
                int GetScenicScore()
                {
                    int countRight = 0;
                    foreach (int x in (j + 1)..forest.GetLength(1))
                    {
                        int otherSize = TypeWorker.CastPrimitive<int>(forest[i, x]);
                        if (otherSize >= treeSize)
                        {
                            countRight++;
                            break;
                        }
                        else
                            countRight++;
                    }
                    int countLeft = 0;
                    foreach (int x in j * -1)
                    {
                        int otherSize = TypeWorker.CastPrimitive<int>(forest[i, x]);
                        if (otherSize >= treeSize)
                        {
                            countLeft++;
                            break;
                        }
                        else
                            countLeft++;
                    }
                    int countDown = 0;
                    foreach (int y in (i + 1)..input.Length)
                    {
                        int otherSize = TypeWorker.CastPrimitive<int>(forest[y, j]);
                        if (otherSize >= treeSize)
                        {
                            countDown++;
                            break;
                        }
                        else
                            countDown++;
                    }
                    int countUp = 0;
                    foreach (int y in i * -1)
                    {
                        int otherSize = TypeWorker.CastPrimitive<int>(forest[y, j]);
                        if (otherSize >= treeSize)
                        {
                            countUp++;
                            break;
                        }
                        else
                            countUp++;
                    }
                    return countUp * countRight * countLeft * countDown;
                }
            }
        }
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.White;
        return result.Max();
    }
}
