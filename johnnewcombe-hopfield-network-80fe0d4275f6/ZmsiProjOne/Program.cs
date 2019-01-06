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
            Matrix macierzWag = new Matrix(new double[,] { { 0d, 1d }, { -0.5d, 0d } });
            Matrix macierzI = new Matrix(new double[] { 0d,0d });
            //List<Tuple<Matrix, int>> potencjalWejsciowy = new List<Tuple<Matrix, int>>();
            var potencjalWejsciowy = GenerujTablicePotencjalowWejsciowych(2, false);
            //potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(new double[] { -1d, -1d, -1d }), 0));
            //potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(new double[] { -1d, -1d, 1d }), 0));
            //potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(new double[] { -1d, 1d, -1d }), 0));
            //potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(new double[] { -1d, 1d, 1d }), 0));
            //potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(new double[] { 1d, -1d, -1d }), 0));
            //potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(new double[] { 1d, -1d, 1d }), 0));
            //potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(new double[] { 1d, 1d, -1d }), 0));
            //potencjalWejsciowy.Add(new Tuple<Matrix, int>(new Matrix(new double[] { 1d, 1d, 1d }), 0));

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
                    newStep.Numer = steps.Count + 1;

                    if (steps.Count > 0)
                        newStep.PotencjalWejsciowy = steps[steps.Count - 1].PotencjalWyjsciowy;

                    Console.WriteLine($"Badany wektor:");
                    Console.Write(String.Join(',', potencjalWejsciowy[i].Item1.ToArray()));

                    Console.WriteLine($"\nKrok: {newStep.Numer}-------------------");
                    Console.WriteLine($"Potencjał wejściowy (U):");

                    newStep.ObliczonyPotencjalWejsciowy = Matrix.Multiply(potencjalWejsciowy[i].Item1, macierzWag);
                    Console.Write(String.Join(',', newStep.ObliczonyPotencjalWejsciowy.ToArray()));

                    Console.WriteLine($"\nPotencjał wyjściowy (V):");
                    List<double> wyjsciowy = new List<double>();
                    foreach (var item in newStep.ObliczonyPotencjalWejsciowy.ToArray())
                    {
                        wyjsciowy.Add(FunkcjaAktywacjiBiPolarna(item));
                    }
                    newStep.PotencjalWyjsciowy = new Matrix(wyjsciowy.ToArray());

                    Console.Write(String.Join(",", wyjsciowy));

                    var obliczonaEnergia = EnergiaSync(macierzWag, macierzI,newStep);
                    Console.WriteLine($"\nEnergia({newStep.Numer}) = {obliczonaEnergia}\n");


                    // Sprawdzanie warunków stopu kroku
                    if (Matrix.Equals(newStep.PotencjalWejsciowy, newStep.PotencjalWyjsciowy))
                    {
                        Console.WriteLine("1) Sieć podczas działania wyprodukowała taki sam wektor jaki trafił na wejście w kroku T");
                        isExamining = false;
                    }
                    //else if (steps.Count > 0 && newStep.PotencjalWejsciowy == newStep.PotencjalWyjsciowy && newStep.Energia == steps[steps.Count - 1].Energia && MacierzWagJestSymetryczna)
                    //{
                    //    Console.WriteLine("Wyprodukowana przez sieć wartość energii jest równa w dwóch kolejnych krokach jej działania (warunek ten należy sprawdzać przy założeniu, że macierz wag jest symetryczna!).");
                    //    isExamining = false;
                    //}

                    steps.Add(newStep);
                    //isExamining = false;
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


        static double EnergiaSync(Matrix w,Matrix I,ExaminationStep x)
        {
            double suma = 0;
            int n = w.RowCount;


            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    suma += w.GetElement(i, j) * x.PotencjalWyjsciowy.GetElement(0, i) * x.PotencjalWejsciowy.GetElement(0, j);
                }
            }
            suma *= -1;

            for (int i = 0; i < n; i++)
            {
                suma+=I.GetElement(0,i) * (x.PotencjalWyjsciowy.GetElement(0,i) + x.PotencjalWejsciowy.GetElement(0, i));
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
