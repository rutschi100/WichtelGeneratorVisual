using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Medica.Corona.CostUnitManager.UI.Commands;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.Models;
using WichtelGenerator.WPF.Commands;

namespace WichtelGenerator.WPF.ViewModels
{
    
    //TODO: Binding des Commands funktioniert nicht!
    
    
    internal class AddUserViewModel : BaseViewModel
    {
        private string _santaName;
        private string _mailAdres;
        private ObservableCollection<SecretSantaModel> _activeUsers;

        public AddUserViewModel(ConfigModel configModel)
        {
            ConfigModel = configModel;
            Initalize();
            InitCommands();
        }

        private void Initalize()
        {
            if (ConfigModel.SecretSantaModels == null)
            {
                return;
            }
            ActiveUsers = new ObservableCollection<SecretSantaModel>(ConfigModel.SecretSantaModels);
        }

        private ConfigModel ConfigModel { get; }
        private IAsyncCommand AddUser { get; set; }

        public ObservableCollection<SecretSantaModel> ActiveUsers
        {
            get => _activeUsers;
            set => SetAndRaise(ref _activeUsers, value);
        }

        public string SantaName
        {
            get => _santaName;
            set => SetAndRaise(ref _santaName, value);
        }

        public string MailAdres
        {
            get => _mailAdres;
            set => SetAndRaise(ref _mailAdres, value);
        }


        internal sealed override void InitCommands()
        {
            AddUser = AsyncCommand.Create(parameter => AddNewUserAsync());
        }

        private async Task AddNewUserAsync()
        {
            await Task.CompletedTask;

            var newSanta = new SecretSantaModel
            {
                Name = SantaName
            };

            if (!string.IsNullOrEmpty(MailAdres))
            {
                newSanta.MailAdress = MailAdres;
            }

            ConfigModel.SecretSantaModels ??= new List<SecretSantaModel>();
            ConfigModel.SecretSantaModels.Add(newSanta);
            ActiveUsers = new ObservableCollection<SecretSantaModel>(ConfigModel.SecretSantaModels);
        }
        
    }
}