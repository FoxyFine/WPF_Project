using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WarhammerAGM.Models;

namespace WarhammerAGM
{
    public partial class MainWindow : Window
    {
       /* public BestiaryCreature BestiaryCreature { get; } = new();*/
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ApplicationViewModel();
        }
       /* private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int val;
            if (!Int32.TryParse(e.Text, out val))
            {
                e.Handled = true; // отклоняем ввод
            }
        }*/

        private void TextBoxStats_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true; // если пробел, отклоняем ввод
            }
        }

        private void Button_Click_ADD_Bestary(object sender, RoutedEventArgs e)
        {
           /* BestiaryCreature bestiaryCreature = BestiaryCreature;*/
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        /* private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste) e.Handled = true;
        }*/
    }
}
