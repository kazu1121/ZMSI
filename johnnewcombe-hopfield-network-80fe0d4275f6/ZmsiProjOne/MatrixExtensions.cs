using DMU.Math;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static bool IsSymetric(this Matrix matrix)
        {
            if (matrix.RowCount != matrix.ColumnCount)
                return false;

            var tempMatrix = new Matrix(matrix.RowCount, matrix.ColumnCount);

            for (int i = 0; i < matrix.RowCount; i++)
            {
                for (int j = 0; j < matrix.ColumnCount; j++)
                {
                    //tempMatrix.SetElement(tempMatrix.RowCount - 1 - i, tempMatrix.ColumnCount - 1 - j, matrix.GetElement(i, j));
                    tempMatrix.SetElement(j, i, matrix.GetElement(i, j));
                }
            }

            return matrix.AreMatrixesEquals(tempMatrix);
        }

        public static T[,] To2D<T>(this T[][] source)
        {
            try
            {
                int FirstDim = source.Length;
                int SecondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

                var result = new T[FirstDim, SecondDim];
                for (int i = 0; i < FirstDim; ++i)
                    for (int j = 0; j < SecondDim; ++j)
                        result[i, j] = source[i][j];

                return result;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("The given jagged array is not rectangular.");
            }
        }
    }
}
