using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
using System.Windows.Threading;
using System.IO;

namespace TypingGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dtUpdate = new DispatcherTimer();
        WordManager wordManager = new WordManager();
        Random rnd = new Random();
        int totalScore = 0;
        int lives = 1;
        int timerInterval = 999;

        int difficultySpeed = 0;

        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer(); // Create timer used for 'Update' method

            WordGenerator.ReadWordList("google-10000-english-no-swears.txt");

            wordManager.AddWord();
            UpdateScore(totalScore);
            UpdateLives(lives);

            switch (MainMenu.difficultyLevel)
            {
                case "Easy":
                    difficultySpeed = 10;
                    break;

                case "Normal":
                    difficultySpeed = 20;
                    break;

                case "Hard":
                    difficultySpeed = 30;
                    break;
                default:
                    break;
            }
        }

        private void InitializeTimer()
        {

            dtUpdate.Interval = new TimeSpan(0, 0, 0, 0, timerInterval);
            dtUpdate.Tick += dt_Update;

            dtUpdate.Start();
        }

        private void dt_Update(object sender, EventArgs e)
        {
            wordManager.AddWord();

            for (int i = 0; i < wordManager.words.Count; i++)
            {
                //Canvas.SetTop(wordManager.words[i].text, Canvas.GetTop(wordManager.words[i].text) + wordManager.words[i].fallSpeed);
                Canvas.SetTop(wordManager.words[i].text, Canvas.GetTop(wordManager.words[i].text) + difficultySpeed);
                if (Canvas.GetTop(wordManager.words[i].text) > Canv.ActualHeight - 50)
                {
                    lives--;
                    UpdateLives(lives);
                    RemoveWordCanvas(wordManager.words[i].text);
                    wordManager.RemoveWord(wordManager.words[i]);
                }
            }
        }

        private void Window_TextInput(object sender, TextCompositionEventArgs e)
        {
            //DebugTextBlock.Text += e.Text;
            wordManager.TypeLetter(e.Text[0]);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //// ... Test for Esc key.
            if (e.Key == Key.Escape)
            {
                if (wordManager.hasActiveWord)
                {
                    wordManager.EscapeWord(wordManager.activeWord);
                    //MessageBox.Show("Escape");
                }

            }
        }

        public void SpawnWordCanvas(TextBlock text)
        {
            double minValue = 0;
            double maxValue = Canv.ActualWidth;

            double randomValue = rnd.NextDouble() * (maxValue - minValue) + minValue;

            Canvas.SetTop(text, 0);
            Canvas.SetLeft(text, randomValue);
            Canv.Children.Add(text);
        }

        public void RemoveWordCanvas(TextBlock text)
        {
            Canv.Children.Remove(text);
        }

        public void UpdateScore(int score)
        {
            totalScore += score;
            Score.Content = "Score: " + totalScore;
            if (timerInterval >= 101)
            {
                timerInterval -= 10;
                dtUpdate.Interval = new TimeSpan(0, 0, 0, 0, timerInterval);
            }            
        }

        public void UpdateLives(int lives)
        {


            Lives.Content = "Lives: " + lives;
            if (lives <= 0)
            {                
                GameOver();
            }
        }

        public void GameOver()
        {
            //MessageBox.Show("Game Over!");
            //System.Windows.Application.Current.Shutdown();

            Window window = new Window
            {
                Title = "Game Over",
                Content = new GameOver(),
                Height = 300,
                Width = 300,
                //SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize
            };
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            window.Left = (screenWidth / 2) - (windowWidth / 2);
            window.Top = (screenHeight / 2) - (windowHeight / 2);

            window.ShowDialog();
        }
    }


    public class WordManager
    {
        MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        public List<Word> words = new List<Word>();

        public bool hasActiveWord;
        public Word activeWord;
        private int scoreToAdd = 0;

        public void AddWord()
        {

            Word word = new Word(WordGenerator.GetRandomWord());

            words.Add(word);
        }

        public void RemoveWord(Word word)
        {
            hasActiveWord = false;
            words.Remove(word);
        }

        public void EscapeWord(Word word)
        {
            double originalLeft = Canvas.GetLeft(word.text);
            double originalTop = Canvas.GetTop(word.text);

            Word word1 = new Word(word.text.Text);
            words.Add(word1);

            Canvas.SetTop(word1.text, originalTop);
            Canvas.SetLeft(word1.text, originalLeft);
            word.RemoveCanvas(word.text);
            //mainWindow.RemoveWordCanvas(word.text);
            words.Remove(word);
            hasActiveWord = false;
        }

        public void TypeLetter(char letter)
        {
            if (hasActiveWord)
            {
                if (activeWord.GetNextLetter() == letter)
                {
                    activeWord.TypeLetter();
                    scoreToAdd++;
                }
            }
            else
            {
                foreach (Word word in words)
                {
                    if (word.GetNextLetter() == letter)
                    {
                        activeWord = word;
                        hasActiveWord = true;
                        word.TypeLetter();
                        break;
                    }
                }
            }

            if (hasActiveWord && activeWord.WordTyped())
            {
                hasActiveWord = false;
                words.Remove(activeWord);
                mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                mainWindow.UpdateScore(scoreToAdd);
                scoreToAdd = 0;
            }
        }
    }

    public class Word
    {
        public TextBlock text = new TextBlock();
        public string wordValue;
        private int typeIndex;
        public int fallSpeed = 12;

        MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

        public Word(string word)
        {
            this.wordValue = word;
            typeIndex = 0;
            text.Text = word;
            text.FontSize = 24;
            text.Foreground = Brushes.MintCream;
            text.Background = new SolidColorBrush(Color.FromArgb(0xBF, 0x00, 0, 0));
            text.FontWeight = FontWeights.Thin;
            text.Padding = new Thickness(5);
            mainWindow.SpawnWordCanvas(text);
        }

        public char GetNextLetter()
        {
            return wordValue[typeIndex];
        }

        public void TypeLetter()
        {
            typeIndex++;
            RemoveLetter();
            //Console.Beep(10000,2);
        }

        public void RemoveCanvas(TextBlock text)
        {
            mainWindow.RemoveWordCanvas(text);
        }

        public bool WordTyped()
        {
            bool wordTyped = (typeIndex >= wordValue.Length);

            if (wordTyped)
            {
                // remove word
                mainWindow.RemoveWordCanvas(text);
            }

            return wordTyped;
        }

        public void RemoveLetter()
        {
            text.Text = text.Text.Remove(0, 1);
            text.Foreground = Brushes.Orange;
        }
    }

    public class WordGenerator
    {
       static Random rnd = new Random();

        public static string[] wordList =
        {
        "alpha",
        "bravo",
        "charlie",
        "delta",
        "echo",
        "foxtrot",
        "golf",
        "hotel",
        "india",
        "juliet",
        "kilo",
        "lima",
        "mike",
        "november",
        "oscar",
        "papa",
        "quebec",
        "romeo",
        "sierra",
        "tango",
        "uniform",
        "victor",
        "whiskey",
        "xray",
        "yankee",
        "zulu"
        };
        public static string GetRandomWord()
        {
            
            int randomIndex = rnd.Next(0, wordList.Length);
            string randomWord = wordList[randomIndex];

            return randomWord;
        }

        //https://docs.microsoft.com/en-us/dotnet/api/system.io.file.readalllines?view=netframework-4.8
        //string[] lines = File.ReadAllLines("The file path");

        public static void ReadWordList(string path)
        {
            string[] lines = File.ReadAllLines(path);
            wordList = lines;
        }
    }
}
