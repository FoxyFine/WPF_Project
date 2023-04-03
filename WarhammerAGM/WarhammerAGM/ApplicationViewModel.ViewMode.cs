﻿using Mapping;
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
        public ViewMode Mode { get => Get<ViewMode>(); private set => Set(value); }

        public RelayCommand Update => GetCommand
        (
            () =>
            {
                EditableBC = SelectedBC!.Create<BestiaryCreature>();
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
                    if (db.BestiaryCreatures.Find(EditableBC.Id) is null)
                    {
                        MessageBox.Show("Такое Id уже существует");
                    }
                    else
                    {
                        EntityEntry<BestiaryCreature> entry = db.BestiaryCreatures.Add(EditableBC);
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
                else if (Mode == ViewMode.Update)
                {
                    int index = BestiaryCreatures.TakeWhile(bc => bc.Id != EditableBC.Id).Count();
                    if (index < 0)
                    {
                        MessageBox.Show("Такого Id не существует");
                    }
                    else
                    {
                        BestiaryCreature bestCrOld = BestiaryCreatures[index];
                        try
                        {
                            BestiaryCreatures[index] = EditableBC;
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            BestiaryCreatures[index] = bestCrOld;
                            MessageBox.Show("Такое название уже существует");
                            return;
                        }
                    }
                }
                MessageBox.Show("Сохранение прошло успешно");
                Mode = ViewMode.View;
            }
        );
    }

    public enum ViewMode { View, Update, Add }
}
