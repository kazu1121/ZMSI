using DMU.Math;
using System;
using System.Collections.Generic;

namespace ZmsiProjOne
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            

            Console.ReadKey();
        }

        void SynchHopfield(Matrix matrix)
        {

        }


        double EnergiaSync(Matrix w,Matrix I,Matrix x)
        {
            double suma = 0;
            int n = w.RowCount;


            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    suma += w.GetElement(i, j) * x.GetElement(i, 1) * x.GetElement(j, 0);
                }
            }
            suma *= -1;

            for (int i = 0; i < n; i++)
            {
                suma+=I.GetElement(i, 0) * (x.GetElement(i, 1) + x.GetElement(i, 0));
            }

            return suma;

        }


        double EnergiaAsync(Matrix w,Matrix x)
        {
            double suma = 0;
            int n = w.RowCount;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    suma += w.GetElement(i, j) * x.GetElement(i, 0) * x.GetElement(j, 0);
                }

            }


            suma *= -0.5;

            return suma;
        }


        double RoznicaEnergiSync(List<double> e)
        {
            double roznica = 0;

            int liczbaElementow= e.Count;

            roznica = e[liczbaElementow - 1] - e[liczbaElementow - 2];
            return roznica;

        }

        int FunkcjaAktywacjiPolarna(double element)
        {
            if(element <=0)
                return 0;
            else
                return 1;
        }

        int FunkcjaAktywacjiBiPolarna(double element)
        {
            if (element <= 0)
                return -1;
            else
                return 1;   
        }


    }
}
