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
            Matrix macierzI = new Matrix(new double[] { 0.5d, 0.5d, 0.5d });
            List<Tuple<Matrix, int>> potencjalWejsciowy = new List<Tuple<Matrix, int>>();

            potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(new double[] { -1d, -1d, -1d }), 0));
            potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(new double[] { -1d, -1d, 1d }), 0));
            potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(new double[] { -1d, 1d, -1d }), 0));
            potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(new double[] { -1d, 1d, 1d }), 0));
            potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(new double[] { 1d, -1d, -1d }), 0));
            potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(new double[] { 1d, -1d, 1d }), 0));
            potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(new double[] { 1d, 1d, -1d }), 0));
            potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(new double[] { 1d, 1d, 1d }), 0));
            
            for (int i = 0; i < potencjalWejsciowy.Count; i++)
            {
                List<ExaminationStep> steps = new List<ExaminationStep>();
                var newStep = new ExaminationStep()
                {
                    PotencjalWejsciowy = potencjalWejsciowy[i].Item1
                };

                // Jak skończy wszystkie kroki to ustawić na false
                bool isExamining = true;

                Console.WriteLine($"\n\n--- Rozpoczęto badanie nr. {i + 1} ---");
                while(isExamining)
                {
                    if (steps.Count > 1)
                        newStep.PotencjalWejsciowy = steps[steps.Count - 2].PotencjalWyjsciowy;

                    newStep.Numer = steps.Count + 1;
                    Console.WriteLine($"Badany wektor:");
                    foreach (var item in potencjalWejsciowy[i].Item1.ToArray())
                    {
                        Console.Write(item + ", ");
                    }

                    Console.WriteLine($"\nKrok: {newStep.Numer}-------------------");
                    Console.WriteLine($"Potencjał wejściowy (U):");

                    newStep.ObliczonyPotencjalWejsciowy = Matrix.Multiply(potencjalWejsciowy[i].Item1, macierzWag); 
                    foreach (var item in newStep.ObliczonyPotencjalWejsciowy.ToArray())
                    {
                        Console.Write(item + ", ");
                    }

                    List<double> wyjsciowy = new List<double>();
                    Console.WriteLine($"\nPotencjał wyjściowy (V):");
                    foreach (var item in newStep.ObliczonyPotencjalWejsciowy.ToArray())
                    {
                        wyjsciowy.Add(FunkcjaAktywacjiBiPolarna(item));
                    }
                    newStep.PotencjalWyjsciowy = new Matrix(wyjsciowy.ToArray());

                    Console.Write(String.Join(",", wyjsciowy));

                    var obliczonaEnergia = 1d;// EnergiaSync(macierzWag, macierzI, new Matrix(new double[,] { potencjalWejsciowy[i].Item1.ToArray(), wyjsciowy.ToArray() }));
                    Console.WriteLine($"\nEnergia({newStep.Numer}) = {obliczonaEnergia}\n");

                    steps.Add(newStep);

                    // Sprawdzanie warunków
                    isExamining = false;
                }
            }
        }

        static List<Tuple<Matrix, int>> GenerujTablicePotencjalowWejsciowych(int n, bool isSync)
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


        static double EnergiaSync(Matrix w,Matrix I,Matrix x)
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

        static double FunkcjaAktywacjiBiPolarna(double element)
        {
            if (element <= 0)
                return -1;
            else
                return 1;   
        }


    }
}
