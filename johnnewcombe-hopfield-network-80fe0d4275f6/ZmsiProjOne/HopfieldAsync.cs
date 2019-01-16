using DMU.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZmsiProjOne.ViewModels;

namespace ZmsiProjOne
{
    public class HopfieldAsync
    {
        public Network network { get; set; }
        private int[] sekwencja;

        public Network runHopfield(Matrix w,Matrix i, int[] Sekwencja, ActivationFunction activationFunction = ActivationFunction.Unipolar)
        {
            sekwencja = Sekwencja;

            network = new Network(Program.GenerujTablicePotencjalowWejsciowych(w.ColumnCount, activationFunction));

            foreach (var item in network.BadanePunkty)
            {
                if (String.IsNullOrEmpty(item.Wniosek))
                {
                    oblicz(w,i, item, activationFunction == ActivationFunction.Unipolar);
                }

                item.BadanyPunktToString();
                item.ListaKrorkow.ForEach(x => x.UstawMacierzeToString());
            }

            network.ToString();

            return network;
        }

        // Sprawdza wystąpeinei warunku koncowego, powstanie cykli itp...
        public bool checkExaminationAsync(int n,Examination examination)
        {
            List<Matrix> listaWystapionychWektorow = new List<Matrix>();
            int licznikCykluTegoSamegoVektora = 0;

            listaWystapionychWektorow.Add(examination.ListaKrorkow[0].PotencjalWejsciowy);
            for (int i = 0; i < examination.ListaKrorkow.Count; i++)
            {
                // sprawdzamy czy v(t) i  v(t-1) sa takie same
                if (listaWystapionychWektorow[listaWystapionychWektorow.Count - 1].AreMatrixesEquals(examination.ListaKrorkow[i].PotencjalWyjsciowy))
                {
                
                    licznikCykluTegoSamegoVektora++;
                    if (licznikCykluTegoSamegoVektora >= n)
                    {
                        // ostatni jest punktem ktory jest staly
                        Examination ostatni = network.BadanePunkty.FirstOrDefault(x => x.BadanyPunkt.AreMatrixesEquals(listaWystapionychWektorow[listaWystapionychWektorow.Count - 1]));

                        //int StalyIndex = network.BadanePunkty.FindIndex(0, x => x == ostatni); 


                        if (listaWystapionychWektorow.Count == 1)
                        {
                            //examination.Wniosek = "s";
                            var networkPoint = network.BadanePunkty.FirstOrDefault(x => x.BadanyPunktString == examination.BadanyPunktString);
                            networkPoint.Wniosek = $"Punkt [{String.Join(" ", examination.BadanyPunkt.ToArray())}] jest stały!";
                            networkPoint.CzyPunktStaly = true;
                        }
                        else
                        {

                            //jezeli przeszlismy przez wiecej niz jeden wektor V
                            for (var j=0;j<listaWystapionychWektorow.Count;j++)
                            {
                                var item = listaWystapionychWektorow[j];

                                // wyszukujemy punkty ktore potem przechodza do ostatniego
                                var findedExamination=network.BadanePunkty.FirstOrDefault(x => x.BadanyPunkt.AreMatrixesEquals(item));

                                if(findedExamination != null)
                                { // Czy tutaj oznacza, że zbiega do tego punktu?
                                    //findedExamination.Wniosek = StalyIndex+"";
                                    findedExamination.CzyPunktZbiezny = true;
                                    findedExamination.Wniosek = $"Punkt [{String.Join(" ", findedExamination.BadanyPunkt.ToArray())}] zbiega do punktu: {String.Join($" ", ostatni.BadanyPunkt.ToArray())}";
                                    findedExamination.PunktDoKtoregoZbiegaFinalnie = ostatni;
                                }

                                if (item.AreMatrixesEquals(ostatni.BadanyPunkt))
                                {// Czy tutaj oznacza, że punkt jest stały?
                                    //findedExamination.Wniosek = "s";

                                //    var networkPoint = network.BadanePunkty.FirstOrDefault(x => x.BadanyPunktString == findedExamination.BadanyPunktString);
                                  //  networkPoint.Wniosek = $"Punkt [{String.Join(" ", findedExamination.BadanyPunkt.ToArray())}] jest stały!";
                                    //networkPoint.CzyPunktStaly = true;
                                    //var networkPoint = network.BadanePunkty.FirstOrDefault(x => x.BadanyPunktString == findedExamination.BadanyPunktString);
                                    ostatni.Wniosek = $"Punkt [{String.Join(" ", findedExamination.BadanyPunkt.ToArray())}] jest stały!";
                                    ostatni.CzyPunktStaly = true;
                                }
                            }

                        }
                        return true;
                    }
                }
                else
                {
                    var indexPowtarzajacego = listaWystapionychWektorow.FindIndex(0, x => x == examination.ListaKrorkow[i].PotencjalWyjsciowy);                    

                    if (indexPowtarzajacego >= 0)
                    {
                        /// teraz petla od ostatniego do ostatniego ktory sie powtorzyl...
                        for (int j = indexPowtarzajacego + 1; j < listaWystapionychWektorow.Count; j++)
                        {
                            var findedExamination = network.BadanePunkty.FirstOrDefault(x => x.BadanyPunkt.AreMatrixesEquals(listaWystapionychWektorow[j]));

                            // Jako, że nie ma w asynchronicznych cykli, to oznaczamy tylko, że zbiega do któregoś z nich?
                            findedExamination.Wniosek = "c" + 1;

                        }
                        return true;
                    }
                    else
                    {



                        var networkPoint = network.BadanePunkty.FirstOrDefault(x => x.BadanyPunkt.AreMatrixesEquals(examination.ListaKrorkow[i].PotencjalWyjsciowy));
                        // sprawdzamy czy vektor do ktorego zbiegł obecny wektor zostął juz obliczony ( jak tak to konczymy dzialanie)!!
                        if (!String.IsNullOrEmpty(networkPoint.Wniosek))
                        {
                            if (networkPoint.CzyPunktStaly.HasValue && networkPoint.CzyPunktStaly.Value==true)
                            {
                                examination.Wniosek = $"Punkt [{String.Join(" ", examination.BadanyPunkt.ToArray())}] zbiega do punktu: {String.Join($" ", networkPoint.BadanyPunkt.ToArray())}";
                            }
                            else
                            {
                                //examination.Wniosek = $"Punkt [{String.Join(" ", examination.BadanyPunkt.ToArray())}] zbiega do punktu: {String.Join($" ", networkPoint.PunktDoKtoregoZbiegaFinalnie.BadanyPunkt.ToArray())}";
                                examination.Wniosek = $"Punkt [{String.Join(" ", examination.BadanyPunkt.ToArray())}] zbiega do punktu: {String.Join($" ", networkPoint.BadanyPunkt.ToArray())}";

                            }
                            examination.CzyPunktZbiezny = true;
                            return true;

                        }
                        listaWystapionychWektorow.Add(examination.ListaKrorkow[i].PotencjalWyjsciowy);
                    }
                    licznikCykluTegoSamegoVektora = 0;

                }

            }
            return false;

        }

        public void oblicz(Matrix w,Matrix I, Examination examination, bool isFunkcjaAktywackiUnipolarna)
        {
            int n = w.ColumnCount;
            int iteratorSekwencji = 0;
            int ineratorStep = 1;

            var tempExaminationStep = new ExaminationStep();
            tempExaminationStep.PotencjalWejsciowy = examination.BadanyPunkt;

            do
            {
                tempExaminationStep.Numer = ineratorStep;
                ineratorStep++;

                tempExaminationStep.ObliczonyPotencjalWejsciowy = Matrix.Add(Matrix.Multiply(tempExaminationStep.PotencjalWejsciowy, w), I);

                double[] tempPotencjalWyjsciowy = new double[n];
                for (int i = 0; i < n; i++)
                {
                    if (i != sekwencja[iteratorSekwencji])
                    {
                        tempPotencjalWyjsciowy[i] = tempExaminationStep.PotencjalWejsciowy.GetElement(0, i);
                    }
                    else
                    {
                        if (isFunkcjaAktywackiUnipolarna)
                        {
                            tempPotencjalWyjsciowy[i] = Program.FunkcjaAktywacjiPolarna(tempExaminationStep.ObliczonyPotencjalWejsciowy.GetElement(0, i));

                        }
                        else
                        {
                            tempPotencjalWyjsciowy[i] = Program.FunkcjaAktywacjiBiPolarna(tempExaminationStep.ObliczonyPotencjalWejsciowy.GetElement(0, i));
                        }
                    }
                }

                //Twirzenie V(t)
                var tempPoteWyjMatrix = new Matrix(tempPotencjalWyjsciowy);

                tempExaminationStep.PotencjalWyjsciowy = tempPoteWyjMatrix;
                tempExaminationStep.Energia = Program.EnergiaAsync(w, tempExaminationStep);
                examination.ListaKrorkow.Add(tempExaminationStep);
                
                iteratorSekwencji++;
                if (iteratorSekwencji == n)
                {
                    iteratorSekwencji = 0;
                }
                //Console.WriteLine(iteratorSekwencji+":\t"+tempExaminationStep.PotencjalWyjsciowy.ToString());

                tempExaminationStep = new ExaminationStep();
                //ustawianie V(t) do koljenego kroku jako V(t-1) 
                tempExaminationStep.PotencjalWejsciowy = tempPoteWyjMatrix;
            } while (!checkExaminationAsync(n,examination));
        }
        
    }
}
