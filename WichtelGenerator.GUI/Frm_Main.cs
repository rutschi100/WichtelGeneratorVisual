using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WichtelGenerator.Core.Models;

namespace WichtelGeneratorVisual
{
    public partial class FrmMain : Form
    {
        private List<WichtelModel> _allUsers = new List<WichtelModel>();
        private readonly List<string> _nameList = new List<string>();
        
        public FrmMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Füllt die Listbox mit den aktuellen registrierten User ab.
        /// </summary>
        private void FillListBoxUser()
        {
            lb_User.Items.Clear();
            lb_User.BeginUpdate();
            foreach (var tUser in _nameList)
            {
                lb_User.Items.Add(tUser);
            }
            lb_User.EndUpdate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aNameOfUser"></param>
        /// <returns></returns>
        private WichtelModel GetWichtelByName(string aNameOfUser)
        {
            var tSelectedUserObject = _allUsers.FirstOrDefault(p => p.UserName == aNameOfUser);
            return tSelectedUserObject;
        }

        /// <summary>
        /// Befüllt den BlackList bereich, des angegebenen Wichtels
        /// </summary>
        /// <param name="aCurrentUser"></param>
        private void FillListBoxBlackListFromUser(WichtelModel aCurrentUser)
        {
            var tBlackList = aCurrentUser.BlackList;
            lb_BlackList.Items.Clear();
            lb_BlackList.BeginUpdate();
            foreach(var aName in tBlackList)
            {
                lb_BlackList.Items.Add(aName);
            }
            lb_BlackList.EndUpdate();
        }
        
        /// <summary>
        /// Korrigiert die WhiteList.
        /// Ist in BlackList und WhiteList ein Eintrag identisch, so wird es aus der WhiteList gelöscht.
        /// </summary>
        /// <param name="aCurrentUser"></param>
        private static void RevisionWhiteList(WichtelModel aCurrentUser)
        {
            var toRemove = (
                from oneWhite in aCurrentUser.WhiteList
                from oneBlack in aCurrentUser.BlackList
                where oneBlack == oneWhite
                select oneBlack).ToList();

            foreach (var one2Remove in toRemove)
            {
                aCurrentUser.WhiteList.Remove(one2Remove);
            }

        }

        /// <summary>
        /// Befüllt den WhiteList bereich vom Selektierten Wichtel.
        /// </summary>
        /// <param name="aCurrentUser"></param>
        private void FillListBoxWhiteListFromUser(WichtelModel aCurrentUser)
        {
            lb_WhiteList.Items.Clear();
            lb_WhiteList.BeginUpdate();
            foreach (var tWhiteListUser in aCurrentUser.WhiteList)
            {
                lb_WhiteList.Items.Add(tWhiteListUser);
            }
            lb_WhiteList.EndUpdate();
        }

        /// <summary>
        /// Resettet die WhiteList aller User.
        /// </summary>
        private void ReloadAllWhiteLists()
        {
            foreach (var tUser in _allUsers)
            {
                tUser.WhiteList = new List<string>(_nameList);
            }
        }

        /// <summary>
        /// Verschiebt den einen User aus der einen zur anderen Liste.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="toMovedWichtel"></param>
        /// <param name="targedList"></param>
        private void ChangeUserInInternLists(WichtelModel owner, WichtelModel toMovedWichtel, WichtelLists targedList)
        {
            switch (targedList)
            {
                case WichtelLists.BlackList:
                    owner.WhiteList.Remove(toMovedWichtel.UserName);
                    owner.BlackList.Add(toMovedWichtel.UserName);
                    toMovedWichtel.IamRegisteredBy.Add(owner.UserName);
                    break;
                case WichtelLists.WhiteList:
                    owner.BlackList.Remove(toMovedWichtel.UserName);
                    owner.WhiteList.Add(toMovedWichtel.UserName);
                    toMovedWichtel.IamRegisteredBy.Remove(owner.UserName);
                    break;
                case WichtelLists.IamRegisteredByList:
                    throw new Exception($"Nicht unterstützer WichtelListen Type: IamRegisteredByList");
                default:
                    throw new ArgumentOutOfRangeException(nameof(targedList), targedList, null);
            }
        }

        /// <summary>
        /// Kontrolle ob in BlackList der Eintrag vorkommt.
        /// </summary>
        /// <param name="aUser">BlackList ist zu suchen in angegebenen User</param>
        /// <param name="aSearchingItem">Nach was gesucht werden muss</param>
        /// <returns>True: Kein Vorkommen, False: Treffer, es kommt vor.</returns>
        public bool IsNotInBlackList(WichtelModel aUser, string aSearchingItem)
        {
            return aUser.BlackList.All(tBlackListUser => !tBlackListUser.Equals(aSearchingItem));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool RandomVerlosung() //TODO: Am schluss Refaktorisieren.
        {
            var allreadyUsed = new List<string>();

            var random = new Random();

            foreach (var onePlayer in _allUsers)
            {
                var i = 0;
                for (i = 0; i < onePlayer.WhiteList.Count(); i++)
                {
                    var posibleChoise = onePlayer.WhiteList[random.Next(onePlayer.WhiteList.Count)];
                    var result = allreadyUsed.FirstOrDefault(p => p.Equals(posibleChoise));
                    if (string.IsNullOrEmpty(result))
                    {
                        //noch nicht verwendet
                        onePlayer.GezogenerWichtel = posibleChoise;
                        allreadyUsed.Add(posibleChoise);
                        break;
                    }
                }
                if (i >= onePlayer.WhiteList.Count()) //---Wenns nicht geklappt hat...
                {
                    foreach (var item in onePlayer.WhiteList)
                    {
                        var result = allreadyUsed.FirstOrDefault(p => p.Equals(item));
                        if (string.IsNullOrEmpty(result))
                        {
                            //noch nicht verwendet
                            onePlayer.GezogenerWichtel = item;
                            allreadyUsed.Add(item);
                            break;
                        }
                    }
                    if (string.IsNullOrEmpty(onePlayer.GezogenerWichtel))
                    {
                        return false;
                    }
                }
            }

            foreach (var item in allreadyUsed)
            {
                for (var k = 0; k < allreadyUsed.Count; k++)
                {
                    if (k <= allreadyUsed.Count)
                    {
                        break;
                    }
                    if (allreadyUsed[k+1] == item)
                    {
                        MessageBox.Show($"Grober Fehler unterlaufen. Jemand wurde doppelt gezogen...");
                    }
                }
            }


            return true;
        }

        /// <summary>
        /// Befüllt das Grit mit den Resultaten.
        /// </summary>
        private void FillGridView()
        {
            dataGridView1.Rows.Clear();
            foreach (var item in _allUsers)
            {
                object[] oneRow = { item.UserName, item.GezogenerWichtel };
                dataGridView1.Rows.Add(oneRow);
            }

            dataGridView1.AutoResizeColumns();
            dataGridView1.Refresh();
        }

        /// <summary>
        /// Prüft, ob die Konstellation noch nicht verwendet wurde.
        /// </summary>
        /// <param name="aCurrentUser"></param>
        /// <param name="lastChoise"></param>
        /// <returns>
        /// true = noch nicht verwednet |
        /// false = bereits verwendet
        /// </returns>
        private bool IsConstelationNotUsed(WichtelModel aCurrentUser, string lastChoise)
        {
            try
            {
                aCurrentUser.BlackList.Add(lastChoise);
                foreach (var oneUser in _allUsers)
                {
                    var grouped = from aOther in oneUser.BlackList
                        from aCurrent in aCurrentUser.BlackList
                        where aOther == aCurrent
                        select aCurrent;

                    var identische = grouped.Count();
                    var anzBlackList = aCurrentUser.BlackList.Count;
                        aCurrentUser.BlackList.Remove(lastChoise);

                    return identische != anzBlackList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            return false;
        }
        
        /// <summary>
        /// Sortiert die User in der Liste nach anzahl eingetragenen Blacklist Wichtel.
        /// </summary>
        private void SortUserByCoundDesc()
        {
            _allUsers = _allUsers.OrderByDescending(p => p.BlackList.Count).ToList();
        }

        /// <summary>
        /// Erstellt einen neuen User, falls es möglich ist.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Erfolg der Erstellung</returns>
        private bool CreateNewUser(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                return false;
            }

            var result = _nameList.FirstOrDefault(p => p.Equals(name));
            if (! string.IsNullOrEmpty(result))
            {
                return false;
            }
            
            _allUsers.Add(new WichtelModel(name));
            _nameList.Add(name);
            return true;
        }

        /// <summary>
        /// Managet das erstellen eines neuen Users und die Anzeigung im Frame.
        /// </summary>
        private void UpdateFrameByCreatingUser()
        {
            if (CreateNewUser(ed_UserName.Text))
            {
                ReloadAllWhiteLists();
                FillListBoxUser();
                ed_UserName.Text = "";
                ed_UserName.Focus();
            }
            else
            {
                MessageBox.Show($"Der User {ed_UserName.Text} existiert bereits.");
            }
        }

        /// <summary>
        /// Validiert die Wahl des zu verschiebenden Wichtels in die BlackList
        /// </summary>
        /// <param name="selectedModel"></param>
        /// <param name="whiteListModel"></param>
        /// <returns></returns>
        private bool ValidateBlackListMove(WichtelModel selectedModel, WichtelModel whiteListModel)
        {
            if ((selectedModel == null) || (whiteListModel == null))
            {
                return false;
            }
            if ((selectedModel.BlackList.Count) >= (_allUsers.Count - 1)) //Maximale wahl an BL Menge
            {
                MessageBox.Show("Leider sind können nicht mehr auf die BlackList von diesem Mitspieler gelegt werden.");
                return false;
            }
            if ((whiteListModel.IamRegisteredBy.Count) >= (_allUsers.Count - 2)) //Maximales Vorkommen auf BLs
            {
                MessageBox.Show("Leider ist der Mitspieler der auf die Blacklist geschoben werden soll, bereits zu viel auf einer BlackList vorhanden.");
                return false;
            }

            if ((selectedModel.BlackList.Count) < (_allUsers.Count - 2)) return true;
            if (IsConstelationNotUsed(selectedModel, whiteListModel.UserName)) return true;

            MessageBox.Show("Dieser User kann nicht zur BlackList hinzugefügt werden, da diese maximale Konstelation bereits existiert!");
            return false;
        }

        /// <summary>
        /// Lässt die Listen des angegebenen Users wieder korrekt darstellen im GUI
        /// </summary>
        /// <param name="wichtel"></param>
        private void RefreshUserListsInFrame(WichtelModel wichtel)
        {
            FillListBoxBlackListFromUser(wichtel);
            RevisionWhiteList(wichtel);
            FillListBoxWhiteListFromUser(wichtel);
        }

        //===========Events===========================================

        /// <summary>
        /// Event, um User zu erstellen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_CreateNewUser_Click(object sender, EventArgs e)
        {
            UpdateFrameByCreatingUser();
        }

        /// <summary>
        /// Event, wenn ein User ausgewählt wird, um zu konfigurieren.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Lb_User_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lb_User.SelectedIndex < 0)
                {
                    return;
                }
                var tSelectedUserObject = GetWichtelByName(lb_User.Items[lb_User.SelectedIndex].ToString());
                if (tSelectedUserObject == null)
                {
                    return;
                }
                RevisionWhiteList(tSelectedUserObject);
                FillListBoxBlackListFromUser(tSelectedUserObject);
                FillListBoxWhiteListFromUser(tSelectedUserObject);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.GetType().ToString());
                MessageBox.Show(exception.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Event um selektierter Whitelist-User auf die BlackList zu verschieben.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_addUserToBlackList_Click(object sender, EventArgs e)
        {
            // Preconditions
            if ((lb_User.SelectedIndex < 0) || (lb_WhiteList.SelectedIndex < 0))
            {
                return;
            }

            //Korrekte Models Holen
            var tSelectedUserObject = GetWichtelByName(lb_User.Items[lb_User.SelectedIndex].ToString());
            var tWhiteListUser = GetWichtelByName(lb_WhiteList.Items[lb_WhiteList.SelectedIndex].ToString());

            ValidateBlackListMove(tSelectedUserObject, tWhiteListUser);
            ChangeUserInInternLists(tSelectedUserObject, tWhiteListUser, WichtelLists.WhiteList);
            
            RefreshUserListsInFrame(tSelectedUserObject);
        }

        /// <summary>
        /// Event um selektierter BlackList-User auf die WhiteList zu verschieben.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_addusertoWhiteList_Click(object sender, EventArgs e)
        {
            if ((lb_User.SelectedIndex < 0) || (lb_BlackList.SelectedIndex < 0))
            {
                return;
            }

            //Selektierter auf WhiteList zu BlackList verschieben
            var tSelectedUserObject = GetWichtelByName(lb_User.Items[lb_User.SelectedIndex].ToString());
            var tBlackListUser = GetWichtelByName(lb_BlackList.Items[lb_BlackList.SelectedIndex].ToString());

            if ((tSelectedUserObject == null) || (tBlackListUser == null))
            {
                return;
            }

            ChangeUserInInternLists(tSelectedUserObject, tBlackListUser, WichtelLists.BlackList);
            RefreshUserListsInFrame(tSelectedUserObject);
        }

        /// <summary>
        /// Event, dass die Verlosung auslöst und das Resultat anzeigen lässt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_verlosung_Click(object sender, EventArgs e)
        {
            try
            {
                if (_allUsers.Count == 0)
                {
                    return;
                }

                SortUserByCoundDesc();

                while (! RandomVerlosung())
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            FillGridView   ();
        }

        /// <summary>
        /// Event für User erstellung mit dem Keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ed_UserName_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) && (ed_UserName.Text != ""))
            {
                UpdateFrameByCreatingUser();
            }
        }
    }
}