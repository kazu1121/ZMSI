using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIF.Math;

namespace AIF.NeuralNetwork
{
    public class Perceptron
    {
        //delta rule
        //returns Weight change required for the specified input(i) of a neuron(j)


    }

    public class Neuron
    {

        private double CalculateDelta(double learningRate, double wieghtedSumOfNeuronsInputs, double input, double targetOutput, double actualOutput)
        {
            //input refers to one specific input of the neuron
            //return learningRate * (targetOutput - actualOutput) * activationFunction(wieghtedSumOfNeuronsInputs) * input;
            return 0.0;

        }
    }
        
}
