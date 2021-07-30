using System;
using RutschiSwiss.Helpers.WPF.Services;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.WPF.ViewModels;

namespace WichtelGenerator.WPF.Pages
{
    /// <summary>
    ///     Interaktionslogik für SettingPage.xaml
    /// </summary>
    public partial class SettingPage : IBindableView
    {
        public SettingPage(ConfigModel configModel, SettingViewModel settingViewModel)
        {
            ConfigModel = configModel;
            SettingViewModel = settingViewModel;
            InitializeComponent();

            PortTextBox.DataContext = SettingViewModel.ConfigModel;
            MailTextBox.DataContext = SettingViewModel.ConfigModel;
            SSLCheckBox.DataContext = SettingViewModel.ConfigModel;
            ServerTextBox.DataContext = SettingViewModel.ConfigModel;
            UserNameTextBox.DataContext = SettingViewModel.ConfigModel;
            FontSizeTextBox.DataContext = settingViewModel.ConfigModel;

            SaveSettingButton.Click += SettingViewModel.SaveSettings_OnClick;
            settingViewModel.SavedAsModelEventHandler += GetPWintoModel;
        }

        private ConfigModel ConfigModel { get; }
        private SettingViewModel SettingViewModel { get; }

        private void GetPWintoModel(object sender, EventArgs e)
        {
            //TODO: Sicherheitshalber noch einen Besseren Schutz einrichten. Vorallem soll es nicht in AppData als klartext vorkommen.
            ConfigModel.Passwort =
                PasswodTextBox.Password; //--- Muss hier geschehen, das im ViewModel nicht erreichbar.
        }
    }
}