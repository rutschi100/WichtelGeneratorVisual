using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.Logging;

namespace WichtelGenerator.WPF.Pages
{
    /// <summary>
    ///     Interaktionslogik für StorageSettingPage.xaml
    /// </summary>
    public partial class StorageSettingPage : Page
    {
        public StorageSettingPage()
        {
            InitializeComponent();

            const string noSettingMessage = "Keine Einstellung vorhanden!";
            Settings.Add(noSettingMessage);
            
            //SettingList.Items.Add(noSettingMessage);
        }

        public List<string> Settings { get; set; } = new List<string>();


        private void LoadSetting_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SelectSetting_OnSelect(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}