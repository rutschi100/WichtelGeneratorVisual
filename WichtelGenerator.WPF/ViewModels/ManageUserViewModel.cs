using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using WichtelGenerator.Core.Models;
using WichtelGenerator.Core.SantaManaager;
using WichtelGenerator.WPF.Commands;

// ReSharper disable MemberCanBePrivate.Global

namespace WichtelGenerator.WPF.ViewModels
{
    internal class ManageUserViewModel : BaseViewModel
    {
        private ObservableCollection<string> _activeSantas;
        private string _manipulatedMail;
        private string _manipulatedName;
        private IAsyncCommand _onSave;
        private string _selectedSantaname;

        public ManageUserViewModel(ISantaManager santaManager)
        {
            SantaManager = santaManager;
            UpdateSantaList();
            SantaManager.NewUserAddedEvent += UpdateSantaList;
            InitCommands();
        }

        //todo: Möglichkeit die User zu löschen fehlt noch!

        public string SelectedSantaname
        {
            get => _selectedSantaname;
            set
            {
                SetAndRaise(ref _selectedSantaname, value);
                SetSelectedSantaByName(value);
                FillManipulationFields();
            }
        }

        public IAsyncCommand OnSave
        {
            get => _onSave;
            set => SetAndRaise(ref _onSave, value);
        }

        private SecretSantaModel
            SelectedModel { get; set; } //todo: Der korrigierte Name, muss nach eindeutigkeit überprüft werden!

        public string ManipulatedName
        {
            get => _manipulatedName;
            set => SetAndRaise(ref _manipulatedName, value);
        }

        public string ManipulatedMail
        {
            get => _manipulatedMail;
            set => SetAndRaise(ref _manipulatedMail, value);
        }

        private ISantaManager SantaManager { get; }

        public ObservableCollection<string> ActiveSantas
        {
            get => _activeSantas;
            set => SetAndRaise(ref _activeSantas, value);
        }

        private void UpdateSantaList(object sender, EventArgs e)
        {
            UpdateSantaList();
        }

        internal sealed override void InitCommands()
        {
            OnSave = AsyncCommand.Create(SaveChanges);
        }

        private async Task SaveChanges()
        {
            await Task.CompletedTask;

            if (IsManipulationNotValid()) return;

            SelectedModel.Name = ManipulatedName;
            SelectedModel.MailAdress = ManipulatedMail;

            MessageBox.Show("Die Änderungen wurden übernommen.");
        }

        private bool IsManipulationNotValid()
        {
            if (SelectedModel == null)
            {
                return true;
            }

            if (string.IsNullOrEmpty(ManipulatedName))
            {
                MessageBox.Show("Es wurde kein Namen angegeben!");
                return true;
            }

            if (SelectedModel.Name != ManipulatedName)
            {
                if (SantaManager.SecretSantaModels.Any(p => p.Name == ManipulatedName))
                {
                    MessageBox.Show($"Der Name {ManipulatedName} existiert bereits. Bitte wähle einen anderen");
                    return true;
                }
            }

            var isAnMailAdress = Regex.IsMatch(ManipulatedMail, @"[a-zA-Z0-9]*@[a-zA-Z0-9]*\.[a-zA-Z0-9]*");

            if (isAnMailAdress || string.IsNullOrEmpty(ManipulatedMail)) return false;
            MessageBox.Show("Die Mailadresse ist nicht Valide");
            return true;
        }

        private void FillManipulationFields()
        {
            ManipulatedName = SelectedModel?.Name;
            ManipulatedMail = SelectedModel?.MailAdress;
        }

        private void SetSelectedSantaByName(string modelName)
        {
            SelectedModel = SantaManager.SecretSantaModels.FirstOrDefault(p => p.Name == modelName);
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
    }
}