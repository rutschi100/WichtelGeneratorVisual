namespace WichtelGenerator.Core.Configuration
{
    public interface IConfigManager
    {
        public ConfigModel ConfigModel { get; set; }
        public ConfigModel Read();
        public void Write(ConfigModel configModel);
    }
}