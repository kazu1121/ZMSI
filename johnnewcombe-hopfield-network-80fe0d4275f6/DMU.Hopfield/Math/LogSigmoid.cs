using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMU.Math
{
    public class LogSigmoid : ActivationFunction
    {

        public override double Function(double value)
        {
            return ActivationFunction.LogSigmoid(value);
        }

        public override double DerivativeFunction(double value)
        {
            return value * (1.0 - value);
        }
    }
}
