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
                    returnMessage.Add(wordToOutput(ChatbotCore.WordToNumber(word), ChatbotCore.WordToNumber(returnMessage[returnMessage.Count - 1])));
                }
                else
                {
                    returnMessage.Add(wordToOutput(ChatbotCore.WordToNumber(word), -1));
                }
            }

            //return text
            String r = "";
            foreach (String word in returnMessage)
            {
                r += word + " ";
            }

            return r;
        }

        //communicate with perceptrons
        private String wordToOutput(float word, float lastWordSet)
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

            value = inputs[0];

            return ChatbotCore.NumberToWord(value);
        }

        //train perceptrons
        private void trainPerceptrons()
        {

        }
    }
}
