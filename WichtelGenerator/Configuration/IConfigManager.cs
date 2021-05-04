namespace WichtelGenerator.Core.Configuration
{
    public interface IConfigManager
    {
        public ConfigModel Read();
        public void Write(ConfigModel configModel);
    }
}