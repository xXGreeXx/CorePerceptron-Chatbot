using System;
using System.Collections.Generic;

namespace CorePerceptron_Chatbot
{
    public class NeuralNetwork
    {
        //define global variables
        private List<List<Perceptron>> perceptrons = new List<List<Perceptron>>();

        //constructor
        public NeuralNetwork()
        {
            //input layer(last word said, user word)
            perceptrons.Add(new List<Perceptron>() { new Perceptron(0, 16), new Perceptron(0, 16) });

            //hidden layers
            perceptrons.Add(new List<Perceptron>() { new Perceptron(2, 16), new Perceptron(2, 16) , new Perceptron(2, 16) , new Perceptron(2, 16) , new Perceptron(2, 16) , new Perceptron(2, 16) , new Perceptron(2, 16) , new Perceptron(2, 16) , new Perceptron(2, 16) , new Perceptron(2, 16) , new Perceptron(2, 16) , new Perceptron(2, 16) , new Perceptron(2, 16) , new Perceptron(2, 16) , new Perceptron(2, 16) , new Perceptron(2, 16) });

            perceptrons.Add(new List<Perceptron>() { new Perceptron(16, 16), new Perceptron(16, 16), new Perceptron(16, 16), new Perceptron(16, 16), new Perceptron(16, 16), new Perceptron(16, 16), new Perceptron(16, 16), new Perceptron(16, 16), new Perceptron(16, 16), new Perceptron(16, 16), new Perceptron(16, 16), new Perceptron(16, 16), new Perceptron(16, 16), new Perceptron(16, 16), new Perceptron(16, 16), new Perceptron(16, 16) });

            perceptrons.Add(new List<Perceptron>() { new Perceptron(16, 8), new Perceptron(16, 8), new Perceptron(16, 16), new Perceptron(16, 8), new Perceptron(16, 8), new Perceptron(16, 16), new Perceptron(16, 8), new Perceptron(16, 8), new Perceptron(16, 8), new Perceptron(16, 8), new Perceptron(16, 8), new Perceptron(16, 8), new Perceptron(16, 8), new Perceptron(16, 8), new Perceptron(16, 8), new Perceptron(16, 8) });

            perceptrons.Add(new List<Perceptron>() { new Perceptron(16, 1), new Perceptron(16, 1), new Perceptron(16, 1), new Perceptron(16, 1), new Perceptron(16, 8), new Perceptron(16, 1), new Perceptron(16, 1), new Perceptron(16, 1) });

            //output layer(word to return)
            perceptrons.Add(new List<Perceptron>() { new Perceptron(0, 0) });
        }

        //output data
        public String OutputDataFromNetwork(String data)
        {
            String returnMessage = "";


            return returnMessage;
        }
    }
}
