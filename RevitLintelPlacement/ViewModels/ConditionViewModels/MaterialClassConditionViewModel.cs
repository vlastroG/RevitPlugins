﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using dosymep.WPF.ViewModels;

using RevitLintelPlacement.ViewModels.Interfaces;

namespace RevitLintelPlacement.ViewModels {
    internal class MaterialClassesConditionViewModel : BaseViewModel, IConditionViewModel {
        private ObservableCollection<MaterialClassConditionViewModel> _materialClassConditions;

        public ObservableCollection<MaterialClassConditionViewModel> MaterialClassConditions { 
            get => _materialClassConditions; 
            set => this.RaiseAndSetIfChanged(ref _materialClassConditions, value); 
        }
    }

    internal class MaterialClassConditionViewModel : BaseViewModel {
        private bool _isChecked;
        private string _name;

        public bool IsChecked {
            get => _isChecked;
            set => this.RaiseAndSetIfChanged(ref _isChecked, value);
        }

        public string Name { 
            get => _name; 
            set => this.RaiseAndSetIfChanged(ref _name, value); 
        }
    }
}
