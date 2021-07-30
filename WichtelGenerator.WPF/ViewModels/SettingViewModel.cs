using System;
using WichtelGenerator.Core.Configuration;

namespace WichtelGenerator.WPF.ViewModels
{
    public class SettingViewModel : BaseViewModel
    {
        public EventHandler<EventArgs> SavedAsModelEventHandler;

        public SettingViewModel(ConfigModel configModel)
        {
            ConfigModel = configModel;
        }

        //TODO: Aktueller Stand kopieren, um den nicht gespeicherter Stand wieder zu löschen
        //TODO: Wenn die Seite Verlassen wird, und es Änderungen hat, dann nachfragen, ob die Änderungen gespeichert werden sollen.

        public ConfigModel ConfigModel { get; set; }

        public void SaveSettings_OnClick(object sender, EventArgs e)
        {
            SavedAsModelEventHandler?.Invoke(this, EventArgs.Empty);
        }

        internal override void InitCommands()
        {
            throw new NotImplementedException();
        }
    }
}