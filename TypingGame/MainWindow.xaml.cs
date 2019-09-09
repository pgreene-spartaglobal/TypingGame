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
using System.Windows.Threading;

namespace TypingGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WordManager wordManager = new WordManager();
        Random rnd = new Random();
        int totalScore = 0;
        int lives = 10;

        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer(); // Create timer used for 'Update' method

            wordManager.AddWord();
            UpdateScore(totalScore);
            UpdateLives(lives);
        }

        private void InitializeTimer()
        {
            DispatcherTimer dtUpdate = new DispatcherTimer();

            dtUpdate.Interval = new TimeSpan(0, 0, 0, 1, 0);
            dtUpdate.Tick += dt_Update;

            dtUpdate.Start();
        }

        private void dt_Update(object sender, EventArgs e)
        {
            wordManager.AddWord();

            for (int i = 0; i < wordManager.words.Count; i++)
            {
                Canvas.SetTop(wordManager.words[i].text, Canvas.GetTop(wordManager.words[i].text) + wordManager.words[i].fallSpeed);
                if (Canvas.GetTop(wordManager.words[i].text) > Canv.ActualHeight)
                {
                    lives--;
                    UpdateLives(lives);
                    RemoveWordCanvas(wordManager.words[i].text);
                    wordManager.RemoveWord(wordManager.words[i]);
                }
            }

            //foreach (Word word in wordManager.words)
            //{
            //    Canvas.SetTop(word.text, Canvas.GetTop(word.text) + word.fallSpeed);
            //    if (Canvas.GetTop(word.text) > Canv.ActualHeight)
            //    {
            //        lives--;
            //        UpdateLives(lives);                    
            //        RemoveWordCanvas(word.text);
            //        wordManager.RemoveWord(word);
            //    }
            //}
        }

        private void Window_TextInput(object sender, TextCompositionEventArgs e)
        {
            //DebugTextBlock.Text += e.Text;
            wordManager.TypeLetter(e.Text[0]);
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
        }

        public void UpdateLives(int lives)
        {
            Lives.Content = "Lives: " + lives;
        }
    }


    public class WordManager
    {
        MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        public List<Word> words = new List<Word>();

        private bool hasActiveWord;
        private Word activeWord;
        private int scoreToAdd = 0;

        public void AddWord()
        {

            Word word = new Word(WordGenerator.GetRandomWord());

            words.Add(word);
        }

        public void RemoveWord(Word word)
        {
            words.Remove(word);
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
    }
}
