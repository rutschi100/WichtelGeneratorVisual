using System;
using System.Collections.ObjectModel;
using System.Linq;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.Models;
using WichtelGenerator.Core.SantaManaager;

namespace WichtelGenerator.WPF.ViewModels
{
    internal class ManageBlackListViewModel : BaseViewModel
    {
        private ObservableCollection<string> _activeSantas = new ObservableCollection<string>();
        private string _selcectedSantaName;
        private SecretSantaModel _selectedSantaModel;

        public ManageBlackListViewModel(ConfigModel configModel, AddUserViewModel addUserViewModel,
            ISantaManager santaManager)
        {
            ConfigModel = configModel;
            AddUserViewModel = addUserViewModel;
            SantaManager = santaManager;

            Initialize();
        }

        private ISantaManager SantaManager { get; }
        public AddUserViewModel AddUserViewModel { get; set; }

        private ConfigModel ConfigModel { get; }

        public string SelcectedSantaName
        {
            get => _selcectedSantaName;
            set
            {
                SetAndRaise(ref _selcectedSantaName, value);
                var santa = SantaManager.SecretSantaModels.FirstOrDefault(p => p.Name == value);
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

        private void Initialize()
        {
            UpdateSantaList();
            SantaManager.NewUserAddedEvent += UpdateSantaList;
        }

        private void UpdateSantaList(object sender, EventArgs e)
        {
            UpdateSantaList();
        }

        private void UpdateSantaList()
        {
            ActiveSantas ??= new ObservableCollection<string>();
            ActiveSantas.Clear();

            foreach (var oneSanta in SantaManager.SecretSantaModels)
            {
                ActiveSantas.Add(oneSanta.Name);
            }
        }

        internal override void InitCommands()
        {
            throw new NotImplementedException();
        }
    }
}