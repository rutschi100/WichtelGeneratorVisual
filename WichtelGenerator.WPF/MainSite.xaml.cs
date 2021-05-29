using System.Windows;
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
        }


        private void StorageSetting_OnClick(object sender, RoutedEventArgs e)
        {
            WorkPlace.Content = new StorageSettingPage();
        }
    }
}