using System;

namespace WichtelGenerator.Core.Configuration
{
    public interface IConfigManager
    {
        public ConfigModel ConfigModel { get; set; }
        public ConfigModel ReadSettings();
        public void SaveSettings(ConfigModel configModel);
        public EventHandler OnSaveEventHandler { get; set; }
        public EventHandler OnReadEventHandler { get; set; }
    }
}