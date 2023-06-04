using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ViewModels;
using WarhammerAGM.Models;
using WarhammerAGM.Models.Arsenal.Money;
using WarhammerAGM.Models.Arsenal.Weapons;
using WarhammerAGM.Models.WarhammerAGM.Models;

namespace WarhammerAGM
{
    public partial class ApplicationViewModel : ViewModelBase
    {
        public ObservableCollection<string> FileNames { get; set; }
        public ApplicationViewModel()
        {
            /*В конструкторе класса загружаем данные из бд в локальный кэш*/
            db.Database.EnsureCreated();
            db.BestiaryCreatures.Load();
            db.Characters.Load();
            db.CreatureBases.Load();

            db.Initiatives.Load();
            db.TemporaryInitiatives.Load();
            db.DeathListInitiatives.Load();

            db.IncomeAndSocialClasses.Load();
            db.ScumbagIncomes.Load();

            db.MeleeWeapons.Load();
            db.RangedWeapons.Load();
            db.WeaponProperties.Load();
            db.WeaponImprovements.Load();
            db.Ammunitions.Load();
            db.Worlds.Load();

            EditableBC = new();
            EditableBCViewInitiative = new();
            EditableBCView = new();
            EditableC_CheckBox = new();
            EditableC = new();
            EditableI = new();
            EditableDeath = new();

            CreatureBases = db.CreatureBases.Local.ToObservableCollection();
            BestiaryCreatures = db.BestiaryCreatures.Local.ToObservableCollection();
            Characters = db.Characters.Local.ToObservableCollection();

            Initiatives = db.Initiatives.Local.ToBindingList();
            Initiatives.ListChanged += OnInitiativesChanged;

            TemporaryInitiatives = db.TemporaryInitiatives.Local.ToBindingList();
            TemporaryInitiatives.ListChanged += OnInitiativesChanged;
            DeathListInitiatives = db.DeathListInitiatives.Local.ToBindingList();

            IncomeAndSocialClasses = db.IncomeAndSocialClasses.Local.ToObservableCollection();
            ScumbagIncomes = db.ScumbagIncomes.Local.ToObservableCollection();

            MeleeWeapons = db.MeleeWeapons.Local.ToObservableCollection();
            RangedWeapons = db.RangedWeapons.Local.ToObservableCollection();
            WeaponProperties = db.WeaponProperties.Local.ToObservableCollection();
            WeaponImprovements = db.WeaponImprovements.Local.ToObservableCollection();
            Ammunitions = db.Ammunitions.Local.ToObservableCollection();
            Worlds = db.Worlds.Local.ToBindingList();

            SelectedWorldWeaponsMelee = Worlds.FirstOrDefault();
            SelectedWorldWeaponsRanged = Worlds.FirstOrDefault();

            FileNames = new ObservableCollection<string>();

            ColorPick = Colors.Black;

            Round = Properties.Settings.Default.RoundSetting;
            SliderValueChange = "0";
            LoadFileNames();
        }
        private readonly ApplicationContext db = new();
        public ObservableCollection<CreatureBase> CreatureBases { get; }
        public ObservableCollection<BestiaryCreature> BestiaryCreatures { get; }
        public ObservableCollection<Character> Characters { get; }

        public BindingList<Initiative> Initiatives { get; set; }
        public BindingList<TemporaryInitiative> TemporaryInitiatives { get; }
        public BindingList<DeathListInitiative> DeathListInitiatives { get; set; }

        public ObservableCollection<IncomeAndSocialClass> IncomeAndSocialClasses { get; set; }
        public ObservableCollection<ScumbagIncome> ScumbagIncomes { get; set; }

        public ObservableCollection<MeleeWeapon> MeleeWeapons { get; set; }
        public ObservableCollection<RangedWeapon> RangedWeapons { get; set; }
        public ObservableCollection<WeaponPropertie> WeaponProperties { get; set; }
        public ObservableCollection<WeaponImprovement> WeaponImprovements { get; set; }
        public ObservableCollection<Ammunition> Ammunitions { get; set; }
        public BindingList<World> Worlds { get; set; }

        //private List<Initiative> OldListInitiative;
        //private List<TemporaryInitiative> OldListTemporary;
        /// <summary>Сущность для региона детализации.</summary>
        public BestiaryCreature EditableBC
        {
            get => Get<BestiaryCreature>()!;
            private set => Set(value ?? throw new ArgumentNullException(nameof(value)));
        }
        public BestiaryCreature EditableBCViewInitiative
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
        public DeathListInitiative EditableDeath
        {
            get => Get<DeathListInitiative>()!;
            private set => Set(value ?? throw new ArgumentNullException(nameof(value)));
        }
        public BestiaryCreature EditableBCViewInitiativeStats
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
        public BestiaryCreature? SelectedBCViewInitiative
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
        public DeathListInitiative? SelectedDeath
        {
            get => Get<DeathListInitiative?>();
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
            if (propertyName == nameof(SelectedBCViewInitiative))
            {
                BestiaryCreature? @new = (BestiaryCreature?)newValue;
                if (@new is null)
                    EditableBCViewInitiative = new();
                else
                    EditableBCViewInitiative = db.BestiaryCreatures.AsNoTracking().First(bc => bc.Id == @new.Id);
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
                //int index = BestiaryCreatures.TakeWhile(bc => bc.Id != EditableI.CreatureBaseId).Count();
                //EditableI.CreatureBase.Ballistics = 10;
                //EditableBCViewInitiativeStats = BestiaryCreatures[EditableI.Id];
            }
            if (propertyName == nameof(SelectedDeath))
            {
                DeathListInitiative? @new = (DeathListInitiative?)newValue;
                if (@new is null)
                    EditableDeath = new();
                else
                    EditableDeath = db.DeathListInitiatives.AsNoTracking().First(bc => bc.Id == @new.Id);
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
                    if (EditableI.Importancenitiative != Initiatives[e.NewIndex].Importancenitiative)
                    {
                        db.SaveChanges();
                        SortInitiative();
                    }
                }
                else
                    db.SaveChanges();
                db.SaveChanges();
            }
            if (e.ListChangedType == ListChangedType.Reset)
            {
                db.SaveChanges();
            }
            //OldListInitiative = Initiatives.ToList();
            //OldListTemporary = TemporaryInitiatives.ToList();
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
                    db.SaveChanges();
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
        public double PositionX
        {
            get => Get<double>()!;
            private set => Set(value);
        }
        public RelayCommand PopupMove => GetCommand(
        (parametr) =>
        {
            if (parametr is not null)
            {
                Thumb thumb = (Thumb)parametr;
                if (thumb.IsDragging)
                {
                    var position = Mouse.GetPosition(thumb);
                    PositionX = position.X;
                    UpdatePopupHorizontalOffset();
                }
            }
        });
        //private void UpdatePopupHorizontalOffset()
        //{
        //    Application.Current.Dispatcher.Invoke(() =>
        //    {
        //        // Обновление значения HorizontalOffset в UI-потоке
        //        var popup = FindVisualChild<Popup>(Application.Current.MainWindow); // Найдите Popup по имени или другим способом
        //        if (popup != null)
        //        {
        //            popup.HorizontalOffset = PositionX;
        //        }
        //    });
        //}
        private void UpdatePopupHorizontalOffset()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var popup = FindVisualChild<Popup>(Application.Current.MainWindow);
                if (popup != null)
                {
                    double targetOffset = PositionX;

                    DoubleAnimation animation = new()
                    {
                        To = targetOffset,
                        Duration = TimeSpan.FromSeconds(0.0001), // Продолжительность анимации (в данном случае 0.1 секунды)
                        EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut } // Функция сглаживания для плавности анимации
                    };

                    popup.BeginAnimation(Popup.HorizontalOffsetProperty, animation);
                }
            });
        }
        public static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    if (child is Popup popup && popup.Name == "PopupSlider" && child is T typedChild)
                        return typedChild;

                    var foundChild = FindVisualChild<T>(child);
                    if (foundChild != null)
                        return foundChild;
                }
            }
            return null;
        }
        public RelayCommand DragCompletedSliderInitiative => GetCommand<Initiative>(
        (value) =>
        {
            SliderValueChange = "0";
            int index = Initiatives.TakeWhile(bc => bc.Id != EditableI.Id).Count();
            if (Initiatives[index].СurrentWounds == 0)
            {
                DeathListInitiative deathListInitiative = new()
                {
                    Name = Initiatives[index].Name,
                    CreatureBase = Initiatives[index].CreatureBase,
                    CreatureBaseId = Initiatives[index].CreatureBaseId,
                    Wounds = Initiatives[index].Wounds,
                    СurrentWounds = Initiatives[index].Wounds,
                    Importancenitiative = Initiatives[index].Importancenitiative,
                    DexterityModifier = Initiatives[index].DexterityModifier,
                    Type = Initiatives[index].Type
                };
                DeathListInitiatives.Add(deathListInitiative);
                Initiatives.Remove(Initiatives[index]);
                db.SaveChanges();
            }
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
        public RelayCommand ReturnInitiative => GetCommand(
            () =>
            {
                Random rnd = new();
                int indexDI = DeathListInitiatives.TakeWhile(bc => bc.Id != EditableDeath.Id).Count();
                int indexCB = CreatureBases.TakeWhile(bc => bc.Id != EditableDeath.CreatureBaseId).Count();
                Initiative initiative = new()
                {
                    CreatureBase = CreatureBases[indexCB],
                    Name = EditableDeath.Name,
                    Wounds = EditableDeath.Wounds,
                    СurrentWounds = EditableDeath.Wounds,
                    Importancenitiative = rnd.Next(1, 10) + EditableDeath.DexterityModifier,
                    DexterityModifier = EditableDeath.DexterityModifier,
                    Type = EditableDeath.Type
                };
                initiative.Id = 0;
                Initiatives.Add(initiative);
                DeathListInitiatives.Remove(DeathListInitiatives[indexDI]);
                db.SaveChanges();
            });
        public RelayCommand EndInitiative => GetCommand(
            () =>
            {
                Initiatives.Clear();
                TemporaryInitiatives.Clear();
                DeathListInitiatives.Clear();

                Round = 0;
                Properties.Settings.Default.RoundSetting = Round;
                Properties.Settings.Default.Save();
            });
        public RelayCommand SlectedBCViewInitiativeNull => GetCommand(
            () =>
            {
                SelectedBCViewInitiative = null;
            });
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
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public string? FileName
        {
            get => Get<string>()!;
            set => Set(value);
        }
        private string selectedFileNameView;
        public string SelectedFileNameView
        {
            get { return selectedFileNameView; }
            set
            {
                selectedFileNameView = value;
                OnPropertyChanged(nameof(SelectedFileNameView));
                LoadFileContentView();
            }
        }
        private string selectedFileName;
        public string? SelectedFileName
        {
            get { return selectedFileName; }
            set
            {
                selectedFileName = value;
                OnPropertyChanged(nameof(SelectedFileName));
                LoadFileContent();
            }
        }
        public string? RichTextDocument
        {
            get => Get<string>()!;
            set => Set(value);
        }
        public string? NoteContent
        {
            get => Get<string>()!;
            set => Set(value);
        }
        private void LoadFileContent()
        {
            string appFolderPath = AppDomain.CurrentDomain.BaseDirectory;
            string notesFolderPath = Path.Combine(appFolderPath, "Notes");
            string filePath = Path.Combine(notesFolderPath, SelectedFileName + ".xaml");

            if (File.Exists(filePath))
            {
                RichTextDocument = File.ReadAllText(filePath);
                FileName = SelectedFileName;
            }
            else
            {
                RichTextDocument = null;
            }
        }
        private void LoadFileContentView()
        {
            string appFolderPath = AppDomain.CurrentDomain.BaseDirectory;
            string notesFolderPath = Path.Combine(appFolderPath, "Notes");
            string filePath = Path.Combine(notesFolderPath, SelectedFileNameView + ".xaml");

            if (File.Exists(filePath))
            {
                NoteContent = File.ReadAllText(filePath);
            }
            else
            {
                NoteContent = null;
            }
        }
        public RelayCommand SaveNotesCommand => GetCommand(() =>
        {
            string appFolderPath = AppDomain.CurrentDomain.BaseDirectory; // Получаем путь к основной папке приложения
            string notesFolderPath = Path.Combine(appFolderPath, "Notes"); // Объединяем путь с подпапкой "Notes"
                                                                           // Создаем уникальное имя файла на основе текущей даты и времени
            if (string.IsNullOrWhiteSpace(FileName))
            {
                MessageBox.Show("Введите название заметки(оно не должно быть пустым или состоять только из проблелов");
                return;
            }
            string filePath = Path.Combine(notesFolderPath, FileName + ".xaml");
            if (File.Exists(filePath))
            {
                MessageBoxResult result = MessageBox.Show("Файл с таким именем уже существует. Хотите заменить его?", "Подтверждение замены файла", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
                else if (result == MessageBoxResult.Yes)
                {
                    string? buf = RichTextDocument;
                    File.Delete(filePath); // Удаление старого файла
                    FileNames.Remove(FileName);
                    RichTextDocument = buf;
                }
            }
            File.WriteAllText(filePath, RichTextDocument);
            FileNames.Add(FileName);
            RichTextDocument = null;
            FileName = null;
        });
        public void LoadFileNames()
        {
            string appFolderPath = AppDomain.CurrentDomain.BaseDirectory; // Получаем путь к основной папке приложения
            string notesFolderPath = Path.Combine(appFolderPath, "Notes"); // Объединяем путь с подпапкой "Notes"
            FileNames.Clear();
            if (!Directory.Exists(notesFolderPath))
            {
                // Если папка не существует, выходим из метода
                return;
            }

            // Получаем список файлов в папке "Notes"
            string[] files = Directory.GetFiles(notesFolderPath, "*.xaml");
            if (files.Length == 0)
                return;
            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                FileNames.Add(fileName);
            }
        }
        public RelayCommand DeleteContentNotes => GetCommand(
            () =>
            {
                SelectedFileName = null;
                RichTextDocument = null;
                FileName = null;
            });
        public Color ColorPick
        {
            get => Get<Color>()!;
            set => Set(value);
        }
        public double? FontSizeRichTextBoxSelectedText
        {
            get => Get<double>()!;
            set => Set(value);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ObservableCollection<MeleeWeapon> MeleeWeaponsViewPrimitive { get; set; } = new ObservableCollection<MeleeWeapon>();
        public ObservableCollection<MeleeWeapon> MeleeWeaponsViewChain { get; set; } = new ObservableCollection<MeleeWeapon>();
        public ObservableCollection<MeleeWeapon> MeleeWeaponsViewPower { get; set; } = new ObservableCollection<MeleeWeapon>();
        public ObservableCollection<MeleeWeapon> MeleeWeaponsViewShock { get; set; } = new ObservableCollection<MeleeWeapon>();

        public ObservableCollection<RangedWeapon> RangedWeaponsViewLaser { get; set; } = new ObservableCollection<RangedWeapon>();
        public ObservableCollection<RangedWeapon> RangedWeaponsViewBullet { get; set; } = new ObservableCollection<RangedWeapon>();
        public ObservableCollection<RangedWeapon> RangedWeaponsViewBolter { get; set; } = new ObservableCollection<RangedWeapon>();
        public ObservableCollection<RangedWeapon> RangedWeaponsViewMelta { get; set; } = new ObservableCollection<RangedWeapon>();
        public ObservableCollection<RangedWeapon> RangedWeaponsViewPlasma { get; set; } = new ObservableCollection<RangedWeapon>();
        public ObservableCollection<RangedWeapon> RangedWeaponsViewFlamethrower { get; set; } = new ObservableCollection<RangedWeapon>();
        public ObservableCollection<RangedWeapon> RangedWeaponsViewPrimitive { get; set; } = new ObservableCollection<RangedWeapon>();
        public ObservableCollection<RangedWeapon> RangedWeaponsViewBeam { get; set; } = new ObservableCollection<RangedWeapon>();
        public ObservableCollection<RangedWeapon> RangedWeaponsViewGrenades { get; set; } = new ObservableCollection<RangedWeapon>();
        public ObservableCollection<RangedWeapon> RangedWeaponsViewExotic { get; set; } = new ObservableCollection<RangedWeapon>();


        public bool VisibilityPrimitiveMelee
        {
            get => Get<bool>()!;
            set => Set(value);
        }
        public bool VisibilityChainMelee
        {
            get => Get<bool>()!;
            set => Set(value);
        }
        public bool VisibilityPowerMelee
        {
            get => Get<bool>()!;
            set => Set(value);
        }
        public bool VisibilityShockMelee
        {
            get => Get<bool>()!;
            set => Set(value);
        }

        private World selectedworldWeaponsMelee;
        public World SelectedWorldWeaponsMelee
        {
            get { return selectedworldWeaponsMelee; }
            set
            {
                selectedworldWeaponsMelee = value;
                WeaponViewWorld();
            }
        }
        public void WeaponViewWorld()
        {
            if (MeleeWeapons.Count == 0)
                return;
            var weapons = db.MeleeWeapons.Where(w => w.World == SelectedWorldWeaponsMelee.Name).ToList();

            MeleeWeaponsViewPrimitive.Clear();
            MeleeWeaponsViewChain.Clear();
            MeleeWeaponsViewPower.Clear();
            MeleeWeaponsViewShock.Clear();
            db.SaveChanges();

            // Добавляем элементы в существующую коллекцию
            foreach (var weapon in weapons)
            {
                if (weapon.Type == "Primitive")
                {
                    MeleeWeaponsViewPrimitive.Add(weapon);
                }
                else
                {
                    if (weapon.Type == "Chain")
                    {
                        MeleeWeaponsViewChain.Add(weapon);
                    }
                    else
                    {
                        if (weapon.Type == "Power")
                        {
                            MeleeWeaponsViewPower.Add(weapon);
                        }
                        else
                        {
                            if (weapon.Type == "Shock")
                            {
                                MeleeWeaponsViewShock.Add(weapon);
                            }
                        }
                    }
                }
            }
            db.SaveChanges();
            if (MeleeWeaponsViewPrimitive.Count != 0)
                VisibilityPrimitiveMelee = true;
            else
            { VisibilityPrimitiveMelee = false; }

            if (MeleeWeaponsViewChain.Count != 0)
            { VisibilityChainMelee = true; }
            else
            { VisibilityChainMelee = false; }

            if (MeleeWeaponsViewPower.Count != 0)
                VisibilityPowerMelee = true;
            else
            { VisibilityPowerMelee = false; }

            if (MeleeWeaponsViewShock.Count != 0)
                VisibilityShockMelee = true;
            else 
            { VisibilityShockMelee = false; }
        }
        private World selectedworldWeaponsRanged;
        public World SelectedWorldWeaponsRanged
        {
            get { return selectedworldWeaponsRanged; }
            set
            {
                selectedworldWeaponsRanged = value;
                RangedViewWorld();
            }
        }
        public bool VisibilityLaserRanged
        {
            get => Get<bool>()!;
            set => Set(value);
        }
        public bool VisibilityBulletRanged
        {
            get => Get<bool>()!;
            set => Set(value);
        }
        public bool VisibilityBolterRanged
        {
            get => Get<bool>()!;
            set => Set(value);
        }
        public bool VisibilityMeltaRanged
        {
            get => Get<bool>()!;
            set => Set(value);
        }
        public bool VisibilityPlasmaRanged
        {
            get => Get<bool>()!;
            set => Set(value);
        }
        public bool VisibilityFlamethrowerRanged
        {
            get => Get<bool>()!;
            set => Set(value);
        }
        public bool VisibilityPrimitiveRanged
        {
            get => Get<bool>()!;
            set => Set(value);
        }
        public bool VisibilityBeamRanged
        {
            get => Get<bool>()!;
            set => Set(value);
        }
        public bool VisibilityGrenadesRanged
        {
            get => Get<bool>()!;
            set => Set(value);
        }
        public bool VisibilityExoticRanged
        {
            get => Get<bool>()!;
            set => Set(value);
        }
        public void RangedViewWorld()
        {

            if (MeleeWeapons.Count == 0)
                return;
            var weapons = db.RangedWeapons.Where(w => w.World == SelectedWorldWeaponsMelee.Name).ToList();

            RangedWeaponsViewLaser.Clear();
            RangedWeaponsViewBullet.Clear();
            RangedWeaponsViewBolter.Clear();
            RangedWeaponsViewMelta.Clear();
            RangedWeaponsViewPlasma.Clear();
            RangedWeaponsViewFlamethrower.Clear();
            RangedWeaponsViewPrimitive.Clear();
            RangedWeaponsViewBeam.Clear();
            RangedWeaponsViewGrenades.Clear();
            RangedWeaponsViewExotic.Clear();
            db.SaveChanges();

            // Добавляем элементы в существующую коллекцию
            foreach (var weapon in weapons)
            {
                if (weapon.Type == "Laser")
                {
                    RangedWeaponsViewLaser.Add(weapon);
                }
                else
                {
                    if (weapon.Type == "Bullet")
                    {
                        RangedWeaponsViewBullet.Add(weapon);
                    }
                    else
                    {
                        if (weapon.Type == "Bolter")
                        {
                            RangedWeaponsViewBolter.Add(weapon);
                        }
                        else
                        {
                            if (weapon.Type == "Melta")
                            {
                                RangedWeaponsViewMelta.Add(weapon);
                            }
                            else
                            {
                                if (weapon.Type == "Plasma")
                                {
                                    RangedWeaponsViewPlasma.Add(weapon);
                                }
                                else
                                {
                                    if (weapon.Type == "Flamethrower")
                                    {
                                        RangedWeaponsViewFlamethrower.Add(weapon);
                                    }
                                    else
                                    {
                                        if (weapon.Type == "Primitive")
                                        {
                                            RangedWeaponsViewPrimitive.Add(weapon);
                                        }
                                        else
                                        {
                                            if (weapon.Type == "Beam")
                                            {
                                                RangedWeaponsViewBeam.Add(weapon);
                                            }
                                            else
                                            {
                                                if (weapon.Type == "Grenades")
                                                {
                                                    RangedWeaponsViewGrenades.Add(weapon);
                                                }
                                                else
                                                {
                                                    if (weapon.Type == "Exotic")
                                                    {
                                                        RangedWeaponsViewExotic.Add(weapon);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                db.SaveChanges();
                if (RangedWeaponsViewLaser.Count == 0)
                    VisibilityLaserRanged = true;
                else
                { VisibilityLaserRanged = false; }


                if (RangedWeaponsViewBullet.Count == 0)
                    VisibilityBulletRanged = true;
                else
                { VisibilityBulletRanged = false; }


                if (RangedWeaponsViewBolter.Count == 0)
                    VisibilityBolterRanged = true;
                else
                { VisibilityBolterRanged = false; }


                if (RangedWeaponsViewMelta.Count == 0)
                    VisibilityMeltaRanged = true;
                else
                { VisibilityMeltaRanged = false; }


                if (RangedWeaponsViewPlasma.Count == 0)
                    VisibilityPlasmaRanged = true;
                else
                { VisibilityPlasmaRanged = false; }


                if (RangedWeaponsViewFlamethrower.Count == 0)
                    VisibilityFlamethrowerRanged = true;
                else
                { VisibilityFlamethrowerRanged = false; }


                if (RangedWeaponsViewPrimitive.Count == 0)
                    VisibilityPrimitiveRanged = false;
                else
                { VisibilityPrimitiveRanged = false; }


                if (RangedWeaponsViewBeam.Count == 0)
                    VisibilityBeamRanged = false;
                else
                { VisibilityBeamRanged = false; }


                if (RangedWeaponsViewGrenades.Count == 0)
                    VisibilityGrenadesRanged = false;
                else
                { VisibilityGrenadesRanged = false; }


                if (RangedWeaponsViewExotic.Count == 0)
                    VisibilityExoticRanged = false;
                else
                { VisibilityExoticRanged = false; }
            }
        }
    }
}
