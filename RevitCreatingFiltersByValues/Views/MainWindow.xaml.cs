﻿using System.Windows;

namespace RevitCreatingFiltersByValues.Views {
    public partial class MainWindow {
        public MainWindow() {
            InitializeComponent();
        }

        public override string PluginName => nameof(RevitCreatingFiltersByValues);
        public override string ProjectConfigName => nameof(MainWindow);

        private void ButtonOk_Click(object sender, RoutedEventArgs e) {
            DialogResult = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e) {
            DialogResult = false;
        }

        private void SelectAllCategories(object sender, RoutedEventArgs e) {
            categories.SelectAll();
        }
        private void UnselectAllCategories(object sender, RoutedEventArgs e) {
            categories.UnselectAll();
        }


        private void SelectAllValues(object sender, RoutedEventArgs e) {
            values.SelectAll();
        }
        private void UnselectAllValues(object sender, RoutedEventArgs e) {
            values.UnselectAll();
        }
    }
}