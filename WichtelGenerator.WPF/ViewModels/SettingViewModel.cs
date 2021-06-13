using System;
using WichtelGenerator.Core.Configuration;

namespace WichtelGenerator.WPF.ViewModels
{
    public class SettingViewModel
    {
        public EventHandler<EventArgs> SavedAsModelEventHandler;

        public SettingViewModel(ConfigModel configModel)
        {
            ConfigModel = configModel;
        }

        public ConfigModel ConfigModel { get; set; }

        public void SaveSettings_OnClick(object sender, EventArgs e)
        {
            SavedAsModelEventHandler?.Invoke(this, EventArgs.Empty);
        }
    }
}