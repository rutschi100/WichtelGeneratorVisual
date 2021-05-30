using System.Windows;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.WPF.Pages;

namespace WichtelGenerator.WPF
{
    /// <summary>
    ///     Interaktionslogik für MainSite.xaml
    /// </summary>
    public partial class MainSite : Window
    {
        public MainSite()
        {
            InitializeComponent();

            WorkPlace.Content = new WelcomePage();
            WorkPlace.DataContext = ConfigModel; // FontSize hat bisher keine Wirkung!

            MainWindow.DataContext = ConfigModel; // Test Kommentar
        }

        //TODO: Universelle Schriftgrösse in den Einstellungen ermöglichen!
        public ConfigModel ConfigModel { get; set; } = new ConfigModel();


        //TODO: Alle Pages fertigstellen
        private void StorageSetting_OnClick(object sender, RoutedEventArgs e)
        {
            WorkPlace.Content = new StorageSettingPage();
        }

        private void StartSetting_OnClick(object sender, RoutedEventArgs e)
        {
            WorkPlace.Content = new SettingPage();
        }

        private void StartAddUserPage_OnClick(object sender, RoutedEventArgs e)
        {
            WorkPlace.Content = new AddUserPage();
        }

        private void StartManageUserPage_OnClick(object sender, RoutedEventArgs e)
        {
            WorkPlace.Content = new ManageUserPage();
        }

        private void StartManageBlackListPage_OnClick(object sender, RoutedEventArgs e)
        {
            WorkPlace.Content = new ManageBlackListsPage();
        }

        private void StartRufflePage_OnCklick(object sender, RoutedEventArgs e)
        {
            WorkPlace.Content = new RufflePage();
        }
    }
}