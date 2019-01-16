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

            var result = new HopfieldResultViewModel();
            Network resultNetwork = null;

            if (viewModel.HopfieldBaseData.ExaminationMode == ExaminationMode.Sync)
            {
                resultNetwork = ZmsiProjOne.Program.SynchHopfield(
                    new DMU.Math.Matrix(viewModel.WeightMatrix.To2D<double>()),
                    new DMU.Math.Matrix(viewModel.IMatrix),
                    viewModel.HopfieldBaseData.ActivationFunction);
            }
            else
            {
                HopfieldAsync ha = new HopfieldAsync();

                resultNetwork = ha.runHopfield(new DMU.Math.Matrix(viewModel.WeightMatrix.To2D<double>()),
                                                new DMU.Math.Matrix(viewModel.IMatrix),
                                                viewModel.AsyncExaminingOrder,
                                                viewModel.HopfieldBaseData.ActivationFunction);
            }

            result = new HopfieldResultViewModel()
            {
                HopfieldViewModel = viewModel,
                ResultNetwork = resultNetwork
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

            if (viewModel.ExaminationMode == ExaminationMode.Async && viewModel.AsyncExaminingOrder == null)
            {
                viewModel.AsyncExaminingOrder = new int[viewModel.WeightMatrixSize];
                return View("HopfieldResultTwoAsyncOrderInfo", viewModel);
            }

            double iMatrixValue = 25;
            var generatedMatrixes = ZmsiProjOne.Program.GenerateRandomMatrixes(viewModel.MatrixQuantity, viewModel.WeightMatrixSize, viewModel.WeightMatrixSize);

            HopfieldResultTwoViewModel result = new HopfieldResultTwoViewModel();

            foreach (var generatedMatrix in generatedMatrixes)
            {
                var hvm = new HopfieldViewModel()
                {
                    WeightMatrix = new double[viewModel.WeightMatrixSize][],
                    IMatrix = new double[viewModel.WeightMatrixSize],
                    AsyncExaminingOrder = viewModel.AsyncExaminingOrder,
                    HopfieldBaseData = new HopfieldBaseViewModel()
                    {
                        ActivationFunction = viewModel.ActivationFunction,
                        ExaminationMode = viewModel.ExaminationMode
                    }
                };
                for (int i = 0; i < viewModel.WeightMatrixSize; i++)
                {
                    hvm.WeightMatrix[i] = generatedMatrix.GetRow(i).ToArray();
                    hvm.IMatrix[i] = iMatrixValue;
                }

                Network resultNetwork = null;
                if(viewModel.ExaminationMode == ExaminationMode.Async)
                {
                    HopfieldAsync ha = new HopfieldAsync();
                    resultNetwork = ha.runHopfield(generatedMatrix,
                                                    new DMU.Math.Matrix(1, generatedMatrix.ColumnCount, iMatrixValue),
                                                    viewModel.AsyncExaminingOrder,
                                                    viewModel.ActivationFunction);
                }
                else
                {
                    resultNetwork = ZmsiProjOne.Program.SynchHopfield(
                                                                generatedMatrix,
                                                            new DMU.Math.Matrix(1, generatedMatrix.ColumnCount, iMatrixValue),
                                                            viewModel.ActivationFunction);
                }

                var hrvm = new HopfieldResultViewModel()
                {
                    ResultNetwork = resultNetwork,
                    HopfieldViewModel = hvm
                };

                result.HopfieldResultViewModel.Add(hrvm);
            }

            for (int i = 0; i < result.HopfieldResultViewModel[0].ResultNetwork.BadanePunkty.Count; i++)
            {
                result.PointSummaryViewModelList.Add(new PointSummaryViewModel()
                {
                    PunktString = result.HopfieldResultViewModel[0].ResultNetwork.BadanePunkty[i].BadanyPunktString
                });
            }

            foreach (var hrvm in result.HopfieldResultViewModel)
            {
                foreach (var punkt in hrvm.ResultNetwork.BadanePunkty)
                {                    
                    if (punkt.CzyPunktStaly.HasValue && punkt.CzyPunktStaly.Value == true)
                    {
                        result.PointSummaryViewModelList.FirstOrDefault(x => x.PunktString == punkt.BadanyPunktString).IleStaly++;
                    }
                    else if (punkt.CzyPunktZbiezny.HasValue && punkt.CzyPunktZbiezny.Value == true)
                    {
                        result.PointSummaryViewModelList.FirstOrDefault(x => x.PunktString == punkt.BadanyPunktString).IleZbiezny++;
                    }
                    else if (punkt.CzyPunktTworzyCykl.HasValue && punkt.CzyPunktTworzyCykl.Value == true)
                    {
                        result.PointSummaryViewModelList.FirstOrDefault(x => x.PunktString == punkt.BadanyPunktString).IleTworzyCykl++;
                    }
                    else if (punkt.CzyPunktWpadaWCykl.HasValue && punkt.CzyPunktWpadaWCykl.Value == true)
                    {
                        result.PointSummaryViewModelList.FirstOrDefault(x => x.PunktString == punkt.BadanyPunktString).IleWpadaWCykl++;
                    }                    
                }
            }

            return View("HopfieldResultTwo", result);
        }
    }
}
