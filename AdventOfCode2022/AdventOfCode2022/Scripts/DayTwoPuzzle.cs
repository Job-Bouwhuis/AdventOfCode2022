using System.Security.Cryptography.X509Certificates;
using WinterRose.FileManagement;
using static AdventOfCode2022.Scripts.DayTwoPuzzle;
using static System.Formats.Asn1.AsnWriter;

namespace AdventOfCode2022.Scripts;

internal class DayTwoPuzzle : IMyPuzzle
{
    string[] input;

        /*
        0 if lost
        3 for draw
        6 for win

        rock is 1
        paper is 2
        scisors is 3

        the sum is the total score of a round
        */

    public DayTwoPuzzle()
    {
        input = FileManager.ReadAllLines("Inputs/Day2.txt").ToStringArray();
    }

    public object? SolveFirst()
    {


        int score = 0;
        foreach (var move in input)
        {
            char[] chars = move.ToCharArray();
            Move elfMove = GetMove(chars[0]);
            Move myMove = GetMove(chars[2]);

            GameState state = CheckState(myMove, elfMove);

            if(state is GameState.Win)
                score += 6 + (int)myMove;
            if (state is GameState.Lost)
                score += (int)myMove;
            if (state is GameState.Draw)
                score += 3 + (int)myMove;
        }

        return score;
    }

    public object? SolveSecond()
    {
        int score = 0;
        foreach(string moves in input)
        {
            char[] chars = moves.ToCharArray();
            Move elfMove = GetMove(chars[0]);
            Move myMove = getDesiredMove(chars[2], elfMove);

            GameState state = CheckState(myMove, elfMove);

            if (state is GameState.Win)
                score += 6 + (int)myMove;
            if (state is GameState.Lost)
                score += (int)myMove;
            if (state is GameState.Draw)
                score += 3 + (int)myMove;
        }

        return score;
       
        Move getDesiredMove(char c, Move elfMove)
        {
            c = c.ToLower();
            if (c is 'x')
                return getLosingMove(elfMove);
            if (c is 'y')
                return elfMove;
            if (c is 'z')
                return getWinningMove(elfMove);
            return Move.Invalid;
        }

        Move getWinningMove(Move other)
        {
            if (other is Move.Rock)
                return Move.Paper;
            if (other is Move.Paper)
                return Move.Scissors;
            if (other is Move.Scissors)
                return Move.Rock;
            return Move.Invalid;
        }
        Move getLosingMove(Move other)
        {
            if (other is Move.Rock)
                return Move.Scissors;
            if (other is Move.Paper)
                return Move.Rock;
            if (other is Move.Scissors)
                return Move.Paper;
            return Move.Invalid;
        }
    }



    public enum Move
    {
        Rock = 1,
        Paper,
        Scissors,
        Invalid = 0
    }

    public enum GameState
    {
        Win,
        Lost,
        Draw
    }

    Move GetMove(char c)
    {
        return c.ToLower() switch
        {
            'a' => Move.Rock,
            'x' => Move.Rock,

            'b' => Move.Paper,
            'y' => Move.Paper,

            'c' => Move.Scissors,
            'z' => Move.Scissors,

            _ => Move.Invalid
        };
    }

    GameState CheckState(Move a, Move b)
    {
        if (a == b)
            return GameState.Draw;

        if (a is Move.Paper && b is Move.Rock)
            return GameState.Win;
        if (a is Move.Scissors && b is Move.Paper)
            return GameState.Win;
        if (a is Move.Rock && b is Move.Scissors)
            return GameState.Win;

        if (b is Move.Paper && a is Move.Rock)
            return GameState.Lost;
        if (b is Move.Scissors && a is Move.Paper)
            return GameState.Lost;
        if (b is Move.Rock && a is Move.Scissors)
            return GameState.Lost;

        return GameState.Draw;
    }
}
