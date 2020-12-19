using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver
{
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
}
