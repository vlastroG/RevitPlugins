﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using dosymep;
using dosymep.Bim4Everyone;

using RevitLintelPlacement.Models;
using RevitLintelPlacement.ViewModels;
using RevitLintelPlacement.Views;

namespace RevitLintelPlacement {

    [Transaction(TransactionMode.Manual)]
    public class ViewLintelsCommand : BasePluginCommand {
        public ViewLintelsCommand() {
            PluginName = "Расстановщик перемычек";
        }
        
        protected override void Execute(UIApplication uiApplication) {
            var lintelsConfig = LintelsConfig.GetLintelsConfig();
            var revitRepository = new RevitRepository(uiApplication.Application, uiApplication.ActiveUIDocument.Document, lintelsConfig);
            var elementInfos = new ElementInfosViewModel(revitRepository);
            var lintelsView = new LintelCollectionViewModel(revitRepository, elementInfos);
            var view = new LintelsView() { DataContext = lintelsView};

            var helper = new WindowInteropHelper(view) { Owner = uiApplication.MainWindowHandle };
            view.Show();
        }
    }
}
