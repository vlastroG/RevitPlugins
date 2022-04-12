﻿using System.Windows.Input;

using dosymep.WPF.ViewModels;

namespace RevitWindowGapPlacement.ViewModels {
    internal class SettingsWindowGapsViewModel : BaseViewModel {
        private string _windowTitle;
        
        public ICommand PerformWindowCommand { get; }

        public string WindowTitle {
            get => _windowTitle;
            set => this.RaiseAndSetIfChanged(ref _windowTitle, value);
        }
    }
}