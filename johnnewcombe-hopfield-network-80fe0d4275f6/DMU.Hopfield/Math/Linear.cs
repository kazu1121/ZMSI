using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMU.Math
{
    public class Linear: ActivationFunction
    {
        public override double Function(double value)
        {
            return ActivationFunction.Linear(value);
        }
        public override double DerivativeFunction(double value)
        {
            throw new NotImplementedException();
        }
    }
}
