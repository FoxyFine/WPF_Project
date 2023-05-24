using Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Windows;
using ViewModels;
using WarhammerAGM.Models;

namespace WarhammerAGM
{
    public partial class ApplicationViewModel
    {
        public ViewModeC ModeC { get => Get<ViewModeC>(); private set => Set(value); }

        public RelayCommand UpdateC => GetCommand
        (
            () =>
            {
                EditableC = SelectedC!.Create<Character>();
                ModeC = ViewModeC.UpdateC;
            },
            () => SelectedC is not null
        );
        public RelayCommand AddC => GetCommand
        (
            () =>
            {
                EditableC = new Character();
                ModeC = ViewModeC.AddC;
            }
        );
        public RelayCommand CloneC => GetCommand
        (
            () =>
            {
                EditableC = SelectedC!.Create<Character>();
                EditableC.Id = 0;
                ModeC = ViewModeC.AddC;
            },
            () => SelectedC is not null
        );
        public RelayCommand ExitC => GetCommand
        (
            () =>
            {
                ModeC = ViewModeC.ViewC;
                SelectedC = null;
            }
        );
        public RelayCommand SaveC => GetCommand
        (
            () =>
            {
                if (ModeC == ViewModeC.AddC)
                {
                    if (db.Characters.Find(EditableC.Id) is not null)
                    {
                        MessageBox.Show("Такое Id уже существует");
                        return;
                    }
                    else
                    {
                        string nametrim = EditableC.Name.Trim();
                        if (nametrim == string.Empty || EditableC.Name == "")
                        {
                            EditableC.Name = "";
                            MessageBox.Show("Введите корректное название");
                            return;
                        }
                        EntityEntry<Character> entry = db.Characters.Add(EditableC);
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            entry.State = EntityState.Detached;
                            MessageBox.Show("Такое название уже существует");
                            return;
                        }
                    }
                }
                else if (ModeC == ViewModeC.UpdateC)
                {
                    int index = Characters.TakeWhile(bc => bc.Id != EditableC.Id).Count();
                    if (index < 0)
                    {
                        MessageBox.Show("Такого Id не существует");
                        return;
                    }
                    else
                    {
                        string nametrim = EditableC.Name.Trim();
                        if (nametrim == string.Empty || EditableC.Name == "")
                        {
                            EditableC.Name = "";
                            MessageBox.Show("Введите корректное название");
                            return;
                        }
                        Character bestCrOld = Characters[index];
                        try
                        {
                            Characters[index] = EditableC;
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Characters[index] = bestCrOld;
                            MessageBox.Show("Такое название уже существует");
                            return;
                        }
                    }
                }
                SelectedC = null;
                MessageBox.Show("Сохранение прошло успешно");              
                ModeC = ViewModeC.ViewC;
            }
        );
        public enum ViewModeC { ViewC, UpdateC, AddC }
    }
}
