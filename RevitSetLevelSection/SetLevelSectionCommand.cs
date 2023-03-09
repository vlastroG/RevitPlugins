﻿using System;
using System.Windows;
using System.Windows.Interop;

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using dosymep;
using dosymep.Bim4Everyone;
using dosymep.Bim4Everyone.SharedParams;
using dosymep.Bim4Everyone.Templates;
using dosymep.SimpleServices;

using Ninject;

using RevitSetLevelSection.Models;
using RevitSetLevelSection.ViewModels;
using RevitSetLevelSection.Views;

using Application = Autodesk.Revit.ApplicationServices.Application;

namespace RevitSetLevelSection {
    [Transaction(TransactionMode.Manual)]
    public class SetLevelSectionCommand : BasePluginCommand {
        public SetLevelSectionCommand() {
            PluginName = "Назначение уровня/секции";
        }

        protected override void Execute(UIApplication uiApplication) {
            using(IKernel kernel = new StandardKernel()) {
                kernel.Bind<UIApplication>()
                    .ToConstant(uiApplication)
                    .InTransientScope();
                kernel.Bind<Application>()
                    .ToConstant(uiApplication.Application)
                    .InTransientScope();

                kernel.Bind<RevitRepository>().ToSelf()
                    .InSingletonScope();

                kernel.Bind<MainViewModel>().ToSelf();
                kernel.Bind<MainWindow>().ToSelf()
                    .WithPropertyValue(nameof(Window.Title), PluginName)
                    .WithPropertyValue(nameof(Window.DataContext),
                        c => c.Kernel.Get<MainViewModel>());

                UpdateParams(uiApplication);
                MainWindow window = kernel.Get<MainWindow>();
                if(window.ShowDialog() == true) {
                    GetPlatformService<INotificationService>()
                        .CreateNotification(PluginName, "Выполнение скрипта завершено успешно.", "C#")
                        .ShowAsync();
                } else {
                    GetPlatformService<INotificationService>()
                        .CreateWarningNotification(PluginName, "Выполнение скрипта отменено.")
                        .ShowAsync();
                }
            }
        }

        private static void UpdateParams(UIApplication uiApplication) {
            ProjectParameters projectParameters = ProjectParameters.Create(uiApplication.Application);
            projectParameters.SetupRevitParams(uiApplication.ActiveUIDocument.Document,
                SharedParamsConfig.Instance.BuildingWorksLevel,
                SharedParamsConfig.Instance.BuildingWorksBlock,
                SharedParamsConfig.Instance.BuildingWorksSection,
                SharedParamsConfig.Instance.BuildingWorksTyping,
                SharedParamsConfig.Instance.FixBuildingWorks);
        }
    }
}
