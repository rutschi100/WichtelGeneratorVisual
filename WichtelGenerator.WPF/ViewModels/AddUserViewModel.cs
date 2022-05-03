using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.Enums;
using WichtelGenerator.Core.Models;
using WichtelGenerator.Core.SantaManager;
using WichtelGenerator.WPF.Commands;

// ReSharper disable MemberCanBePrivate.Global

namespace WichtelGenerator.WPF.ViewModels
{
    public class AddUserViewModel : BaseViewModel
    {
        private ObservableCollection<string> _activeUsers;
        private string _mailAdres;
        private string _santaName;

        public AddUserViewModel(ConfigModel configModel, ISantaManager santaManager)
        {
            ConfigModel = configModel;
            SantaManager = santaManager;
            Initalize();
            InitCommands();
        }

        private ConfigModel ConfigModel { get; }
        public IAsyncCommand AddNewUser { get; set; }

        public ISantaManager SantaManager { get; }

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
            if (SantaManager.SecretSantaModels == null)
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

            //todo: spechern in DB nach Hinzufügen des Users
            
            switch (SantaManager.AddNewSanta(newSanta))
            {
                case AddUserResult.Done:
                    ActiveUsers = GetAllActiveUserNames();
                    break;
                case AddUserResult.SantaAllReadyExists:
                    MessageBox.Show("Der User Existierts bereits. Bitte Wähle einen neuen eindeutigen Namen");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private ObservableCollection<string> GetAllActiveUserNames()
        {
            var names = new ObservableCollection<string>();
            foreach (var oneSanta in SantaManager.SecretSantaModels)
            {
                names.Add(oneSanta.Name);
            }

            return names;
        }
    }
}