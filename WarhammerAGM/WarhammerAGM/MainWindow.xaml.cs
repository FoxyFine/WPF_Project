using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using WarhammerAGM.Models;
using Xceed.Wpf.Toolkit;

namespace WarhammerAGM
{
    public partial class MainWindow : Window
    {
        private ApplicationViewModel viewModel;
        private readonly CollectionViewSource bestiaryCreatureViewInitiative;
        private readonly CollectionViewSource bestiaryCreatureView;
        private readonly CollectionViewSource bestiaryCreatureEdit;
        private readonly CollectionViewSource characterView;
        public MainWindow()
        {
            InitializeComponent();
            bestiaryCreatureViewInitiative = (CollectionViewSource)Resources[nameof(bestiaryCreatureViewInitiative)];
            bestiaryCreatureView = (CollectionViewSource)Resources[nameof(bestiaryCreatureView)];
            bestiaryCreatureEdit = (CollectionViewSource)Resources[nameof(bestiaryCreatureEdit)];
            characterView = (CollectionViewSource)Resources[nameof(characterView)];

            viewModel = new ApplicationViewModel();
            DataContext = viewModel;
        }

        private string inputText = string.Empty;
        public string SearchTextBCViewInitiative
        {
            get => inputText;
            set
            {
                inputText = value?.Trim() ?? string.Empty;
                SearchChangedBCViewInitiative();
            }
        }
        public string SearchTextBCView
        {
            get => inputText;
            set
            {
                inputText = value?.Trim() ?? string.Empty;
                SearchChangedBCView();
            }
        }
        public string SearchTextBCEdit
        {
            get => inputText;
            set
            {
                inputText = value?.Trim() ?? string.Empty;
                SearchChangedBCEdit();
            }
        }
        public string SearchTextC
        {
            get => inputText;
            set
            {
                inputText = value?.Trim() ?? string.Empty;
                SearchChangedC();
            }
        }

        private FilterEventHandler? filter;
        private void SearchChangedBCViewInitiative()
        {
            string searchText = inputText;
            bestiaryCreatureViewInitiative.Filter -= filter;

            filter = (string.IsNullOrEmpty(searchText)) switch
            {
                (false) => (object sender, FilterEventArgs e) =>
                {
                    BestiaryCreature bestiaryCreature = (BestiaryCreature)e.Item; // Item Получает объект, который должен проверить фильтр
                    e.Accepted = bestiaryCreature.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase); // Accepted Получает или задает значение, которое показывает, проходит ли данный элемент фильтр
                    //true, если элемент проходит фильтр; в противном случае, false. Значение по умолчанию — true.
                    //InvariantCulture/InvariantCultureIgnoreCase — сравнение по словам без учёта правил языка и культуры и языка
                }
                ,
                (true) => null
            };

            bestiaryCreatureViewInitiative.Filter += filter;
        }
        private void SearchChangedBCView()
        {
            string searchText = inputText;
            bestiaryCreatureView.Filter -= filter;

            filter = (string.IsNullOrEmpty(searchText)) switch
            {
                (false) => (object sender, FilterEventArgs e) =>
                {
                    BestiaryCreature bestiaryCreature = (BestiaryCreature)e.Item; // Item Получает объект, который должен проверить фильтр
                    e.Accepted = bestiaryCreature.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase); // Accepted Получает или задает значение, которое показывает, проходит ли данный элемент фильтр
                    //true, если элемент проходит фильтр; в противном случае, false. Значение по умолчанию — true.
                    //InvariantCulture/InvariantCultureIgnoreCase — сравнение по словам без учёта правил языка и культуры и языка
                }
                ,
                (true) => null
            };

            bestiaryCreatureView.Filter += filter;
        }
        private void SearchChangedBCEdit()
        {
            string searchText = inputText;
            bestiaryCreatureEdit.Filter -= filter;

            filter = (string.IsNullOrEmpty(searchText)) switch
            {
                (false) => (object sender, FilterEventArgs e) =>
                {
                    BestiaryCreature bestiaryCreature = (BestiaryCreature)e.Item;
                    e.Accepted = bestiaryCreature.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase);
                }
                ,
                (true) => null
            };

            bestiaryCreatureEdit.Filter += filter;
        }
        private void SearchChangedC()
        {
            string searchText = inputText;
            characterView.Filter -= filter;

            filter = (string.IsNullOrEmpty(searchText)) switch
            {
                (false) => (object sender, FilterEventArgs e) =>
                {
                    Character character = (Character)e.Item;
                    e.Accepted = character.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase);
                }
                ,
                (true) => null
            };

            characterView.Filter += filter;
        }

        private void TextColorChange(object sender, RoutedEventArgs e)
        {

            TextRange selectedText = new TextRange(CreatingNotes.Selection.Start, CreatingNotes.Selection.End);
            if (!selectedText.IsEmpty)
            {
                Brush colorBrush = new SolidColorBrush(viewModel.ColorPick);
                // Применить кисть к свойству Foreground выделенного текста
                selectedText.ApplyPropertyValue(TextElement.ForegroundProperty, colorBrush);

            }

        }
        private void BacgroundColorChange(object sender, RoutedEventArgs e)
        {

            TextRange selectedText = new TextRange(CreatingNotes.Selection.Start, CreatingNotes.Selection.End);
            if (!selectedText.IsEmpty)
            {
                Brush colorBrush = new SolidColorBrush(viewModel.ColorPick);
                // Применить кисть к свойству Foreground выделенного текста
                selectedText.ApplyPropertyValue(TextElement.BackgroundProperty, colorBrush);

            }

        }
    }
}
