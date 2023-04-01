using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ViewModels;
using WarhammerAGM.Models;
using WarhammerAGM.Models.WarhammerAGM.Models;
using Xceed.Wpf.Toolkit;

namespace WarhammerAGM
{
    public class ApplicationViewModel : ViewModelBase
    {
        private readonly ApplicationContext db = new();

        public ObservableCollection<BestiaryCreature> BestiaryCreatures { get; }
        public ObservableCollection<Weapon> Weapons { get; }

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

        public RelayCommand AddCommand => GetCommand(() =>
        {

        });
        public RelayCommand Cancel => GetCommand(() =>
        {

        });

        /// <summary>Добавление сущности <see cref="BestiaryCreature"/>.</summary>
        public RelayCommand Save => GetCommand(() =>
        {
            var bestCrId = BestiaryCreature.Id;
            // Обнуляем Id и добавляем как новую
            BestiaryCreature.Id = 0;
            db.BestiaryCreatures.Add(BestiaryCreature);
            try
            {
                db.SaveChanges();
                MessageBox.Show("Добавление прошло успешно");

                //отменяем выделение элемента ListView
                if (SelectedItem is null)
                    BestiaryCreature = new();
                else
                    SelectedItem = null;
            }
            catch (Exception ex)
            {
                // Здесь обработка ошибок ex
                db.BestiaryCreatures.Remove(BestiaryCreature);
                BestiaryCreature.Id = bestCrId;
                MessageBox.Show("Такое название уже существует");

            }
        });

        /// <summary>Обновление сущности <see cref="BestiaryCreature"/>.</summary>
        public RelayCommand EditCommand => GetCommand(
        () =>
        {
            var bestCrOld = SelectedItem;
            try
            {
                // Запоминаем в локальной переменной
                var bestCr = BestiaryCreature;

                // Заменяем сущности
                int index = BestiaryCreatures.TakeWhile(bc => bc.Id != bestCr.Id).Count();
                BestiaryCreatures.RemoveAt(index);
                BestiaryCreatures.Insert(index, bestCr);

                // Сохраняем изменения
                db.SaveChanges();

                //отменяем вделение элемента ListView
                SelectedItem = null;
            }
            catch (Exception ex)
            {
                int index = BestiaryCreatures.TakeWhile(bc => bc.Id != bestCrOld.Id).Count();
                BestiaryCreatures.RemoveAt(index);
                BestiaryCreatures.Insert(index, bestCrOld);
                MessageBox.Show("Такое название уже существует");
            }
        },
        () => SelectedItem is BestiaryCreature);

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
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// Кубики
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private RollCube _rollcube = new();
        public RollCube RollCube
        {
            get => _rollcube;
            private set
            {
                _rollcube = value;
                OnPropertyChanged("RollCube");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public ObservableCollection<ListCube> ListCubeCollection { get; } = new();
        public RelayCommand Roll => GetCommand(() =>
        {
            Random rnd = new();
            int value = 0;
            ListCube listCube = new();
            int sum = 0;
            int count = 0;
            listCube.NameCube = "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + RollCube.CubesNumber + "d" + RollCube.D_Cube;
            if (RollCube.NumberAdditionSubtraction != 0) listCube.NameCube += RollCube.PlusMinus + RollCube.NumberAdditionSubtraction + ": ";
            else listCube.NameCube += ": ";

            for (int i = 0; i < RollCube.CubesNumber; i++) {
                value = rnd.Next(1, RollCube.D_Cube);
                sum += value;
                if(RollCube.CubesNumber > 1 || RollCube.NumberAdditionSubtraction != 0)
                    listCube.CubeResultToolTip += value;
                count++;
                if (count < RollCube.CubesNumber)
                    listCube.CubeResultToolTip += "+";
            }
            if (RollCube.NumberAdditionSubtraction != 0)
            {
                listCube.CubeResultToolTip += RollCube.PlusMinus + RollCube.NumberAdditionSubtraction;
                if(RollCube.PlusMinus == "+") sum += RollCube.NumberAdditionSubtraction;
                else sum -= RollCube.NumberAdditionSubtraction;
            }
            listCube.CubeResult = sum.ToString();
            ListCubeCollection.Add(listCube);
        });
        public RelayCommand ClearHistory => GetCommand(() =>
        {
            ListCubeCollection.Clear();
        });   
    }
}
