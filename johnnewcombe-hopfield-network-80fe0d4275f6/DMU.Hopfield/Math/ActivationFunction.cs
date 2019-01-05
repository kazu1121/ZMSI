using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMU.Math
{
    /// <summary>
    /// This is a generic Activation Function class. 
    /// All Activation Function types should derive from this class.
    /// The class also provides Activation Functiona as static methods.
    /// </summary>
    public abstract class ActivationFunction
    {
        //derived types must override these
        public abstract double Function(double value);
        public abstract double DerivativeFunction(double value);

        /// <summary>
        /// Log Sigmoid function.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double LogSigmoid(double value)
        {
            return 1.0 / (1 + System.Math.Exp(-1.0 * value));
        }
        /// <summary>
        /// Tan Sigmoid function.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double TanSigmoid(double value)
        {
            return (System.Math.Exp(value * 2.0) - 1.0) / (System.Math.Exp(value * 2.0) + 1.0);
        }
        /// <summary>
        /// Linear function.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double Linear(double value)
        {
            //This function may need to be enhanced to handle scaled forms of linear function.
            return value;
        }

    }
}
