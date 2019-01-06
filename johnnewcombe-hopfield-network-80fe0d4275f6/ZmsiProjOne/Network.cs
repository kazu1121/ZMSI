using DMU.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZmsiProjOne
{
    public class Network
    {
        public List<Matrix> TablicaPotencjalowWejsciowych { get; set; }
        public List<Examination> Badania { get; set; }

        public Network(List<Matrix> tablicaPotencjalowWejsciowych)
        {
            TablicaPotencjalowWejsciowych = tablicaPotencjalowWejsciowych;
            Badania = new List<Examination>();
        }
    }
}
