using DMU.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZmsiProjOne
{
    class HopfieldAsync
    {
        public Network network { get; set; }
        private int[] sekwencja;




        public void runHopfield(Matrix w, int[] Sekwencja,bool isFunkcjaAktywacjiUnipolarna = true)
        {
            sekwencja = Sekwencja;

            network = new Network(Program.GenerujTablicePotencjalowWejsciowych(w.ColumnCount,isFunkcjaAktywacjiUnipolarna));

            foreach (var item in network.BadanePunkty)
            {
                if (item.Wniosek == "")
                {
                    oblicz(w, item, isFunkcjaAktywacjiUnipolarna);
                }
            }
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
                        Examination ostatni = network.BadanePunkty.FirstOrDefault(x => x.BadanyPunkt.AreMatrixesEquals(listaWystapionychWektorow[listaWystapionychWektorow.Count - 1]));

                        int StalyIndex = network.BadanePunkty.FindIndex(0,x=>x==ostatni); 


                        if (listaWystapionychWektorow.Count == 1)
                        {
                            examination.Wniosek = "s";
                        }
                        else
                        {
                            foreach (var item in listaWystapionychWektorow)
                            {

                                var findedExamination=network.BadanePunkty.FirstOrDefault(x => x.BadanyPunkt.AreMatrixesEquals(item));

                                if(findedExamination != null)
                                {
                                    findedExamination.Wniosek = StalyIndex+"";
                                }

                                if (item.AreMatrixesEquals(ostatni.BadanyPunkt))
                                {
                                    findedExamination.Wniosek = "s";

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

                            findedExamination.Wniosek = "c" + 1;

                        }
                        return true;
                    }
                    else
                    {
                        listaWystapionychWektorow.Add(examination.ListaKrorkow[i].PotencjalWyjsciowy);
                    }

                }

            }
            return false;

        }





        public void oblicz(Matrix w, Examination examination, bool isFunkcjaAktywackiUnipolarna)
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

                tempExaminationStep.ObliczonyPotencjalWejsciowy = Matrix.Multiply(tempExaminationStep.PotencjalWejsciowy, w);


                double[] tempPotencjalWyjsciowy = new double[n];
                for (int i = 0; i < n; i++)
                {
                    if (i != iteratorSekwencji)
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
                examination.ListaKrorkow.Add(tempExaminationStep);

                tempExaminationStep = new ExaminationStep();
                //ustawianie V(t) do koljenego kroku jako V(t-1) 
                tempExaminationStep.PotencjalWejsciowy = tempPoteWyjMatrix;

                iteratorSekwencji++;
                if (iteratorSekwencji == n)
                {
                    iteratorSekwencji = 0;
                }


            } while (!checkExaminationAsync(n,examination));






        }
        
    }
}
