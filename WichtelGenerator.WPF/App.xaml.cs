﻿using System;
using System.Threading.Tasks;
using System.Windows;
using RutschiSwiss.Helpers.WPF.Services;
using SimpleInjector;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.WPF.Pages;
using WichtelGenerator.WPF.ViewModels;
using DependencyContainer = WichtelGenerator.WPF.Services.DependencyContainer;
using IViewLocatorService = WichtelGenerator.WPF.Services.IViewLocatorService;
using ViewLocatorService = WichtelGenerator.WPF.Services.ViewLocatorService;

namespace WichtelGenerator.WPF
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
                var registerPages = RegisterSites();
                await registerPages;
                var registerCore = RegisterCore();
                await registerCore;

                DependencyContainer.Instance.Verify();
                var locator = DependencyContainer.Instance.GetInstance<IViewLocatorService>();
                var startView = (Window) locator.GetViewFor<MainSiteViewModel>();
                
                // var mainWindow = DependencyContainer.Instance.GetInstance<MainSite>();
                startView.Show();

                base.OnStartup(e);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                throw;
            }
        }

        private async Task RegisterCore()
        {
            DependencyContainer.Instance.Register<ConfigModel>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<IViewLocatorService, ViewLocatorService>(Lifestyle.Singleton);
            await Task.CompletedTask;
        }

        private async Task RegisterSites()
        {
            DependencyContainer.Instance.Register<MainSite>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<MainSiteViewModel>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<WelcomePage>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<WelcomeViewModel>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<StorageSettingPage>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<StorageSettingViewModel>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<AddUserPage>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<AddUserViewModel>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<ManageUserPage>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<ManageUserViewModel>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<ManageBlackListsPage>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<ManageBlackListViewModel>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<RufflePage>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<RuffleViewModel>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<SettingPage>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<SettingViewModel>(Lifestyle.Singleton);
            await Task.CompletedTask;
        }
    }
}