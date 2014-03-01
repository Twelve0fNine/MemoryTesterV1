using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MemoryTest
{
    /// <summary>
    /// This is a new Dialog-Window to get Userinput
    /// </summary>
    public partial class PlayerNameDialog : Form
    {
        private string playername = "";
        public string Playername
        {
            get { return playername; }
            set { playername = value; }
        }

        public PlayerNameDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Textchanged-Event triggered every time the text inside the Textbox changed
        /// </summary>
        private void textBox1_TextChanged(object sender, EventArgs e) 
        {
            this.textBox1.ForeColor = System.Drawing.Color.Black;
            this.textBox1.Refresh();
        }

        /// <summary>
        /// Function to store the Value of Textbox2 to the Variable Playername
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            Playername = textBox1.Text;
        }
    }
}
