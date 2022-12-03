namespace AdventOfCode2022.Scripts;

public class Day3Puzzle : IMyPuzzle
{
    public string[] input;

    public Day3Puzzle()
    {
        input = FileManager.ReadAllLines("Inputs/day3.txt").ToStringArray();
    }
    public object? SolveFirst()
    {
        int score = 0;
        foreach (ReadOnlySpan<char> backpack in input)
        {
            char[] comp1 = backpack[..(backpack.Length / 2)].ToArray();
            char[] comp2 = backpack[(backpack.Length / 2)..].ToArray();

            comp1 = comp1.OrderBy(x => x).ToArray();
            comp2 = comp2.OrderBy(x => x).ToArray();

            foreach (char c1 in comp1)
                foreach (char c2 in comp2)
                {
                    if (c1 == c2)
                    {
                        score += (int)Enum.Parse(typeof(Priority), c1.ToString());
                        goto EndLoop;
                    }

                }
            EndLoop:
            string i;

        }

        return score;
    }

    public object? SolveSecond()
    {
        int score = 0;
        for (int i = 0; i < input.Length; i += 3)
        {
            string elf1 = input[i];
            string elf2 = input[i + 1];
            string elf3 = input[i + 2];

            foreach(char c in elf1)
            {
                if(elf1.Contains(c) && elf2.Contains(c) && elf3.Contains(c))
                {
                    score += (int)Enum.Parse(typeof(Priority), c.ToString());
                    goto EndForeach;
                }
            }
        EndForeach:
            string s;
        }
        return score;
    }

    class Data
    {
        public char c;
        public int i;

        public override string ToString()
        {
            return $"c: {c} -- i: {i}";
        }
    }

    public enum Priority
    {
        a = 1, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z, A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z
    }
}
