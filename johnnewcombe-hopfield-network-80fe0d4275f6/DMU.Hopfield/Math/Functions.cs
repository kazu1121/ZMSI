using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIF.Math
{
    /// <summary>
    /// Utility class to perform various math conversions.
    /// </summary>
    public static class Functions
    {
        ///// <summary>
        ///// Coverts a boolean value to a double, returns 1.0 for true and -1.0 for false.
        ///// </summary>
        //public static double BooleanToBiPolar(bool value)
        //{
        //    if (value)
        //        return 1.0;
        //    else
        //        return -1.0;
        //}

        ///// <summary>
        ///// Convert a boolean array to a double array, returns 1.0 for true and -1.0 for false.
        ///// </summary>
        //public static double[] BooleanToBiPolarArray(bool[] value)
        //{
        //    double[] result = new double[value.Length];

        //    for (int i = 0; i < value.Length; i++)
        //    {
        //        result[i] = ToDouble(value[i]);
        //    }

        //    return result;
        //}
        ///// <summary>
        ///// Convert a boolean array to a double array, returns 1.0 for true and -1.0 for false.
        ///// </summary>
        //public static double[,] ToDoubleArray2D(bool[,] value)
        //{
        //    double[,] result = new double[value.GetUpperBound(0), value.GetUpperBound(1)];

        //    for (int row = 0; row < value.GetUpperBound(0); row++)
        //    {
        //        for (int col = 0; col < value.GetUpperBound(1); col++)
        //        {
        //            result[row, col] = ToDouble(value[row, col]);
        //        }
        //    }

        //    return result;
        //}
        ///// <summary>
        ///// Converts a biploar value to a boolean, returns 1.0 for true and -1.0 for false.
        ///// </summary>
        //public static bool ToBoolean(double value)
        //{
        //    if (value > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        ///// <summary>
        ///// Converts a bipolar array to a boolean array, returns 1.0 for true and -1.0 for false.
        ///// </summary>
        //public static bool[] ToBooleanArray(double[] value)
        //{
        //    bool[] result = new bool[value.Length];

        //    for (int i = 0; i < value.Length; i++)
        //    {
        //        result[i] = ToBoolean(value[i]);
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// Converts a 2D bipolar array to a 2D boolean array, returns 1.0 for true and -1.0 for false.
        ///// </summary>
        //public static bool[,] ToBooleanArray2D(double[,] value)
        //{
        //    bool[,] result = new bool[value.GetUpperBound(0), value.GetUpperBound(1)];

        //    for (int row = 0; row < value.GetUpperBound(0); row++)
        //    {
        //        for (int col = 0; col < value.GetUpperBound(0); col++)
        //        {
        //            result[row, col] = ToBoolean(value[row, col]);
        //        }
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// Converts a number to either a 1 or a 0.  returns 1 if value is > 0, otherwise returns 0.
        ///// </summary>
        //public static double ToNormalisedBinary(double value)
        //{
        //    if (value > 0)
        //    {
        //        return 1;
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}

        //#region  Heaton Research, (not yet implemented).

        /////// <summary>
        /////// Convert a single number from bipolar to binary.
        /////// </summary>
        ////public static double ToBinary(double value)
        ////{
        ////    return (value + 1) / 2.0;
        ////}

        /////// <summary>
        /////// Convert a number to bipolar.
        /////// </summary>
        /////// <param name="d">A binary number.</param>
        /////// <returns></returns>
        ////public static double ToBiPolar(double value)
        ////{
        ////    return (2 * ToNormalisedBinary(value)) - 1;
        ////}

        /////// <summary>
        /////// Normalize a number and convert to binary.
        /////// </summary>
        /////// <param name="d">A bipolar number.</param>
        /////// <returns>A binary number stored as a double</returns>
        ////public static double ToNormalizedBinary(double d)
        ////{
        ////    return ToNormalisedBinary(ToBinary(d));
        ////}
        ///#endregion
    }
}
