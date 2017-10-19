using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CorePerceptron_Chatbot
{
    public partial class Game : Form
    {
        List<Tuple<String, int>> chatLog = new List<Tuple<String, int>>();

        //initialize
        public Game()
        {
            InitializeComponent();

            //set window not resizable
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            //start game loop
            updateTimer.Start();
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

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}
