using System;
using System.Collections.Generic;
using System.Text;

namespace ZmsiProjOne.ViewModels
{
    public class HopfieldResultTwoViewModel
    {
        public HopfieldResultTwoViewModel()
        {
            ResultNetworks = new List<Network>();
        }

        public HopfieldViewModel HopfieldViewModel { get; set; }
        public List<Network> ResultNetworks { get; set; }
    }
}
