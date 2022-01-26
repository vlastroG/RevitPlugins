﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

using Autodesk.Revit.DB;

using dosymep.Revit.Comparators;
using dosymep.WPF.Commands;
using dosymep.WPF.ViewModels;

using RevitSuperfilter.Models;
using RevitSuperfilter.Views;

namespace RevitSuperfilter.ViewModels {
    internal class CategoryViewModel : SelectableObjectViewModel<Category> {
        private ParamsView _paramsView;
        private ObservableCollection<ParametersViewModel> _parameters;

        private string _filterValue;
        private string _buttonFilterName;
        private bool _currentSelection = true;
        private ICollectionView _parametersView;

        public CategoryViewModel(Category category, IEnumerable<Element> elements)
            : base(category) {
            Category = category;
            Elements = new ObservableCollection<Element>(elements);
            
            SelectCommand = new RelayCommand(Select, CanSelect);
            ChangeCurrentSelection();
        }

        public Category Category { get; }
        public ObservableCollection<Element> Elements { get; }

        public ParamsView ParamsView {
            get {
                if(_paramsView == null) {
                    _paramsView = new ParamsView() { DataContext = this };
                }

                return _paramsView;
            }
        }

        public ICommand SelectCommand { get; }

        public ICollectionView ParametersView {
            get {
                if(_parameters == null) {
                    _parameters = new ObservableCollection<ParametersViewModel>(GetParamsViewModel().Where(item => item.Count > 1));
                    foreach(ParametersViewModel item in _parameters) {
                        item.PropertyChanged += ParametersViewModelPropertyChanged;
                    }

                    _parametersView = CollectionViewSource.GetDefaultView(_parameters);
                    _parametersView.Filter = item => Filter(item as ParametersViewModel);
                }

                return _parametersView;
            }
        }

        public override string DisplayData {
            get => Category?.Name ?? "Без категории";
        }

        public int Count {
            get => Elements.Count - Elements.OfType<ElementType>().Count();
        }

        public IEnumerable<Element> GetSelectedElements() {
            if(IsSelected == true) {
                return Elements;
            }

            if(IsSelected == null) {
                return _parameters.SelectMany(item => item.GetSelectedElements());
            }

            return Enumerable.Empty<Element>();
        }

        private IEnumerable<ParametersViewModel> GetParamsViewModel() {
            return Elements
                .SelectMany(element => element.GetOrderedParameters().Where(param => param.HasValue))
                .GroupBy(param => param, new ParamComparer())
                .Select(param => new ParametersViewModel(param.Key.Definition, param))
                .OrderBy(param => param.DisplayData);
        }

        private void ParametersViewModelPropertyChanged(object sender, PropertyChangedEventArgs e) {
            if(e.PropertyName.Equals(nameof(ParameterViewModel.IsSelected))) {
                UpdateSelection(_parameters);
            }
        }

        #region Filter

        public string FilterValue {
            get => _filterValue;
            set {
                this.RaiseAndSetIfChanged(ref _filterValue, value);
                ParametersView.Refresh();
            }
        }

        public string ButtonFilterName {
            get => _buttonFilterName;
            set => this.RaiseAndSetIfChanged(ref _buttonFilterName, value);
        }

        private bool Filter(ParametersViewModel param) {
            if(string.IsNullOrEmpty(FilterValue)) {
                return true;
            }

            return param.DisplayData.IndexOf(FilterValue, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        #endregion

        #region SelectCommand

        private void Select(object p) {
            ChangeCurrentSelection();

            IEnumerable<ParametersViewModel> @params = ParametersView.OfType<ParametersViewModel>();
            foreach(ParametersViewModel param in @params) {
                param.IsSelected = _currentSelection;
            }
        }

        private bool CanSelect(object p) {
            return ParametersView.CanFilter;
        }

        private void ChangeCurrentSelection() {
            _currentSelection = !_currentSelection;
            ButtonFilterName = _currentSelection ? "Убрать выделение" : "Выделить всё";
        }

        #endregion
    }
}
