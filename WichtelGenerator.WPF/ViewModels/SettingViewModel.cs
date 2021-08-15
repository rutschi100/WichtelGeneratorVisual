using System;
using System.Threading.Tasks;
using System.Windows;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.SantaManaager;
using WichtelGenerator.WPF.Commands;
using WichtelGenerator.WPF.Pages;

namespace WichtelGenerator.WPF.ViewModels
{
    public class SettingViewModel : BaseViewModel
    {
        public EventHandler<EventArgs> SavedAsModelEventHandler;
        private ConfigModel _configModel;
        private string _password;


        public SettingViewModel(ConfigModel configModel, IConfigManager configManager, ISantaManager santaManager, SettingPage settingPage)
        {
            _configModel = configModel;
            ConfigManager = configManager;
            SantaManager = santaManager;
            SettingPage = settingPage;
            InitCommands();
        }

        //TODO: Aktueller Stand kopieren, um den nicht gespeicherter Stand wieder zu löschen
        //TODO: Wenn die Seite Verlassen wird, und es Änderungen hat, dann nachfragen, ob die Änderungen gespeichert werden sollen.

        // public ConfigModel ConfigModel { get;set setan };
        public ConfigModel ConfigModel
        {
            get => _configModel;
            set => SetAndRaise(ref _configModel, value);
        }

        private IConfigManager ConfigManager { get; }
        public IAsyncCommand OnSave { get; set; }

        private ISantaManager SantaManager { get; }
        private SettingPage SettingPage { get; }

        public string Password { get; set; }

        public async Task SaveSettingsAsync()
        {
            await Task.CompletedTask;

            try
            {
                ConfigModel.SecretSantaModels = SantaManager.SecretSantaModels;
                //Todo: Bindings funktionieren! kontrollieren ob save und load funktioneirt!

                ConfigModel.Passwort = SettingPage.PasswodTextBox.Password;
            
                ConfigManager.SaveSettings(ConfigModel);
                SavedAsModelEventHandler?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error! {e.Message}");
                throw;
            }
        }

        internal override void InitCommands()
        {
            OnSave = AsyncCommand.Create(para=>SaveSettingsAsync());
        }
    }
}