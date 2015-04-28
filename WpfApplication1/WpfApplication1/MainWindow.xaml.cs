using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Timer myTimer;
        static Timer remainingTime; 
        int currTimeAlottment = 0;
        double currRemainTime = 0;
        Boolean testMode = false;
        int minSents, minWords;
        string testName;
        string assignmentName; 
        string currEncrypt;
        string userID = null; 
        List<string> wordBank = new List<string>(0);
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            mode.Content = "Mode: Assignment";
            textContent.IsReadOnly = false;
            currTimeAlottment = 0;
            minSents = 0;
            minWords = 0;
            testName = null;
            //sentences.Content = "";
            //words.Content = "";
            //remainingTime = "";

            Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog();
            openFileDialog1.Filter = "tySon (.)|*.tySon|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = false;
            bool? userClickedOK = openFileDialog1.ShowDialog();
            if (userClickedOK == true)
            {
                // Open the selected file to read.

                assignmentName = openFileDialog1.SafeFileName.Replace(".tySon", "");
                currentProject.Content = assignmentName;
                String fileText = System.IO.File.ReadAllText(openFileDialog1.FileName);

                textContent.Text = fileText;

            }
            
        }
        private void timerBegin()
        {
            switchMode(1);
            myTimer = new Timer(currTimeAlottment*60*1000);
            myTimer.Elapsed += new ElapsedEventHandler(myTimer_Elapsed);
            myTimer.Enabled = true;
            
        }
        private void updateTimer()
        {
            int minutes = (int)(currRemainTime / 60);
            double seconds = currRemainTime - (minutes * 60);
            Action act = () => { timeRemaining.Content = "Time" + minutes.ToString() + " : 0" + seconds.ToString(); };
            remainingTime = new Timer(1000);
            remainingTime.Elapsed += new ElapsedEventHandler(updateInterval);
            remainingTime.Enabled = true;
            remainingTime.AutoReset = true;

        }
        private void changeContent(string content)
        {
            //textContent.Text = content;


            Action act = () => { textContent.IsReadOnly = !textContent.IsReadOnly; };
            textContent.Dispatcher.Invoke(act);

        
        }
        private void updateInterval(object sender, ElapsedEventArgs e)
        {
            
            if (currRemainTime == 0)
            {
                remainingTime.Enabled = false;
                int minutes = (int)(currRemainTime / 60);
                double seconds = currRemainTime - (minutes * 60);
                Action act = () => { timeRemaining.Content = "Time: " + minutes.ToString() + " : 0" + seconds.ToString(); };
                textContent.Dispatcher.Invoke(act);


            }
            else
            {
                currRemainTime = currRemainTime - 1;
                int minutes = (int)(currRemainTime / 60);
                double seconds = currRemainTime - (minutes * 60);
                if (seconds >= 10)
                {
                    Action act = () => { timeRemaining.Content = "Time: " + minutes.ToString() + " : " + seconds.ToString(); };
                    textContent.Dispatcher.Invoke(act);
                }
                else
                {
                    Action act = () => { timeRemaining.Content = "Time: " + minutes.ToString() + " : 0" + seconds.ToString(); };
                    textContent.Dispatcher.Invoke(act);    
                }

                

                
            }
            

        }
        private void myTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
             myTimer.Enabled = false;
            changeContent(DateTime.Now.ToString());
            realEncrypt();
            string path = Directory.GetCurrentDirectory();
            string fileID = "Example";
            using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(path+ fileID + ".txt", false))
            {
                outputFile.WriteLine(currEncrypt);
            }
            switchMode(0);
        }
        private void encrypt(object sender, RoutedEventArgs e)
        {
            byte[] keyArray;
            
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(textContent.Text);

            string key = "Password";
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0,
      toEncryptArray.Length);
            tdes.Clear();
            currEncrypt = Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        private void realEncrypt()
        {
            byte[] keyArray;
            string myContent = "";
            Action act = () => { myContent = textContent.Text; };
            textContent.Dispatcher.Invoke(act);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(myContent);

            string key = "Marmalade";
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0,
      toEncryptArray.Length);
            tdes.Clear();
            currEncrypt = Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        private void decrypt(object sender, RoutedEventArgs e)
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(textContent.Text);

            
            //Get your key from config file to open the lock!
            string key = "Password";

            
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
           
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            textContent.Text = UTF8Encoding.UTF8.GetString(resultArray);
        }
        private void hide(object sender, RoutedEventArgs e)
        {
            if (textContent.IsVisible)
            {
                textContent.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                textContent.Visibility = System.Windows.Visibility.Visible;
                }
        }
        private void loadTest(object sender, RoutedEventArgs e)
        {

            List<bool> contents = new List<bool>(0);
            Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog();
            openFileDialog1.Filter = "Tyson Test Files (.tySon)|*.tySon|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = false;
            bool? userClickedOK = openFileDialog1.ShowDialog();
            if (userClickedOK == true)
            {
                // Open the selected file to read.

                /*Order of test settings 
                * Minimum Sentences
                * Minimum Words
                * Duration of Test
                * WordBank
                */

                mode.Content = "Mode: Test";
                textContent.IsReadOnly = true; 
                int currPointer = 4;
                string[] lines = System.IO.File.ReadAllLines(openFileDialog1.FileName);
                for (int i = 0; i < 4; i++)
                {
                    if (lines[i].ToLower().Equals("true"))
                    {
                        contents.Add(true);
                    }
                    else
                        contents.Add(false);

                }
                bool[] contentA = contents.ToArray();

                if (contentA[0] == true)
                {
                    minSents= Int32.Parse(lines[currPointer]);
                    sentences.Content = "Sentences: " + minSents.ToString();
                    currPointer++;

                }
                if (contentA[1] == true)
                {
                    minWords = Int32.Parse( lines[currPointer]);
                    words.Content = "Words: " + minWords.ToString();
                    currPointer++;
                }
                if (contentA[2] == true)
                {
                    currTimeAlottment = Int32.Parse(lines[currPointer]);
                    timeRemaining.Content = "Time: " + currTimeAlottment + ": 00";
                    currPointer++;

                }
                if (contentA[3] == true)
                {
                    List<string> temporaryWordBank = new List<string>(0);
                    for (int index = currPointer; index < lines.Length; index++)
                    {
                        
                        temporaryWordBank.Add(lines[index]);
                    }
                    wordBank = temporaryWordBank;
                }


                
                testName = openFileDialog1.SafeFileName.Replace(".tySon", "");
                currentProject.Content = testName;
            }
        }
        private void testBegin(object sender, RoutedEventArgs e)
        {
            userID = testName + muID.Text.Replace("901", "") + studentName.Text.Replace(" ", ""); 
            if (currTimeAlottment != 0)
            {
                currRemainTime = currTimeAlottment*60; 
                textContent.IsReadOnly = false;                
                updateTimer();
                timerBegin();
                
            }
            else
            {
                return;
            }
        }
        private void loadText(object sender, RoutedEventArgs e)
        {
          

            Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog();
            openFileDialog1.Filter = "Text (.)|*.txt|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = false;
            bool? userClickedOK = openFileDialog1.ShowDialog();
            if (userClickedOK == true)
            {
                // Open the selected file to read.

                currentProject.Content = openFileDialog1.SafeFileName.Replace(".txt", "");

                String fileText = System.IO.File.ReadAllText(openFileDialog1.FileName);

                textContent.Text = fileText;

            }
        }
        private void switchMode(int modeSetting) {
            
            // sets to initial mode meaning all locks are removed, all files are cleared, and the system is refreshed
            if (modeSetting == 0)
            {
                Action act = () =>
                {
                    fileMenu.IsEnabled = true; 
                    currentDisplay.WindowStyle = System.Windows.WindowStyle.None;
                };
                testMode = false;
                textContent.Dispatcher.Invoke(act);
                myTimer.Close();
                remainingTime.Close();
                
            }
            //Locks system down to test mode
            else if(modeSetting == 1)
            {
                
                Action act = () =>
                {
                    currentDisplay.WindowStyle = System.Windows.WindowStyle.None;
                    currentDisplay.WindowState = System.Windows.WindowState.Maximized;
                    fileMenu.IsEnabled = false;
                };
                textContent.Dispatcher.Invoke(act);
                testMode = true;
            }

        
        }

        private void changeIt(object sender, RoutedEventArgs e)
        {
            if (testMode == true)
            {
                realEncrypt();
                string path = Directory.GetCurrentDirectory();
                if (userID != null)
                {
                    using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(path + "\\" + userID + ".tyTxt", false))
                    {
                        outputFile.WriteLine(currEncrypt);
                    }
                    
                }
                else
                {
                    using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(path + "\\" + "noNameProvided" + ".tyTxt", false))
                    {
                        outputFile.WriteLine(currEncrypt);
                    }
                    


                }

            }
            switchMode(0);
        }

        private void currentDisplay_StateChanged(object sender, EventArgs e)
        {
            
        }

        private void currentDisplay_Deactivated(object sender, EventArgs e)
        {
            if (testMode == true)
            {
                realEncrypt();
                string path = Directory.GetCurrentDirectory();
                if (userID != null)
                {
                    using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(path + "\\" + userID + ".tyTxt", false))
                    {
                        outputFile.WriteLine(currEncrypt);
                    }
                    Application.Current.Shutdown();
                }
                else
                {
                    using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(path + "\\" + "noNameProvided" + ".tyTxt", false))
                    {
                        outputFile.WriteLine(currEncrypt);
                    }
                    Application.Current.Shutdown();
                
                
                }

            }
        }

        private void saveAssignment(object sender, RoutedEventArgs e)
        {
                
                using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(assignmentName, false))
                {
                    string Essay = Content.Text;
                    outputFile.WriteLine(Essay);
                    outputFile.WriteLine("-:Tyson Results:-");
                    outputFile.WriteLine("----------------------------------------------------");
                    outputFile.WriteLine("Total Sentences: " + currentSentCount.ToString());
                    outputFile.WriteLine("Word Count: " + currentWordCount.ToString());
                    outputFile.WriteLine("Unique Words: " + (UniqueWords(Essay).ToString()));
                    if (paperPass == true)
                    {
                        outputFile.WriteLine("Paper Passes the Criteria");
                    }
                    else
                        outputFile.WriteLine("Paper Does not pass the Criteria");
                    outputFile.WriteLine("File up to date as of: " + DateTime.Now.ToShortDateString());


                }

        }


    }
}
