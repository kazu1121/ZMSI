using DMU.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZmsiProjOne
{
    public class Examination
    {
        public Matrix BadanyPunkt { get; set; }
        public List<ExaminationStep> ListaKrorkow { get; set; }
        /*
         * s- staly
         * c* = cykl (* to numer cyklu)
         * numer = zbiega do numer
         */
        public string Wniosek { get; set; }

        public Examination()
        {
            ListaKrorkow = new List<ExaminationStep>();
        }

        public override string ToString()
        {
            string wyjscie = "-----------------------------------------\n";
            wyjscie += "Badanie punktu: "+BadanyPunkt.ToString()+"\n";

            foreach (var item in ListaKrorkow)
            {
                wyjscie += item.ToString();
            }

            wyjscie += $"Wniosek:\t{wyjscie}\n";

            return wyjscie;
        }






    }

}

