using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.Enums;
using WichtelGenerator.Core.Models;
using WichtelGenerator.Core.SantaManaager;
using WichtelGenerator.Core.Services;
using WichtelGenerator.WPF.Commands;

namespace WichtelGenerator.WPF.ViewModels
{
    internal class ManageBlackListViewModel : BaseViewModel
    {
        private ObservableCollection<string> _activeSantas = new ObservableCollection<string>();
        private ObservableCollection<string> _blackList;
        private string _selcectedSantaName;
        private string _selectedBlack;
        private SecretSantaModel _selectedSantaModel;
        private string _selectedWhite;
        private ObservableCollection<string> _whiteList;
        private readonly IListMappingService _listMappingService;

        public ManageBlackListViewModel(
            ConfigModel configModel,
            AddUserViewModel addUserViewModel,
            ISantaManager santaManager,
            IListMappingService listMappingService
        )
        {
            ConfigModel = configModel;
            AddUserViewModel = addUserViewModel;
            SantaManager = santaManager;
            _listMappingService = listMappingService;

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
                OnSantaSelect(value);
            }
        }

        public ObservableCollection<string> BlackList
        {
            get => _blackList;
            set => SetAndRaise(ref _blackList, value);
        }

        public ObservableCollection<string> WhiteList
        {
            get => _whiteList;
            set => SetAndRaise(ref _whiteList, value);
        }

        //todo: nach änderung der Blacklist --> speichern in db

        public IAsyncCommand OnClickWhiteList { get; set; }

        public string SelectedBlack
        {
            get => _selectedBlack;
            set => SetAndRaise(ref _selectedBlack, value);
        }

        public string SelectedWhite
        {
            get => _selectedWhite;
            set => SetAndRaise(ref _selectedWhite, value);
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

        public IAsyncCommand OnClickBlackList { get; set; }

        private void OnSantaSelect(string santaName)
        {
            var santa = SantaManager.SecretSantaModels.FirstOrDefault(p => p.Name == santaName);
            SelectedSantaModel = santa;
            FillTheLists(santa);
        }


        private void FillTheLists(SecretSantaModel santa)
        {
            BlackList ??= new ObservableCollection<string>();
            WhiteList ??= new ObservableCollection<string>();
            WhiteList.Clear();
            BlackList.Clear();

            if (santa == null)
            {
                return;
            }

            foreach (var oneWhite in _listMappingService.GetHoleList(
                santa,
                SecredSantaMappingType.WhiteListed,
                true
            ))
            {
                WhiteList.Add(oneWhite.Name);
            }

            foreach (var oneBlack in _listMappingService.GetHoleList(
                santa,
                SecredSantaMappingType.BlackListed,
                true
            ))
            {
                BlackList.Add(oneBlack.Name);
            }
        }

        private void Initialize()
        {
            UpdateSantaList();
            SantaManager.NewUserAddedEvent += UpdateSantaList;
            InitCommands();
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
        /*
         private void UpdateSantaList()
        {
            var names = SantaManager.SecretSantaModels.Select(oneSanta => oneSanta.Name).ToList();

            ActiveSantas = new ObservableCollection<string>(names);
        }
         */

        private async Task MoveWhiteToBlack()
        {
            await Task.CompletedTask;
            var model = SantaManager.SecretSantaModels.FirstOrDefault(p => p.Name == SelectedWhite);
            if (model == null)
            {
                throw new Exception($"Value isnn't in the List: {SelectedWhite}");
            }


            switch (SantaManager.AddSantaToBlackList(SelectedSantaModel, model))
            {
                case SantaBlackListWishResult.Valid:
                    break;
                case SantaBlackListWishResult.CombinationAlreadyExist:
                    MessageBox.Show(
                        "Wichtel konnte nicht zur Blacklist hinzugefügt werden, da diese Kombination bereits besteht!"
                    );
                    return;
                case SantaBlackListWishResult.MaxValueAlreadyUsed:
                    MessageBox.Show(
                        "Es können nicht mehr zur Blacklist hinzugefügt werden. Die maximale Anzahl ist erreicht."
                    );
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            OnSantaSelect(SelectedSantaModel.Name); // Fürs Frontend update
        }

        private async Task MoveBlackToWhite()
        {
            await Task.CompletedTask;

            var model = SantaManager.SecretSantaModels.FirstOrDefault(p => p.Name == SelectedBlack);

            if (model == null)
            {
                return;
            }

            if (model == SelectedSantaModel)
            {
                MessageBox.Show("Ein Model kann sich nicht selbst auf der White List halten.");
            }

            SantaManager.AddSantaToWhiteList(SelectedSantaModel, model);

            OnSantaSelect(SelectedSantaModel.Name); // Fürs Frontend update
        }

        internal override void InitCommands()
        {
            OnClickWhiteList = AsyncCommand.Create(para => MoveWhiteToBlack());
            OnClickBlackList = AsyncCommand.Create(para => MoveBlackToWhite());
        }
    }
}