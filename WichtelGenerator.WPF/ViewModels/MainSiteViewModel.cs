using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.WPF.Commands;
using WichtelGenerator.WPF.Services;

// ReSharper disable MemberCanBePrivate.Global --> Xaml propertys have to be public

namespace WichtelGenerator.WPF.ViewModels
{
    public class MainSiteViewModel : BaseViewModel
    {
        private Page _addUserPage;
        private Page _manageBlackListPage;
        private Page _mangageUserPage;
        private Page _rufflePage;
        private Page _settingPage;

        public MainSiteViewModel(IConfigManager configManager)
        {
            ConfigManager = configManager;
            // SetPropertys();
        }

        public IViewLocatorService locator { get; set; } =
            DependencyContainer.Instance.GetInstance<IViewLocatorService>();

        private IConfigManager ConfigManager { get; }

        public EventHandler PageChangedEventHandler { get; set; }
        
        // public IAsyncCommand OnChangeTab { get; set; }

        public Page SettingPage
        {
            get => _settingPage;
            set => SetAndRaise(ref _settingPage, value);
        }

        public Page AddUserPage
        {
            get => _addUserPage;
            set => SetAndRaise(ref _addUserPage, value);
        }

        public Page MangageUserPage
        {
            get => _mangageUserPage;
            set => SetAndRaise(ref _mangageUserPage, value);
        }


        public Page ManageBlackListPage
        {
            get => _manageBlackListPage;
            set => SetAndRaise(ref _manageBlackListPage, value);
        }

        public Page RufflePage
        {
            get => _rufflePage;
            set => SetAndRaise(ref _rufflePage, value);
        }

        private void SetPages()
        {
            SettingPage = (Page) locator.GetViewFor<SettingViewModel>();
            AddUserPage = (Page) locator.GetViewFor<AddUserViewModel>();
            MangageUserPage = (Page) locator.GetViewFor<ManageUserViewModel>();
            ManageBlackListPage = (Page) locator.GetViewFor<ManageBlackListViewModel>();
            RufflePage = (Page) locator.GetViewFor<RuffleViewModel>();
        }
        
        public void SetPropertys()
        {
            SetPages();
            ConfigManager.ReadSettings(); //Doesnt need the Model in here --> Ignore return.
            InitCommands();
        }
        

        internal override void InitCommands()
        {
            // OnChangeTab = AsyncCommand.Create(InformThatPageHasChanged);
        }

        // private async Task InformThatPageHasChanged()
        // {
        //     //todo: Tabcontroll hat mühe ein passedes Event zu finden mit den Behevoirs....
        //     await Task.CompletedTask;
        //     PageChangedEventHandler.Invoke(this, EventArgs.Empty);
        // }
    }
}