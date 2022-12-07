using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterRose.ConsoleExtentions;

namespace AdventOfCode2022.Scripts
{
    internal class Day7Puzzle : IMyPuzzle
    {
        Directory Root = new();
        Dictionary<string, int> largestfiles = new();
        string[] input;
        
        public Day7Puzzle()
        {
            input = FileManager.ReadAllLines("Inputs/Day7.txt").ToStringArray();
        }
        
        public object? SolveFirst()
        {
            MutableString currentPath = "";
            Directory currentDir;
            foreach (MutableString line in input)
            {
                //command handle
                if (line.StartsWith('$'))
                {
                    MutableString[] split = line.Split(' ');
                    MutableString command = split[1];
                    MutableString? argument = null;
                    if (split.Length > 2)
                        argument = split[2];
                    switch (command)
                    {
                        case "cd":
                            if (argument is null)
                                throw new InvalidOperationException("Argument is null. command CD needs an argument.");
                            GotoNewDirectory($"/{argument}");         
                            break;
                    }
                }
                //directory handle
                if (line.StartsWith("dir"))
                {
                    MutableString[] split = line.Split(' ');
                    MutableString name = split[1];
                }
                //file handle
                if (line[0].IsNumber())
                {

                }
                return null;
            }

            void GotoNewDirectory(string pathChanges)
            {
                if(pathChanges == "..")
                {
                    var e = currentPath.Split("/");
                    currentPath = new();
                    foreach(int i in e.Length)
                    {
                        if (i == e.Length)
                            break;
                        currentPath += e[i];
                    }
                }
            }
        }

        public object? SolveSecond()
        {
            return null;
        }
    }
    public struct Directory
    {
        List<string> directories;
        List<string> files;
        
        public Directory(List<string> directories, List<string> files)
        {
            this.directories = directories;
            this.files = files;
        }
    }
}
