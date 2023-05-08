using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using ViewModels;
using WarhammerAGM.Models;
using WarhammerAGM.Models.WarhammerAGM.Models;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace WarhammerAGM
{
    public partial class ApplicationViewModel : ViewModelBase
    {
        public ApplicationViewModel()
        {
            /*В конструкторе класса загружаем данные из бд в локальный кэш*/
            db.Database.EnsureCreated();
            db.BestiaryCreatures.Load();
            db.Characters.Load();
            db.Initiatives.Load();

            ////var bc=  db.BestiaryCreatures.First();
            ////var ch = db.Characters.First();

            //db.BestiaryCreatures.Add(new BestiaryCreature() { Name = "123456" });
            //db.Characters.Add(new Character() { Name = "qwerty" });
            //db.SaveChanges();

            //db.Initiatives.Add(new Initiative() { Name = "zxc", BestiaryCreature = db.BestiaryCreatures.First() });
            //db.Initiatives.Add(new Initiative() { Name = "bnm", BestiaryCreature = db.Characters.First() });

            //db.SaveChanges();

            EditableBC = new();
            EditableBCView = new();
            EditableC_CheckBox = new();
            EditableC = new();
            EditableI = new();
            BestiaryCreatures = db.BestiaryCreatures.Local.ToObservableCollection();
            Characters = db.Characters.Local.ToObservableCollection();
            Initiatives = db.Initiatives.Local.ToBindingList();
            Initiatives.ListChanged += OnInitiativesChanged;
            IsToolTipVisible = false;
        }

        private void OnInitiativesChanged(object? sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {
                db.SaveChanges();
            }
        }

        private void AddCreatureInInitiatives(IEnumerable<BestiaryCreature> bestiaryCreatures)
        {
            foreach (BestiaryCreature bestiaryCreature in bestiaryCreatures)
            {
                Initiative? initiative = db.Initiatives.Local.FirstOrDefault(i => i.BestiaryCreatureId == bestiaryCreature.Id);
                if (initiative is null)
                {
                    string name = bestiaryCreature.Name;
                    if (bestiaryCreature is not Character)
                        name += " #1";
                    initiative = new() { BestiaryCreature = bestiaryCreature, Name = name, СurrentWounds = bestiaryCreature.Wounds };

                }
                else if (bestiaryCreature is not Character)
                {
                    string lastName = db.Initiatives.Local.Where(i => i.BestiaryCreatureId == bestiaryCreature.Id)
                        .Select(i => i.Name).OrderBy(name => name).Last()!;
                    var split = lastName.Split(" #");
                    int num = int.Parse(split[1]) + 1;
                    var name = bestiaryCreature.Name + " #" + num;
                    initiative = new() { BestiaryCreature = bestiaryCreature, Name = name, СurrentWounds = bestiaryCreature.Wounds };
                }
                if (initiative is not null)
                {

                    db.Add(initiative);
                }

                db.SaveChanges();
            }
        }

        private readonly ApplicationContext db = new ApplicationContext();
        public ObservableCollection<BestiaryCreature> BestiaryCreatures { get; }
        public ObservableCollection<Character> Characters { get; }
        public BindingList<Initiative> Initiatives { get; }
        /// <summary>Сущность для региона детализации.</summary>
        public BestiaryCreature EditableBC
        {
            get => Get<BestiaryCreature>()!;
            private set => Set(value ?? throw new ArgumentNullException(nameof(value)));
        }
        public BestiaryCreature EditableBCView
        {
            get => Get<BestiaryCreature>()!;
            private set => Set(value ?? throw new ArgumentNullException(nameof(value)));
        }
        public Character EditableC
        {
            get => Get<Character>()!;
            private set => Set(value ?? throw new ArgumentNullException(nameof(value)));
        }
        public Character EditableC_CheckBox
        {
            get => Get<Character>()!;
            private set => Set(value ?? throw new ArgumentNullException(nameof(value)));
        }
        public Initiative EditableI
        {
            get => Get<Initiative>()!;
            private set => Set(value ?? throw new ArgumentNullException(nameof(value)));
        }
        /// <summary>Выбранная сущность.</summary>
        public BestiaryCreature? SelectedBC
        {
            get => Get<BestiaryCreature?>();
            set => Set(value);
        }
        public BestiaryCreature? SelectedBCView
        {
            get => Get<BestiaryCreature?>();
            set => Set(value);
        }
        public Character? SelectedC
        {
            get => Get<Character?>();
            set => Set(value);
        }
        public Character? SelectedC_CheckBox
        {
            get => Get<Character?>();
            set => Set(value);
        }
        public Initiative? SelectedI
        {
            get => Get<Initiative?>();
            set => Set(value);
        }

        protected override void OnPropertyChanged(string propertyName, object? oldValue, object? newValue)
        {
            base.OnPropertyChanged(propertyName, oldValue, newValue);

            if (propertyName == nameof(SelectedBC))
            {
                BestiaryCreature? @new = (BestiaryCreature?)newValue; //@new. Символ @ предшествует элементу кода, который компилятор должен интерпретировать как идентификатор, а не ключевое слово C#
                if (@new is null)
                    EditableBC = new();
                else
                    EditableBC = db.BestiaryCreatures.AsNoTracking().First(bc => bc.Id == @new.Id); //Чтобы данные не помещались в кэш, применяется метод AsNoTracking()
            }
            if (propertyName == nameof(SelectedBCView))
            {
                BestiaryCreature? @new = (BestiaryCreature?)newValue;
                if (@new is null)
                    EditableBCView = new();
                else
                    EditableBCView = db.BestiaryCreatures.AsNoTracking().First(bc => bc.Id == @new.Id);
            }
            if (propertyName == nameof(SelectedC))
            {
                Character? @new = (Character?)newValue;
                if (@new is null)
                    EditableC = new();
                else
                    EditableC = db.Characters.AsNoTracking().First(bc => bc.Id == @new.Id);
            }
            if (propertyName == nameof(SelectedC_CheckBox))
            {
                Character? @new = (Character?)newValue;
                if (@new is null)
                {
                    EditableC_CheckBox = new();
                    return;
                }
                else
                    EditableC_CheckBox = db.Characters.AsNoTracking().First(bc => bc.Id == @new.Id);
                int index = Characters.TakeWhile(bc => bc.Id != EditableC_CheckBox.Id).Count();
                if (EditableC_CheckBox.OnOfCharacter == false)
                    EditableC_CheckBox.OnOfCharacter = true;
                else
                    EditableC_CheckBox.OnOfCharacter = false;
                if (index < 0)
                {
                    MessageBox.Show("Такого Id не существует");
                    return;
                }
                else
                {
                    Character bestCrOld = Characters[index];
                    try
                    {
                        Characters[index] = EditableC_CheckBox;
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Characters[index] = bestCrOld;
                        MessageBox.Show("Что-то пошло не так..");
                        return;
                    }
                }
            }
            if (propertyName == nameof(SelectedI))
            {
                Initiative? @new = (Initiative?)newValue;
                if (@new is null)
                {
                    EditableI = new();
                    return;
                }
                else
                    EditableI = db.Initiatives.AsNoTracking().First(bc => bc.Id == @new.Id);
            }
        }
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
        public RelayCommand DeleteCommandC => GetCommand(
            () =>
            {
                if (SelectedC is Character selectedItem)
                {
                    db.Characters.Remove(selectedItem);
                    db.SaveChanges();
                }
            },
            () => SelectedBC is BestiaryCreature);
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool IsToolTipVisible
        {
            get => Get<bool>();
            set => Set(value);
        }
        public RelayCommand ShowToolTipCommand => GetCommand(
            () =>
            {
                IsToolTipVisible = true;
            });
        public RelayCommand HideToolTipCommand => GetCommand(
            () =>
            {
                IsToolTipVisible = false;
            });
        //public RelayCommand SliderChangeInitiative => GetCommand(
        //(double Value) =>
        //{
        //    if (EditableI.СurrentWounds != (int)Value)
        //    {
        //        // Отключить механизм привязки данных временно
        //        BindingOperations.DisableCollectionSynchronization(Initiatives);
        //        lock (Initiatives)
        //        {
        //            EditableI.СurrentWounds = EditableI.Wounds - (EditableI.Wounds - (int)Value);
        //            EditableI.MinPlusSlider = (int)Value - EditableI.Wounds;
        //            int index = Initiatives.TakeWhile(bc => bc.Id != EditableI.Id).Count();
        //            Initiatives[index] = EditableI;
        //            db.SaveChanges();
        //        }
        //        // Включить механизм привязки данных снова
        //        BindingOperations.EnableCollectionSynchronization(Initiatives, new object());
        //    }
        //});
        public RelayCommand AddCreatureInitiative => GetCommand<IList>(
            list =>
            {
                AddCreatureInInitiatives(list.OfType<Character>());
            });
        public RelayCommand AddBestiaryInitiative => GetCommand<BestiaryCreature>(
            bc =>
            {
                AddCreatureInInitiatives(new BestiaryCreature[] { bc });
            });
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public RollCube RollCube
        {
            get => Get<RollCube>()!;
            private set => Set(value);
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

            for (int i = 0; i < RollCube.CubesNumber; i++)
            {
                value = rnd.Next(1, RollCube.D_Cube);
                sum += value;
                if (RollCube.CubesNumber > 1)
                    listCube.CubeResultToolTip += value;
                count++;
                if (count < RollCube.CubesNumber)
                    listCube.CubeResultToolTip += "+";
            }
            if (RollCube.NumberAdditionSubtraction != 0)
            {
                listCube.CubeResultToolTip += RollCube.PlusMinus + RollCube.NumberAdditionSubtraction;
                if (RollCube.PlusMinus == "+") sum += RollCube.NumberAdditionSubtraction;
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
