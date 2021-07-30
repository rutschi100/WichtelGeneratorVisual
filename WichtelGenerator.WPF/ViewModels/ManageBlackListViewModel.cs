using System;
using System.Collections.ObjectModel;
using System.Linq;
using SimpleInjector.Advanced;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.Models;
using WichtelGenerator.WPF.Services;

namespace WichtelGenerator.WPF.ViewModels
{
    internal class ManageBlackListViewModel : BaseViewModel
    {
        private ObservableCollection<string> _activeSantas = new ObservableCollection<string>();
        private string _selcectedSantaName;
        private SecretSantaModel _selectedSantaModel;

        public ManageBlackListViewModel(ConfigModel configModel, AddUserViewModel addUserViewModel)
        {
            ConfigModel = configModel;
            AddUserViewModel = addUserViewModel;

            Initialize();
        }

        public AddUserViewModel AddUserViewModel { get; set; }
        
        private void Initialize()
        {
            UpdateSantaList();
            AddUserViewModel.NewUserAddedEvent += UpdateSantaList;
        }

        private void UpdateSantaList(object sender, EventArgs e)
        {
            UpdateSantaList();
        }

        private void UpdateSantaList()
        {
            ActiveSantas ??= new ObservableCollection<string>();
            ActiveSantas.Clear();

            foreach (var oneSanta in ConfigModel.SecretSantaModels)
            {
                ActiveSantas.Add(oneSanta.Name);
            }
        }

        private ConfigModel ConfigModel { get; }

        public string SelcectedSantaName
        {
            get => _selcectedSantaName;
            set
            {
                SetAndRaise(ref _selcectedSantaName, value);
                var santa = ConfigModel.SecretSantaModels.FirstOrDefault(p => p.Name == value);
                SelectedSantaModel = santa;
            }
        }

        public SecretSantaModel SelectedSantaModel
        {
            get => _selectedSantaModel;
            set => SetAndRaise(ref _selectedSantaModel, value);
        }

        public ObservableCollection<string> ActiveSantas
        {
            get => _activeSantas;
            set => SetAndRaise(ref _activeSantas, value);
        }

        internal override void InitCommands()
        {
            throw new NotImplementedException();
        }
        
        
        //!!!!!!!  Evtl. Kann die gesamte Logik zurück ins Core. Mittels eines Enums kann dan die Validität gegeben werdne.
        private enum SantaBlackListWishResult
        {
            Valid,
            CombinationAlreadyExist,
            MaxValueAlreadyUsed
        }
        
        //todo: BlackList Management
        //todo: max Blacklist erstellen
        //todo: Kombination existenz überprüfen
    }
}