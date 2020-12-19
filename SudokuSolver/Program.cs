using System;
using System.Linq;

using System.Collections.Generic;

namespace SudokuSolver
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Sodoku Solver");
            string demoFile = @"C:\Users\TimBa\Programming\repos\SudokuSolverCSharp\demo.csv";
            Solver solver = Solver.ParseFromFile(demoFile);
            solver.Solve();
            solver.PrintOutput();
        }
    }
}
