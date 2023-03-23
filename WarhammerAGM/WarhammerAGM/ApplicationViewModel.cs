using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WarhammerAGM.Models;
using Xceed.Wpf.Toolkit;

namespace WarhammerAGM
{
    public class ApplicationViewModel
    {
        ApplicationContext db = new ApplicationContext();
        RelayCommand? addCommand;
        RelayCommand? editCommand;
        RelayCommand? deleteCommand;
        public ObservableCollection<BestiaryCreature> BestiaryCreatures { get; set; }

        private BestiaryCreature _bestiaryCreature = new();
        public BestiaryCreature BestiaryCreature
        {
            get => _bestiaryCreature;
            private set
            {
                _bestiaryCreature = value;
                OnPropertyChanged("BestiaryCreature");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public ApplicationViewModel()
        {
            /*В конструкторе класса загружаем данные из бд в локальный кэш*/
            db.Database.EnsureCreated();
            db.BestiaryCreatures.Load();
            BestiaryCreatures = db.BestiaryCreatures.Local.ToObservableCollection();
        }  
        // команда добавления
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand((o) =>
                  {

                      db.BestiaryCreatures.Add(BestiaryCreature);
                      db.SaveChanges();
                      BestiaryCreature = new BestiaryCreature();
                      MessageBox.Show("Добавление прошло успешно");
                  }));
            }
        }
       /*private void ClearSpecFields()
        {
            if (BestiaryCreature.Name != null || BestiaryCreature.Description != null || BestiaryCreature.Melee != null || BestiaryCreature.Ballistics != null || BestiaryCreature.Power != null
                || BestiaryCreature.Endurance != null || BestiaryCreature.Dexterity != null || BestiaryCreature.Intelligence != null || BestiaryCreature.Perception != null || BestiaryCreature.Willpower != null || BestiaryCreature.Partnership != null || BestiaryCreature.Wounds != null
                || BestiaryCreature.Speed != null || BestiaryCreature.Skills != null || BestiaryCreature.Talents != null || BestiaryCreature.Armor != null || BestiaryCreature.Weapons != null || BestiaryCreature.Equipment != null || BestiaryCreature.Features != null || BestiaryCreature.AdditionalFeatures != null)
            {
                BestiaryCreature.Name = string.Empty;
                BestiaryCreature.Description = string.Empty;
                BestiaryCreature.Melee = null;
                BestiaryCreature.Ballistics = null;
                BestiaryCreature.Power = null;
                BestiaryCreature.Endurance = null;
                BestiaryCreature.Dexterity = null;
                BestiaryCreature.Intelligence = null;
                BestiaryCreature.Perception = null;
                BestiaryCreature.Willpower = null;
                BestiaryCreature.Partnership = null;
                BestiaryCreature.Wounds = null;
                BestiaryCreature.Speed = string.Empty;
                BestiaryCreature.Skills = string.Empty;
                BestiaryCreature.Talents = string.Empty;
                BestiaryCreature.Armor = string.Empty;
                BestiaryCreature.Weapons = string.Empty;
                BestiaryCreature.Equipment = string.Empty;
                BestiaryCreature.Features = string.Empty;
                BestiaryCreature.AdditionalFeatures = string.Empty;
            }
        }*/
        // команда редактирования
        /*public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??
                  (editCommand = new RelayCommand((selectedItem) =>
                  {
                      // получаем выделенный объект
                      BestiaryCreature? bestiaryCreature = selectedItem as BestiaryCreature;
                      if (bestiaryCreature == null) return;

                      BestiaryCreature vm = new BestiaryCreature
                      {
                          Id = bestiaryCreature.Id,
                          Name = bestiaryCreature.Name,
                          Description = bestiaryCreature.Description,

                      };
                      bestiaryCreature.Name = userWindow.User.Name;
                      bestiaryCreature.Description = userWindow.User.Age;
                          db.Entry(user).State = EntityState.Modified;
                          db.SaveChanges();
                  }));
            }
        }
       */
        // команда удаления
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                  (deleteCommand = new RelayCommand((selectedItem) =>
                  {
                      // получаем выделенный объект
                      BestiaryCreature? bestiaryCreature = selectedItem as BestiaryCreature;
                      if (bestiaryCreature == null) return;
                      db.BestiaryCreatures.Remove(bestiaryCreature);
                      db.SaveChanges();
                  }));
            }
        }

        public object? Wounds { get; private set; }
    }
}
