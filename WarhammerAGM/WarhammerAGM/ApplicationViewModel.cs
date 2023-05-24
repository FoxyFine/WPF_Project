using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Windows;
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
            db.CreatureBases.Load();

            db.Initiatives.Load();
            db.TemporaryInitiatives.Load();

            EditableBC = new();
            EditableBCView = new();
            EditableC_CheckBox = new();
            EditableC = new();
            //EditableI = new();
            CreatureBases = db.CreatureBases.Local.ToObservableCollection();
            BestiaryCreatures = db.BestiaryCreatures.Local.ToObservableCollection();
            Characters = db.Characters.Local.ToObservableCollection();

            Initiatives = db.Initiatives.Local.ToBindingList();
            Initiatives.ListChanged += OnInitiativesChanged;

            TemporaryInitiatives = db.TemporaryInitiatives.Local.ToBindingList();
            TemporaryInitiatives.ListChanged += OnInitiativesChanged;

            Round = Properties.Settings.Default.RoundSetting;
            SliderValueChange = "0";
        }
        private readonly ApplicationContext db = new();
        public ObservableCollection<CreatureBase> CreatureBases { get; }
        public ObservableCollection<BestiaryCreature> BestiaryCreatures { get; }
        public ObservableCollection<Character> Characters { get; }

        public BindingList<Initiative> Initiatives { get; set; }
        public BindingList<TemporaryInitiative> TemporaryInitiatives { get; }
        private List<Initiative> OldListInitiative;
        private List<TemporaryInitiative> OldListTemporary;
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
        public int Round
        {
            get => Get<int>()!;
            private set => Set(value);
        }
        private void OnInitiativesChanged(object? sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {
                if (EditableI is not null)
                {
                    if (EditableI.Importancenitiative != Initiatives[e.NewIndex].Importancenitiative && EditableI is not null)
                    {
                        db.SaveChanges();
                        SortInitiative();
                    }
                }
                else
                    db.SaveChanges();
            }
            if (e.ListChangedType == ListChangedType.Reset)
            {
                db.SaveChanges();
            }
            if (e.ListChangedType == ListChangedType.ItemDeleted)
            {
                db.SaveChanges();
            }
            OldListInitiative = Initiatives.ToList();
            OldListTemporary = TemporaryInitiatives.ToList();
        }
        public void SortInitiative()
        {
            if (Initiatives.Count > 1 && Initiatives.Count != 0)
            {
                List<Initiative> initiativeList = Initiatives.ToList();
                initiativeList = initiativeList.OrderByDescending(item => item.Importancenitiative).ToList();
                Initiatives.Clear();
                foreach (Initiative initiative in initiativeList)
                {
                    Initiatives.Add(initiative);
                }
                db.SaveChanges();
            }
        }
        public void SortTemporaryInitiative()
        {
            if (TemporaryInitiatives.Count > 1 && TemporaryInitiatives.Count != 0)
            {
                List<TemporaryInitiative> temporaryInitiativeList = TemporaryInitiatives.ToList();
                temporaryInitiativeList = temporaryInitiativeList.OrderByDescending(item => item.Importancenitiative).ToList();
                TemporaryInitiatives.Clear();
                foreach (TemporaryInitiative temporaryinitiative in temporaryInitiativeList)
                {
                    TemporaryInitiatives.Add(temporaryinitiative);
                }
                db.SaveChanges();
            }
        }
        private void AddCreatureInInitiatives(IEnumerable<CreatureBase> creatures)
        {
            foreach (CreatureBase creature in creatures)
            {
                Initiative? initiative = db.Initiatives.Local.FirstOrDefault(i => i.CreatureBaseId == creature.Id);
                if (initiative is null)
                {
                    string name = creature.Name;
                    if (creature is not Character)
                        name += " #1";

                    Random rnd = new();
                    int dex_mod = 0;
                    if (creature.Dexterity != 0)
                        dex_mod = creature.Dexterity / 10;
                    int initiative_num = rnd.Next(1, 10);

                    initiative = new() { CreatureBase = creature, Name = name, Wounds = creature.Wounds, СurrentWounds = creature.Wounds, Importancenitiative = initiative_num + dex_mod, DexterityModifier = dex_mod, Type = creature.Discriminator };

                    if (initiative is not null)
                    {
                        db.Add(initiative);
                    }
                }
                else if (creature is not Character)
                {
                    string lastName = db.Initiatives.Local.Where(i => i.CreatureBaseId == creature.Id)
                        .Select(i => i.Name).OrderBy(name => name).Last()!;
                    var split = lastName.Split(" #");
                    int num = int.Parse(split[1]) + 1;
                    var name = creature.Name + " #" + num;

                    Random rnd = new();
                    int dex_mod = 0;
                    if (creature.Dexterity != 0)
                        dex_mod = creature.Dexterity / 10;
                    int initiative_num = rnd.Next(1, 10);

                    initiative = new() { CreatureBase = creature, Name = name, Wounds = creature.Wounds, СurrentWounds = creature.Wounds, Importancenitiative = initiative_num + dex_mod, DexterityModifier = dex_mod, Type = creature.Discriminator };
                    if (initiative is not null)
                    {
                        db.Add(initiative);
                    }
                }
            }
            db.SaveChanges();
            SortInitiative();
        }
        public RelayCommand AddCreatureInitiative => GetCommand<IList>(
             list =>
             {
                 AddCreatureInInitiatives(list.OfType<Character>());
             });
        public RelayCommand AddBestiaryInitiative => GetCommand<CreatureBase>(
            bc =>
            {
                AddCreatureInInitiatives(new CreatureBase[] { bc });
            });
        public RelayCommand NextStep => GetCommand(
            () =>
            {
                Initiative? initiative = db.Initiatives.FirstOrDefault();
                if (initiative != null)
                {
                    db.Initiatives.Remove(initiative);
                    TemporaryInitiative? temporaryInitiative = new()
                    {
                        Id = initiative.Id,
                        Name = initiative.Name,
                        Type = initiative.Type,
                        Wounds = initiative.Wounds,
                        СurrentWounds = initiative.СurrentWounds,
                        DexterityModifier = initiative.DexterityModifier,
                        Importancenitiative = initiative.Importancenitiative,
                        CreatureBaseId = initiative.CreatureBaseId,
                        CreatureBase = initiative.CreatureBase
                    };
                    db.Add(temporaryInitiative);
                    db.SaveChanges();
                    SortTemporaryInitiative();
                }
                if (Initiatives.Count == 0 && TemporaryInitiatives.Count != 0)
                {
                    foreach (var item in TemporaryInitiatives)
                    {
                        Initiative? initiative1 = new()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Type = item.Type,
                            Wounds = item.Wounds,
                            СurrentWounds = item.СurrentWounds,
                            DexterityModifier = item.DexterityModifier,
                            Importancenitiative = item.Importancenitiative,
                            CreatureBaseId = item.CreatureBaseId,
                            CreatureBase = item.CreatureBase
                        };
                        db.Add(initiative1);
                    }
                    db.SaveChanges();
                    SortInitiative();
                    TemporaryInitiatives.Clear();

                    Round++;
                    Properties.Settings.Default.RoundSetting = Round;
                    Properties.Settings.Default.Save();
                }
            });
        public RelayCommand NextRound => GetCommand(
        () =>
        {
            if (Initiatives.Count == 0)
                return;
            Round++;
            Properties.Settings.Default.RoundSetting = Round;
            Properties.Settings.Default.Save();
            if (TemporaryInitiatives.Count != 0)
            {
                foreach (var item in TemporaryInitiatives)
                {
                    Initiative? initiative1 = new()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Type = item.Type,
                        Wounds = item.Wounds,
                        СurrentWounds = item.СurrentWounds,
                        DexterityModifier = item.DexterityModifier,
                        Importancenitiative = item.Importancenitiative,
                        CreatureBaseId = item.CreatureBaseId,
                        CreatureBase = item.CreatureBase
                    };
                    db.Add(initiative1);
                }
                db.SaveChanges();
                TemporaryInitiatives.Clear();
                SortInitiative();
            }
        });
        public RelayCommand RecalculateKInitiativeBestiary => GetCommand(
        () =>
        {
            if (Initiatives.Count == 0) return;
            List<Initiative> initiativeList = Initiatives.ToList();
            Random rnd = new();
            foreach (var item in initiativeList)
            {
                if (item.Type == "BestiaryCreature")
                {
                    int rndnum = rnd.Next(1, 10) + item.DexterityModifier;
                    int index = Initiatives.TakeWhile(bc => bc.Id != item.Id).Count();
                    Initiatives[index].Importancenitiative = rndnum;
                    db.SaveChanges();
                }
            }
        });
        public RelayCommand DragCompletedSliderInitiative => GetCommand<Initiative>(
        (value) =>
        {
            SliderValueChange = "0";
        });
        public string SliderValueChange
        {
            get => Get<string>()!;
            private set => Set(value);
        }
        int OldСurrentWounds;
        public RelayCommand DragStartedSliderInitiative => GetCommand<Initiative>(
            (value) =>
            {
                OldСurrentWounds = value.СurrentWounds;
            });
        public RelayCommand DragDeltaSliderInitiative => GetCommand<Initiative>(
        (value) =>
        {
            int changevalue = value.СurrentWounds - OldСurrentWounds;
            if (changevalue > 0)
            {
                SliderValueChange = "+" + changevalue.ToString();
            }
            else
                SliderValueChange = changevalue.ToString();

        });
        public double X
        {
            get => Get<double>()!;
            private set => Set(value);
        }
        public double Y
        {
            get => Get<double>()!;
            private set => Set(value);
        }
        public RelayCommand EndInitiative => GetCommand(
            () =>
            {
                Initiatives.Clear();
                TemporaryInitiatives.Clear();

                Round = 0;
                Properties.Settings.Default.RoundSetting = Round;
                Properties.Settings.Default.Save();
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
