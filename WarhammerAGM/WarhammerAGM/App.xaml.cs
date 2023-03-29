using System;
using System.IO;
using System.Windows;

namespace WarhammerAGM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            if (MessageBox.Show("Удалить БД?", "Старт", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                File.Delete("Bestiary.db");
            }
        }
    }
}
