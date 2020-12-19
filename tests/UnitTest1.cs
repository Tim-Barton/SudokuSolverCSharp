using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;

namespace tests
{
    [TestClass]
    public class UnitTestSolver
    {
        [TestMethod]
        public void TestMethodHandleWorkItem()
        {
            SudokuSolver.Solver solver;
            SudokuSolver.WorkItem item;
            Queue<SudokuSolver.WorkItem> retVal;
            for ( int row = 0; row <9; row++)
            {
                for( int column = 0; column<9; column++)
                {
                    solver = new SudokuSolver.Solver();
                    solver.SetValue(row, column, 1);
                    item = new SudokuSolver.WorkItem(row, column, 1);
                    retVal = solver.HandleWorkItem(item);
                    Assert.IsTrue(retVal.Count == 0);
                    for (int i = 0; i < 9; i++)
                    {
                        if (i == row)
                        {
                            int value = solver.GetValue(i, column);
                            Assert.AreEqual(value, 1);
                        }
                        else
                        {
                            int[] options = solver.GetOptions(i, column);
                            foreach( int option in options)
                            {
                                Assert.AreNotEqual(options, 1);
                            }
                        }
                    }
                    for (int j = 0; j < 9; j++)
                    {
                        if (j == column)
                        {
                            int value = solver.GetValue(row, j);
                            Assert.AreEqual(value, 1);
                        }
                        else
                        {
                            int[] options = solver.GetOptions(row, j);
                            foreach (int option in options)
                            {
                                Assert.AreNotEqual(options, 1);
                            }
                        }
                    }
                }
                
            }
            solver = new SudokuSolver.Solver();
            solver.SetValue(0, 0, 1);
            for (int i = 1; i<8; i++)
            {
                item = new SudokuSolver.WorkItem(0, 0, i);
                retVal = solver.HandleWorkItem(item);
                Assert.IsTrue(retVal.Count == 0);
            }
            item = new SudokuSolver.WorkItem(0, 0, 8);
            retVal = solver.HandleWorkItem(item);
            Assert.IsTrue(retVal.Count > 0);


        }
    }
}
