using System;
using System.Threading.Tasks;
using System.Windows;
using SimpleInjector;
using WichtelGenerator.WPF.Pages;
using WichtelGenerator.WPF.ViewModels;

namespace WichtelGenerator.WPF
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Container Container { get; } = new Container();

        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
                var registerPages = RegisterSites();
                await registerPages;
                var registerCore = RegisterCore();
                await registerCore;

                Container.Verify();
                var mainWindow = Container.GetInstance<MainSite>();
                mainWindow.Show();

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
            await Task.CompletedTask;
        }

        private async Task RegisterSites()
        {
            Container.Register<MainSite>(Lifestyle.Singleton);
            Container.Register<MainSiteViewModel>(Lifestyle.Singleton);
            Container.Register<WelcomePage>(Lifestyle.Singleton);
            Container.Register<WelcomeViewModel>(Lifestyle.Singleton);
            Container.Register<StorageSettingPage>(Lifestyle.Singleton);
            Container.Register<StorageSettingViewModel>(Lifestyle.Singleton);
            Container.Register<AddUserPage>(Lifestyle.Singleton);
            Container.Register<AddUserViewModel>(Lifestyle.Singleton);
            Container.Register<ManageUserPage>(Lifestyle.Singleton);
            Container.Register<ManageUserViewModel>(Lifestyle.Singleton);
            Container.Register<ManageBlackListsPage>(Lifestyle.Singleton);
            Container.Register<ManageBlackListViewModel>(Lifestyle.Singleton);
            Container.Register<RufflePage>(Lifestyle.Singleton);
            Container.Register<RuffleViewModel>(Lifestyle.Singleton);
            Container.Register<SettingPage>(Lifestyle.Singleton);
            Container.Register<SettingViewModel>(Lifestyle.Singleton);
            await Task.CompletedTask;
        }
    }
}