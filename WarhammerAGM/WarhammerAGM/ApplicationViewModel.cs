using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    public partial class ApplicationViewModel : ViewModelBase
    {
        private readonly ApplicationContext db = new ApplicationContext();

        public ObservableCollection<BestiaryCreature> BestiaryCreatures { get; }

        /// <summary>Сущность для региона детализации.</summary>
        public BestiaryCreature EditableBC
        {
            get => Get<BestiaryCreature>()!;
            private set => Set(value ?? throw new ArgumentNullException(nameof(value)));
        }

        /// <summary>Выбранная сущность.</summary>
        public BestiaryCreature? SelectedBC
        {
            get => Get<BestiaryCreature?>();
            set => Set(value);
        }

        protected override void OnPropertyChanged(string propertyName, object? oldValue, object? newValue)
        {
            base.OnPropertyChanged(propertyName, oldValue, newValue);

            if (propertyName == nameof(SelectedBC))
            {
                BestiaryCreature? @new = (BestiaryCreature?)newValue;
                if (@new is null)
                    EditableBC = new();
                else
                    EditableBC = db.BestiaryCreatures.AsNoTracking().First(bc => bc.Id == @new.Id);
            }
        }

        public ApplicationViewModel()
        {
            /*В конструкторе класса загружаем данные из бд в локальный кэш*/
            db.Database.EnsureCreated();
            db.BestiaryCreatures.Load();
            EditableBC = new();
            BestiaryCreatures = db.BestiaryCreatures.Local.ToObservableCollection();
        }

        /// <summary>Добавление сущности <see cref="EditableBC"/>.</summary>
        public RelayCommand AddCommand => GetCommand(() =>
        {
            var bestCrId = EditableBC.Id;
            // Обнуляем Id и добавляем как новую
            EditableBC.Id = 0;
            db.BestiaryCreatures.Add(EditableBC);
            try
            {
                db.SaveChanges();
                MessageBox.Show("Добавление прошло успешно");

                //отменяем выделение элемента ListView
                if (SelectedBC is null)
                    EditableBC = new();
                else
                    SelectedBC = null;
            }
            catch (Exception ex)
            {
                // Здесь обработка ошибок ex
                db.BestiaryCreatures.Remove(EditableBC);
                EditableBC.Id = bestCrId;
                MessageBox.Show("Такое название уже существует");

            }
        });

        /// <summary>Обновление сущности <see cref="EditableBC"/>.</summary>
        public RelayCommand EditCommand => GetCommand(
        () =>
        {
            var bestCrOld = SelectedBC;
            try
            {
                // Запоминаем в локальной переменной
                var bestCr = EditableBC;

                // Заменяем сущности
                int index = BestiaryCreatures.TakeWhile(bc => bc.Id != bestCr.Id).Count();
                BestiaryCreatures.RemoveAt(index);
                BestiaryCreatures.Insert(index, bestCr);

                // Сохраняем изменения
                db.SaveChanges();

                //отменяем вделение элемента ListView
                SelectedBC = null;
            }
            catch (Exception ex)
            {
                int index = BestiaryCreatures.TakeWhile(bc => bc.Id != bestCrOld.Id).Count();
                BestiaryCreatures.RemoveAt(index);
                BestiaryCreatures.Insert(index, bestCrOld);
                MessageBox.Show("Такое название уже существует");
            }
        },
        () => SelectedBC is BestiaryCreature);

        /// <summary>Удаление сущности <see cref="SelectedBC"/>.</summary>
        public RelayCommand DeleteCommand => GetCommand(
            () =>
            {
                if (SelectedBC is BestiaryCreature selectedItem)
                {
                    db.BestiaryCreatures.Remove(selectedItem);
                    db.SaveChanges();
                }
            },
            () => SelectedBC is BestiaryCreature);
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
                if(RollCube.CubesNumber > 1)
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
