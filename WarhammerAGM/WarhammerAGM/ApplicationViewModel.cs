using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using ViewModels;
using WarhammerAGM.Models;
using Xceed.Wpf.Toolkit;

namespace WarhammerAGM
{
    public class ApplicationViewModel : ViewModelBase
    {
        private readonly ApplicationContext db = new ApplicationContext();

        public ObservableCollection<BestiaryCreature> BestiaryCreatures { get; }

        public BestiaryCreature BestiaryCreature
        {
            get => Get<BestiaryCreature>()!;
            private set => Set(value ?? throw new ArgumentNullException(nameof(value)));
        }
       private BestiaryCreature _selectedItem;

        public BestiaryCreature? SelectedItem
        {
            get => _selectedItem; 
            set => Set(ref _selectedItem, value);
        }

        protected override void OnPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnPropertyChanged(propertyName, oldValue, newValue);

            if (propertyName == nameof(SelectedItem))
            {
                if (SelectedItem == null)
                { return; }
               //Присваивание BestiaryCreature данных выделенного элемента SelectedItem кроме Id
                BestiaryCreature = new()
                {
                    Name = SelectedItem.Name,
                    Description = SelectedItem.Description,
                    Melee = SelectedItem.Melee,
                    Ballistics = SelectedItem.Ballistics,
                    Power = SelectedItem.Power,
                    Endurance = SelectedItem.Endurance,
                    Dexterity = SelectedItem.Dexterity,
                    Intelligence = SelectedItem.Intelligence,
                    Perception = SelectedItem.Perception,
                    Willpower = SelectedItem.Willpower,
                    Partnership = SelectedItem.Partnership,
                    Wounds = SelectedItem.Wounds,
                    Speed = SelectedItem.Speed,
                    Skills = SelectedItem.Skills,
                    Talents = SelectedItem.Talents,
                    Armor = SelectedItem.Armor,
                    Weapons = SelectedItem.Weapons,
                    Equipment = SelectedItem.Equipment,
                    AdditionalFeatures = SelectedItem.AdditionalFeatures,
                    Features = SelectedItem.Features
                };
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
        // команда добавления
        public RelayCommand AddCommand => GetCommand(() =>
        {
            //Если был выделен уже элемент из списка ListView
            BestiaryCreature? bestiaryCreature = SelectedItem as BestiaryCreature;
            if (bestiaryCreature != null)
            {

                if (bestiaryCreature.Name == BestiaryCreature.Name)
                {
                    MessageBox.Show("Такое название уже существует");
                    return;
                }
                else
                {
                    db.BestiaryCreatures.Add(BestiaryCreature);
                    db.SaveChanges();
                    BestiaryCreature = new();
                    MessageBox.Show("Добавление прошло успешно");
                    SelectedItem = null; //отменяем веделение элемента ListView
                }
            }
            else //если не был выделен
            {
                db.BestiaryCreatures.Add(BestiaryCreature);
                db.SaveChanges();
                BestiaryCreature = new();
                MessageBox.Show("Добавление прошло успешно");
            }
            
        });
        // команда редактирования
        public RelayCommand EditCommand => GetCommand<BestiaryCreature>(selectedItem =>
        {
            // получаем выделенный объект
            BestiaryCreature? bestiaryCreature = selectedItem as BestiaryCreature;
            if (bestiaryCreature == null) return;
            bestiaryCreature.Name = BestiaryCreature.Name;
            bestiaryCreature.Description = BestiaryCreature.Description;
            bestiaryCreature.Melee = BestiaryCreature.Melee;
            bestiaryCreature.Ballistics = BestiaryCreature.Ballistics;
            bestiaryCreature.Power = BestiaryCreature.Power;
            bestiaryCreature.Endurance = BestiaryCreature.Endurance;
            bestiaryCreature.Dexterity = BestiaryCreature.Dexterity;
            bestiaryCreature.Intelligence = BestiaryCreature.Intelligence;
            bestiaryCreature.Perception = BestiaryCreature.Perception;
            bestiaryCreature.Willpower = BestiaryCreature.Willpower;
            bestiaryCreature.Partnership = BestiaryCreature.Partnership;
            bestiaryCreature.Wounds = BestiaryCreature.Wounds;
            bestiaryCreature.Speed = BestiaryCreature.Speed;
            bestiaryCreature.Skills = BestiaryCreature.Skills;
            bestiaryCreature.Talents = BestiaryCreature.Talents;
            bestiaryCreature.Armor = BestiaryCreature.Armor;
            bestiaryCreature.Weapons = BestiaryCreature.Weapons;
            bestiaryCreature.Equipment = BestiaryCreature.Equipment;
            bestiaryCreature.AdditionalFeatures = BestiaryCreature.AdditionalFeatures;
            bestiaryCreature.Features = BestiaryCreature.Features;

                db.Entry(bestiaryCreature).State = EntityState.Modified;
                db.SaveChanges();
            BestiaryCreature = new();
            SelectedItem = null; //отменяем веделение элемента ListView
        });
       
        // команда удаления
        public RelayCommand DeleteCommand => GetCommand<BestiaryCreature>(selectedItem =>
        {
            db.BestiaryCreatures.Remove(selectedItem);
            db.SaveChanges();
        });
    }
}
