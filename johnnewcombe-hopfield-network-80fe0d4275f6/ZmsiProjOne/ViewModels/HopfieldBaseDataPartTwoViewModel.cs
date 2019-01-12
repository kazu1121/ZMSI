using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZmsiProjOne.ViewModels
{
    public class HopfieldBaseDataPartTwoViewModel
    {
        public HopfieldBaseDataPartTwoViewModel()
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

            MatrixQuantitySelectList = new List<SelectListItem>();
            for (int i = 1; i <= 10; i++)
            {
                MatrixQuantitySelectList.Add(new SelectListItem()
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }
        }

        [DisplayName("Ilość generowanych macierzy")]
        public int MatrixQuantity { get; set; }
        public List<SelectListItem> MatrixQuantitySelectList { set; get; }

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
}
