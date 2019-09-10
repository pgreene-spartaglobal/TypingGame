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
using System.Windows.Shapes;

namespace TypingGame
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public static string difficultyLevel = "";

        public MainMenu()
        {
            InitializeComponent();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            difficultyLevel = ((ComboBoxItem)DifficultySelection.SelectedItem).Content.ToString();
            MainWindow mainWindow = new MainWindow();
            this.Visibility = Visibility.Hidden; // hide current window
            mainWindow.Show();
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
