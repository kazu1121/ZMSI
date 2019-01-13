using System;
using System.Collections.Generic;
using System.Text;

namespace ZmsiProjOne.ViewModels
{
    public class HopfieldResultTwoViewModel
    {
        public HopfieldResultTwoViewModel()
        {
            //ResultNetworks = new List<Network>();
            HopfieldResultViewModel = new List<HopfieldResultViewModel>();
        }

        //public HopfieldViewModel HopfieldViewModel { get; set; }
        //public List<Network> ResultNetworks { get; set; }

        public List<HopfieldResultViewModel> HopfieldResultViewModel { get; set; }
    }
}
