﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using dosymep.WPF.ViewModels;

using RevitClashDetective.Models;

namespace RevitClashDetective.ViewModels.FilterCreatorViewModels {
    internal class CategoriesInfoViewModel : BaseViewModel {
        private readonly RevitRepository _revitRepository;
        private ObservableCollection<CategoryViewModel> _categories;
        private ObservableCollection<ParameterViewModel> _parameters;

        public CategoriesInfoViewModel(RevitRepository revitRepository, IEnumerable<CategoryViewModel> categories) {
            _revitRepository = revitRepository;
            Categories = new ObservableCollection<CategoryViewModel>(categories);
            InitializeParameters();
        }

        public ObservableCollection<CategoryViewModel> Categories {
            get => _categories;
            set => this.RaiseAndSetIfChanged(ref _categories, value);
        }


        public ObservableCollection<ParameterViewModel> Parameters { 
            get => _parameters; 
            set => this.RaiseAndSetIfChanged(ref _parameters, value); 
        }

        public override bool Equals(object obj) {
            return obj is CategoriesInfoViewModel model &&
                   EqualityComparer<ObservableCollection<CategoryViewModel>>.Default.Equals(Categories, model.Categories) &&
                   EqualityComparer<ObservableCollection<ParameterViewModel>>.Default.Equals(Parameters, model.Parameters);
        }

        public override int GetHashCode() {
            int hashCode = 331271310;
            hashCode = hashCode * -1521134295 + EqualityComparer<ObservableCollection<CategoryViewModel>>.Default.GetHashCode(Categories);
            hashCode = hashCode * -1521134295 + EqualityComparer<ObservableCollection<ParameterViewModel>>.Default.GetHashCode(Parameters);
            return hashCode;
        }

        public void InitializeParameters() {
            Parameters = new ObservableCollection<ParameterViewModel>(
                _revitRepository.GetParameters(Categories.Select(item=>item.Category))
                .OrderBy(item=>item.Name)
                .Select(item => new ParameterViewModel(item.Name)));
        }
    }
}
