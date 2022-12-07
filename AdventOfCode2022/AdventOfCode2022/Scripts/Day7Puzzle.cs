using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WinterRose.ConsoleExtentions;

namespace AdventOfCode2022.Scripts
{
    [Puzzle(7, "Directory Tree. searching for files to cleanup for update")]
    internal class Day7Puzzle : IMyPuzzle
    {
        Dictionary<string, int> largestfiles = new();
        string[] input;
        public const int MAXSTORAGE = 70000000;
        public const int UPDATESIZE = 30000000;
        static Directory root = new("Root", null);
        MutableString currentPath = "";
        Directory currentDir = root;

        public void Setup()
        {
            input = FileManager.ReadAllLines("Inputs/Day7.txt").ToStringArray();

            Console.WriteLine("Creating directory tree...");
            for (int i = 0; i < input.Length; i++)
            {
                MutableString line = (MutableString)input[i];

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
                                throw new InvalidOperationException("This should not be reached");

                            GotoNewDirectory($"/{argument}");
                            break;
                        case "ls":
                            MutableString handle = "m";
                            int index = i + 1;
                            int skipped = 0;
                            while (true)
                            {
                                if (index >= input.Length)
                                    break;
                                handle = (MutableString)input[index];
                                if (handle[0] == '$')
                                    break;
                                if (handle.StartsWith("dir"))
                                {
                                    MutableString name = handle.Split(' ')[1];
                                    currentDir.MakeDir(name);
                                }
                                if (handle[0].IsNumber())
                                {
                                    MutableString[] split2 = handle.Split(' ');
                                    MutableString name = split2[1];
                                    int size = TypeWorker.CastPrimitive<int>(split2[0]);
                                    currentDir.MakeFile(name, size);
                                }
                                index++;
                                skipped++;
                            }

                            i += skipped;
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
                    MutableString[] split = line.Split(' ');

                    currentDir.MakeFile(split[1], TypeWorker.CastPrimitive<int>(split[0]));
                }
            }
            Console.WriteLine("Computing Puzzle Answer...");
        }

        public object? SolveFirst()
        {
            GotoNewDirectory("//");
            List<Directory> tempDirs = new();
            int result = 0;


            fillListofDir(tempDirs, root);
            foreach (var dir in tempDirs)
                if (dir.GetSize() > 100000 || dir.GetSize() == 0)
                    continue;
                else
                    result += dir.GetSize();
            
            return result;
        }

        public object? SolveSecond()
        {
            int result = 0;
            int spaceFree = MAXSTORAGE - root.GetSize();
            int spaceRequired = UPDATESIZE - spaceFree;
            bool e = spaceFree > MAXSTORAGE;

            List<Directory> temp = new();
            fillListofDir(temp, root);
            temp.OrderByDescending(x => x.GetSize());
            var validOptions = temp.Where(x => x.GetSize() >= spaceRequired);
            List<int> sizes = new();
            foreach (Directory dir in validOptions)
            {
                sizes.Add(dir.GetSize());
            }
            result = sizes.Min();
            
            return result;
        }

        void fillListofDir(List<Directory> buffer, Directory dir)
        {
            foreach (var d in dir.directories)
            {
                buffer.Add(d);
                fillListofDir(buffer, d);
                
            }
        }

        int ComputeLargestDirector(Directory dir)
        {
            List<Directory> temp = new();
            fillListofDir(temp, dir);
            int size = 0;
            foreach(var d in temp)
            {
                size += d.GetSize();
            }
            return size;
        }
        
        void GotoNewDirectory(string pathChanges)
        {
            if (pathChanges == "/..")
            {
                currentDir = currentDir!.Parent!;
            }
            else if (pathChanges == "//")
            {
                currentDir = root;
            }
            else
            {
                if (!root.ExistsDir(pathChanges.TrimStart('/')))
                    currentDir.MakeDir(pathChanges.TrimStart('/'));
                currentDir = currentDir.FindDir(pathChanges.TrimStart('/'));
            }
        }
    }
    public class Directory
    {
        public string name;
        public Directory? Parent;
        public List<Directory> directories;
        public Dictionary<string, int> files;

        public Directory(string name, Directory? parent)
        {
            directories = new();
            files = new();
            this.name = name;
            Parent = parent;
        }
        public int GetSize()
        {
            int size = 0;
            foreach (var file in files)
            {
                size += file.Value;
            }
            foreach (var dir in directories)
            {
                size += dir.GetSize();
            }
            return size;
        }

        public bool ExistsDir(string name)
        {
            foreach (var dir in directories)
            {
                if (dir.name == name)
                    return true;
                bool e = dir.ExistsDir(name);
                if (e) return true;
            }
            return false;
        }
        public void MakeDir(string name)
        {
            directories.Add(new(name, this));
        }
        public void MakeFile(string name, int size)
        {
            files.Add(name, size);
        }
        public Directory FindDir(string path)
        {
            foreach (string dir in path.Split('/'))
            {
                foreach (var d in directories)
                {
                    if (d.name == dir)
                        return d;
                }
            }
            throw new Exception("dir not fount at path " + path);
        }

        public override string ToString()
        {
            return $"{name} -- {GetSize()}";
        }
    }
}
