using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace CorePerceptron_Chatbot
{
    public partial class Game : Form
    {
        List<Tuple<String, int>> chatLog = new List<Tuple<String, int>>();
        ChatbotCore chatbot = new ChatbotCore();

        public static String wordDatabaseSavePath = "WordDataBase.txt";
        public static String neuralNetworkWeightsSavePath = "NeuralNetworkSave.txt";
        public static Random randomGeneration { get; } = new Random();

        float scroll = 0;
        Font f = new Font(FontFamily.GenericSansSerif, 15, FontStyle.Bold);

        //initialize
        public Game()
        {
            InitializeComponent();

            //set window not resizable
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            //set application exit handle
            Application.ApplicationExit += SaveOnExit;

            //start game loop
            updateTimer.Start();

            chatbot.brain.trainPerceptrons("Hello how are you?", "Good how are you?");
            chatbot.brain.trainPerceptrons("Test", "What?");
            chatbot.brain.trainPerceptrons("Who are you?", "I don't know");
            chatbot.brain.trainPerceptrons("What are you?", "I don't know");
        }

        //save data before exit
        private void SaveOnExit(object sender, EventArgs e)
        {
            //save neural network weights
            File.Delete(neuralNetworkWeightsSavePath);
            StreamWriter weightWriter = new StreamWriter(File.OpenWrite(neuralNetworkWeightsSavePath));

            for (int index = 1; index < chatbot.brain.perceptrons.Count; index++)
            {
                List<Perceptron> layer = chatbot.brain.perceptrons[index];

                int perceptronIndex = 1;
                foreach (Perceptron p in layer)
                {
                    weightWriter.WriteLine("<BEGIN-NODE> : layer " + index + " : node " + perceptronIndex);

                    foreach (float weight in p.weights)
                    {
                        weightWriter.WriteLine(weight);
                    }

                    perceptronIndex++;
                }
            }
            weightWriter.Close();

            //save word database
            File.Delete(wordDatabaseSavePath);
            StreamWriter writer = new StreamWriter(File.OpenWrite(wordDatabaseSavePath));

            foreach (var data in ChatbotCore.loadedDataBase)
            {
                writer.WriteLine(data.Key + ":" + data.Value);
            }
            writer.Close();
        }

        //refresh timer
        private void updateTimer_Tick(object sender, EventArgs e)
        {
            canvas.Refresh();
        }

        //rendering engine
        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int width = canvas.Width;
            int height = canvas.Height;

            float yOffs = 10;
            foreach (Tuple<String, int> msg in chatLog)
            {
                String starter = msg.Item2 == 0 ? "You: " : "Bot: ";

                g.DrawString(starter + msg.Item1, f, Brushes.Black, 10, yOffs - scroll);

                yOffs += g.MeasureString(starter + msg.Item1, f).Height + 10;
            }
        }

        //key down handler
        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                //add text
                String text = textBox.Text;
                textBox.Text = "";
                textBox.Refresh();

                chatLog.Add(Tuple.Create(text, 0));

                chatLog.Add(Tuple.Create("...", 1));
                canvas.Refresh();
                System.Threading.Thread.Sleep(500);

                chatLog.RemoveAt(chatLog.Count - 1);
                chatLog.Add(Tuple.Create(chatbot.SendData(text), 1));

                e.Handled = true;
                e.SuppressKeyPress = true;

                //scroll chat
                Graphics g = canvas.CreateGraphics();
                if (chatLog.Count * g.MeasureString("test", f).Height * 1.5F + 10 > canvas.Height)
                {
                    scroll += g.MeasureString("test", f).Height * 2 + 10;
                }

                //train chatbot
                int amountOfUserPosts = 0;
                foreach (var elemenent in chatLog)
                {
                    if (elemenent.Item2.Equals(0))
                    {
                        amountOfUserPosts++;
                    }
                }

                if (amountOfUserPosts % 2 == 0)
                {
                    String chatbotWord = chatLog[chatLog.Count - 3].Item1;
                    String userReply = chatLog[chatLog.Count - 2].Item1;

                    chatbot.brain.trainPerceptrons(chatbotWord, userReply);
                }
            }
        }
    }
}
