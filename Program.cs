using System;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;

namespace SudokuSolver
{

    class GridSpace
    {
        int[] options = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        public int value { get; set; } = 0;

        public GridSpace() { }

        public bool DiscardOption(int option)
        {
            //short curcuit if we've already solved this square
            if(HasValue())
            {
                //since we've solved already we don't want this readded to the work queue again
                return false;
            }
            //since this is copying memory could get expensive - look at using proper collection maybe?
            options = options.Where(val => val != option).ToArray();
            //if we've gotten to only a single possibility, set our value and notify that we've been solved
            if (options.Length == 1)
            {
                value = options[0];
                return true;
            }
            return false;
        }

        public bool HasValue()
        {
            return value > 0;
        }
    }

    public readonly struct WorkItem 
    {
        public WorkItem(int row, int column, int value)
        {
            Row = row;
            Column = column;
            Value = value;
        }
        public int Row { get; }
        public int Column { get; }
        public int Value { get; }
    }

    class SudokuSolver
    {
        GridSpace[,] grid = new GridSpace[9,9];

        Queue<WorkItem> workQueue = new Queue<WorkItem>();


        public SudokuSolver() 
        {
          for( int i=0; i<9; i++)
          {
              for ( int j=0; j<9; j++)
              {
                    grid[i,j] = new GridSpace();
              }
          }  
        }

        private void SetValue(int row, int column, int value)
        {
            if( row < 9 && column < 9)
            {
                grid[row, column].value = value;
                workQueue.Enqueue(new WorkItem(row, column, value));
            }
            
        }

        public void PrintOutput()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(grid[i, j].value + ",");
                }
                Console.WriteLine();
            }

        }


        public bool IsSolved()
        {
            foreach( GridSpace gridSpace in grid)
            {
                if (!gridSpace.HasValue())
                {
                    return false;
                }
            }
            return true;
        }

        public void Solve()
        {
            WorkItem workItem;
            while (workQueue.TryDequeue(out workItem))
            {
                if ( ! IsSolved() )
                {
                    //clear the row
                    for( int column = 0; column < 9; column++)
                    {
                        if( grid[workItem.Row, column].DiscardOption(workItem.Value) )
                        {
                            workQueue.Enqueue(new WorkItem(workItem.Row, column, workItem.Value));
                        }
                    }
                    //clear the column
                    for( int row = 0; row < 9; row++)
                    {
                        if ( grid[row, workItem.Column].DiscardOption(workItem.Value) )
                        {
                            workQueue.Enqueue(new WorkItem(row, workItem.Column, workItem.Value));
                        }
                    }
                    int x = workItem.Column / 3;
                    int y = workItem.Row / 3;
                    //clear the square
                    for( int row = y*3; row < (y*3)+3; row++)
                    {
                        for( int column = x*3; column < (x*3)+3; column++)
                        {
                            if( grid[row, column].DiscardOption(workItem.Value) )
                            {
                                workQueue.Enqueue(new WorkItem(row, column, workItem.Value));
                            }
                        }

                    }
                }

            }
        }

        public static SudokuSolver ParseFromFile(string filename)
        {
            SudokuSolver solver = new SudokuSolver();
            using (TextFieldParser parser = new TextFieldParser(filename))
            {
                parser.SetDelimiters(",");
                int i = 0;
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    int j = 0;
                    foreach (string field in fields)
                    {
                        if (! String.IsNullOrEmpty(field))
                        {
                            solver.SetValue(i, j, int.Parse(field));
                        }
                        j++;
                    }
                    i++;
                }
            }
            return solver;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Sodoku Solver");
            string demoFile = @"C:\Users\TimBa\Programming\repos\SudokuSolverCSharp\demo.csv";
            SudokuSolver solver = SudokuSolver.ParseFromFile(demoFile);
            solver.Solve();
            solver.PrintOutput();
        }
    }
}
