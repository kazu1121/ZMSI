using System;
using System.Collections.Generic;
using System.Text;

namespace ZmsiProjOne.ViewModels
{
    public class PointSummaryViewModel
    {
        public string PunktString { get; set; }

        public int IleStaly { get; set; }
        public int IleZbiezny { get; set; }
        public int IleTworzyCykl { get; set; }
        public int IleWpadaWCykl { get; set; }
    }
}
