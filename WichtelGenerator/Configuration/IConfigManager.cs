namespace WichtelGenerator.Core.Configuration
{
    public interface IConfigManager
    {
        public string Read(string xPath);
        public string Write(string xPath, string value);
    }
}