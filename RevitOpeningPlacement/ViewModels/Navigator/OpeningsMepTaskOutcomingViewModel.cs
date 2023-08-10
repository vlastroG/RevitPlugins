﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;

using Autodesk.Revit.DB;

using dosymep.WPF.Commands;
using dosymep.WPF.ViewModels;

using RevitOpeningPlacement.Models;

namespace RevitOpeningPlacement.ViewModels.Navigator {
    /// <summary>
    /// Модель представления окна для просмотра исходящих заданий на отверстия в файле инженера
    /// </summary>
    internal class OpeningsMepTaskOutcomingViewModel : BaseViewModel {
        private readonly RevitRepository _revitRepository;
        private OpeningMepTaskOutcomingViewModel _selectedOpeningMepTaskOutcoming;


        public OpeningsMepTaskOutcomingViewModel(RevitRepository revitRepository, ICollection<OpeningMepTaskOutcomingViewModel> openingsMepTasksOutcoming) {
            if(revitRepository is null) {
                throw new ArgumentNullException(nameof(revitRepository));
            }
            if(openingsMepTasksOutcoming is null) {
                throw new ArgumentNullException(nameof(openingsMepTasksOutcoming));
            }

            _revitRepository = revitRepository;

            OpeningsMepTaskOutcoming = new ObservableCollection<OpeningMepTaskOutcomingViewModel>(openingsMepTasksOutcoming);
            OpeningsMepTasksOutcomingViewSource = new CollectionViewSource() { Source = OpeningsMepTaskOutcoming };

            SelectCommand = new RelayCommand(SelectElement);
            SelectionChangedCommand = new RelayCommand(SelectionChanged, CanSelect);
            RenewCommand = new RelayCommand(Renew);
        }


        public ObservableCollection<OpeningMepTaskOutcomingViewModel> OpeningsMepTaskOutcoming { get; }

        public CollectionViewSource OpeningsMepTasksOutcomingViewSource { get; private set; }

        public OpeningMepTaskOutcomingViewModel SelectedOpeningMepTaskOutcoming {
            get => _selectedOpeningMepTaskOutcoming;
            set => RaiseAndSetIfChanged(ref _selectedOpeningMepTaskOutcoming, value);
        }


        public ICommand SelectCommand { get; }

        public ICommand SelectionChangedCommand { get; }

        public ICommand RenewCommand { get; }


        private void SelectElement(object p) {
            if(!(p is OpeningMepTaskOutcomingViewModel opening)) { return; }
            var elements = new Element[] { opening.GetFamilyInstance() };
            _revitRepository.SelectAndShowElement(elements);
        }

        private void SelectionChanged(object p) {
            if(OpeningsMepTasksOutcomingViewSource.View.CurrentPosition > -1
                && OpeningsMepTasksOutcomingViewSource.View.CurrentPosition < OpeningsMepTaskOutcoming.Count) {
                SelectElement((OpeningMepTaskOutcomingViewModel) p);
            }
        }

        private bool CanSelect(object p) {
            return p is OpeningMepTaskOutcomingViewModel;
        }

        private void Renew(object p) {
            Action action = () => {
                var command = new GetOpeningTaskCommand();
                command.ExecuteCommand(_revitRepository.UIApplication);
            };
            _revitRepository.DoAction(action);
        }
    }
}