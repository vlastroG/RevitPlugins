﻿using System.Windows;

namespace PlatformSettings.Views {
    public partial class MainWindow {
        public MainWindow() {
            InitializeComponent();
        }
        
        public override string PluginName => nameof(PlatformSettings);
        public override string ProjectConfigName => nameof(MainWindow);
        
        private void ButtonOk_OnClick(object sender, RoutedEventArgs e) {
            DialogResult = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e) {
            DialogResult = false;
        }
    }
}