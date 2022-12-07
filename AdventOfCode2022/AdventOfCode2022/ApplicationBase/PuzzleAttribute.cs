namespace AdventOfCode2022;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class PuzzleAttribute : Attribute
{
    internal int day;
    internal string description;
    
    public PuzzleAttribute(int day, string description)
    {
        this.day = day;
        this.description = description;
    }
}
