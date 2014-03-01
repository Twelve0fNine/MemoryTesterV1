using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;



namespace MemoryTest
{
    public partial class Form1 : Form
    {
        private const int blinkTime1 = 500; // Time a Flag is enabled
        private const int blinkTime2 = 750; // Time a Flag is disabled
        private static int combinationLength = 2; //Level based
        private static int level = 1; // Game-Level
        private static Decimal score = 0; //Game-Score
        private static Decimal lastscore = 0;//Last Saved Game Score
        private static List<int> combination = new List<int>(); // List to store the Combination
        private List<int> repeatcombination = new List<int>(); // List to store the Combination from Userinput
        /// <summary>
        /// Encapsulated Field 
        /// </summary>
        public List<int> Repeatcombination
        {
            get { return repeatcombination; }  // Get is the Method used to get the private value from "repeatcombination"
            set { repeatcombination = value; } // Set is the Method used to set the private value from "repeatcombination"
        }

        /// <summary>
        /// Random Class Object
        /// Is able to generate random Numbers
        /// http://msdn.microsoft.com/library/system.random%28v=vs.110%29.aspx
        /// </summary>
        private static Random random = new Random(); // Random Number-Generator

        /// <summary>
        /// Constructor of the Designed Window 
        /// Position and Properties of all added Controls (specified in File Form1.Designer.cs)
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Method that is triggered by the "Load"-Event
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            ReadHighscore(); // Read Highscore from File
            foreach (Highscore item in Highscore.scoreList) // Copy Highscore to Listbox
            {
                listBox1.Items.Add(item);
            }
            listBox1.Refresh(); // Refresh the Listbox
        }

        /// <summary>
        /// Method that is triggered by the "Closing"-Event
        /// will be triggered before all Objects get destroyed by the "Garbagecollector" 
        /// http://msdn.microsoft.com/en-us/library/0xy59wtx%28v=vs.110%29.aspx
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (score > lastscore)
            {
                SaveScore(score);
            }

            WriteHighscore();
        }

        /// <summary>
        /// Function to show a random "Color-Combination"
        /// </summary>
        private void ShowCombination()
        {
                int a; /*Variable to store the "Color-Code"
                        * | 1 = Yellow |
                        * | 2 = Green  |
                        * | 3 = Red    |
                        * | 4 = Blue   |
                        */

                combination.Clear(); //Clear the Combination List<> to get ready for next Round
                repeatcombination.Clear(); //Clear the Combination List<> to get ready for next Round
                Button_EnableDisabble(); //Disable "Dial-Buttons"

                for (int i = 0; i <= combinationLength; i++)//First "Game-Level" you have to remeber 3 colors (Indexbased)
                {                               //"level" is defiened with a value of "2"

                    a = random.Next(1, 4);      //Get a random "Color-Code"

                    switch (a)                  //Switch to show an upper "X" as an flag
                    {
                        case 1: button1.Text = "X";     //Change the Button.Text property
                            button1.Refresh();          //Redraw the Button ( needed to actually see the flag )
                            Thread.Sleep(blinkTime1);   //Wait a specified Time ( even needed to actually see the flag )
                            button1.Text = "";          //Clear the Text  
                            button1.Refresh();          //Redraw the Button
                            Thread.Sleep(blinkTime2);   //Wait ( used to recognize if the same Color is repeated )
                            combination.Add(1);         //Add the "Color-Code" to the Combination
                            continue;

                        case 2: button2.Text = "X";
                            button2.Refresh();
                            Thread.Sleep(blinkTime1);
                            button2.Text = "";
                            button2.Refresh();
                            combination.Add(2);
                            Thread.Sleep(blinkTime2);
                            continue;

                        case 3: button3.Text = "X";
                            button3.Refresh();
                            Thread.Sleep(blinkTime1);
                            button3.Text = "";
                            button3.Refresh();
                            combination.Add(3);
                            Thread.Sleep(blinkTime2);
                            continue;

                        case 4: button4.Text = "X";
                            button4.Refresh();
                            Thread.Sleep(blinkTime1);
                            button4.Text = "";
                            button4.Refresh();
                            combination.Add(4);
                            Thread.Sleep(blinkTime2);
                            continue;

                        default: break;

                    //End of Switch
                    }

                //End of For-Loop
                }
            combinationLength++; //Increase the Combination Length
            label2.Text = "Repeat it!";
            Button_EnableDisabble(); // Enable "Dial-Buttons"
        }

        /// <summary>
        /// Method to check if the repeated Combination matches to the given Combination 
        /// </summary>
        public void ValidateCombination()
        {
            int winCount = 0; // Number of matching Colors
            List<bool> valid = new List<bool>(); // List of Match(true)/Dismatch(false)

            for (int i = 0; i < combination.Count; i++) // Loop to compare the repeated "Color-Code" with the given "Color-Code"
            {
                if (repeatcombination[i] == combination[i])// Equal
                {
                    valid.Add(true); // Add value "true"
                }
                else // Not Equal
                {
                    valid.Add(false); // Add value "false"
                }
            }

            foreach (bool item in valid) // Count the matches
            {
                if (item == true)
                {
                    winCount++; // Increase 
                }
            }

            if (winCount == combination.Count) // Compare Number of Matches to the given Combination length
            {
                label2.Text = "Winner !!!"; // Notice to Player
                IncreaseScore(); // Distribute Profit
                Thread.Sleep(400); // Wait 400ms to switch to next Notice
                label2.Text = "Start to Continue ..."; // Notice to Player
            }
            else
            {
                label2.Text = "You loose ..."; // Notice to Player
                SaveScore(score); //save Highscore
                level = 1; //Reset Level
                score = 0; // Reset Score
                label3.Text = "0"; // Reset Score Label
            }

        }

        /// <summary>
        /// Methof to increase Highscore und update Label
        /// </summary>
        private void IncreaseScore()
        {
            Decimal scoreTmp = Convert.ToDecimal((level * 1000) * 0.75); // Calculation of new Highscore
            score += Math.Round(scoreTmp); // Round the new added Scorepoint
            label3.Text = Convert.ToString(score); //Update Label
            label3.Refresh();
            level++; //Increase Game-Level
        }

        /// <summary>
        /// The triggered Event when somebody clicks on the Start-Button
        /// </summary>
        private void button5_Click(object sender, EventArgs e)
        {
            label2.Text = "Stay Focused!";
            label2.Refresh();
            ShowCombination(); // Start the GAme
        }

        /// <summary>
        /// Save Playername and Highscore
        /// </summary>
        /// <param name="_score">Highscore</param>
        private void SaveScore(Decimal _score)
        { 
            string player = "";
            PlayerNameDialog pnd = new PlayerNameDialog(); //Open "PlayerNameDialog" for Userinput 
            if (pnd.ShowDialog() == DialogResult.OK && pnd.Playername != "Type your Nickname") //Check the Dialog Result and the entered Playername
            {
                player = pnd.Playername;
            }
            else
            {
                player = "Anonymous";
            }
            Highscore hs = new Highscore(player, score); // Create new Highscore from Class "Highscore"
            Highscore.scoreList.Add(hs); // Add Highscore to Highscorelist
            listBox1.Items.Add(hs); // Add Highscore to Listbox
            listBox1.Items.Clear(); //Clear Listbox
            Highscore.scoreList.Sort(); //Sort Listbox
            foreach (Highscore item in Highscore.scoreList)//Add Scores ti Listbox after Playername :(
            {
                listBox1.Items.Add(item);
            }
            listBox1.Refresh(); // Refresh Listbox
        }
        
        /// <summary>
        /// Function to change the Status of the "Dial-Buttons" 
        /// Used to dissable the "Repeat functionality" while the Programm is showing a Combination
        /// Important to avoid wrong Input from Users "Layer 8" ;)
        /// </summary>
        private void Button_EnableDisabble() 
        {
            button1.Enabled = !button1.Enabled; // You can use the "!" Operator to invoke boolean
            button2.Enabled = !button2.Enabled;
            button3.Enabled = !button3.Enabled;
            button4.Enabled = !button4.Enabled;
        }

        /// <summary>
        /// Method to check of maximum Number of repeates Colors is reached
        /// </summary>
        private void CheckMaxRepeatCount(int _code)
        {
            Repeatcombination.Add(_code); //Add "Color-Code" to Repeatlist
            if (repeatcombination.Count == combination.Count) // Check if maximum Repeatcount is reached
            {
                label2.Text = "Check in Progress ...";
                ValidateCombination(); // Start to check matched "Color-Codes"
            }
        }

        /// <summary>
        /// Yellow-Button
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            CheckMaxRepeatCount(1);
        }

        /// <summary>
        /// Green-Button
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            CheckMaxRepeatCount(2);
        }

        /// <summary>
        /// Red
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            CheckMaxRepeatCount(3);
        }

        /// <summary>
        /// Blue-Button
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            CheckMaxRepeatCount(4);
        }

        /// <summary>
        /// Save-Button
        /// </summary>
        private void button6_Click(object sender, EventArgs e)
        {
            if (score > lastscore)// Check if actual score is higher as the last save one ... avoid Duplicates
            {
                lastscore = score;
                SaveScore(score); 
            }
            
        }

        
        /// <summary>
        /// Write Highscore to File
        /// Code taken from MSDN
        /// msdn.microsoft.com/library/system.runtime.serialization.formatters.binary.binaryformatter(v=vs.110).aspx
        /// </summary>
        private void WriteHighscore()
        {
            FileStream fs = new FileStream("Highscore.dat", FileMode.Create);

            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, Highscore.scoreList);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        /// <summary>
        /// Read Highscore from File
        /// Code taken from MSDN
        /// msdn.microsoft.com/library/system.runtime.serialization.formatters.binary.binaryformatter(v=vs.110).aspx
        /// </summary>
        private void ReadHighscore()
        {
            FileStream fs = new FileStream("Highscore.dat", FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();

                // Deserialize the hashtable from the file and 
                // assign the reference to the local variable.
                Highscore.scoreList = (List<Highscore>)formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        

    }

    /// <summary>
    /// Serializable Class
    /// Contains Playername and Highscore to create new Highscore-Object
    /// </summary>
    [Serializable]
    public class Highscore
    {
        public static List<Highscore> scoreList = new List<Highscore>(); // Highscorelist with all Scores

        private Decimal score; //Score-Property
        public Decimal Score
        {
            get { return score; }
            set { score = value; }
        }

        private string name; //NAme-Property
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Highscore(string _name, Decimal _score) //Constructor
        {
            Name = _name;
            Score = _score;
        }

        public override string ToString() // Override to String Method to give the Listbox a nicer Look
        {
            string scoreStr = Convert.ToString(Score);
            StringBuilder sb = new StringBuilder(scoreStr,0,scoreStr.Length,20);
            return sb + "\t" +Name;
        }

    
    }

}
