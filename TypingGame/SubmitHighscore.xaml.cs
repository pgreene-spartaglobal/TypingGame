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

namespace TypingGame
{
    /// <summary>
    /// Interaction logic for SubmitHighscore.xaml
    /// </summary>
    public partial class SubmitHighscore : UserControl
    {
        public SubmitHighscore()
        {
            InitializeComponent();
            ShowScore();
        }

        // Display the score in the user control
        private void ShowScore()
        {
            Score.Content = "You scored " + MainWindow.totalScore;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            // Write to the textfile
            File.AppendAllText("highscores.txt", "\n" + Name.Text + "," + MainWindow.totalScore + "," + MainMenu.difficultyLevel);

            // Hide the user control
            grid.Background = Brushes.Transparent;
            this.Visibility = Visibility.Hidden;

            // Show the gameover user control
            Window window = new Window
            {
                Title = "Game Over",
                Content = new GameOver(),
                Height = 300,
                Width = 300,
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
}
