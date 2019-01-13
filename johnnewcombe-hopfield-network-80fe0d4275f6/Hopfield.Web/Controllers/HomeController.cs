using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hopfield.Web.Models;
using ZmsiProjOne.ViewModels;
using ZmsiProjOne;

namespace Hopfield.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult HopfieldBaseData()
        {
            return View(new HopfieldBaseViewModel());
        }

        [HttpPost]
        public IActionResult HopfieldBaseData(HopfieldBaseViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            return RedirectToAction("HopfieldDetailedData", viewModel);
        }

        public IActionResult HopfieldDetailedData(HopfieldBaseViewModel baseModel)
        {
            var viewModel = new HopfieldViewModel()
            {
                HopfieldBaseData = baseModel,
                WeightMatrix = new double[baseModel.WeightMatrixSize][],
                IMatrix = new double[baseModel.WeightMatrixSize],
                AsyncExaminingOrder = new int[baseModel.WeightMatrixSize]
            };

            for (int i = 0; i < viewModel.WeightMatrix.Length; i++)
            {
                viewModel.WeightMatrix[i] = new double[baseModel.WeightMatrixSize];
                viewModel.IMatrix[i] = 0.5;
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult HopfieldDetailedData(HopfieldViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var computedNetwork = ZmsiProjOne.Program.SynchHopfield(
                new DMU.Math.Matrix(viewModel.WeightMatrix.To2D<double>()),
                new DMU.Math.Matrix(viewModel.IMatrix),
                viewModel.HopfieldBaseData.ActivationFunction);

            var result = new HopfieldResultViewModel()
            {
                HopfieldViewModel = viewModel,
                ResultNetwork = computedNetwork
            };


            return View("HopfieldResultOne", result);
        }

        public IActionResult HopfieldBaseDataPartTwo()
        {
            return View(new HopfieldBaseDataPartTwoViewModel());
        }

        [HttpPost]
        public IActionResult HopfieldBaseDataPartTwo(HopfieldBaseDataPartTwoViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
                                   
            var generatedMatrixes = ZmsiProjOne.Program.GenerateRandomMatrixes(viewModel.MatrixQuantity, viewModel.WeightMatrixSize, viewModel.WeightMatrixSize);

            HopfieldResultTwoViewModel result = new HopfieldResultTwoViewModel();

            foreach (var generatedMatrix in generatedMatrixes)
            {
                //result.HopfieldResultViewModel.Add(new HopfieldResultViewModel()
                //{
                //    ResultNetwork = ZmsiProjOne.Program.SynchHopfield(
                //                                                generatedMatrix,
                //                                            new DMU.Math.Matrix(1, generatedMatrix.ColumnCount, 0.5),
                //                                            viewModel.ActivationFunction),
                //    HopfieldViewModel = new HopfieldViewModel()
                //    {
                //        WeightMatrix = new double[viewModel.WeightMatrixSize][],
                //        IMatrix = new double[viewModel.WeightMatrixSize],
                //        AsyncExaminingOrder = new int[viewModel.WeightMatrixSize],
                //        HopfieldBaseData = new HopfieldBaseViewModel()
                //        {
                //            ActivationFunction = viewModel.ActivationFunction,
                //            ExaminationMode = viewModel.ExaminationMode
                //        }
                //    }
                //});

                var hvm = new HopfieldViewModel()
                {
                    WeightMatrix = new double[viewModel.WeightMatrixSize][],
                    IMatrix = new double[viewModel.WeightMatrixSize],
                    AsyncExaminingOrder = new int[viewModel.WeightMatrixSize],
                    HopfieldBaseData = new HopfieldBaseViewModel()
                    {
                        ActivationFunction = viewModel.ActivationFunction,
                        ExaminationMode = viewModel.ExaminationMode
                    }
                };
                for (int i = 0; i < viewModel.WeightMatrixSize; i++)
                {
                    hvm.WeightMatrix[i] = generatedMatrix.GetRow(i).ToArray();
                    hvm.IMatrix[i] = 0.5;
                }

                var hrvm = new HopfieldResultViewModel()
                {
                    ResultNetwork = ZmsiProjOne.Program.SynchHopfield(
                                                                generatedMatrix,
                                                            new DMU.Math.Matrix(1, generatedMatrix.ColumnCount, 0.5),
                                                            viewModel.ActivationFunction),
                    HopfieldViewModel = hvm
                };

                result.HopfieldResultViewModel.Add(hrvm);
            }

            return View("HopfieldResultTwo", result);
        }
    }
}
