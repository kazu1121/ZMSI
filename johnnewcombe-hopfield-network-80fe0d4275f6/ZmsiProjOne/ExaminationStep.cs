using DMU.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZmsiProjOne
{
    public class ExaminationStep
    {
        public int Numer { get; set; }
        public Matrix PotencjalWejsciowy { get; set; }
        public Matrix ObliczonyPotencjalWejsciowy { get; set; }
        public Matrix PotencjalWyjsciowy { get; set; }
        public double Energia { get; set; }
    }
}
