using DMU.Math;
using System;
using System.Collections.Generic;

namespace ZmsiProjOne
{
    class Program
    {
        static void Main(string[] args)
        {
            SynchHopfield();

            Console.ReadKey();
        }

        static void SynchHopfield()
        {
            Matrix macierzWag = new Matrix(new double[,] { { 0d, 1d, 2d }, { 1d, 0d, -1d }, { 2d, -1d, 0d } });
            List<Tuple<Matrix, int>> potencjalWejsciowy = new List<Tuple<Matrix, int>>();

            potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(new double[] { -1d, -1d, -1d }), 0));
            potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(new double[] { -1d, -1d, 1d }), 0));
            potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(new double[] { -1d, 1d, -1d }), 0));
            potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(new double[] { -1d, 1d, 1d }), 0));
            potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(new double[] { 1d, -1d, -1d }), 0));
            potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(new double[] { 1d, -1d, 1d }), 0));
            potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(new double[] { 1d, 1d, -1d }), 0));
            potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(new double[] { 1d, 1d, 1d }), 0));

            for(int i = 0; i < potencjalWejsciowy.Count; i++)
            {
                // Jak skończy wszystkie kroki to ustawić na false
                bool isExamining = true;
                int stepCounter = 0;

                Console.WriteLine($"\n\n--- Rozpoczęto badanie nr. {i + 1} ---");
                while(isExamining)
                {
                    stepCounter++;
                    Console.WriteLine($"Badany wektor:");
                    foreach (var item in potencjalWejsciowy[i].Item1.ToArray())
                    {
                        Console.Write(item + ", ");
                    }

                    Console.WriteLine($"\nKrok: {stepCounter}-------------------");
                    Console.WriteLine($"Potencjał wejściowy (U):");

                    var obliczonyPotencjalWejsciowy = Matrix.Multiply(potencjalWejsciowy[i].Item1, macierzWag); 
                    foreach (var item in obliczonyPotencjalWejsciowy.ToArray())
                    {
                        Console.Write(item + ", ");
                    }

                    Console.WriteLine($"\nPotencjał wyjściowy (V):");
                    foreach (var item in obliczonyPotencjalWejsciowy.ToArray())
                    {
                        Console.Write(FunkcjaAktywacjiBiPolarna(item) + ", ");
                    }

                    // Sprawdzanie warunków
                    isExamining = false;
                }
            }
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

        static int FunkcjaAktywacjiBiPolarna(double element)
        {
            if (element <= 0)
                return -1;
            else
                return 1;   
        }


    }
}
