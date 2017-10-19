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
        }

        //save data before exit
        private void SaveOnExit(object sender, EventArgs e)
        {
            //save neural network weights

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
            Font f = new Font(FontFamily.GenericSansSerif, 15, FontStyle.Bold);

            float yOffs = 10;
            foreach (Tuple<String, int> msg in chatLog)
            {
                String starter = msg.Item2 == 0 ? "You: " : "Bot: ";

                g.DrawString(starter + msg.Item1, f, Brushes.Black, 10, yOffs);

                yOffs += g.MeasureString(starter + msg.Item1, f).Height + 10;
            }
        }

        //key down handler
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                chatLog.Add(Tuple.Create(textBox.Text, 0));
                textBox.Text = "";
                textBox.Refresh();

                chatLog.Add(Tuple.Create("...", 1));
                canvas.Refresh();
                System.Threading.Thread.Sleep(500);

                chatLog.RemoveAt(chatLog.Count - 1);
                chatLog.Add(Tuple.Create(chatbot.SendData(textBox.Text), 1));

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}
