using System.Threading.Tasks;
using System.Windows.Controls;
using WichtelGenerator.WPF.Commands;
using WichtelGenerator.WPF.Pages;
using WichtelGenerator.WPF.Services;

namespace WichtelGenerator.WPF.ViewModels
{
    public class MainSiteViewModel : BaseViewModel
    {
        private Page _savePage; // = (Page) locator.GetViewFor<StorageSettingViewModel>();// = (Page) DependencyContainer.Instance.GetInstance<IViewLocatorService>()

        // .GetViewFor<StorageSettingViewModel>();
        private Page _settingPage = DependencyContainer.Instance.GetInstance<SettingPage>();
        private Page _addUserPage = DependencyContainer.Instance.GetInstance<AddUserPage>();
        private Page _mangageUserPage = DependencyContainer.Instance.GetInstance<ManageUserPage>();
        private Page _manageBlackListPage = DependencyContainer.Instance.GetInstance<ManageUserPage>();
        private Page _rufflePage = DependencyContainer.Instance.GetInstance<RufflePage>();
        private string _testtest;

        private void SetPropertys()
        {
            SavePage = (Page) locator.GetViewFor<StorageSettingViewModel>();
        }


        public MainSiteViewModel()
        {
            SetPropertys();
        }

        public IViewLocatorService locator { get; set; } =
            DependencyContainer.Instance.GetInstance<IViewLocatorService>();

        private async Task SetPage(Page toLoad, Page toSet)
        {
            await Task.CompletedTask;
            toLoad = toSet;
        }


        public IAsyncCommand OnSaveTab { get; set; }
        // public IAsyncCommand OnSettingTab { get; set; }
        // public IAsyncCommand OnAddUserTab { get; set; }
        // public IAsyncCommand OnManageUserTab { get; set; }
        // public IAsyncCommand OnManageBlackListTab { get; set; }
        // public IAsyncCommand OnRuffleTab { get; set; }

        public string testtest
        {
            get => _testtest;
            set => SetAndRaise(ref _testtest, value);
        }

        public Page SavePage

        {
            get => _savePage;
            set => SetAndRaise(ref _savePage, value);
        }

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

        internal sealed override void InitCommands()
        {
            OnSaveTab = AsyncCommand.Create(p => SetPage(SavePage, (Page) DependencyContainer.Instance
                .GetInstance<IViewLocatorService>()
                .GetViewFor<StorageSettingViewModel>()));
            /*
             * SavePage = (Page) DependencyContainer.Instance.GetInstance<IViewLocatorService>()
                .GetViewFor<StorageSettingViewModel>();
             * 
             */
        }
    }
}