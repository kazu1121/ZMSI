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

        public void WyswietlPrzebiegNaKonsoli()
        {
            for (int i = 0; i < BadanePunkty.Count; i++)
            {
                Console.WriteLine($"\n----------------------------------------------------\n------------- Rozpoczęto badanie nr. {i + 1} -------------\n----------------------------------------------------");
                Console.WriteLine($"Badany wektor: {String.Join(" ", BadanePunkty[i].BadanyPunkt.ToArray())}\n");

                for (int j = 0; j < BadanePunkty[i].ListaKrorkow.Count; j++)
                {
                    Console.WriteLine($"Krok: {BadanePunkty[i].ListaKrorkow[j].Numer} -------------------");
                    Console.WriteLine($"Potencjał wejściowy (U): {String.Join(" ", BadanePunkty[i].ListaKrorkow[j].ObliczonyPotencjalWejsciowy.ToArray())}");
                    Console.WriteLine($"Potencjał wyjściowy (V): {String.Join(" ", BadanePunkty[i].ListaKrorkow[j].PotencjalWyjsciowy.ToArray())}");

                    Console.WriteLine($"\nEnergia({j + 1}) = {BadanePunkty[i].ListaKrorkow[j].Energia}\n");
                }

                Console.WriteLine($"Wniosek: {BadanePunkty[i].Wniosek}");
            }
        }

        public override string ToString()
        {
            string wyjscie = "";

            foreach (var item in BadanePunkty)
            {
                wyjscie += item.ToString() + "\n";

            }

            return wyjscie;

        }
    }
}
