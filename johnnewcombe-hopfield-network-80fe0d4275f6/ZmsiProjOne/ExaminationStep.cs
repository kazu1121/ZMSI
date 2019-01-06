using DMU.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZmsiProjOne
{
    public class ExaminationStep
    {
        // t
        public int Numer { get; set; }

        // V(t-1)
        public Matrix PotencjalWejsciowy { get; set; }

        // U(t)
        public Matrix ObliczonyPotencjalWejsciowy { get; set; }

        // V(t)
        public Matrix PotencjalWyjsciowy { get; set; }
        public double Energia { get; set; }



        public override string ToString()
        {
            string wyjscie = "";
            wyjscie += "\n-----------------------------------------------------------------------------------\n";
            wyjscie += $"\n\t numer kroku { Numer }";
            wyjscie += $"\nV({Numer - 1}):\t" + PotencjalWejsciowy.ToString();
            wyjscie += $"\nU({Numer }):\t" + ObliczonyPotencjalWejsciowy.ToString();
            wyjscie += $"\nV({Numer }):\t" + PotencjalWyjsciowy.ToString();
            wyjscie += $"\nE({Numer }):\t{Energia}";
     
            wyjscie += "\n-----------------------------------------------------------------------------------\n";


            return wyjscie;
        }
    }
}
