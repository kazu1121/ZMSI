using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZmsiProjOne.ViewModels
{
    public class HopfieldViewModel
    {
        public HopfieldViewModel()
        {

        }

        public HopfieldViewModel(HopfieldBaseViewModel baseModel)
        {
            HopfieldBaseData = baseModel;
        }

        public HopfieldBaseViewModel HopfieldBaseData { get; set; }

        [DisplayName("Macierz wag")]
        public double[][] WeightMatrix { get; set; }

        [DisplayName("Macierz I")]
        public double[] IMatrix { get; set; }

        [DisplayName("Kolejność punktów badania asynchronicznego")]
        public int[] AsyncExaminingOrder { get; set; }
    }
}
