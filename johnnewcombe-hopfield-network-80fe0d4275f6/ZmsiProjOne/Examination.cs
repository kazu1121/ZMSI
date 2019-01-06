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
        public string Wniosek { get; set; }

        public Examination()
        {
            ListaKrorkow = new List<ExaminationStep>();
        }
    }
}
