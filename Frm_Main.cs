using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WichtelGeneratorVisual
{
    public partial class Frm_Main : Form
    {
        //======Lokale Variablen======================
        private List<WichtelPlayer> allUsers = new List<WichtelPlayer>();
        private List<string> nameList = new List<string>();
        
        //======Initialisierung======================
        public Frm_Main()
        {
            InitializeComponent();
        }

        //===========Funktionen======================================
        public void FillListBoxUser()
        {
            lb_User.Items.Clear();
            lb_User.BeginUpdate();
            foreach (string tUser in nameList)
            {
                lb_User.Items.Add(tUser);
            }
            lb_User.EndUpdate();
        }

        //-------------------------
        public WichtelPlayer GetWichtelPlayer(string aNameOfUser)
        {
            var tSelectedUserObject = allUsers.Where(p => p.UserName == aNameOfUser).FirstOrDefault();
            return tSelectedUserObject;
        }

        //-------------------------
        public void FillListBoxBlackListFromUser(WichtelPlayer aCurrentUser)
        {
            ArrayList tBlackList = aCurrentUser.GetBlackList();
            lb_BlackList.Items.Clear();
            lb_BlackList.BeginUpdate();
            foreach(string aName in tBlackList)
            {
                lb_BlackList.Items.Add(aName);
            }
            lb_BlackList.EndUpdate();
        }

        //-------------------------
        public void RevisionWhiteList(WichtelPlayer aCurrentUser)
        {
            var tBlackList = aCurrentUser.GetBlackList();
            var tWhiteList = aCurrentUser.GetWhiteList();
            foreach (string tBlackListUser in tBlackList)
            {
                foreach (string tWhiteListUser in tWhiteList)
                {
                    if (tWhiteListUser.Equals(tBlackListUser))
                    {
                        aCurrentUser.RemoveItemFromWhiteList(tWhiteListUser);
                        RevisionWhiteList(aCurrentUser); //Da sich die Liste geändert hat, funktioniert die Schleife nicht mehr und fängt komplett von vorne an.
                        break; //Verhindert bei Rückkehr von Rekursion die weiterführung, welche zum absturz geführt hätte.
                    }
                }
            }
        }

        //-------------------------
        public void FillRemainingUserToLBwhiteList(WichtelPlayer aCurrentUser)
        {
            var tWhiteList = aCurrentUser.GetWhiteList();
            lb_WhiteList.Items.Clear();
            lb_WhiteList      .BeginUpdate();
            foreach (string tWhiteListUser in tWhiteList)
            {
                lb_WhiteList.Items.Add(tWhiteListUser);
            }
            lb_WhiteList.EndUpdate();
        }

        //-------------------------
        public void ReloadAllWhiteLists()
        {
            foreach (WichtelPlayer tUser in allUsers)
            {
                tUser.WhiteList = new List<string>(nameList);
            }
        }

        //-------------------------
        public void MoveItemFromWhiteToBlackList(WichtelPlayer aUser, WichtelPlayer aWhiteListUser)
        {
            aUser.RemoveItemFromWhiteList(aWhiteListUser.UserName);
            aUser.SetBlackList           (aWhiteListUser.UserName);
            aWhiteListUser.SetbinInBlackListEingetragenVon(aUser.UserName);
        }

        //-------------------------
        public void MoveItemFromBlackToWhiteList(WichtelPlayer aUser, WichtelPlayer aBlackListUser)
        {
            aUser.RemoveItemInBlackList(aBlackListUser.UserName);
            aUser.SetWhiteListItem     (aBlackListUser.UserName);
            aBlackListUser.RemoveItemBinInBlackListEingetragenVon(aUser.UserName);
        }

        //-------------------------
        /// <summary>
        /// Kontrolle ob in BlackList der Eintrag vorkommt.
        /// </summary>
        /// <param name="aUser">BlackList ist zu suchen in angegebenen User</param>
        /// <param name="aSearchingItem">Nach was gesucht werden muss</param>
        /// <returns>True: Kein Vorkommen, False: Treffer, es kommt vor.</returns>
        public Boolean KontrolliereObVorhandenInBlackList(WichtelPlayer aUser, string aSearchingItem)
        {
            ArrayList tBlackList = aUser.GetBlackList();
            foreach (string tBlackListUser in tBlackList)
            {
                if (tBlackListUser.Equals(aSearchingItem))
                {
                    return false;
                }
            }
            return true;
        }

      
        public bool RandomVerlosung() //--2ter Versuch
        {
            List<string> allreadyUsed = new List<string>();

            Random random = new Random();
            int i = 0;

            foreach (var onePlayer in allUsers)
            {
                for (i = 0; i < onePlayer.GetWhiteList().Count(); i++)
                {
                    var posibleChoise = onePlayer.GetWhiteList()[random.Next(onePlayer.GetWhiteList().Count)];
                    var result = allreadyUsed.Where(p => p.Equals(posibleChoise)).FirstOrDefault();
                    if (string.IsNullOrEmpty(result))
                    {
                        //noch nicht verwendet
                        onePlayer.GezogenerWichtel = posibleChoise;
                        allreadyUsed.Add(posibleChoise);
                        break;
                    }
                }
                if (i >= onePlayer.GetWhiteList().Count()) //---Wenns nicht geklappt hat...
                {
                    foreach (var item in onePlayer.GetWhiteList())
                    {
                        var result = allreadyUsed.Where(p => p.Equals(item)).FirstOrDefault();
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
                for (int k = 0; k < allreadyUsed.Count; k++)
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

        //-------------------------
        public void FillGridView()
        {
            dataGridView1.Rows.Clear();
            foreach (var item in allUsers)
            {
                string[] oneRow = { item.UserName, item.GezogenerWichtel };

                dataGridView1.Rows.Add(oneRow);
            }

            dataGridView1.AutoResizeColumns();
            dataGridView1.Refresh();
            
        }

        //-------------------------
        public Boolean KontrolleObKostelationBereitsVerwendet(WichtelPlayer aCurrentUser, string lastChoiseOfBLuser)
        {
            try
            {
                foreach (WichtelPlayer tUser in allUsers)                                       //jeder User
                {
                    Boolean isLastChoiseControlled = false;
                    int identischCounter = 0;
                    if (!aCurrentUser.UserName.Equals(tUser.UserName))                      //Nicht er selber
                    {
                        ArrayList tOtherUsersBlackList = tUser.GetBlackList();
                        foreach (string tBLuserTuser in tOtherUsersBlackList)
                        {
                            ArrayList tCurrentUserBlackList = aCurrentUser.GetBlackList();
                            foreach (string tBLuserAcurrent in tCurrentUserBlackList)         //jeder in der BL von anderen User
                            {
                                if (tBLuserAcurrent.Equals(tBLuserTuser))                   //Kontrolle vorkommen
                                {
                                    identischCounter++;                                     //Zähle identische
                                }
                                if (lastChoiseOfBLuser.Equals(tBLuserTuser) && !isLastChoiseControlled)                //Auch zu kontrollieren die mögliche letzte wahl von Currentuser
                                {
                                    identischCounter++;
                                    isLastChoiseControlled = true;
                                }
                                if (identischCounter == aCurrentUser.GetBlackList().Count)  //ist gleiche Konstelation
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            
            return true;
        }

        //-------------------------
        public void SortUserListAnzahlBLAbsteigend()
        {
            //allUsers = allUsers.OrderBy(p => p.GetBlackList().Count).ToList();
            allUsers = allUsers.OrderByDescending(p => p.GetBlackList().Count).ToList();
        }

        private void CreateNewUser(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                return;
            }

            var result = nameList.Where(p => p.Equals(name)).FirstOrDefault();
            if (! string.IsNullOrEmpty(result))
            {
                MessageBox.Show($"Der User {name.ToUpper()} existiert bereits.");
                return;
            }

            WichtelPlayer neuerUser = new WichtelPlayer(name);
            allUsers.Add(neuerUser);
            nameList.Add(neuerUser.UserName);
            ReloadAllWhiteLists();
            FillListBoxUser();
            ed_UserName.Text = "";
            ed_UserName.Focus();
        }

        //===========Events===========================================
        private void Bt_CreateNewUser_Click(object sender, EventArgs e)
        {
            CreateNewUser(ed_UserName.Text);
        }

        //-------------------------
        private void Lb_User_MouseClick(object sender, MouseEventArgs e)
        {
            
            if (lb_User.SelectedIndex < 0)
            {
                return;
            }
            var tSelectedUserObject = GetWichtelPlayer(lb_User.Items[lb_User.SelectedIndex].ToString());
            if (tSelectedUserObject == null)
            {
                return;
            }
            FillListBoxBlackListFromUser  (tSelectedUserObject);
            RevisionWhiteList             (tSelectedUserObject);
            FillRemainingUserToLBwhiteList(tSelectedUserObject);
        }

        //-------------------------
        private void Bt_addUserToBlackList_Click(object sender, EventArgs e)
        {
            if ((lb_User.SelectedIndex < 0) || (lb_WhiteList.SelectedIndex < 0))
            {
                return;
            }

            //Selektierter auf WhiteList zu BlackList verschieben
            WichtelPlayer tSelectedUserObject = GetWichtelPlayer(lb_User.Items[lb_User.SelectedIndex].ToString());
            WichtelPlayer tWhiteListUser = GetWichtelPlayer(lb_WhiteList.Items[lb_WhiteList.SelectedIndex].ToString());

            if ((tSelectedUserObject == null) || (tWhiteListUser == null))
            {
                return;
            }
            
            //Regelung Kontrolle
            if ( (tSelectedUserObject.GetBlackList().Count) >= (allUsers.Count-1) ) //Maximale wahl an BL Menge
            {
                MessageBox.Show("Leider sind können nicht mehr auf die BlackList von diesem Mitspieler gelegt werden.");
                return;
            }
            if ( (tWhiteListUser.GetbinInBlackListEingetragenVon().Count) >= (allUsers.Count-2) ) //Maximales Vorkommen auf BLs
            {
                MessageBox.Show("Leider ist der Mitspieler der auf die Blacklist geschoben werden soll, bereits zu viel auf einer BlackList vorhanden.");
                return;
            }
            if ((tSelectedUserObject.GetBlackList().Count) >= (allUsers.Count - 2)) //Letztes mal vor Max BL Wahl
            {
                if (!KontrolleObKostelationBereitsVerwendet(tSelectedUserObject, tWhiteListUser.UserName))
                {
                    MessageBox.Show("Dieser User kann nicht zur BlackList hinzugefügt werden, da diese maximale Konstelation bereits existiert!");
                    return;
                }
            }

            MoveItemFromWhiteToBlackList  (tSelectedUserObject, tWhiteListUser);
            //Neu Laden
            FillListBoxBlackListFromUser  (tSelectedUserObject);
            RevisionWhiteList             (tSelectedUserObject);
            FillRemainingUserToLBwhiteList(tSelectedUserObject);
        }

        //-------------------------
        private void Bt_addusertoWhiteList_Click(object sender, EventArgs e)
        {
            if ((lb_User.SelectedIndex < 0) || (lb_BlackList.SelectedIndex < 0))
            {
                return;
            }

            //Selektierter auf WhiteList zu BlackList verschieben
            WichtelPlayer tSelectedUserObject = GetWichtelPlayer(lb_User.Items[lb_User.SelectedIndex].ToString());
            WichtelPlayer tBlackListUser = GetWichtelPlayer(lb_BlackList.Items[lb_BlackList.SelectedIndex].ToString());

            if ((tSelectedUserObject == null) || (tBlackListUser == null))
            {
                return;
            }


            MoveItemFromBlackToWhiteList  (tSelectedUserObject, tBlackListUser);
            //Neu Laden
            FillListBoxBlackListFromUser  (tSelectedUserObject);
            RevisionWhiteList             (tSelectedUserObject);
            FillRemainingUserToLBwhiteList(tSelectedUserObject);
        }

        //-------------------------
        private void Bt_verlosung_Click(object sender, EventArgs e)
        {
            if (allUsers.Count == 0)
            {
                return;
            }

            SortUserListAnzahlBLAbsteigend();

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

        private void Ed_UserName_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) && (ed_UserName.Text != ""))
            {
                CreateNewUser(ed_UserName.Text);
            }
        }

    }
}
//======================================================
//===============END OF FILE============================
//======================================================
