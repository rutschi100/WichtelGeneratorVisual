using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.Models;
using WichtelGenerator.WPF.Commands;

// ReSharper disable MemberCanBePrivate.Global

namespace WichtelGenerator.WPF.ViewModels
{
    public class AddUserViewModel : BaseViewModel
    {
        private ObservableCollection<string> _activeUsers;
        private string _mailAdres;
        private string _santaName;

        public AddUserViewModel(ConfigModel configModel)
        {
            ConfigModel = configModel;
            Initalize();
            InitCommands();
        }

        private ConfigModel ConfigModel { get; }
        public IAsyncCommand AddNewUser { get; set; }
        public EventHandler<EventArgs> NewUserAddedEvent { get; set; }
        
        public ObservableCollection<string> ActiveUsers
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

        private void Initalize()
        {
            if (ConfigModel.SecretSantaModels == null)
            {
                return;
            }

            ActiveUsers = GetAllActiveUserNames();
        }

        internal sealed override void InitCommands()
        {
            AddNewUser = AsyncCommand.Create(parameter => AddNewUserAsync());
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
            NewUserAddedEvent.Invoke(this, EventArgs.Empty);

            ActiveUsers = GetAllActiveUserNames();
        }

        //todo: whitelist von allen befüllen
        
        private ObservableCollection<string> GetAllActiveUserNames()
        {
            var names = new ObservableCollection<string>();
            foreach (var oneSanta in ConfigModel.SecretSantaModels)
            {
                names.Add(oneSanta.Name);
            }

            return names;
        }
    }
}