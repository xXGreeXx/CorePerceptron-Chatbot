using System;
using System.Collections.Generic;

namespace CorePerceptron_Chatbot
{
    public class NeuralNetwork
    {
        //define global variables
        public List<List<Perceptron>> perceptrons { get; private set; }  = new List<List<Perceptron>>();

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
            perceptrons.Add(new List<Perceptron>() { new Perceptron(8, 0) });
        }

        //output data
        public String OutputDataFromNetwork(String data)
        {
            List<String> returnMessage = new List<String>();
            
            //parse words
            String wordToAddToList = "";
            List<String> wordList = new List<String>();
            for (int index = 0; index < data.Length; index++)
            {
                String c = data.Substring(index, 1);

                if (c.Equals(" "))
                {
                    wordList.Add(wordToAddToList);
                    wordToAddToList = "";
                }
                else
                {
                    wordToAddToList += c;
                }
            }
            wordList.Add(wordToAddToList);

            //add words that to database that aren't in it
            foreach (String word in wordList)
            {
                if (!ChatbotCore.loadedDataBase.ContainsKey(word))
                {
                    ChatbotCore.AddWordToDataBase(word);
                }

                if (returnMessage.Count > 0)
                {
                    returnMessage.Add(forwardPropagateData(ChatbotCore.WordToNumber(word), ChatbotCore.WordToNumber(returnMessage[returnMessage.Count - 1])));
                }
                else
                {
                    returnMessage.Add(forwardPropagateData(ChatbotCore.WordToNumber(word), -1));
                }
            }

            //return text
            String r = "";
            foreach (String word in returnMessage)
            {
                r += word + " ";
            }

            //return text
            return r;
        }

        //communicate with perceptrons
        private String forwardPropagateData(float word, float lastWordSet)
        {
            float value = 0.0F;

            //iterate over neural net
            float[] inputs = new float[] { word, lastWordSet };
            for (int index = 1; index < perceptrons.Count; index++)
            {
                List<Perceptron> layer = perceptrons[index];
                float[] inputsBuffer = new float[layer.Count];

                int perceptronIndex = 0;
                foreach (Perceptron p in layer)
                {
                    inputsBuffer[perceptronIndex] = p.output(inputs);
                    
                    perceptronIndex++;
                }

                //feed forward
                inputs = inputsBuffer;
            }
            
            value = (float)Math.Round(inputs[0], 2);
            Console.WriteLine(value);
            return ChatbotCore.NumberToWord(value);
        }

        //train perceptrons
        public void trainPerceptrons(String user1Words, String user2Words)
        {
            //iterate over words
            List<String> inputWords = new List<String>();
            List<String> targetWords = new List<String>();
            String currentWord = "";

            for (int index = 0; index < user1Words.Length; index++)
            {
                String c = user1Words.Substring(index, 1);

                if (c.Equals(" "))
                {
                    inputWords.Add(currentWord);
                    currentWord = "";
                }
                else
                {
                    currentWord += c;
                }
            }

            for (int index = 0; index < user2Words.Length; index++)
            {
                String c = user2Words.Substring(index, 1);

                if (c.Equals(" "))
                {
                    targetWords.Add(currentWord);
                    currentWord = "";
                }
                else
                {
                    currentWord += c;
                }
            }

            //iterate over longer list of words for training
            for (int index = 0; index < (inputWords.Count > targetWords.Count ? targetWords.Count : inputWords.Count); index++)
            {
                float inputWord = ChatbotCore.WordToNumber(inputWords[index]);
                float targetWord = ChatbotCore.WordToNumber(targetWords[index]);

                backPropagateData(inputWord, targetWord);
            }
        }

        //back propagate data
        private void backPropagateData(float inputWord, float targetWord)
        {
            //iterate over layers
            List<float[]> error = new List<float[]>();
            float[] errorOfOuterLayer = new float[1];

            for (int layerIndex = perceptrons.Count - 1; layerIndex > 1; layerIndex--)
            {
                List<Perceptron> layer = perceptrons[layerIndex];
                float[] errorBuffer = new float[layer.Count];

                //iterate over neurons
                for (int neuronIndex = 0; neuronIndex < layer.Count; neuronIndex++)
                {
                    Perceptron neuron = layer[neuronIndex];

                    //fetch inputs with forward propagation
                    float[] inputs = new float[2];
                    for (int indexOfPropagateLayer = 0; indexOfPropagateLayer < layerIndex; indexOfPropagateLayer++)
                    {
                        List<Perceptron> layerToPropagate = perceptrons[indexOfPropagateLayer];
                        float[] inputsBuffer = new float[layerToPropagate.Count];

                        //iterate over neurons
                        for (int indexOfNeuronToPropagate = 0; indexOfNeuronToPropagate < layerToPropagate.Count; indexOfNeuronToPropagate++)
                        {
                            inputsBuffer[indexOfNeuronToPropagate] = layerToPropagate[indexOfNeuronToPropagate].output(inputs);
                        }

                        inputs = inputsBuffer;
                    }

                    //train
                    if (layerIndex == perceptrons.Count - 1)
                    {
                        errorBuffer[neuronIndex] = neuron.train(inputs, targetWord, false);
                    }
                    else
                    {
                        //sum error 
                        float errorToPassToNetwork = 0;
                        for (int i = 0; i < errorOfOuterLayer.Length; i++)
                        {
                            Perceptron layerOverCurrent = perceptrons[layerIndex + 1][i];
                            errorToPassToNetwork += errorOfOuterLayer[i] * layerOverCurrent.weights[neuronIndex];
                        }

                        //pass to network
                        errorBuffer[neuronIndex] = neuron.train(inputs, errorToPassToNetwork, true);
                    }
                }

                error.Add(errorBuffer);

                if (error.Count > 1)
                {
                    errorOfOuterLayer = error[error.Count - 1];
                }
            }
        }
    }
}
