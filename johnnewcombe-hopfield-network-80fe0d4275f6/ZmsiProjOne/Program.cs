using DMU.Math;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ZmsiProjOne
{
    class Program
    {
        static void Main(string[] args)
        {
            //SynchHopfield();
            HopfieldAsync ha = new HopfieldAsync();
            Matrix w = new Matrix(new double[,] { { 0d, 2d,-1d }, { 2d, 0d,1d }, { -1d, 1d, 0d } });
            ha.runHopfield(w,new int[] { 0, 2, 1 }, true);

            Console.Write(ha.network.ToString());

            Console.ReadKey();
        }

        static void SynchHopfield()
        {
            Matrix macierzWag = new Matrix(new double[,] { { 0d, 4d }, { 4d, 0d } });
            Matrix macierzI = new Matrix(new double[] { 0.5d, 0.5d });

            var network = new Network(GenerujTablicePotencjalowWejsciowych(2, true));

            for (int i = 0; i < network.BadanePunkty.Count; i++)
            {
                var noweBadanie = network.BadanePunkty[i];
                var nowyKrok = new ExaminationStep();

                // Jak skończy wszystkie kroki to ustawić na false
                bool isExamining = true;

                Console.WriteLine($"\n----------------------------------------------------\n--- Rozpoczęto badanie nr. {i + 1} ---\n----------------------------------------------------");
                while (isExamining)
                {
                    nowyKrok.Numer = noweBadanie.ListaKrorkow.Count + 1;

                    if (noweBadanie.ListaKrorkow.Count > 0)
                        nowyKrok.PotencjalWejsciowy = noweBadanie.ListaKrorkow[noweBadanie.ListaKrorkow.Count - 1].PotencjalWyjsciowy;
                    else
                        nowyKrok.PotencjalWejsciowy = network.BadanePunkty[i].BadanyPunkt;

                    Console.WriteLine($"\nKrok: {nowyKrok.Numer}-------------------");

                    Console.Write($"Badany wektor: ");
                    Console.Write(String.Join(',', nowyKrok.PotencjalWejsciowy.ToArray()));

                    Console.Write($"\nPotencjał wejściowy (U): ");
                    nowyKrok.ObliczonyPotencjalWejsciowy = Matrix.Add(Matrix.Multiply(nowyKrok.PotencjalWejsciowy, macierzWag), macierzI);
                    Console.Write(String.Join(',', nowyKrok.ObliczonyPotencjalWejsciowy.ToArray()));

                    Console.Write($"\nPotencjał wyjściowy (V): ");
                    List<double> wyjsciowy = new List<double>();
                    foreach (var item in nowyKrok.ObliczonyPotencjalWejsciowy.ToArray())
                    {
                        wyjsciowy.Add(FunkcjaAktywacjiBiPolarna(item));
                    }
                    nowyKrok.PotencjalWyjsciowy = new Matrix(wyjsciowy.ToArray());
                    Console.Write(String.Join(",", wyjsciowy));

                    var obliczonaEnergia = EnergiaSync(macierzWag, macierzI, nowyKrok);
                    Console.WriteLine($"\nEnergia({nowyKrok.Numer}) = {obliczonaEnergia}\n");

                    // Sprawdzanie warunków stopu badania
                    var punktDoKtoregoZbiega = network.BadanePunkty.FirstOrDefault(x => x.BadanyPunkt.AreMatrixesEquals(nowyKrok.PotencjalWyjsciowy));

                    if (nowyKrok.PotencjalWejsciowy.AreMatrixesEquals(nowyKrok.PotencjalWyjsciowy))
                    { // Punkt stały
                        Console.WriteLine("1) Sieć podczas działania wyprodukowała taki sam wektor jaki trafił na wejście w kroku T");
                        isExamining = false;

                        if (noweBadanie.ListaKrorkow.Count == 0)
                            noweBadanie.Wniosek = $"Punkt {String.Join(',', nowyKrok.PotencjalWejsciowy.ToArray())} jest stały.";
                    }
                    else if (noweBadanie.ListaKrorkow.Count > 0
                        && nowyKrok.PotencjalWejsciowy == nowyKrok.PotencjalWyjsciowy
                        && nowyKrok.Energia == noweBadanie.ListaKrorkow[noweBadanie.ListaKrorkow.Count - 1].Energia
                        && macierzWag.IsSymetric())
                    {
                        Console.WriteLine("Wyprodukowana przez sieć wartość energii jest równa w dwóch kolejnych krokach jej działania (warunek ten należy sprawdzać przy założeniu, że macierz wag jest symetryczna!).");
                        isExamining = false;
                    }
                    else if (punktDoKtoregoZbiega != null)
                    { // Punkt zbiega do innego punktu
                        noweBadanie.Wniosek = $"Punkt {String.Join(',', nowyKrok.PotencjalWejsciowy.ToArray())} zbiega do punktu {String.Join(',', punktDoKtoregoZbiega.BadanyPunkt.ToArray())}.";
                    }

                    noweBadanie.ListaKrorkow.Add(nowyKrok);
                }

                network.BadanePunkty[i] = noweBadanie;
                Console.WriteLine($"Wniosek: {noweBadanie.Wniosek}");
            }
        }

        public static List<Matrix> GenerujTablicePotencjalowWejsciowych(int n, bool isUnipolarna)
        {

            List<Matrix> potencjalWejsciowy = new List<Matrix>();

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
                        if (isUnipolarna == true)
                            temp[j] = 0d;
                        else
                            temp[j] = -1d;
                    else
                        temp[j] = 1d;

                }

                potencjalWejsciowy.Add(new Matrix(temp));
            }

            return potencjalWejsciowy;
        }


        static double EnergiaSync(Matrix w, Matrix I, ExaminationStep x)
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
                suma += I.GetElement(0, i) * (x.PotencjalWyjsciowy.GetElement(0, i) + x.PotencjalWejsciowy.GetElement(0, i));
            }

            return suma;

        }


        public double EnergiaAsync(Matrix w, ExaminationStep e)
        {
            double suma = 0;
            int n = w.RowCount;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    suma += w.GetElement(i, j) * e.PotencjalWejsciowy.GetElement(0,i) * e.PotencjalWyjsciowy.GetElement(0,j);
                }

            }


            suma *= -0.5;

            return suma;
        }


        double RoznicaEnergiSync(List<double> e)
        {
            double roznica = 0;

            int liczbaElementow = e.Count;

            roznica = e[liczbaElementow - 1] - e[liczbaElementow - 2];
            return roznica;

        }

        public static double FunkcjaAktywacjiPolarna(double element)
        {
            if (element <= 0)
                return 0;
            else
                return 1;
        }

        public static double FunkcjaAktywacjiBiPolarna(double element)
        {
            if (element <= 0)
                return -1;
            else
                return 1;
        }

    }
}
