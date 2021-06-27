using SimpleInjector;

namespace WichtelGenerator.WPF.Services
{
    public static class DependencyContainer
    {
        public static Container Instance { get; set; } = new Container();
    }
}