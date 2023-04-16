using System.Windows;
using System.Windows.Data;
using WarhammerAGM.Models;

namespace WarhammerAGM
{
    public partial class MainWindow : Window
    {
        private readonly CollectionViewSource bestiaryCreatureView;
        public MainWindow()
        {
            InitializeComponent();
            bestiaryCreatureView = (CollectionViewSource)Resources[nameof(bestiaryCreatureView)];
        }

        private string inputText = string.Empty;
        public string SearchText
        {
            get => inputText;
            set 
            {
                inputText = value?.Trim() ?? string.Empty; 
                SearchChanged(); 
            }
        }
        private FilterEventHandler? filter;
        private void SearchChanged()
        {
            string searchText = inputText;
            bestiaryCreatureView.Filter -= filter;

            filter = (string.IsNullOrEmpty(searchText)) switch
            {
                (false) => (object sender, FilterEventArgs e) =>
                {
                    BestiaryCreature bestiaryCreature = (BestiaryCreature)e.Item;
                    e.Accepted = bestiaryCreature.Name.Contains(searchText);
                }
                ,
                (true) => null
            };

            bestiaryCreatureView.Filter += filter;
        }

    }
}
