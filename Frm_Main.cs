using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        List<WichtelPlayer> allUsers = new List<WichtelPlayer>();
        ArrayList nameList       = new ArrayList();
        int versucheBisAbbruchBeiVerlosung;
        
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
            ArrayList tBlackList = aCurrentUser.GetBlackList();
            ArrayList tWhiteList = aCurrentUser.GetWhiteList();
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
            ArrayList tWhiteList = aCurrentUser.GetWhiteList();
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
            foreach (WichtelPlayer tUser in allUsers )
            {
                tUser.SetWhiteList((ArrayList)nameList.Clone()); // Veranlasst das jeder seine eigene Liste hat mit allen Usern.
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

        //-------------------------
        public void RandomVerlosung()
        {
            try
            {
                //Abbruch Kontrolle
                versucheBisAbbruchBeiVerlosung++;
                if (versucheBisAbbruchBeiVerlosung > Int32.MaxValue)
                {
                    MessageBox.Show("Ein Fehler ist aufgetreten. Bitte versuche es nochmals");
                    return;
                }

                //Temporäre Variablen
                Boolean kontrolle;
                ArrayList unUsedUserList = (ArrayList)nameList.Clone();
                Random random            = new Random();
                Boolean everythingToUse;
                string tempValueString;

                //Verlosung
                foreach (WichtelPlayer tUser in allUsers)
                {
                    int counter = 0;
                    kontrolle   = false;
                    while (!kontrolle)
                    {
                        //bei Zu vielen Fehlversuchen
                        if (counter > allUsers.Count-2)
                        {
                            //Suche ob überhaupt etwas frei ist
                            everythingToUse = false;
                            foreach (string tUnusedUser in unUsedUserList)
                            {
                                everythingToUse = KontrolliereObVorhandenInBlackList(tUser, tUnusedUser);
                                if (everythingToUse)
                                {
                                    tUser.GezogenerWichtel = tUnusedUser;
                                    unUsedUserList.Remove(tUnusedUser);
                                    everythingToUse = true;
                                    break;
                                }
                            }
                            if (!everythingToUse)
                            {
                                RandomVerlosung();
                                return;
                            }
                        }

                        //Zufällige verlosung
                        else
                        {
                            tempValueString = unUsedUserList[random.Next(unUsedUserList.Count)].ToString();
                            if (!KontrolliereObVorhandenInBlackList(tUser, tempValueString))
                            {
                                counter++;
                            }
                            else
                            {
                                    kontrolle              = true;
                                    tUser.GezogenerWichtel = tempValueString;
                                    unUsedUserList.Remove(tempValueString);
                                    break;
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Bei der Ziehung wurde einen Fehler endeckt. Bitte versuche es nochmals. Möglicherweise, geht die Rechnung nicht" +
                    "auf, weil du teilweise zu viel in der BlackList hast.");
                return;
            }

        }

        //-------------------------
        public void FillGridView()
        {
            BindingSource verknüpfteDaten = new BindingSource();
            verknüpfteDaten.DataSource    = allUsers;
            dataGridView1.DataSource      = verknüpfteDaten;
            dataGridView1.AutoResizeColumns();
            dataGridView1.Refresh();
            
        }

        //-------------------------
        public Boolean KontrolleObKostelationBereitsVerwendet(WichtelPlayer aCurrentUser, string lastChoiseOfBLuser)
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
            return true;
        }

        //-------------------------
        public void SortUserListAnzahlBLAbsteigend()
        {
            //var sortetUserList = allUsers.Sort(p, k => p.GetBlackList().Count);


            for (int n = allUsers.Count; n > 1; n--)
            {
                for (int i = 0; i < n-1; i++)
                {
                    if (allUsers[i].GetBlackList().Count < allUsers[i+1].GetBlackList().Count)
                    {
                        WichtelPlayer tSwapUser = allUsers[i];
                        allUsers[i] = allUsers[i+1];
                        allUsers[i + 1] = tSwapUser;
                    }
                }
            }
        }

        private void CreateNewUser(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
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

            versucheBisAbbruchBeiVerlosung = 0;
            RandomVerlosung();
            FillGridView   ();
        }

        private void ed_UserName_KeyDown(object sender, KeyEventArgs e)
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
