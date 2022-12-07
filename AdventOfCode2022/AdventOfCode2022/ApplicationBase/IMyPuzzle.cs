using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public interface IMyPuzzle
    {
        public void Setup();
        public object? SolveFirst();
        public object? SolveSecond();
    }
}
