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
        int currTimeAlottment = 0;
        int minSents, minWords;
        string testName;
        string currEncrypt;
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
            sentences.Content = "";
            words.Content = "";
            time.Content = "";

            Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog();
            openFileDialog1.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = false;
            bool? userClickedOK = openFileDialog1.ShowDialog();
            if (userClickedOK == true)
            {
                // Open the selected file to read.

                currentProject.Content = openFileDialog1.SafeFileName;

                String fileText = System.IO.File.ReadAllText(openFileDialog1.FileName);

                textContent.Text = fileText;

            }
            
        }
        private void timerBegin()
        {
            myTimer = new Timer(currTimeAlottment*60*1000);
            myTimer.Elapsed += new ElapsedEventHandler(myTimer_Elapsed);
            myTimer.Enabled = true;
            
        }
        private void changeContent(string content)
        {
            //textContent.Text = content;


            Action act = () => { textContent.IsReadOnly = !textContent.IsReadOnly; };
            textContent.Dispatcher.Invoke(act);

        
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
                    time.Content = "Time: " + currTimeAlottment;
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
            if (currTimeAlottment != 0)
            {
                textContent.IsReadOnly = false;
                timerBegin();
            }
            else
            {
                return;
            }
        }
    }
}
