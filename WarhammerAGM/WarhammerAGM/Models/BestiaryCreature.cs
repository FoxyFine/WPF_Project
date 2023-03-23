using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Windows.Controls;
using System.Windows.Markup;

namespace WarhammerAGM.Models
{
    [Index(nameof(Id), IsUnique = true)]
    public partial class BestiaryCreature : INotifyPropertyChanged
    {
        string? name;
        string? description;
        int? melee;
        int? ballistics;
        int? power;
        int? endurance;
        int? dexterity;
        int? intelligence;
        int? perception;
        int? willpower;
        int? partnership;
        int? wounds;
        string? speed;
        string? skills;
        string? talents;
        string? armor;
        string? weapons;
        string? equipment;
        string? additionalfeatures;
        string? features;
        [Key]
        public int Id {get; set;}
        public string? Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public string? Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        public int? Melee
        {
            get { return melee; }
            set
            {
                melee = value;
                OnPropertyChanged("Melee");
            }
        }

        public int? Ballistics
        {
            get { return ballistics; }
            set
            {
                ballistics = value;
                OnPropertyChanged("Ballistics");
            }
        }

        public int? Power
        {
            get { return power; }
            set
            {
                power = value;
                OnPropertyChanged("Power");
            }
        }

        public int? Endurance
        {
            get { return endurance; }
            set
            {
                endurance = value;
                OnPropertyChanged("Endurance");
            }
        }

        public int? Dexterity
        {
            get { return dexterity; }
            set
            {
                dexterity = value;
                OnPropertyChanged("Dexterity");
            }
        }

        public int? Intelligence
        {
            get { return intelligence; }
            set
            {
                intelligence = value;
                OnPropertyChanged("Intelligence");
            }
        }

        public int? Perception
        {
            get { return perception; }
            set
            {
                perception = value;
                OnPropertyChanged("Perception");
            }
        }

        public int? Willpower
        {
            get { return willpower; }
            set
            {
                willpower = value;
                OnPropertyChanged("Willpower");
            }
        }

        public int? Partnership
        {
            get { return partnership; }
            set
            {
                partnership = value;
                OnPropertyChanged("Partnership");
            }
        }

        public int? Wounds
        {
            get { return wounds; }
            set
            {
                wounds = value;
                OnPropertyChanged("Wounds");
            }
        }

        public string? Speed
        {
            get { return speed; }
            set
            {
                speed = value;
                OnPropertyChanged("Speed");
            }
        }

        public string? Skills
        {
            get { return skills; }
            set
            {
                skills = value;
                OnPropertyChanged("Skills");
            }
        }

        public string? Talents
        {
            get { return talents; }
            set
            {
                talents = value;
                OnPropertyChanged("Talents");
            }
        }

        public string? Armor
        {
            get { return armor; }
            set
            {
                armor = value;
                OnPropertyChanged("Armor");
            }
        }

        public string? Weapons
        {
            get { return weapons; }
            set
            {
                weapons = value;
                OnPropertyChanged("Weapons");
            }
        }

        public string? Equipment
        {
            get { return equipment; }
            set
            {
                equipment = value;
                OnPropertyChanged("Equipment");
            }
        }

        public string? AdditionalFeatures
        {
            get { return additionalfeatures; }
            set
            {
                additionalfeatures = value;
                OnPropertyChanged("AdditionalFeatures");
            }
        }

        public string? Features
        {
            get { return features; }
            set
            {
                features = value;
                OnPropertyChanged("Features");
            }
        }
        public BestiaryCreature()
        {
            Name = default(string);
            Melee = default(int);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}