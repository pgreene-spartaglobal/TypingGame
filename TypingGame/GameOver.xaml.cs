using System;
using System.Collections.Generic;
using System.IO;
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


namespace TypingGame
{
    /// <summary>
    /// Interaction logic for GameOver.xaml
    /// </summary>
    public partial class GameOver : UserControl
    {
        public GameOver()
        {
            InitializeComponent();
            ReadHighscores();
            ShowSubmitHighscore();
        }

        private void ShowSubmitHighscore()
        {
            //Window window = new Window
            //{
            //    Title = "Submit Highscore",
            //    Content = new SubmitHighscore(),
            //    Height = 250,
            //    Width = 800,
            //    //SizeToContent = SizeToContent.WidthAndHeight,
            //    ResizeMode = ResizeMode.NoResize
            //};
            //double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            //double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            //double windowWidth = this.Width;
            //double windowHeight = this.Height;
            //window.Left = (screenWidth / 2) - (windowWidth / 2);
            //window.Top = (screenHeight / 2) - (windowHeight / 2);

            //window.ShowDialog();
        }

        private void ReadHighscores()
        {

            foreach (var line in File.ReadLines("highscores.txt"))
            {
                string[] words = line.Split(',');
                HighscoreText.Text += "\n" + words[0] + "\t" + words[1] + "\t" + words[2];
            }

            //string[] lines = File.ReadAllLines("highscores.txt");
            //foreach (string line in lines)
            //{
            //    HighscoreText.Text += line;
            //    HighscoreText.Text += "\n";
            //}
            
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
