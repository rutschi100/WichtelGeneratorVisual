using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.Lottery;
using WichtelGenerator.Core.Models;
using WichtelGenerator.Core.SantaManaager;
using WichtelGenerator.WPF.Commands;

namespace WichtelGenerator.WPF.ViewModels
{
    internal class RuffleViewModel : BaseViewModel
    {
        private ObservableCollection<SecretSantaModel> _resultList;
        private bool _resultPerGui;
        private bool _resultPerMail;
        private Visibility _resultVisibility = Visibility.Collapsed;

        public RuffleViewModel(ILotteryService lotteryService, ISantaManager santaManager, IConfigManager configManager)
        {
            LotteryService = lotteryService;
            SantaManager = santaManager;
            ConfigManager = configManager;

            InitCommands();
        }

        public IAsyncCommand OnRuffle { get; set; }
        private ILotteryService LotteryService { get; }
        private ISantaManager SantaManager { get; }
        private IConfigManager ConfigManager { get; }

        public ObservableCollection<SecretSantaModel> ResultList
        {
            get => _resultList;
            set => SetAndRaise(ref _resultList, value);
        }

        public bool ResultPerMail
        {
            get => _resultPerMail;
            set => SetAndRaise(ref _resultPerMail, value);
        }

        public bool ResultPerGui
        {
            get => _resultPerGui;
            set => SetAndRaise(ref _resultPerGui, value);
        }

        public Visibility ResultVisibility
        {
            get => _resultVisibility;
            set => SetAndRaise(ref _resultVisibility, value);
        }

        internal sealed override void InitCommands()
        {
            OnRuffle = AsyncCommand.Create(StartRuffle);
        }

        private async Task StartRuffle()
        {
            await Task.CompletedTask;
            try
            {
                if (!IsSettingValid())
                {
                    throw new Exception("Ausgewählte Resultatanzeige ist gemäss Einstellungen, nicht verfügbar");
                }

                var result = LotteryService.Raffle(SantaManager.SecretSantaModels);
                ResultList = new ObservableCollection<SecretSantaModel>(result);
                if (!ConfigManager.ConfigModel.NotificationsEnabled)
                {
                    ResultVisibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Die Nachricht wurde an die Teilnehmer versendet.");
                }
#if DEBUG
                ResultVisibility = Visibility.Visible;
#endif
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ein Fehler ist aufgetreten! {e.Message}");
            }
        }


        private bool IsSettingValid()
        {
            var valid = ConfigManager.ConfigModel.NotificationsEnabled == ResultPerMail;
            return valid;
        }
    }
}