using DMU.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using ZmsiProjOne.ViewModels;

namespace ZmsiProjOne
{
    public class Program
    {
        static void Main(string[] args)
        {
            var qwe = GenerateRandomMatrixes(10, 3, 3);

            //SynchHopfield();
            HopfieldAsync ha = new HopfieldAsync();
            Matrix w = new Matrix(new double[,] { { 0d, 1d,2d }, { 1d, 0d,-1d }, { 2d, -1d, 0d } });
            Matrix I = new Matrix(new double[] { 0, 0, 0 });
            int[] sekwencja = new int[] { 0, 1,2 };
            ha.runHopfield(w,I,sekwencja, ActivationFunction.Bipolar);

            Matrix macierzWag = new Matrix(new double[,] { { 0d, 4d }, { 4d, 0d } });
            Matrix macierzI = new Matrix(new double[] { 0d, 0d });

            var network = SynchHopfield(macierzWag, macierzI, ActivationFunction.Unipolar);
            network.WyswietlPrzebiegNaKonsoli();

            Console.ReadKey();
        }

        public static List<Matrix> GenerateRandomMatrixes(int quantity, int rows, int columns, bool isSymetric = true)
        {
            var result = new List<Matrix>();
            Random random = new Random();

            for (int i = 0; i < quantity; i++)
            {
                Matrix tempMatrix = new Matrix(rows, columns);

                for (int j = 0; j < rows; j++)
                {
                    for (int k = 0; k < columns; k++)
                    {
                        tempMatrix.SetElement(j, k, (random.NextDouble() >= 0.5 ? random.NextDouble() : (random.NextDouble() * -1) * 10));
                    }
                }

                result.Add(tempMatrix);
            }

            return result;
        }

        public static Network SynchHopfield(Matrix macierzWag, Matrix macierzI, ActivationFunction activationFunction)
        {
            var network = new Network(GenerujTablicePotencjalowWejsciowych(macierzWag.ColumnCount, activationFunction));

            for (int i = 0; i < network.BadanePunkty.Count; i++)
            {
                bool isExamining = true;
                var noweBadanie = network.BadanePunkty[i];

                while (isExamining)
                {
                    var nowyKrok = new ExaminationStep();
                    nowyKrok.Numer = noweBadanie.ListaKrorkow.Count + 1;

                    // Ustawienie potencjału wejściowego
                    if (noweBadanie.ListaKrorkow.Count > 0)
                        nowyKrok.PotencjalWejsciowy = noweBadanie.ListaKrorkow[noweBadanie.ListaKrorkow.Count - 1].PotencjalWyjsciowy;
                    else
                        nowyKrok.PotencjalWejsciowy = network.BadanePunkty[i].BadanyPunkt;

                    // Sprawdzanie czy wpadliśmy w cykl
                    if (noweBadanie.ListaKrorkow.Any(x => x.PotencjalWejsciowy.AreMatrixesEquals(nowyKrok.PotencjalWejsciowy)))
                    {
                        isExamining = false;
                        continue;
                    }

                    // Obliczenie potencjału wejściowego dla funkcji aktywacji
                    nowyKrok.ObliczonyPotencjalWejsciowy = Matrix.Add(Matrix.Multiply(nowyKrok.PotencjalWejsciowy, macierzWag), macierzI);
                    //nowyKrok.ObliczonyPotencjalWejsciowy = Matrix.Add(Matrix.Multiply(macierzWag, nowyKrok.PotencjalWejsciowy), macierzI);

                    // Obliczenie potencjału wyjściowego
                    List<double> wyjsciowy = new List<double>();
                    foreach (var item in nowyKrok.ObliczonyPotencjalWejsciowy.ToArray())
                    {
                        wyjsciowy.Add(activationFunction == ActivationFunction.Unipolar ? FunkcjaAktywacjiPolarna(item) : FunkcjaAktywacjiBiPolarna(item));
                    }
                    nowyKrok.PotencjalWyjsciowy = new Matrix(wyjsciowy.ToArray());

                    // Obliczenie energii
                    nowyKrok.Energia = EnergiaSync(macierzWag, macierzI, nowyKrok);

                    // Sprawdzanie warunków stopu badania
                    var punktDoKtoregoZbiega = network
                        .BadanePunkty
                        .FirstOrDefault(x => 
                                        x.BadanyPunkt.AreMatrixesEquals(nowyKrok.PotencjalWyjsciowy)
                                        && !noweBadanie.BadanyPunkt.AreMatrixesEquals(x.BadanyPunkt));

                    if (nowyKrok.PotencjalWejsciowy.AreMatrixesEquals(nowyKrok.PotencjalWyjsciowy))
                    { // Punkt stały
                        isExamining = false;

                        if (noweBadanie.ListaKrorkow.Count == 0)
                            noweBadanie.CzyPunktStaly = true;
                    }
                    else if (noweBadanie.ListaKrorkow.Count > 0
                        && nowyKrok.PotencjalWejsciowy == nowyKrok.PotencjalWyjsciowy
                        && nowyKrok.Energia == noweBadanie.ListaKrorkow[noweBadanie.ListaKrorkow.Count - 1].Energia
                        && macierzWag.IsSymetric())
                    {
                        noweBadanie.CzyPunktStaly = false;
                        isExamining = false;
                        //Console.WriteLine("Wyprodukowana przez sieć wartość energii jest równa w dwóch kolejnych krokach jej działania (warunek ten należy sprawdzać przy założeniu, że macierz wag jest symetryczna!).");
                    }
                    else if (punktDoKtoregoZbiega != null)
                    {// Ustawienie punktu do którego zbiega (Sprawdzanie cykli i ustawianie wniosków odbywa się po wszystkich obliczeniach wszystkich badań)
                        noweBadanie.CzyPunktStaly = false;
                        noweBadanie.PunktDoKtoregoZbiega = punktDoKtoregoZbiega;
                    }
                    noweBadanie.ListaKrorkow.Add(nowyKrok);
                }

                network.BadanePunkty[i] = noweBadanie;
            }

            UstawWyniki(network);
            return network;
        }

        public static void UstawWyniki(Network network)
        {
            SprawdzZbieznosc(network.BadanePunkty);
            SprawdzCykle(network.BadanePunkty);
            UstawWnioski(network);
        }

        public static void UstawWnioski(Network network)
        {
            foreach (var badanyPunkt in network.BadanePunkty)
            {
                badanyPunkt.BadanyPunktToString();
                badanyPunkt.ListaKrorkow.ForEach(x => x.UstawMacierzeToString());

                if (badanyPunkt.CzyPunktStaly.HasValue && badanyPunkt.CzyPunktStaly.Value == true)
                {// Punkt stały
                    badanyPunkt.Wniosek = $"Punkt [{String.Join(" ", badanyPunkt.BadanyPunkt.ToArray())}] jest stały!";
                }
                else if(badanyPunkt.Cykl.Any())
                { // Tworzy cykl

                    badanyPunkt.CzyPunktTworzyCykl = true;
                    List<string> punktyCyklu = new List<string>();
                    foreach (var krokCyklu in badanyPunkt.Cykl)
                    {
                        punktyCyklu.Add($"[{String.Join($" ", krokCyklu.BadanyPunkt.ToArray())}]");
                    }

                    badanyPunkt.Wniosek = $"Punkt [{String.Join(" ", badanyPunkt.BadanyPunkt.ToArray())}] tworzy cykl: {String.Join(" -> ", punktyCyklu)}";
                }
                else if (badanyPunkt.PunktDoKtoregoZbiega != null)
                {
                    if (badanyPunkt.PunktDoKtoregoZbiegaFinalnie != null)
                    {// Zbiega do punktu
                        badanyPunkt.CzyPunktZbiezny = true;
                        badanyPunkt.Wniosek = $"Punkt [{String.Join(" ", badanyPunkt.BadanyPunkt.ToArray())}] zbiega do punktu: {String.Join($" ", badanyPunkt.PunktDoKtoregoZbiegaFinalnie.BadanyPunkt.ToArray())}";
                    }
                    else
                    {// Wpada w cykl
                        badanyPunkt.CzyPunktWpadaWCykl = true;

                        List<string> punktyCyklu = new List<string>();
                        foreach (var krokCyklu in badanyPunkt.PunktDoKtoregoZbiega.Cykl)
                        {
                            punktyCyklu.Add($"[{String.Join($" ", krokCyklu.BadanyPunkt.ToArray())}]");
                        }
                        badanyPunkt.Wniosek = $"Punkt [{String.Join(" ", badanyPunkt.BadanyPunkt.ToArray())}] wpada w cykl: {String.Join(" -> ", punktyCyklu)}";
                    }
                }
            }
        }

        public static void SprawdzCykle(List<Examination> badanePunkty)
        {
            foreach (var badanyPunkt in badanePunkty)
            {
                List<Examination> potencjalnyCykl = new List<Examination>();
                ZwrocListeCykliRekurencja(badanyPunkt.PunktDoKtoregoZbiega, potencjalnyCykl, badanePunkty.Count);
                if (potencjalnyCykl.Any(x => x.BadanyPunkt.AreMatrixesEquals(badanyPunkt.BadanyPunkt)))
                    badanyPunkt.Cykl = new List<Examination>(potencjalnyCykl);
            }
        }

        public static void SprawdzZbieznosc(List<Examination> badanePunkty)
        {
            foreach (var badanyPunkt in badanePunkty)
            {
                var punktStalyDoKtoregoZbiegaFinalnie = ZwrocPunktDoKoregoZbiegaRekurencja(badanyPunkt.PunktDoKtoregoZbiega, badanePunkty.Count);
                if (punktStalyDoKtoregoZbiegaFinalnie != null)
                    badanyPunkt.PunktDoKtoregoZbiegaFinalnie = punktStalyDoKtoregoZbiegaFinalnie;
            }
        }

        public static Examination ZwrocPunktDoKoregoZbiegaRekurencja(Examination punktReferujacyDalej, int maksymalnaMocCyklu)
        {
            if (punktReferujacyDalej == null || maksymalnaMocCyklu == 0)
                return null;

            if (punktReferujacyDalej.CzyPunktStaly.HasValue && punktReferujacyDalej.CzyPunktStaly.Value == true)
                return punktReferujacyDalej;

            return ZwrocPunktDoKoregoZbiegaRekurencja(punktReferujacyDalej.PunktDoKtoregoZbiega, --maksymalnaMocCyklu);
        }

        public static void ZwrocListeCykliRekurencja(Examination punktReferujacyDalej, List<Examination> wykrytyCykl, int maksymalnaMocCyklu)
        {
            if (punktReferujacyDalej == null || maksymalnaMocCyklu == 0 || wykrytyCykl.Any(x => x.BadanyPunkt.AreMatrixesEquals(punktReferujacyDalej.BadanyPunkt)))
                return;

            wykrytyCykl.Add(punktReferujacyDalej);

            ZwrocListeCykliRekurencja(punktReferujacyDalej.PunktDoKtoregoZbiega, wykrytyCykl, --maksymalnaMocCyklu);
        }

        public static void CzyNadalBadac(Examination examination)
        {
            if (examination.PunktDoKtoregoZbiega == null)
                return;

            if (examination.PunktDoKtoregoZbiega.PunktDoKtoregoZbiega.BadanyPunkt.AreMatrixesEquals(examination.BadanyPunkt))
            {// Punkty zbiegają do siebie nawzajem
                if (!examination.PunktDoKtoregoZbiega.Cykl.Any())
                {// Nie było wcześniej cyklu
                    examination.Cykl.Add(examination);
                    examination.Cykl.Add(examination.PunktDoKtoregoZbiega);
                    examination.Wniosek = $"Tworzy cykl {String.Join(',', examination.BadanyPunkt.ToArray())} ->  {String.Join(',', examination.PunktDoKtoregoZbiega.BadanyPunkt.ToArray())}";

                    examination.PunktDoKtoregoZbiega.PunktDoKtoregoZbiega = examination;
                    examination.PunktDoKtoregoZbiega.Cykl.Add(examination.PunktDoKtoregoZbiega);
                    examination.PunktDoKtoregoZbiega.Cykl.Add(examination);
                    examination.PunktDoKtoregoZbiega.Wniosek = $"Tworzy cykl {String.Join(',', examination.PunktDoKtoregoZbiega.BadanyPunkt.ToArray())} ->  {String.Join(',', examination.BadanyPunkt.ToArray())}";
                }
                else
                {
                    examination.Cykl = examination.PunktDoKtoregoZbiega.Cykl;
                    examination.PunktDoKtoregoZbiega.Wniosek = $"Tworzy cykl {String.Join(',', examination.BadanyPunkt.ToArray())} ->  {String.Join(',', examination.PunktDoKtoregoZbiega.BadanyPunkt.ToArray())}";
                }
                return;
            }
        }

        public static List<Matrix> GenerujTablicePotencjalowWejsciowych(int n, ActivationFunction activationFunction)
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
                        if (activationFunction == ActivationFunction.Unipolar)
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


        public static double EnergiaSync(Matrix w, Matrix I, ExaminationStep x)
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


        public static double EnergiaAsync(Matrix w, ExaminationStep e)
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
