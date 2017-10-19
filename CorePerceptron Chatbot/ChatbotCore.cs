using System;
using System.IO;
using System.Collections.Generic;

namespace CorePerceptron_Chatbot
{
    public class ChatbotCore
    {
        //define global variables
        private NeuralNetwork brain = new NeuralNetwork();
        public static float learningRate { get; } = 0.01F;
        public static List<Tuple<String, float>> loadedDataBase { get; set; } = new List<Tuple<String, float>>();

        //constructor
        public ChatbotCore()
        {

        }

        //send data
        public String SendData(String message)
        {
            return brain.OutputDataFromNetwork(message);
        }

        //convert word to number
        public static float WordToNumber(String word)
        {
            float number = 0;

            return number;
        }

        //convert number to word
        public static String NumberToWord(float word)
        {
            String number = "";

            return number;
        }

        //add word to data base
        public static void AddWordToDataBase(String word)
        {
            
        }

        //check if database contains word
        public static float CheckIfDatabaseContainsWord(String word)
        {
            float returnValue = -100;

            return returnValue;
        }
    }
}
