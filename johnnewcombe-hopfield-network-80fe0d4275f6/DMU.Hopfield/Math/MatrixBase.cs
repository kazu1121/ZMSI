using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIF.Math
{
    //TODO Override Hashcode and Equals
    //TODO Determine whether we should override the above (and ToString) or use new. 

    public class MatrixBase
    {
        //marked internal so that derived classes can see it
        //protected double[] _matrix = null;

        //private int _rowCount = 0;
        //private int _colCount = 0;
        //private List<double> _matrixList = null;
        //private Random _rng = new Random();

        /// <summary>
        /// Initialises the matrix based on the specified array. The matrix created 
        /// will contain a single row.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="columnMatrix"></param>
        public MatrixBase(double[] array)
        {
            //int rows = 1;
            //int cols = array.Length;

            //if (cols < 1)
            //    throw new ArgumentException(Resources.ErrorMessages.ArgumentColumnLengthZero);

            ////set the properties
            //_rowCount = rows;
            //_colCount = cols;

            //_matrix = array;
        }
        /// <summary>
        /// Initialises the matrix based on the specified array. If the columnMatrix 
        /// parameter is set to false, the matrix created will contain a single row. If the
        /// coulmnMatrix parameter is set to true, the matrix created will contain a single column.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="columnMatrix"></param>
        public MatrixBase(double[] array, bool columnMatrix)
        {
            //int cols = 1;
            //int rows = 1;

            //if (columnMatrix)
            //{
            //    rows = array.Length;
            //    if (rows < 1)
            //        throw new ArgumentException(Resources.ErrorMessages.ArgumentRowLenthZero);
            //}
            //else
            //{
            //    cols = array.Length;
            //    if (cols < 1)
            //        throw new ArgumentException(Resources.ErrorMessages.ArgumentColumnLengthZero);
            //}


            ////set the properties
            //_rowCount = rows;
            //_colCount = cols;

            //_matrix = array;
        }
        /// <summary>
        /// Initialises the matrix based on the specified array. Dimension 0 of the array will 
        /// be represented as Rows and dimension 1 of the array will be represented as Columns.
        /// </summary>
        /// <param name="array"></param>
        public MatrixBase(double[,] array)
        {
            //int rows = array.GetLength(0);
            //int cols = array.GetLength(1);

            //if (rows < 1)
            //    throw new ArgumentException(Resources.ErrorMessages.ArgumentRowLenthZero);
            //if (cols < 1)
            //    throw new ArgumentException(Resources.ErrorMessages.ArgumentColumnLengthZero);

            ////set the properties
            //_rowCount = rows;
            //_colCount = cols;

            ////populate the matrix (cant use array copy as the arrays are different dimensions)
            //_matrix = new double[rows * cols];
            //for (int row = 0; row < rows; row++)
            //{
            //    for (int col = 0; col < cols; col++)
            //    {
            //        _matrix[row * cols + col] = array[row, col];
            //    }
            //}

        }
        /// <summary>
        /// Initialises the matrix to the specified size. All elements are set to zero.
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        public MatrixBase(int rows, int cols)
            : this(rows, cols, false)
        {
        }
        /// <summary>
        /// Initialises the matrix to the specified size. If the addRandomValues parameter 
        /// is set to true, all elements are set to a random value between 0 and 1.
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="addRandomValues"></param>
        public MatrixBase(int rows, int cols, bool addRandomValues)
        {
            //if (rows < 1)
            //    throw new ArgumentException(Resources.ErrorMessages.ArgumentRowLenthZero);
            //if (cols < 1)
            //    throw new ArgumentException(Resources.ErrorMessages.ArgumentColumnLengthZero);

            ////set the properties
            //_rowCount = rows;
            //_colCount = cols;

            ////store the whole thing in a single array
            ////one row at a time for example the matrix
            ////
            //// 1 2 3
            //// 6 7 8
            ////
            ////would be stored as follows
            ////
            //// 1 2 3 6 7 8

            //_matrix = new double[rows * cols];

            //if (addRandomValues)
            //{
            //    for (int cell = 0; cell < _matrix.Length; cell++)
            //    {
            //        _matrix[cell] = _rng.NextDouble();
            //    }
            //}
            //else
            //{
            //    for (int cell = 0; cell < _matrix.Length; cell++)
            //    {
            //        _matrix[cell] = 0.0;
            //    }
            //}
        }
        ///// <summary>
        ///// Returns an integer representing the number of rows in the matrix.
        ///// </summary>
        //public int RowCount
        //{
        //    get { return _rowCount; }
        //}
        ///// <summary>
        ///// Returns an integer representing the number of columns in the matrix.
        ///// </summary>
        //public int ColumnCount
        //{
        //    get { return _colCount; }
        //}
        /// <summary>
        /// Returns the matrix as a one dimension generic list.
        /// </summary>
        /// <returns></returns>
        public List<double> ToList()
        {
            _matrixList.AddRange(_matrix);
            return _matrixList;
        }
        /// <summary>
        /// Returns the matrix as a one dimension array. Returns Row 1 followed by Row 2 etc.
        /// </summary>
        /// <returns></returns>
        public double[] ToArray()
        {
            return _matrix;
        }
        /// <summary>
        /// Returns the matrix as a comma delimited string. Uses the default format \"F4\" and the default row seperator CrLf.
        /// </summary>
        /// <returns></returns>
        public new string ToString()
        {
            return this.ToString("F4", ",", "\r\n");
        }
        /// <summary>
        /// Returns the matrix with format applied to the numeric values. Uses the default format \"F4\" and the default row seperator CrLf.
        /// </summary>
        /// <returns></returns>
        public string ToString(string format)
        {
            return this.ToString("F4", ",", "\r\n");
        }
        /// <summary>
        /// Returns the matrix with format applied to the numeric values and rowDelimiter
        /// </summary>
        /// <returns></returns>
        public string ToString(string format, string columnDelimiter, string rowDelimiter)
        {
            StringBuilder toString = new StringBuilder();
            for (int cell = 0; cell < _matrix.Length; cell++)
            {
                toString.Append(_matrix[cell].ToString(format));
                if ((cell + 1) % this.ColumnCount == 0)
                {
                    toString.Append(rowDelimiter);
                }
                else
                {
                    toString.Append(columnDelimiter);
                }
            }
            string result = toString.ToString();

            //tidy up spaces at the end, any other character is left in place
            if (result.EndsWith(" "))
                return result.Trim();
            else
                return result;
        }
        /// <summary>
        /// Returns true if the specified matrix is equal in value to this instance.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public bool Equals(MatrixBase matrix)
        {
            //compare rows columns and matrix values
            bool result = true;

            if (this.RowCount == matrix.RowCount && this.ColumnCount == matrix.ColumnCount)
            {
                //check lengths
                if (this._matrix.Length == matrix._matrix.Length)
                {
                    //compare values
                    for (int index = 0; index < this._matrix.Length; index++)
                    {
                        if (this._matrix[index] != matrix._matrix[index])
                            result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }

            return result;

        }
    }
}
