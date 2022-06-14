﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using dosymep;
using dosymep.Bim4Everyone;

using RevitMarkPlacement.Models;
using RevitMarkPlacement.ViewModels;
using RevitMarkPlacement.Views;

namespace RevitMarkPlacement {

    [Transaction(TransactionMode.Manual)]
    public class UpdateAnnotationsCommand : BasePluginCommand {
        public UpdateAnnotationsCommand() {
            PluginName = "Расстановщик отметок";
        }

        protected override void Execute(UIApplication uiApplication) {
            var config = AnnotationsConfig.GetAnnotationsConfig();
            var revitRepository = new RevitRepository(uiApplication.Application, uiApplication.ActiveUIDocument.Document);

            var viewModel = new MainViewModel(revitRepository, config);
            if(!viewModel.CanPlaceAnnotation()) {
                var view = new ReportView() { DataContext = viewModel.InfoElementsViewModel };
                view.ShowDialog();
            } else {
                var marks = new TemplateLevelMarkCollection(revitRepository, new AllElementsSelection());
                marks.UpdateAnnotation();
            }
        }
    }
}
