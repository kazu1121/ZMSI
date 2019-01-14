using System;
using System.Collections.Generic;
using System.Text;

namespace ZmsiProjOne.ViewModels
{
    public class HopfieldResultTwoViewModel
    {
        public HopfieldResultTwoViewModel()
        {
            HopfieldResultViewModel = new List<HopfieldResultViewModel>();
            PointSummaryViewModelList = new List<PointSummaryViewModel>();
        }

        public List<HopfieldResultViewModel> HopfieldResultViewModel { get; set; }
        public List<PointSummaryViewModel> PointSummaryViewModelList { get; set; }
    }
}
