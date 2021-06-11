using System.Windows;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.WPF.Pages;
using WichtelGenerator.WPF.ViewModels;

namespace WichtelGenerator.WPF
{
    /// <summary>
    ///     Interaktionslogik für MainSite.xaml
    /// </summary>
    public partial class MainSite : Window
    {
        public MainSite(MainSiteViewModel mainSiteViewModel, WelcomePage welcomePage,
            StorageSettingPage storageSettingPage, AddUserPage addUserPage, ManageUserPage manageUserPage,
            ManageBlackListsPage manageBlackListsPage, RufflePage rufflePage, SettingPage settingPage)
        {
            MainSiteViewModel = mainSiteViewModel;
            WelcomePage = welcomePage;
            StorageSettingPage = storageSettingPage;
            AddUserPage = addUserPage;
            ManageUserPage = manageUserPage;
            ManageBlackListsPage = manageBlackListsPage;
            RufflePage = rufflePage;
            SettingPage = settingPage;

            InitializeComponent();

            WorkPlace.Content = WelcomePage;
            //WorkPlace.DataContext = ConfigModel; // FontSize hat bisher keine Wirkung!

            //MainWindow.DataContext = ConfigModel; // Test Kommentar


            #region Tab Controll Settings

            SaveTab.Content = StorageSettingPage;
            SettingTab.Content = SettingPage;
            AddUserTab.Content = AddUserPage;
            ManageUserTab.Content = ManageUserPage;
            ManageBlackListTab.Content = ManageBlackListTab;
            RuffleTab.Content = RufflePage;

            #endregion
        }

        public MainSiteViewModel MainSiteViewModel { get; set; }

        //TODO: Universelle Schriftgrösse in den Einstellungen ermöglichen!
        public ConfigModel ConfigModel { get; set; } = new ConfigModel();

        #region Pages

        public WelcomePage WelcomePage { get; set; }
        public StorageSettingPage StorageSettingPage { get; set; }
        public SettingPage SettingPage { get; set; }
        public AddUserPage AddUserPage { get; set; }
        public ManageUserPage ManageUserPage { get; set; }
        public ManageBlackListsPage ManageBlackListsPage { get; set; }
        public RufflePage RufflePage { get; set; }

        #endregion
    }
}