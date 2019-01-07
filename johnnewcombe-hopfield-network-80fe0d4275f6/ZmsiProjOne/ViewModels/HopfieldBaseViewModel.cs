using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZmsiProjOne.ViewModels
{
    public class HopfieldBaseViewModel
    {
        public HopfieldBaseViewModel()
        {
            WeightMatrixSizeSelectList = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "2",
                    Value = "2"
                },
                new SelectListItem()
                {
                    Text = "3",
                    Value = "3"
                },
                new SelectListItem()
                {
                    Text = "4",
                    Value = "4"
                }
            };

            ActivationFunctionSelectList = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "Unipolarna",
                    Value = "0"
                },
                new SelectListItem()
                {
                    Text = "Bipolarna",
                    Value = "1"
                }
            };

            ExaminationModeSelectList = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "Synchroniczny",
                    Value = "0"
                },
                new SelectListItem()
                {
                    Text = "Asynchroniczny",
                    Value = "1"
                }
            };
        }

        [DisplayName("Rozmiar macierzy")]
        public int WeightMatrixSize { get; set; }
        public List<SelectListItem> WeightMatrixSizeSelectList { set; get; }

        [DisplayName("Funkcja aktywacji")]
        public ActivationFunction ActivationFunction { get; set; }
        public List<SelectListItem> ActivationFunctionSelectList { set; get; }

        [DisplayName("Tryb badania")]
        public ExaminationMode ExaminationMode { get; set; }
        public List<SelectListItem> ExaminationModeSelectList { set; get; }
    }

    public enum ExaminationMode
    {
        Sync = 0,
        Async
    }

    public enum ActivationFunction
    {
        Unipolar = 0,
        Bipolar
    }
}
