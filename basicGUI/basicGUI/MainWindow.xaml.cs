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
using System.Timers;
using System.IO;
using edu.stanford.nlp.parser;
using java.util;
using edu.stanford.nlp.ie.crf;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.util;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web;


namespace basicGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> currFileList = new List<string>(0);
        string currfile = null;
        List<string> wordBank = new List<string>(0);
        int currentSentCount = 0;
        int currentWordCount = 0;
        string testKey;
        Boolean paperPass = false;
        Boolean testModeIsEnabled = false;
        static System.Timers.Timer myTimer;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }
        private void Content_TextChanged(object sender, TextChangedEventArgs e)
        {
            try{
                Update2();
            }
                catch(NullReferenceException ex){
                   
                }
            
        }
        private void Update()
        {
            
            string Essay = Content.Text;
            wordCount(Essay);
            WordCount.Text = (currentWordCount.ToString());
            UniqueCount.Text = (UniqueWords(Essay).ToString());
            getSentences(Essay);
            Sentences.Text = (currentSentCount.ToString());

            FrequencyOfWord.Text = frequencyOf(FrequencyWord.Text, Essay).ToString();
            updateListBox();
            checkCriteria();
            
        }

        private void Update2()
        {

            string Essay = Content.Text;
            wordCount(Essay);
            WordCount.Text = (currentWordCount.ToString());
            UniqueCount.Text = (UniqueWords(Essay).ToString());
            getSentences(Essay);
            Sentences.Text = (currentSentCount.ToString());

            FrequencyOfWord.Text = frequencyOf(FrequencyWord.Text, Essay).ToString();
            updateListBox();
            checkCriteria();
        }
        private void wordCount(string inTextBlock)
        {
            inTextBlock = inTextBlock.Trim();
            List<string> words = inTextBlock.Split(' ').ToList();
            if (words.Contains(""))
            {
                words.RemoveAll(delegate(string word){

                    return word.Equals("");
                });

                currentWordCount = words.Count;
            }
            else
            {
                currentWordCount = words.Count;

            }

        }
        
        private void getSentences(string inTextBlock)
        {
            string[] characters = inTextBlock.Split(' ');
            string[] endPoints = new string[3]{".","!","?"};
            int increment = 0;
            int startCut = 0;
            
            inTextBlock = inTextBlock.Trim();
            for (int begin = 0; begin <= inTextBlock.Length; begin++)
            {

                if (inTextBlock.Substring(startCut, begin - startCut).Contains(".") || inTextBlock.Substring(startCut, begin - startCut).Contains("?") || inTextBlock.Substring(startCut, begin - startCut).Contains("!"))
                {
                    
                    if(minWords.Text==""){
                        increment++;
                    }
                    else if (inTextBlock.Split(' ').Length >= Convert.ToInt32(minWords.Text))
                    {
                       increment++;
                    }
                    startCut = begin;
                }
            
            
            
            }



                //foreach (string character in characters)
                //{
                //    for (int i = 0; i < endPoints.Length; i++)
                //    {

                //        if (character.Contains(endPoints[i]) && character.Length > 1)
                //        {

                //            increment++;


                //        }

                //    }
                //}
            currentSentCount = increment;
        }
        private int UniqueWords(string inTextBlock)
        {
            Dictionary<string, int> dictionary =  new Dictionary<string, int>();
            string[] words = inTextBlock.Split(' ');
            
            foreach (string word in words)
            {
                if (dictionary.ContainsKey(word))
                {
                    
                    dictionary[word] = dictionary[word]++;
                    //System.Windows.MessageBox.Show(dictionary[word].ToString());
                    

                }
                else
                    dictionary.Add(word, 1);
                
            
            }
            return dictionary.Count;
        }
        private void loadFile(object sender, RoutedEventArgs e)
        {
            //string fileText = System.IO.File.ReadAllText(@"F:\Users\Jordan\Documents\Projects\test.txt");
            //Content.Text = fileText;

            Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog();
            openFileDialog1.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = false;
            bool? userClickedOK = openFileDialog1.ShowDialog();
            if (userClickedOK == true)
            {
                // Open the selected file to read.
                
                CurrFileName.Text = openFileDialog1.SafeFileName;                    
                
                String fileText = System.IO.File.ReadAllText(openFileDialog1.FileName);
                currfile = openFileDialog1.FileName;
                if (fileText.Contains("-:Tyson Results:-"))
                {
                    int endPoint = fileText.IndexOf("-:Tyson Results:-");
                     fileText = fileText.Substring(0, endPoint);
                
                
                
                }
                Content.Text = fileText;
                filePosition.Text = "N/A";
                Update();
                
            }
        }
        public int frequencyOf(string targetWord, string textToView)
        {
             string[] textArray = textToView.Split(' ');
             int result = 0;
             foreach (string segment in textArray)
             {
                 if (segment.ToLower().Equals(targetWord.ToLower()))
                 {
                     result++;
                 }
             }
            return result;
        }
        private void batchLoad(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog myDialog = new FolderBrowserDialog();
            if (myDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string folderPath = myDialog.SelectedPath;
                
                

                string[] fileArray = Directory.EnumerateFiles(folderPath, "*.txt").ToArray();

                if (currFileList.Capacity != 0)
                {
                    currFileList.Clear();
                    currFileList = new List<string>(0);
                
                }
                
                foreach (string file in fileArray)
                {
                    
                        currFileList.Add(file);
                    

                    //fileStream = new FileStream(file, FileMode.Open);
                    //reader = new StreamReader(fileStream);
                    
                    //fileStream.Close();
                }
                filePosition.Text = "1 out of " + (currFileList.Capacity-1).ToString();
                currfile = currFileList[0];
                string croppedFile = currfile.Substring(currfile.LastIndexOf("\\")+1);
                CurrFileName.Text = croppedFile;
                string fileContents = System.IO.File.ReadAllText(currFileList[0]);
                if (fileContents.Contains("-:Tyson Results:-"))
                {
                    int endPoint = fileContents.IndexOf("-:Tyson Results:-");
                    fileContents = fileContents.Substring(0, endPoint);



                }
                Content.Text = fileContents; 
            }

        }
        private void previousFile(object sender, RoutedEventArgs e)
        {
            if (currFileList.Count == 0)
            {
                
            }
            else if (currfile == null)
            {
                

                
            }
            else if (currFileList.IndexOf(currfile) <= 0)
            {
                
            }
            else
            {
                int index = currFileList.IndexOf(currfile) - 1 ;
                currfile = currFileList[index];
                string croppedFile = currfile.Substring(currfile.LastIndexOf("\\") + 1);
                CurrFileName.Text = croppedFile;
                string fileContents = System.IO.File.ReadAllText(currfile);
                if (fileContents.Contains("-:Tyson Results:-"))
                {
                    int endPoint = fileContents.IndexOf("-:Tyson Results:-");
                    fileContents = fileContents.Substring(0, endPoint);}
                
                filePosition.Text =  (index+1).ToString() + " out of " + (currFileList.Count-1).ToString();
                Content.Text = fileContents;
                Update();
            }

        }        
        private void nextFile(object sender, RoutedEventArgs e)
        {
            if (currFileList.Count == 0)
            {
                
            }
            else if (currfile == null)
            {


                
            }
            else if (currFileList.IndexOf(currfile)+1 >= currFileList.Count-1)
            {
                
            }
            else
            {
                int index = currFileList.IndexOf(currfile) + 1;
                currfile = currFileList[index];
                string croppedFile = currfile.Substring(currfile.LastIndexOf("\\") + 1);
                CurrFileName.Text = croppedFile;
                string fileContents = System.IO.File.ReadAllText(currfile);
                if (fileContents.Contains("-:Tyson Results:-"))
                {
                    int endPoint = fileContents.IndexOf("-:Tyson Results:-");
                    fileContents = fileContents.Substring(0, endPoint);}
                
                filePosition.Text = (index+1).ToString() + " out of " + (currFileList.Count-1).ToString();
                Content.Text = fileContents;
                Update();
            }
        }        
        //Needs to be finished
        private void completeCurrentBatch(object sender, RoutedEventArgs e)
        {

            foreach (string file in currFileList)
            {
               
                    using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(file, false))
                    {
                        string Essay = Content.Text;
                        outputFile.WriteLine(Essay);
                        outputFile.WriteLine("-:Tyson Results:-");
                        outputFile.WriteLine("----------------------------------------------------");
                        outputFile.WriteLine("Total Sentences: " + currentSentCount.ToString());
                        outputFile.WriteLine("Word Count: " + currentWordCount.ToString());
                        outputFile.WriteLine("Unique Words: " + (UniqueWords(Essay).ToString()));
                        outputFile.WriteLine("File up to date as of: " + DateTime.Now.ToShortDateString());


                    }
                
            }
        }

        private void checkCriteria() {
            
            if (minSentences.Text.Equals("") && currentSentCount > 0)
            {
                paperPass = true;
                criteriaMet.IsChecked = true;
            }
            else if(currentSentCount==0 && minSentences.Text.Equals(""))
            {
                paperPass = false;
                criteriaMet.IsChecked = false;
                return;
            }
            else if (currentSentCount >= Convert.ToInt32(minSentences.Text))
            {
                paperPass = true;
                criteriaMet.IsChecked = true;
            }
            else {
                paperPass = false;
                criteriaMet.IsChecked = false;
            }
        
        
        }
          
       

        private void exporttoFile(object sender, RoutedEventArgs e)
        {

            if (true == true)
            {
                using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(currfile, false))
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

        private void startTimer(object sender, RoutedEventArgs e)
        {
            if(timeOfTest.Text!=null){

                    
            Content.IsReadOnly = false;
            myTimer = new System.Timers.Timer(Int32.Parse(timeOfTest.Text) * 1000 * 60);
            myTimer.Enabled = true;            
            myTimer.Elapsed += new ElapsedEventHandler(timerElapsed);
            
            }
        }

        private void FrequencyOfWord_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void timerElapsed(object sender, ElapsedEventArgs e)
        {
            myTimer.Enabled = false;
            Action act = () => { Content.IsReadOnly = !Content.IsReadOnly; };
            Content.Dispatcher.Invoke(act);
            
        
        }

        private void submitWord(object sender, RoutedEventArgs e)
        {
            if (!wordBank.Contains(wordBank_Entry.Text))
            {

                wordBank.Add(wordBank_Entry.Text);
                updateListBox();
            }
        }
        private Boolean wordBankEval()
        {
            if (Content.Text.Length > 0)
            {
                List<string> words = wordBank;
                string content = Content.Text;


                foreach (string word in words)
                {
                    if (!content.ToLower().Contains(word.ToLower()))
                    {
                        return false;

                    }
                    Content.Select(content.IndexOf(word), word.Length);
                }
                if (words.Count > 0)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        private void updateListBox()
        {
            wordBankListBox.ItemsSource = wordBank;
            wordBankListBox.Items.Refresh();
            if (wordBankEval() == true)
            {
                wordBankCheck.IsChecked = true;

            }
            else {
                wordBankCheck.IsChecked = false;
            }
            
            
            
        
        
        
        }
        private void removeSelected(object sender, RoutedEventArgs e)
        {
            if (wordBankListBox.SelectedItem != null)
            {
                string selectedItem = wordBankListBox.SelectedItem.ToString();
                wordBank.Remove(selectedItem);
                updateListBox();
                
            }
        }

        

        private void exportTest(object sender, RoutedEventArgs e)
        {
            List<string> filecontents = new List<string>(0);
            /*Order of test settings 
             * Minimum Sentences
             * Minimum Words
             * Duration of Test
             * WordBank
            */
            List<bool> contents = new List<bool>(4);
            for(int i = 0; i < 4; i++)
            {
                contents.Add(false);
            }
            
            if(!minSentences.Text.Equals("")){
                filecontents.Add( minSentences.Text);
                contents.RemoveAt(0);
                contents.Insert(0, true);
            }
            if (!minWords.Text.Equals("")) {
                filecontents.Add(minWords.Text);
                contents.RemoveAt(1);
                contents.Insert(1, true);
            }
            if (!timeOfTest.Text.Equals(""))
            {
                filecontents.Add(timeOfTest.Text);
                contents.RemoveAt(2);
                contents.Insert(2, true);
            }
            if (wordBank.Count>0)
            {
                foreach (string word in wordBank)
                { 
                    filecontents.Add(word);                
                }
                contents.RemoveAt(3);
                contents.Insert(3, true);
            }
            string input = Microsoft.VisualBasic.Interaction.InputBox("Please enter a an encryption key for this test, anything will work within 8 characters.", "Title", "Apple", -1, -1);
            string testName = Microsoft.VisualBasic.Interaction.InputBox("What would you like to name this test?", "Test Name", "EnglishExamination1", -1, -1);
            //Microsoft.Win32.SaveFileDialog saver = new Microsoft.Win32.SaveFileDialog();
            //Nullable<bool> result = saver.ShowDialog();
            //if (result == true)
            //{
                string path = Directory.GetCurrentDirectory();
                
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path+testName+ ".tySon"))
                {
                    foreach (bool content in contents)
                    {
                        
                            file.WriteLine(content.ToString());
                        
                    }
                    foreach (string piece in filecontents)
                    {

                        file.WriteLine(piece);
                    }
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
                    minSentences.Text = lines[currPointer];
                    currPointer++;
                    
                }
                if (contentA[1] == true)
                {
                    minWords.Text = lines[currPointer];
                    currPointer++;}
                if (contentA[2] == true)
                {
                    timeOfTest.Text = lines[currPointer];
                        currPointer++;
                
                }
                if (contentA[3] == true)
                {
                    List<string> temporaryWordBank = new List<string>(0);
                    for (int index = currPointer; index < lines.Length; index++)
                    {
                        System.Windows.Forms.MessageBox.Show("Running");
                        temporaryWordBank.Add(lines[index]);
                    }
                    wordBank = temporaryWordBank;
                }
                testKey = lines[lines.Length - 1];
                
                Update();

            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            if(testModeIsEnabled == true)
            {
            System.Environment.Exit(0);
            }
        }

        private void shutDown(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void switchTestMode(object sender, RoutedEventArgs e)
        {
            testModeIsEnabled = !testModeIsEnabled;
            if (testModeIsEnabled == true)
            {
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
                testMode.IsChecked = true;
            }
            else
            {
                testMode.IsChecked = false;
                this.WindowStyle = WindowStyle.SingleBorderWindow;
            }
        }

        private void encryptExport(object sender, RoutedEventArgs e)
        {

            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(Content.Text);

            string key = Key.Text;
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
            Content.Text = Convert.ToBase64String(resultArray, 0, resultArray.Length);



            
        }

        

        

        

       
      
       
      

        
        
    }
}
