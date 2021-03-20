using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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
            foreach 
                (var tWhiteListUser in 
                from tBlackListUser 
                    in aCurrentUser.BlackList 
                from tWhiteListUser in aCurrentUser.WhiteList
                where tWhiteListUser.Equals(tBlackListUser)
                select tWhiteListUser)
            {
                aCurrentUser.WhiteList.Remove(tWhiteListUser);
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
        /// Verschiebt den User von White zur BlackList
        /// </summary>
        /// <param name="aUser"></param>
        /// <param name="aWhiteListUser"></param>
        private static void MoveItemFromWhiteToBlackList(WichtelModel aUser, WichtelModel aWhiteListUser)
        {
            aUser.WhiteList.Remove(aWhiteListUser.UserName);
            aUser.BlackList.Add(aWhiteListUser.UserName);
            aWhiteListUser.IamRegisteredBy.Add(aUser.UserName);
        }

        /// <summary>
        /// Verschiebt den User von Black zur WhiteList
        /// </summary>
        /// <param name="aUser"></param>
        /// <param name="aBlackListUser"></param>
        private void MoveItemFromBlackToWhiteList(WichtelModel aUser, WichtelModel aBlackListUser)
        {
            aUser.BlackList.Remove(aBlackListUser.UserName);
            aUser.WhiteList.Add(aBlackListUser.UserName);
            aBlackListUser.IamRegisteredBy.Remove(aUser.UserName);
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
            var i = 0;

            foreach (var onePlayer in _allUsers)
            {
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


        //===========Events===========================================
        private void Bt_CreateNewUser_Click(object sender, EventArgs e)
        {
            UpdateFrameByCreatingUser();
        }

        //-------------------------
        private void Lb_User_MouseClick(object sender, MouseEventArgs e)
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
            FillListBoxBlackListFromUser  (tSelectedUserObject);
            RevisionWhiteList             (tSelectedUserObject);
            FillListBoxWhiteListFromUser(tSelectedUserObject);
        }

        //-------------------------
        private void Bt_addUserToBlackList_Click(object sender, EventArgs e)
        {
            if ((lb_User.SelectedIndex < 0) || (lb_WhiteList.SelectedIndex < 0))
            {
                return;
            }

            //Selektierter auf WhiteList zu BlackList verschieben
            var tSelectedUserObject = GetWichtelByName(lb_User.Items[lb_User.SelectedIndex].ToString());
            var tWhiteListUser = GetWichtelByName(lb_WhiteList.Items[lb_WhiteList.SelectedIndex].ToString());

            if ((tSelectedUserObject == null) || (tWhiteListUser == null))
            {
                return;
            }
            
            //Regelung Kontrolle
            if ( (tSelectedUserObject.BlackList.Count) >= (_allUsers.Count-1) ) //Maximale wahl an BL Menge
            {
                MessageBox.Show("Leider sind können nicht mehr auf die BlackList von diesem Mitspieler gelegt werden.");
                return;
            }
            if ( (tWhiteListUser.IamRegisteredBy.Count) >= (_allUsers.Count-2) ) //Maximales Vorkommen auf BLs
            {
                MessageBox.Show("Leider ist der Mitspieler der auf die Blacklist geschoben werden soll, bereits zu viel auf einer BlackList vorhanden.");
                return;
            }
            if ((tSelectedUserObject.BlackList.Count) >= (_allUsers.Count - 2)) //Letztes mal vor Max BL Wahl
            {
                if (!IsConstelationNotUsed(tSelectedUserObject, tWhiteListUser.UserName))
                {
                    MessageBox.Show("Dieser User kann nicht zur BlackList hinzugefügt werden, da diese maximale Konstelation bereits existiert!");
                    return;
                }
            }

            MoveItemFromWhiteToBlackList  (tSelectedUserObject, tWhiteListUser);
            //Neu Laden
            FillListBoxBlackListFromUser  (tSelectedUserObject);
            RevisionWhiteList             (tSelectedUserObject);
            FillListBoxWhiteListFromUser(tSelectedUserObject);
        }

        //-------------------------
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


            MoveItemFromBlackToWhiteList  (tSelectedUserObject, tBlackListUser);
            //Neu Laden
            FillListBoxBlackListFromUser  (tSelectedUserObject);
            RevisionWhiteList             (tSelectedUserObject);
            FillListBoxWhiteListFromUser(tSelectedUserObject);
        }

        //-------------------------
        private void Bt_verlosung_Click(object sender, EventArgs e)
        {
            if (_allUsers.Count == 0)
            {
                return;
            }

            SortUserByCoundDesc();

            try
            {
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