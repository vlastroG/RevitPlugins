﻿using System.Collections.ObjectModel;
using System.Windows.Input;

using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;

using dosymep.WPF.ViewModels;

namespace RevitWindowGapPlacement.ViewModels {
    internal class NavigatorViewModel : BaseViewModel {
        private string _errorText;
        private string _windowTitle;

        public NavigatorViewModel() {
            AdditionCommands
                = new ObservableCollection<CommandViewModel>() {
                    new CommandViewModel() {
                        ToolTip = "Предыдущий элемент",
                        ImageSource =
                            "pack://application:,,,/DevExpress.Images.v21.2;component/Office2013/Arrows/Prev_32x32.png"
                    },
                    new CommandViewModel() {
                        ToolTip = "Следующий элемент",
                        ImageSource =
                            "pack://application:,,,/DevExpress.Images.v21.2;component/Office2013/Arrows/Next_32x32.png"
                    },
                };
        }

        public ICommand PerformWindowCommand { get; }
        public ObservableCollection<CommandViewModel> AdditionCommands { get; }

        public string ErrorText {
            get => _errorText;
            set => this.RaiseAndSetIfChanged(ref _errorText, value);
        }

        public string WindowTitle {
            get => _windowTitle;
            set => this.RaiseAndSetIfChanged(ref _windowTitle, value);
        }
    }


    internal class CommandViewModel : BaseViewModel {
        private string _toolTip;
        private ICommand _command;
        private string _imageSource;

        public string ToolTip {
            get => _toolTip;
            set => this.RaiseAndSetIfChanged(ref _toolTip, value);
        }

        public ICommand Command {
            get => _command;
            set => this.RaiseAndSetIfChanged(ref _command, value);
        }

        public string ImageSource {
            get => _imageSource;
            set => this.RaiseAndSetIfChanged(ref _imageSource, value);
        }
    }
}