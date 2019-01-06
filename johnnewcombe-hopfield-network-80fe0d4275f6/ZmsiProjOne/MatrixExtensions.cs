using DMU.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZmsiProjOne
{
    public static class MatrixExtensions
    {
        public static bool AreMatrixesEquals(this Matrix matrix, Matrix matrix2)
        {
            if (matrix.RowCount != matrix2.RowCount || matrix.ColumnCount != matrix2.ColumnCount)
                return false;

            for (int i = 0; i < matrix.RowCount; i++)
            {
                for (int j = 0; j < matrix.ColumnCount; j++)
                {
                    if (matrix.GetElement(i, j) != matrix2.GetElement(i, j))
                        return false;
                }
            }

            return true;
        }
    }
}
