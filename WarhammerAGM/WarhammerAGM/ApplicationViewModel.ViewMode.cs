using Mapping;
using System;
using System.Linq;
using System.Windows;
using ViewModels;
using WarhammerAGM.Models;

namespace WarhammerAGM
{
    public partial class ApplicationViewModel
    {
        BestiaryCreature bestCrOld; //создание локальной переменной используемой в редактировании
        public ViewMode Mode { get => Get<ViewMode>(); private set => Set(value); }

        public RelayCommand Update => GetCommand
        (
            () =>
            {
                EditableBC = SelectedBC!.Create<BestiaryCreature>();
                bestCrOld = SelectedBC;
                Mode = ViewMode.Update;
            },
            () => SelectedBC is not null
        );

        public RelayCommand Add => GetCommand
        (
            () =>
            {
                EditableBC = new BestiaryCreature();
                Mode = ViewMode.Add;
            }
        );

        public RelayCommand Clone => GetCommand
        (
            () =>
            {
                EditableBC = SelectedBC!.Create<BestiaryCreature>();
                EditableBC.Id = 0;
                Mode = ViewMode.Add;
            },
            () => SelectedBC is not null
        );

        public RelayCommand Exit => GetCommand
        (
            () =>
            {
                Mode = ViewMode.View;
                SelectedBC = null;
            } 

        );

        public RelayCommand Save => GetCommand
        (
            () => 
            {
                if (Mode == ViewMode.Add)
                {
                    try
                    {
                        db.BestiaryCreatures.Add(EditableBC);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        db.BestiaryCreatures.Remove(EditableBC);
                        MessageBox.Show("Такое название уже существует");
                        return;
                    }
                }
                else
                if(Mode == ViewMode.Update) {
                    var bestCr = EditableBC;
                    int index = BestiaryCreatures.TakeWhile(bc => bc.Id != bestCr.Id).Count();
                    try
                    {
                        BestiaryCreatures.RemoveAt(index);
                        BestiaryCreatures.Insert(index, bestCr);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        BestiaryCreatures.RemoveAt(index);
                        BestiaryCreatures.Insert(index, bestCrOld);
                        EditableBC = bestCrOld;
                        MessageBox.Show("Такое название уже существует");
                        return;
                    }
                }
                MessageBox.Show("Сохранение прошло успешно");
                Mode = ViewMode.View;
            }
        );
    }

    public enum ViewMode { View, Update, Add }
} 
