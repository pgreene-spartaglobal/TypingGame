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
        }

        private void ReadHighscores()
        {
            // Read the highscores file
            foreach (var line in File.ReadLines("highscores.txt"))
            {
                string[] words = line.Split(',');
                HighscoreText.Text += "\n" + words[0] + "\t" + words[1] + "\t" + words[2];
            }            
        }

        // Restart the game
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
