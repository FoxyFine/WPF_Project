using Mapping;
using System.Windows;
using ViewModels;
using WarhammerAGM.Models;

namespace WarhammerAGM
{
    public partial class ApplicationViewModel
    {
        public ViewMode Mode { get => Get<ViewMode>(); private set => Set(value); }

        public RelayCommand Update => GetCommand
        (
            () =>
            {
                EditableBC = SelectedBC!.Create<BestiaryCreature>();
                Mode = ViewMode.Update;
            },
            () => SelectedBC is not null
        );

        public RelayCommand Add => GetCommand
        (
            () =>
            {
                EditableBC = new BestiaryCreature();
                Mode = ViewMode.Add;
            }
        );

        public RelayCommand Clone => GetCommand
        (
            () =>
            {
                EditableBC = SelectedBC!.Create<BestiaryCreature>();
                EditableBC.Id = 0;
                Mode = ViewMode.Add;
            },
            () => SelectedBC is not null
        );

        public RelayCommand Exit => GetCommand
        (
            () => Mode = ViewMode.View,
            () => SelectedBC is not null
        );

        public RelayCommand Save => GetCommand
        (
            () => MessageBox.Show("Сохранение измений"),
            () => SelectedBC is not null
        );
    }

    public enum ViewMode { View, Update, Add }
}
