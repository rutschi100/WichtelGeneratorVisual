using System;
using System.Threading.Tasks;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.SantaManaager;
using WichtelGenerator.WPF.Commands;

namespace WichtelGenerator.WPF.ViewModels
{
    public class SettingViewModel : BaseViewModel
    {
        public EventHandler<EventArgs> SavedAsModelEventHandler;
        private ConfigModel _configModel;


        public SettingViewModel(ConfigModel configModel, IConfigManager configManager, ISantaManager santaManager)
        {
            _configModel = configModel;
            ConfigManager = configManager;
            SantaManager = santaManager;
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
        
        public async Task SaveSettingsAsync()
        {
            await Task.CompletedTask;

            ConfigModel.SecretSantaModels = SantaManager.SecretSantaModels;
            //Todo: Bindings funktionieren! Passwort muss noch und kontrollieren ob save und load funktioneirt!
            ConfigManager.SaveSettings(ConfigModel);
            SavedAsModelEventHandler?.Invoke(this, EventArgs.Empty);
        }

        internal override void InitCommands()
        {
            OnSave = AsyncCommand.Create(para=>SaveSettingsAsync());
        }
    }
}