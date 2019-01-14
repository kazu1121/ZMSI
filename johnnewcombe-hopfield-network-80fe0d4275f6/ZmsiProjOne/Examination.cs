using DMU.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZmsiProjOne
{
    public class Examination
    {
        public Matrix BadanyPunkt { get; set; }
        public string BadanyPunktString { get; set; }
        public bool? CzyPunktStaly { get; set; }
        public bool? CzyPunktZbiezny { get; set; }
        public bool? CzyPunktTworzyCykl { get; set; }
        public bool? CzyPunktWpadaWCykl { get; set; }

        public Examination PunktDoKtoregoZbiega { get; set; }
        public List<Examination> Cykl { get; set; }
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
            Cykl = new List<Examination>();
        }

        public override string ToString()
        {
            string wyjscie = "\n-----------------------------------------\n";
            wyjscie += "Badanie punktu: "+BadanyPunkt.ToString()+"\n";

            foreach (var item in ListaKrorkow)
            {
                wyjscie += item.ToString();
            }

            wyjscie += $"Wniosek! Punkt:[{BadanyPunkt.ToString()}] zbiega do punktu:\t{Wniosek}\n";

            return wyjscie;
        }

        public void BadanyPunktToString()
        {
            BadanyPunktString = String.Join(" ", BadanyPunkt.ToArray());
        }
    }
}

