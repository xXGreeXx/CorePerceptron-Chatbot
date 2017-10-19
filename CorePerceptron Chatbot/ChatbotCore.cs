using System;
using System.IO;
using System.Collections.Generic;

namespace CorePerceptron_Chatbot
{
    public class ChatbotCore
    {
        //define global variables
        public static NeuralNetwork brain { get; private set; } = new NeuralNetwork();
        public static float learningRate { get; } = 0.005F;
        public static Dictionary<String, float> loadedDataBase { get; set; } = new Dictionary<String, float>();
        private static float dataBaseWordOffset { get; } = 0.01F;

        //constructor
        public ChatbotCore()
        {
            //load brain weights
            //TODO\\

            //load word data base
            StreamReader reader = new StreamReader(File.OpenRead(Game.wordDatabaseSavePath));

            String line;
            while ((line = reader.ReadLine()) != null)
            {
                String word = "";
                String value = "";

                int index = 0;
                foreach (char c in line)
                {
                    if (c.ToString().Equals(":"))
                    {
                        value = line.Substring(index + 1, line.Length - (index + 1));
                    }
                    else
                    {
                        word += c.ToString();
                    }

                    index++;
                }

                loadedDataBase.Add(word, float.Parse(value));
            }

            reader.Close();
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

            foreach (var value in loadedDataBase)
            {
                if (value.Key.Equals(word))
                {
                    number = value.Value;
                    break;
                }
            }

            return number;
        }

        //convert number to word
        public static String NumberToWord(float number)
        {
            String word = "";

            foreach (var value in loadedDataBase)
            {
                if (value.Value.Equals(number))
                {
                    word = value.Key;
                    break;
                }
            }

            return word;
        }

        //add word to data base
        public static void AddWordToDataBase(String word)
        {
            float valueForDatabase = loadedDataBase.Count * dataBaseWordOffset;

            loadedDataBase.Add(word, valueForDatabase);
        }
    }
}
