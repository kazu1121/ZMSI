using DMU.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZmsiProjOne
{
    
    public class Examination
    {
        public List<ExaminationStep> ListaKrorkow { get; set;}

        // s -> staly
        // c* ->  cykl (* odpowiada numerowi cyklu)
        // numer -> przechodzi do danego vektora 
        public string Wniosek { get; set; }

        public Examination()
        {
            ListaKrorkow = new List<ExaminationStep>();
        }

        // Sprawdza wystąpeinei warunku koncowego, powstanie cykli itp...
        public bool checkExaminationAsync()
        {
            int t = ListaKrorkow.Capacity;









            return true;

        }





        public void hopfieldAsync(Matrix w, Matrix potencjalWejsciowy,bool isFunkcjaAktywackiUnipolarna)
        {
            int n = w.ColumnCount;
            var sekwencja = setSekwencja(n);
            int iteratorSekwencji = 0;



            var tempExaminationStep = new ExaminationStep();
            tempExaminationStep.PotencjalWejsciowy=potencjalWejsciowy;

            do
            {


                

                
                tempExaminationStep.ObliczonyPotencjalWejsciowy = Matrix.Multiply(potencjalWejsciowy, w);


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

                tempExaminationStep.PotencjalWyjsciowy= new Matrix(tempPotencjalWyjsciowy);
                ListaKrorkow.Add(tempExaminationStep);


            } while (checkExaminationAsync());





                                                                                         
        }


        public double[] setSekwencja(int n)
        {
            double[] sekwencja = new double[n];

            for (int i = 0; i < n; i++)
            {
                sekwencja[i] = i;
            }
            return sekwencja;

        }
    }
}
