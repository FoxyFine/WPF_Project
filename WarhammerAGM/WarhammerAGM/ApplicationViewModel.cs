using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using ViewModels;
using WarhammerAGM.Models;
using Xceed.Wpf.Toolkit;

namespace WarhammerAGM
{
    public class ApplicationViewModel : ViewModelBase
    {
        private readonly ApplicationContext db = new ApplicationContext();

        public ObservableCollection<BestiaryCreature> BestiaryCreatures { get; }

        /// <summary>Сущность для региона детализации.</summary>
        public BestiaryCreature BestiaryCreature
        {
            get => Get<BestiaryCreature>()!;
            private set => Set(value ?? throw new ArgumentNullException(nameof(value)));
        }

        /// <summary>Выбранная сущность.</summary>
        public BestiaryCreature? SelectedItem
        {
            get => Get<BestiaryCreature?>();
            set => Set(value);
        }

        protected override void OnPropertyChanged(string propertyName, object? oldValue, object? newValue)
        {
            base.OnPropertyChanged(propertyName, oldValue, newValue);

            if (propertyName == nameof(SelectedItem))
            {
                BestiaryCreature? @new = (BestiaryCreature?)newValue;
                if (@new is null)
                    BestiaryCreature = new();
                else
                    BestiaryCreature = db.BestiaryCreatures.AsNoTracking().First(bc => bc.Id == @new.Id);
            }
        }

        public ApplicationViewModel()
        {
            /*В конструкторе класса загружаем данные из бд в локальный кэш*/
            db.Database.EnsureCreated();
            db.BestiaryCreatures.Load();
            BestiaryCreature = new();
            BestiaryCreatures = db.BestiaryCreatures.Local.ToObservableCollection();
        }

        /// <summary>Добавление сущности <see cref="BestiaryCreature"/>.</summary>
        public RelayCommand AddCommand => GetCommand(() =>
        {

            string? name = BestiaryCreature.Name;
            if (db.BestiaryCreatures.FirstOrDefault(bc => bc.Name == name) is not null)
            {
                MessageBox.Show("Такое название уже существует");
                return;
            }
            else
            {
                // Обнуляем Id и добавляем как новую
                BestiaryCreature.Id = 0;
                db.BestiaryCreatures.Add(BestiaryCreature);
                db.SaveChanges();

                MessageBox.Show("Добавление прошло успешно");

                //отменяем веделение элемента ListView
                SelectedItem = null;
            }
        });

        /// <summary>Обновление сущности <see cref="BestiaryCreature"/>.</summary>
        public RelayCommand EditCommand => GetCommand(() =>
        {
            // Запоминаем в локальной переменной
            var bestCr = BestiaryCreature;

            // Заменяем сущности
            int index = BestiaryCreatures.TakeWhile(bc => bc.Id != bestCr.Id).Count();
            BestiaryCreatures.RemoveAt(index);
            BestiaryCreatures.Insert(index, bestCr);

            // Сохраняем изменения
            db.SaveChanges();

            //отменяем веделение элемента ListView
            SelectedItem = null;
        });

        /// <summary>Удаление сущности <see cref="SelectedItem"/>.</summary>
        public RelayCommand DeleteCommand => GetCommand(
            () =>
            {
                if (SelectedItem is BestiaryCreature selectedItem)
                {
                    db.BestiaryCreatures.Remove(selectedItem);
                    db.SaveChanges();
                }
            },
            () => SelectedItem is BestiaryCreature);
    }
}
