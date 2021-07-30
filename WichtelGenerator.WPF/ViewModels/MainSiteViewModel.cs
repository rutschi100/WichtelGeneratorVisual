using System;
using System.Windows.Controls;
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
        private Page _savePage;
        private Page _settingPage;
        private string _testtest;

        public MainSiteViewModel()
        {
            SetPropertys();
        }

        public IViewLocatorService locator { get; set; } =
            DependencyContainer.Instance.GetInstance<IViewLocatorService>();

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

        private void SetPropertys()
        {
            SavePage = (Page) locator.GetViewFor<StorageSettingViewModel>();
            SettingPage = (Page) locator.GetViewFor<SettingViewModel>();
            AddUserPage = (Page) locator.GetViewFor<AddUserViewModel>();
            MangageUserPage = (Page) locator.GetViewFor<ManageUserViewModel>();
            ManageBlackListPage = (Page) locator.GetViewFor<ManageBlackListViewModel>();
            RufflePage = (Page) locator.GetViewFor<RuffleViewModel>();
        }

        internal override void InitCommands()
        {
            throw new NotImplementedException();
        }
    }
}