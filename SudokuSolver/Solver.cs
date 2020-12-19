using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace SudokuSolver
{
    public class Solver
    {
        GridSpace[,] grid = new GridSpace[9, 9];

        Queue<WorkItem> workQueue = new Queue<WorkItem>();


        public Solver()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    grid[i, j] = new GridSpace();
                }
            }
        }

        public void SetValue(int row, int column, int value)
        {
            if (row < 9 && column < 9)
            {
                grid[row, column].value = value;
                workQueue.Enqueue(new WorkItem(row, column, value));
            }

        }

        public int GetValue(int row, int column)
        {
            if (row < 9 && column < 9)
            {
                return grid[row, column].value;
            }
            return 0;
        }

        public int[] GetOptions(int row, int column)
        {
            if (row< 9 && column< 9)
            {
                return grid[row, column].options;
            }
            return new int[]{ };
        }


        public void PrintOutput()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(grid[i, j].ToString() + ",");
                }
                Console.WriteLine();
            }

        }


        public bool IsSolved()
        {
            foreach (GridSpace gridSpace in grid)
            {
                if (!gridSpace.HasValue())
                {
                    return false;
                }
            }
            return true;
        }

        public Queue<WorkItem> HandleWorkItem(WorkItem workItem)
        {
            Queue<WorkItem> newItems = new Queue<WorkItem>();
            //clear the row
            for (int column = 0; column < 9; column++)
            {
                if (grid[workItem.Row, column].DiscardOption(workItem.Value))
                {
                    newItems.Enqueue(new WorkItem(workItem.Row, column, grid[workItem.Row, column].value));
                }
            }
            //clear the column
            for (int row = 0; row < 9; row++)
            {
                if (grid[row, workItem.Column].DiscardOption(workItem.Value))
                {
                    newItems.Enqueue(new WorkItem(row, workItem.Column, grid[row, workItem.Column].value));
                }
            }
            int x = workItem.Column / 3;
            int y = workItem.Row / 3;
            //clear the square
            for (int row = y * 3; row < (y * 3) + 3; row++)
            {
                for (int column = x * 3; column < (x * 3) + 3; column++)
                {
                    if (grid[row, column].DiscardOption(workItem.Value))
                    {
                        newItems.Enqueue(new WorkItem(row, column, grid[row, column].value));
                    }
                }

            }


            return newItems;
        }

        public void Solve()
        {
            WorkItem workItem;
            while (workQueue.TryDequeue(out workItem))
            {
                if (!IsSolved())
                {
                    Queue<WorkItem> newItems = HandleWorkItem(workItem);
                    foreach(WorkItem item in newItems)
                    {
                        workQueue.Enqueue(item);
                    }
                }
                PrintOutput();
                Console.WriteLine("----------------");
            }
        }

        public static Solver ParseFromFile(string filename)
        {
            Solver solver = new Solver();
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
                        if (!String.IsNullOrEmpty(field))
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
}
