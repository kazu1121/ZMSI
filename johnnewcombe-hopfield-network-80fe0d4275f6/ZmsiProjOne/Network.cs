using DMU.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZmsiProjOne
{
    public class Network
    {
        public List<Examination> BadanePunkty { get; set; }

        public Network(List<Matrix> tablicaPotencjalowWejsciowych)
        {
            BadanePunkty = new List<Examination>();

            foreach (var item in tablicaPotencjalowWejsciowych)
            {
                BadanePunkty.Add(new Examination()
                {
                    BadanyPunkt = item
                });
            }
        }


        public override string ToString()
        {
            string wyjscie = "";

            foreach (var item in BadanePunkty)
            {
                wyjscie += item.ToString();

            }

            return wyjscie;

        }
    }
}
