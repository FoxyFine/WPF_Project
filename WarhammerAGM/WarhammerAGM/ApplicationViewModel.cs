using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Data;
using ViewModels;
using WarhammerAGM.Models;
using WarhammerAGM.Models.WarhammerAGM.Models;

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
            EditableBC = new();
            EditableBCView = new();
            EditableC_CheckBox = new();
            EditableC = new();
            EditableI = new();
            BestiaryCreatures = db.BestiaryCreatures.Local.ToObservableCollection();
            Characters = db.Characters.Local.ToObservableCollection();
            Initiatives = db.Initiatives.Local.ToObservableCollection();
        }

        private readonly ApplicationContext db = new ApplicationContext();
        public ObservableCollection<BestiaryCreature> BestiaryCreatures { get; }
        public ObservableCollection<Character> Characters { get; }
        public ObservableCollection<Initiative> Initiatives { get; }
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
                    EditableI = new();
                    return;
                }
                else
                    EditableI = db.Initiatives.AsNoTracking().First(bc => bc.Id == @new.Id);
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
            if(propertyName == nameof(SelectedI)) 
            {
                Initiative? @new = (Initiative?)newValue;
                if (@new is null)
                {
                    EditableI = new();
                    return;
                }
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
        public RelayCommand SliderChangeInitiative => GetCommand(
            (Value) =>
            {
                
            }
            );
        public RelayCommand AddCreatureInitiative => GetCommand(
            () =>
            {           
                foreach (Character character in Characters)
                {
                    int k = 0;
                    if (character.OnOfCharacter == true)
                    {
                        foreach (Initiative initiative in Initiatives) 
                        {
                            if (character.Name == initiative.Name)
                            {
                                k = 1;
                                break;
                            }
                        }
                        if (k == 1)
                            continue;
                        EditableI = new()
                        {
                            Name = character.Name,
                            DexterityModifier = character.Dexterity / 10,
                            Wounds = character.Wounds,
                            Importancenitiative = 0,
                            СurrentWounds = character.Wounds
                        };
                        Initiatives.Add(EditableI);
                        db.SaveChanges();
                    }
                }
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
