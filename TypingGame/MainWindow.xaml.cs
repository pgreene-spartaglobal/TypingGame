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

        static public int totalScore = 0;
        int lives = 1;
        int timerInterval = 750;
        int difficultySpeed = 0;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize timer used for 'Update' method
            InitializeTimer(); 

            // Generate the list of words from a text file
            WordGenerator.ReadWordList("google-10000-english-no-swears.txt");

            // Update the score and lives on the canvas
            UpdateScore(totalScore);
            UpdateLives(lives);

            // Set the speed of the game according to the difficulty selected on the main menu
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

        // Set up the timer
        private void InitializeTimer()
        {

            dtUpdate.Interval = new TimeSpan(0, 0, 0, 0, timerInterval);
            dtUpdate.Tick += dt_Update;

            dtUpdate.Start();
        }

        // Called each time the timer 'ticks'
        private void dt_Update(object sender, EventArgs e)
        {
            wordManager.AddWord(); // Add a word to the list

            // Check each word to see if it has fallen off the screen
            for (int i = 0; i < wordManager.words.Count; i++)
            {
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

        // Get the user input from the keyboard 
        private void Window_TextInput(object sender, TextCompositionEventArgs e)
        {
            wordManager.TypeLetter(e.Text[0]);
        }

        // If the user presses escape they can cancel the active word
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (wordManager.hasActiveWord)
                {
                    wordManager.EscapeWord(wordManager.activeWord);
                }

            }
        }

        // Show the word on the canvas
        public void SpawnWordCanvas(TextBlock text)
        {
            double minValue = 0;
            double maxValue = Canv.ActualWidth;

            double randomValue = rnd.NextDouble() * (maxValue - minValue) + minValue;

            Canvas.SetTop(text, 0);
            Canvas.SetLeft(text, randomValue);
            Canv.Children.Add(text);
        }

        // Remove the word from the canvas
        public void RemoveWordCanvas(TextBlock text)
        {
            Canv.Children.Remove(text);
        }

        // Increase the score, decrease the interval
        public void UpdateScore(int score)
        {
            totalScore += score;
            Score.Content = "Score: " + totalScore;

            UpdateInterval();
        }

        // Decrease the time interval, making the game faster
        public void UpdateInterval()
        {
            if (timerInterval >= 101)
            {
                timerInterval -= 10;
                dtUpdate.Interval = new TimeSpan(0, 0, 0, 0, timerInterval);
            }
        }

        // Decrease lives and check for game over
        public void UpdateLives(int lives)
        {
            Lives.Content = "Lives: " + lives;
            if (lives <= 0)
            {                
                GameOver();
            }
        }

        // Game is over, show the input window for highscore
        public void GameOver()
        {
            Window window = new Window
            {
                Title = "Submit Highscore",
                Content = new SubmitHighscore(),
                Height = 250,
                Width = 800,
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
        MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault(); // Get a reference to the main window

        public List<Word> words = new List<Word>();
        public bool hasActiveWord; // Is the user currently in the middle of typing a word?
        public Word activeWord;
        private int scoreToAdd = 0; // The longer the word the more score needs to be added

        // Generate a new random word and add it to the list
        public void AddWord()
        {
            Word word = new Word(WordGenerator.GetRandomWord());
            words.Add(word);
        }

        // The user has finished with the current word so remove it
        public void RemoveWord(Word word)
        {
            hasActiveWord = false;
            words.Remove(word);
        }

        // Stop typing the current word
        public void EscapeWord(Word word)
        {
            double originalLeft = Canvas.GetLeft(word.text);
            double originalTop = Canvas.GetTop(word.text);

            Word word1 = new Word(word.text.Text);
            words.Add(word1);

            Canvas.SetTop(word1.text, originalTop);
            Canvas.SetLeft(word1.text, originalLeft);
            word.RemoveCanvas(word.text);
            words.Remove(word);
            hasActiveWord = false;
        }

        // Type a letter in the word 
        public void TypeLetter(char letter)
        {
            if (hasActiveWord)
            {
                // Type the next letter
                if (activeWord.GetNextLetter() == letter)
                {
                    activeWord.TypeLetter();
                    scoreToAdd++;
                }
            }
            else
            {
                // If the user does not currently have an active word set it
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

            // The user has an active word and has finished typing the word
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

        MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault(); // Get a reference to the main window

        public Word(string word)
        {
            this.wordValue = word;
            typeIndex = 0;

            // Set up the UIElement
            text.Text = word;
            text.FontSize = 24;
            text.Foreground = Brushes.MintCream;
            text.Background = new SolidColorBrush(Color.FromArgb(0xBF, 0x00, 0, 0));
            text.FontWeight = FontWeights.Thin;
            text.Padding = new Thickness(5);

            // Create the word on the canvas
            mainWindow.SpawnWordCanvas(text);
        }

        // The next letter in the word string
        public char GetNextLetter()
        {
            return wordValue[typeIndex];
        }

        public void TypeLetter()
        {
            typeIndex++;
            RemoveLetter();
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

        // Remove letter from canvas
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

        // Choose a random word from the list
        public static string GetRandomWord()
        {           
            int randomIndex = rnd.Next(0, wordList.Length);
            string randomWord = wordList[randomIndex];

            return randomWord;
        }

        // Set the wordlist according to file contents
        public static void ReadWordList(string path)
        {
            string[] lines = File.ReadAllLines(path);
            wordList = lines;
        }
    }
}
