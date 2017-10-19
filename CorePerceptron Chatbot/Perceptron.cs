using System;

namespace CorePerceptron_Chatbot
{
    public class Perceptron
    {
        //define global variables
        float[] weights;
        int numberOfOutputs;
        float bias = 1;

        //constructor
        public Perceptron(int numberOfInputs, int numberOfOutputs)
        {
            weights = new float[numberOfInputs];

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = (float)Game.randomGeneration.NextDouble();
            }

            this.numberOfOutputs = numberOfOutputs;
        }

        //output
        public float output(float[] inputs)
        {
            float returnValue = 0;

            for (int i = 0; i < weights.Length; i++)
            {
                returnValue += weights[i] * inputs[i] + bias;
            }

            return activation(returnValue);
        }

        //train
        public float train(float[] inputs, float target, Boolean takeTargetAsError)
        {
            float guess = output(inputs);
            float error = takeTargetAsError ? target : target - guess;

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] += error * inputs[i] * ChatbotCore.learningRate;
            }

            return error;
        }

        //activation function
        public float activation(float value)
        {
            return Math.Max(0, value);
        }
    }
}
