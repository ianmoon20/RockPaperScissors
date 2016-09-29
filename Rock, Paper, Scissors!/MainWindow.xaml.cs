using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Rock__Paper__Scissors_
{
    [Serializable]

    //Creating a class that will have attributes which contain the users past playthrough information
    class GameData
    {
        public GameData()
        {
            gameList = new List<string>();
        }

        public List<string> gameList { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }
        public int Ties { get; set; }
        public int Rock { get; set; }
        public int Scissors { get; set; }
        public int Paper { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int rocksThrown { get; set; }
        public int scissorsThrown { get; set; }
        public int papersThrown { get; set; }
    }
    public partial class MainWindow : Window
    {
        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyy/MM/dd/HH:mm:ss");
        }

        //Creating a random number generator
        public Random rgen = new Random();

        //Creating a GameData Holder
        GameData data = new GameData();

        //Creating a serializer
        JsonSerializer ser = new JsonSerializer();

        //Creating a list to store events
        List<string> list = new List<string>();
        List<string> dataList = new List<string>();

        //Creating variables to store data
        int rockThrown = 0;
        int scissorsThrown = 0;
        int paperThrown = 0;
        int weightedRock = 0;
        int weightedScissor = 0;
        int weightedPaper = 0;
        string computerChoiceStr = "";
        double playerWins = 0;
        int playerLoses = 0;
        int playerTies = 0;
        double totalGames = 0;
        double winPercentage = 0.0;
        double lossPercentage = 0.0;
        int rockPercentage = 33;
        int scissorPercentage = 33;
        int paperPercentage = 33;
        string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"RPSGdata";
        string name;
        string password = "DEFAULT PASSWORD";
        public string computerNumGen()
        {
            //Generating a random number and multiplying it to each choice
            weightedPaper = paperPercentage * rgen.Next(1, 30);
            weightedRock = rockPercentage * rgen.Next(1, 30);
            weightedScissor = scissorPercentage * rgen.Next(1, 30);

            //Debugging tools for initial calculations
            //Console.WriteLine("");
            //Console.WriteLine("Weighted Rock: {0}, Weighted Paper: {1}, Weighted Scissor: {2}", weightedRock, weightedPaper, weightedScissor);
            //Console.WriteLine("Rock Percentage: {0} Paper Percentage: {1} Scissor Percentage: {2}", rockPercentage, paperPercentage, scissorPercentage);


            //Figuring out which number is highest and making that the computers choice
            if (weightedRock > weightedPaper && weightedRock > weightedScissor)
            {
                computerChoiceStr = "R";
                userInputTxtBox.Text = userInputTxtBox.Text + "\n The computer chose rock.\n";
            }
            else if (weightedPaper > weightedRock && weightedPaper > weightedScissor)
            {
                computerChoiceStr = "P";
                userInputTxtBox.Text = userInputTxtBox.Text + "\n The computer chose paper.\n";
            }
            else if (weightedScissor > weightedRock && weightedScissor > weightedPaper)
            {
                computerChoiceStr = "S";
                userInputTxtBox.Text = userInputTxtBox.Text + "\n The computer chose scissor.\n";
            }
            return computerChoiceStr;
        }
        //A method to print the player's win/loss record.
        public void printStats()
        {
            //Debugging tool for after-math percentage, uncomment to use
            //Console.WriteLine("Rock Percentage: {0} Paper Percentage: {1} Scissor Percentage: {2}", rockPercentage, paperPercentage, scissorPercentage);
            //Console.WriteLine("");

            //Debug: Figuring out where the log file goes
            //Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\RockPaperScissorsLog.txt");

            totalGames = playerLoses + playerWins + playerTies;
            if (totalGames > 0)
            {
                winPercentage = Math.Round((playerWins / totalGames) * 100, 1);
                lossPercentage = Math.Round((playerLoses / totalGames) * 100, 1);
                userInputTxtBox.Text = userInputTxtBox.Text + "\nTotal Games: " + totalGames;
                userInputTxtBox.Text = userInputTxtBox.Text + "\nYour Win/Loss/Tie Record is: " + playerWins + "/" + playerLoses + "/" + playerTies;
                userInputTxtBox.Text = userInputTxtBox.Text + "\nWin Percentage: " + winPercentage + "%";
                userInputTxtBox.Text = userInputTxtBox.Text + "\nLoss Percentage: " + lossPercentage + "%";
            }
            else
            {
                userInputTxtBox.Text = userInputTxtBox.Text + "\nYour Win/Loss/Tie Record is: " + playerWins + "/" + playerLoses + "/" + playerTies;
                userInputTxtBox.Text = userInputTxtBox.Text + "\nWin Percentage: N/A";
                userInputTxtBox.Text = userInputTxtBox.Text + "\nLoss Percentage: N/A";
            }
        }
        public void eventLog()
        {
            for (int i = 0; i < list.Count; i++) // Loop with for.
            {
                Console.WriteLine(list[i]);
            }
            Console.WriteLine("");
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void rockButton_Click(object sender, RoutedEventArgs e)
        {
            userInputTxtBox.Text = "You threw Rock.";
            rockThrown = rockThrown + 1;
            computerNumGen();
            if (paperPercentage <= 98)
            {
                paperPercentage = paperPercentage + 2;
                rockPercentage = rockPercentage - 1;
                scissorPercentage = scissorPercentage - 1;
            }

            if (computerChoiceStr == "R")
            {
                userInputTxtBox.Text = userInputTxtBox.Text + "It's a tie!";
                list.Add("D: RvR");
                playerTies = playerTies + 1;
            }
            else if (computerChoiceStr == "P")
            {
                userInputTxtBox.Text = userInputTxtBox.Text + "Computer wins!";
                list.Add("L: RvP");
                playerLoses = playerLoses + 1;
            }
            else if (computerChoiceStr == "S")
            {
                userInputTxtBox.Text = userInputTxtBox.Text + "You win!";
                list.Add("W: RvS");
                playerWins = playerWins + 1;
            }
            printStats();
        }

        private void scissorButton_Click(object sender, RoutedEventArgs e)
        {
            userInputTxtBox.Text = "You threw Scissors.";
            scissorsThrown = scissorsThrown + 1;
            computerNumGen();
            if (rockPercentage <= 98)
            {
                paperPercentage = paperPercentage - 1;
                rockPercentage = rockPercentage + 2;
                scissorPercentage = scissorPercentage - 1;
            }

            if (computerChoiceStr == "R")
            {
                userInputTxtBox.Text = userInputTxtBox.Text + "Computer wins!";
                list.Add("L: SvR");
                playerLoses = playerLoses + 1;
            }
            else if (computerChoiceStr == "P")
            {
                userInputTxtBox.Text = userInputTxtBox.Text + "You win!";
                list.Add("W: SvP");
                playerWins = playerWins + 1;
            }
            else if (computerChoiceStr == "S")
            {
                userInputTxtBox.Text = userInputTxtBox.Text + "It's a tie!";
                list.Add("D: SvS");
                playerTies = playerTies + 1;
            }
            printStats();
        }
        private void paperButton_Click(object sender, RoutedEventArgs e)
        {
            userInputTxtBox.Text = "You threw Paper.";
            paperThrown = paperThrown + 1;
            computerNumGen();
            if (scissorPercentage <= 98)
            {
                paperPercentage = paperPercentage - 1;
                rockPercentage = rockPercentage - 1;
                scissorPercentage = scissorPercentage + 2;
            }
            if (computerChoiceStr == "R")
            {
                userInputTxtBox.Text = userInputTxtBox.Text + "You win!";
                list.Add("W: PvR");
                playerWins = playerWins + 1;
            }
            else if (computerChoiceStr == "P")
            {
                userInputTxtBox.Text = userInputTxtBox.Text + "It's a tie!";
                list.Add("D: PvP");
                playerTies = playerTies + 1;
            }
            else if (computerChoiceStr == "S")
            {
                userInputTxtBox.Text = userInputTxtBox.Text + "Computer wins!";
                list.Add("L: PvS");
                playerLoses = playerLoses + 1;
            }
            printStats();
        }
        //Confirming passwords and names
        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            nameTextBox.Text = string.Concat(nameTextBox.Text.Where(char.IsLetterOrDigit));
            if (nameTextBox.Text.Length > 0)
            {
                name = nameTextBox.Text;
                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\RPSGdata\" + name + "\\RockPaperScissorsLog.json") == true)
                {
                    //Creating a StreamReader to read the file
                    using (StreamReader r = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\RPSGdata\" + name + "\\RockPaperScissorsLog.json"))
                    {
                        string json = r.ReadToEnd();
                        JsonTextReader reader = new JsonTextReader(new StringReader(json));
                        //Reading the file and adding every value in the file to a list.
                        while (reader.Read())
                        {
                            if (reader.Value != null)
                            {
                                //converting the values to strings
                                dataList.Add(reader.Value.ToString());
                            }
                        }

                        //Reversing the array so I can easily deal with the elements I want to
                        dataList.Reverse();

                        //dataList Debugger for the reverse function
                        //for( int i = 0; i < dataList.Count; i++)
                        //{
                        //Console.WriteLine(dataList[i]);
                        //}

                        //Setting the data from the json file to a string
                        string newRockThrown = dataList[0];
                        string newScissorThrown = dataList[2];
                        string newPaperThrown = dataList[4];
                        string newPassword = dataList[6];
                        string newName = dataList[8];
                        string paperMod = dataList[10];
                        string scissorMod = dataList[12];
                        string rockMod = dataList[14];
                        string tiesMod = dataList[16];
                        string losesMod = dataList[18];
                        string winsMod = dataList[20];

                        //converting the string'd json files to usable int variables for the program to make calculations off of
                        int.TryParse(newRockThrown, out rockThrown);
                        int.TryParse(newScissorThrown, out scissorsThrown);
                        int.TryParse(newPaperThrown, out paperThrown);
                        int.TryParse(paperMod, out paperPercentage);
                        int.TryParse(scissorMod, out scissorPercentage);
                        int.TryParse(rockMod, out rockPercentage);
                        int.TryParse(tiesMod, out playerTies);
                        int.TryParse(losesMod, out playerLoses);
                        double.TryParse(winsMod, out playerWins);
                        name = newName;
                        password = newPassword;

                        passwordBox.Visibility = Visibility.Visible;
                        passwordBox.IsEnabled = true;
                        passwordBox.ToolTip = "Type in your password";
                    }
                    //If they type in the right password, make the appropriate changes
                    if (passwordBox.Password == password && password.Length > 0)
                    {
                        userInputTxtBox.Text = "Click a button below to select your throw:";
                        nameTextBox.Text = name;
                        nameTextBox.IsReadOnly = true;
                        nameTextBox.Margin = new Thickness(10, 5, 0, 0);
                        resetButton.IsEnabled = true;
                        scissorButton.IsEnabled = true;
                        rockButton.IsEnabled = true;
                        paperButton.IsEnabled = true;
                        resetButton.Visibility = Visibility.Visible;
                        scissorButton.Visibility = Visibility.Visible;
                        rockButton.Visibility = Visibility.Visible;
                        paperButton.Visibility = Visibility.Visible;
                        confirmButton.Visibility = Visibility.Collapsed;
                        passwordBox.Visibility = Visibility.Collapsed;
                    }
                    else if (passwordBox.Password != password)
                    {
                        userInputTxtBox.Text = "Invalid password.";
                    }
                }
                //if they are creating an account, have them make a password that's not default password or an empty string
                else
                {
                    nameTextBox.Text = name;
                    nameTextBox.IsReadOnly = true;
                    nameTextBox.Margin = new Thickness(10, 5, 0, 0);
                    passwordBox.Visibility = Visibility.Visible;
                    passwordBox.IsEnabled = true;
                    passwordBox.ToolTip = "Create a password..";
                    if (passwordBox.Password != "DEFAULT PASSWORD" && passwordBox.Password.Length > 0)
                    {
                        password = passwordBox.Password;
                        resetButton.IsEnabled = true;
                        scissorButton.IsEnabled = true;
                        rockButton.IsEnabled = true;
                        paperButton.IsEnabled = true;
                        resetButton.Visibility = Visibility.Visible;
                        scissorButton.Visibility = Visibility.Visible;
                        rockButton.Visibility = Visibility.Visible;
                        paperButton.Visibility = Visibility.Visible;
                        confirmButton.Visibility = Visibility.Collapsed;
                        passwordBox.Visibility = Visibility.Collapsed;
                    }
                }
                //Special Greetings for Special People.
                if (name.ToUpper() == "DSHEDDEN")
                {
                    nameTextBox.Margin = new Thickness(10, 5, 0, 0);
                    userInputTxtBox.Text = "Greetings " + name + ", ready to get 4/20 no-scoped #Blazeit rekeruni'd? Let the edge \noff commence! Mouse over the box below.";
                    passwordBox.Margin = new Thickness(140, 80, 0, 0);
                    confirmButton.Margin = new Thickness(165, 110, 0, 0);
                    scissorPercentage = 98;
                    rockPercentage = 0;
                    paperPercentage = 0;
                }
                else if (name.ToUpper() == "DSINGER")
                {
                    nameTextBox.Margin = new Thickness(10, 5, 0, 0);
                    userInputTxtBox.Text = "Greetings princess! Mouse over the box below.";
                }
                else if (name.ToUpper() == "LIAM")
                {
                    nameTextBox.Margin = new Thickness(10, 5, 0, 0);
                    userInputTxtBox.Text = "Greetings Harry Potter! Mouse over the box below.";
                }
                else
                {
                    nameTextBox.Margin = new Thickness(10, 5, 0, 0);
                    userInputTxtBox.Text = "Greetings " + name + "! Mouse over the box below.";
                }

                if (passwordBox.Password == password)
                {
                    userInputTxtBox.Text = "\nClick a button below to select your throw:";
                }
            }
        }
        //Reset Button resets stats
        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            userInputTxtBox.Text = "\n\n\nAre you sure you want to reset all of your hard earned data?";
            noButton.IsEnabled = true;
            yesButton.IsEnabled = true;
            scissorButton.IsEnabled = false;
            rockButton.IsEnabled = false;
            paperButton.IsEnabled = false;
            yesButton.Visibility = Visibility.Visible;
            noButton.Visibility = Visibility.Visible;
            scissorButton.Visibility = Visibility.Hidden;
            rockButton.Visibility = Visibility.Hidden;
            paperButton.Visibility = Visibility.Hidden;
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            if (name != null && password != "")
            {
                //SpecialFolders
                using (FileStream fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\RockPaperScissorsLog.txt", FileMode.Append, FileAccess.Write))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("");
                    sw.WriteLine(name);
                    string timeStamp = GetTimestamp(DateTime.Now);
                    sw.WriteLine("");
                    sw.WriteLine(timeStamp);
                    sw.WriteLine("Rocks Thrown: {0} Scissors Thrown: {1} Papers Thrown: {2}", rockThrown, scissorsThrown, paperThrown);
                    sw.WriteLine("Your Win/Loss/Tie Record is: {0}/{1}/{2}", playerWins, playerLoses, playerTies);
                    sw.WriteLine("Win Percentage: " + winPercentage + "%");
                    sw.WriteLine("Loss Percentage: " + lossPercentage + "%");
                }
                //Setting the variables to be stored in the json file
                data.Wins = (int)playerWins;
                data.Loses = playerLoses;
                data.Ties = playerTies;
                data.gameList.AddRange(list);
                data.Rock = rockPercentage;
                data.Scissors = scissorPercentage;
                data.Paper = paperPercentage;
                data.Name = name;
                data.Password = password;
                data.rocksThrown = rockThrown;
                data.scissorsThrown = scissorsThrown;
                data.papersThrown = paperThrown;

                if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\RPSGdata\" + name + "\\") == true)
                {
                    ////Writing the variables to a json file if the folder exists
                    using (TextWriter text = File.CreateText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\RPSGdata\" + name + "\\RockPaperScissorsLog.json"))
                    {
                        ser.Serialize(text, data);
                    }
                }
                else
                {
                    //Creating the directory if it doesn't exist, then writing the variables to it.
                    DirectoryInfo newDir = Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\RPSGdata\");
                    DirectoryInfo subDir = newDir.CreateSubdirectory(name);
                    using (TextWriter text = File.CreateText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\RPSGdata\" + name + "\\RockPaperScissorsLog.json"))
                    {
                        ser.Serialize(text, data);
                    }
                }
            }

            this.Close();
        }


        //Clearing a textbox upon selection
        public void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= TextBox_GotFocus;
        }


        //Resetting the players profile
        private void yesButton_Click(object sender, RoutedEventArgs e)
        {
            noButton.IsEnabled = false;
            yesButton.IsEnabled = false;
            scissorButton.IsEnabled = true;
            rockButton.IsEnabled = true;
            paperButton.IsEnabled = true;
            noButton.Visibility = Visibility.Hidden;
            yesButton.Visibility = Visibility.Hidden;
            scissorButton.Visibility = Visibility.Visible;
            rockButton.Visibility = Visibility.Visible;
            paperButton.Visibility = Visibility.Visible;
            rockThrown = 0;
            scissorsThrown = 0;
            paperThrown = 0;
            playerWins = 0;
            playerLoses = 0;
            playerTies = 0;
            totalGames = 0;
            winPercentage = 0.0;
            lossPercentage = 0.0;
            rockPercentage = 33;
            scissorPercentage = 33;
            paperPercentage = 33;
            userInputTxtBox.Text = "\n\nYour data had been successfully reset!";
        }

        //Resetting the UI to the default
        private void noButton_Click(object sender, RoutedEventArgs e)
        {
            noButton.IsEnabled = false;
            yesButton.IsEnabled = false;
            scissorButton.IsEnabled = true;
            rockButton.IsEnabled = true;
            paperButton.IsEnabled = true;
            noButton.Visibility = Visibility.Hidden;
            yesButton.Visibility = Visibility.Hidden;
            scissorButton.Visibility = Visibility.Visible;
            rockButton.Visibility = Visibility.Visible;
            paperButton.Visibility = Visibility.Visible;
            userInputTxtBox.Text = "\n Data not reset. \nClick a button below to select your throw:";
        }

        //private void colorButton_Click(object sender, RoutedEventArgs e)
        //{
        //    BrushConverter bc = new BrushConverter();
        //    if (mainWindow.Background.ToString() == "#FF1E2AB2")
        //    {
        //        mainWindow.Background = Brushes.Black;

        //        colorButton.Background = Brushes.White;
        //        colorButton.Foreground = Brushes.Black;

        //        confirmButton.Background = Brushes.White;
        //        confirmButton.Foreground = Brushes.Black;

        //        exitButton.Background = Brushes.White;
        //        exitButton.Foreground = Brushes.Black;

        //        noButton.Background = Brushes.White;
        //        noButton.Foreground = Brushes.Black;

        //        paperButton.Background = Brushes.White;
        //        paperButton.Foreground = Brushes.Black;

        //        yesButton.Background = Brushes.White;
        //        yesButton.Foreground = Brushes.Black;

        //        rockButton.Background = Brushes.White;
        //        rockButton.Foreground = Brushes.Black;

        //        scissorButton.Background = Brushes.White;
        //        scissorButton.Foreground = Brushes.Black;

        //        resetButton.Background = Brushes.White;
        //        resetButton.Foreground = Brushes.Black;

        //        userInputTxtBox.Foreground = Brushes.White;

        //        nameTextBox.Background = Brushes.White;
        //        nameTextBox.Foreground = Brushes.Black;

        //        passwordBox.Background = Brushes.White;
        //        passwordBox.Foreground = Brushes.Black;
        //    }
        //    else if (mainWindow.Background == Brushes.Black)
        //    {
        //    }
        //}
    }
}
