﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using DevExpress.XtraScheduler.Outlook.Interop;

using dosymep;
using dosymep.Bim4Everyone;
using dosymep.SimpleServices;
using dosymep.WPF.ViewModels;

using Ninject;

using pyRevitLabs.Json.Serialization;

using RevitCheckingLevels.Models;
using RevitCheckingLevels.Services;
using RevitCheckingLevels.ViewModels;
using RevitCheckingLevels.Views;

using Application = Autodesk.Revit.ApplicationServices.Application;

namespace RevitCheckingLevels {
    [Transaction(TransactionMode.Manual)]
    public class RevitCheckingLevelsCommand : BasePluginCommand {
        public RevitCheckingLevelsCommand() {
            PluginName = "Проверка уровней";
        }

        protected override void Execute(UIApplication uiApplication) {
            using(IKernel kernel = new StandardKernel()) {
                kernel.Bind<UIApplication>()
                    .ToConstant(uiApplication);
                kernel.Bind<Application>()
                    .ToConstant(uiApplication.Application);

                kernel.Bind<RevitRepository>()
                    .ToSelf()
                    .InSingletonScope();

                kernel.Bind<MainViewModel>().ToSelf();
                kernel.Bind<CheckingLevelsViewModel>().ToSelf();
                kernel.Bind<CheckingLinkLevelsViewModel>().ToSelf();

                kernel.Bind<MainWindow>().ToSelf()
                    .WithPropertyValue(nameof(Window.Title), PluginName)
                    .WithPropertyValue(nameof(Window.DataContext),
                        c => c.Kernel.Get<MainViewModel>());

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
    }
}
