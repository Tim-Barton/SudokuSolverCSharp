using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SudokuSolver
{
    class GridSpace
    {
        public int[] options = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        public int value { get; set; } = 0;

        public GridSpace() { }

        public bool DiscardOption(int option)
        {
            //short curcuit if we've already solved this square
            if (HasValue())
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

        override public string ToString()
        {
            if (HasValue())
            {
                return value.ToString();
            }
            else
            {
                return "[" + string.Join(",", options.Select(x => x.ToString()).ToArray()) + "]" ;
            }
        }
    }
}
