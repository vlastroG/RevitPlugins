﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using dosymep;
using dosymep.Bim4Everyone;
using dosymep.SimpleServices;

using Ninject;

using RevitEditingZones.Models;
using RevitEditingZones.Services;
using RevitEditingZones.ViewModels;
using RevitEditingZones.Views;

using Application = Autodesk.Revit.ApplicationServices.Application;

namespace RevitEditingZones {
    [Transaction(TransactionMode.Manual)]
    public class RevitEditingZonesCommand : BasePluginCommand {
        public RevitEditingZonesCommand() {
            PluginName = "Редактор зон СМР";
        }

        protected override void Execute(UIApplication uiApplication) {
            using(IKernel kernel = new StandardKernel()) {
                kernel.Bind<UIApplication>()
                    .ToConstant(uiApplication)
                    .InTransientScope();
                kernel.Bind<Application>()
                    .ToConstant(uiApplication.Application)
                    .InTransientScope();

                kernel.Bind<RevitRepository>()
                    .ToSelf()
                    .InSingletonScope();

                kernel.Bind<PluginConfig>()
                    .ToMethod(c => PluginConfig.GetPluginConfig());

                Func<LevelsWindow> levelsWindowFactory = () => kernel.Get<LevelsWindow>();
                kernel.Bind<Func<LevelsWindow>>()
                    .ToConstant(levelsWindowFactory);

                kernel.Bind<ILevelsWindowService>()
                    .To<LevelsWindowService>();
                kernel.Bind<LevelsWindow>().ToSelf()
                    .WithPropertyValue(nameof(Window.Owner), 
                        c => c.Kernel.Get<MainWindow>());

                kernel.Bind<MainViewModel>().ToSelf();
                kernel.Bind<MainWindow>().ToSelf()
                    .InSingletonScope()
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