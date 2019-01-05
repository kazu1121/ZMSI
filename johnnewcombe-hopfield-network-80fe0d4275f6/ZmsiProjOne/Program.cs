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

            SyncHopfield2();
            
            

            Console.ReadKey();
        }

        void SynchHopfield()
        {
            Matrix matrix = new Matrix(new double[,] { { 1d, 2d, 3d }, { 1d, 2d, 3d }, { 1d, 2d, 3d } });
            List<Tuple<Matrix, int>> potencjalWejsciowy = new List<Tuple<Matrix, int>>();


            potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(new double[] { 0d, 0d, 0d }), 0));            
        }


        static void SyncHopfield2()
        {
            Matrix matrix = new Matrix(new double[,] { { 1d, 2d, 3d }, { 1d, 2d, 3d }, { 1d, 2d, 3d } });

            int n = matrix.RowCount;

            var potencjalWejsciowy = GenerujTablicePotencjalowWejsciowych(n, false);




            foreach (var item in potencjalWejsciowy)
            {
                Matrix tempMatrix = item.Item1;

                //    Console.Write(tempMatrix.GetElement(z,0)+'\t');

                Console.WriteLine(tempMatrix.ToString());

            }
        }

        static List<Tuple<Matrix, int>> GenerujTablicePotencjalowWejsciowych(int n,bool isSync)
        {
            List<Tuple<Matrix, int>> potencjalWejsciowy = new List<Tuple<Matrix, int>>();

            for (int i = 0; i < Math.Pow(2, n); i++)
            {
                var reprezentacjaBianarna = Convert.ToString(i, 2);

                reprezentacjaBianarna = reprezentacjaBianarna.PadLeft(n, '0');

                double[] temp = new double[n];

                for (int j = 0; j < n; j++)
                {
                    char tempWartosc;

                    tempWartosc = reprezentacjaBianarna[j];

                    if (tempWartosc == '0')
                        if (isSync == true)
                            temp[j] = 0d;
                        else
                            temp[j] = -1d;
                    else
                        temp[j] = 1d;

                }

                potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(temp), 0));
            }

            return potencjalWejsciowy;

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
