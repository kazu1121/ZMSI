using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMU.Math
{
    public class TanSigmoid : ActivationFunction
    {
        public override double Function(double value)
        {
            return ActivationFunction.TanSigmoid(value);
        }

        public override double DerivativeFunction(double value)
        {
            return (1.0 - System.Math.Pow(ActivationFunction.TanSigmoid(value), 2.0));
        }
    }
}
