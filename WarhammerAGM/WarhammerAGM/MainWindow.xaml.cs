using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WarhammerAGM.Models;

namespace WarhammerAGM
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ApplicationViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
