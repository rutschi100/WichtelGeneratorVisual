using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Xps;
using SimpleInjector;
using TimMailLib;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.Lottery;
using WichtelGenerator.Core.Notification;
using WichtelGenerator.Core.SantaManager;
using WichtelGenerator.WPF.Pages;
using WichtelGenerator.WPF.Services;
using WichtelGenerator.WPF.ViewModels;

namespace WichtelGenerator.WPF
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            //todo: Logger einfügen!
            try
            {
                await RegisterPages();
                await RegisterModels();
                await RegisterCore();

                DependencyContainer.Instance.Verify();
                DependencyContainer.Instance.GetInstance<MainSiteViewModel>().SetPropertys();
                
                var locator = DependencyContainer.Instance.GetInstance<IViewLocatorService>();
                var startView =
                    (Window) locator
                        .GetViewFor<MainSiteViewModel>(); //--- Muss von jedem existieren, damit der Context gesetzt wird!!!

                
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
            DependencyContainer.Instance.Register<ISantaManager, SantaManager>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<ILotteryService, LotteryService>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<INotificationManager, NotificationManager>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<INotificationMail, NotificationMail>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<IConfigManager, ConfigManager>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<IMailSender, MailSender>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<MailSettings>(Lifestyle.Singleton);
            await Task.CompletedTask;
        }

        private async Task RegisterPages()
        {
            DependencyContainer.Instance.Register<MainSite>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<WelcomePage>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<AddUserPage>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<ManageUserPage>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<ManageBlackListsPage>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<RufflePage>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<SettingPage>(Lifestyle.Singleton);
            // Nicht vergessen im ViewLocatorService zu registrieren!
            await Task.CompletedTask;
        }

        private async Task RegisterModels()
        {
            await Task.CompletedTask;
            DependencyContainer.Instance.Register<MainSiteViewModel>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<WelcomeViewModel>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<AddUserViewModel>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<ManageUserViewModel>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<ManageBlackListViewModel>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<RuffleViewModel>(Lifestyle.Singleton);
            DependencyContainer.Instance.Register<SettingViewModel>(Lifestyle.Singleton);
        }
    }
}