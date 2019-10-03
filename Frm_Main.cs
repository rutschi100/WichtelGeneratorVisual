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
        List<UserClass> userList = new List<UserClass>();
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
        public UserClass GetCurrentUserObject(string aNameOfUser)
        {
            UserClass tCurrentUser = new UserClass("FEHLER!!!");
            foreach (UserClass tUser in userList)
            {
                if (aNameOfUser.Equals(tUser.UserName) )
                {
                   return tUser;
                }
            }
            return tCurrentUser;
        }

        //-------------------------
        public void FillListBoxBlackListFromUser(UserClass aCurrentUser)
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
        public void RevisionWhiteList(UserClass aCurrentUser)
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
        public void FillRemainingUserToLBwhiteList(UserClass aCurrentUser)
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
            foreach (UserClass tUser in userList )
            {
                tUser.SetWhiteList((ArrayList)nameList.Clone()); // Veranlasst das jeder seine eigene Liste hat mit allen Usern.
            }
        }

        //-------------------------
        public void MoveItemFromWhiteToBlackList(UserClass aUser, UserClass aWhiteListUser)
        {
            aUser.RemoveItemFromWhiteList(aWhiteListUser.UserName);
            aUser.SetBlackList           (aWhiteListUser.UserName);
            aWhiteListUser.SetbinInBlackListEingetragenVon(aUser.UserName);
        }

        //-------------------------
        public void MoveItemFromBlackToWhiteList(UserClass aUser, UserClass aBlackListUser)
        {
            aUser.RemoveItemInBlackList(aBlackListUser.UserName);
            aUser.SetWhiteListItem     (aBlackListUser.UserName);
            aBlackListUser.RemoveItemBinInBlackListEingetragenVon(aUser.UserName);
        }

        //-------------------------
        public int SetMaxVersucheBisAbbruch()
        {
            return userList.Count();
        }

        //-------------------------
        /// <summary>
        /// Kontrolle ob in BlackList der Eintrag vorkommt.
        /// </summary>
        /// <param name="aUser">BlackList ist zu suchen in angegebenen User</param>
        /// <param name="aSearchingItem">Nach was gesucht werden muss</param>
        /// <returns>True: Kein Vorkommen, False: Treffer, es kommt vor.</returns>
        public Boolean KontrolliereObVorhandenInBlackList(UserClass aUser, string aSearchingItem)
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
        /// <summary>
        /// Kontrolliert ob der User bereits gezogen wurde
        /// </summary>
        /// <param name="aUnUsedList"></param>
        /// <param name="aSearchingUser"></param>
        /// <returns>True: Wurde bereits gezogen, False: Wurde noch nicht gezogen, kann verwendet werden.</returns>
        public Boolean KontrollierObBereitsGezogen(ArrayList aUnUsedList, string aSearchingUser)
        {
            foreach (string tUnusedUser in aUnUsedList)
            {
                if (tUnusedUser.Equals(aSearchingUser))
                {
                    return true;
                }
            }
            return false;
        }

        //-------------------------
        public void RandomVerlosung()
        {
            try
            {
                versucheBisAbbruchBeiVerlosung++;
                if (versucheBisAbbruchBeiVerlosung > 100)//SetMaxVersucheBisAbbruch())
                {
                    MessageBox.Show("Ein Fehler ist aufgetreten. Bitte versuche es nochmals");
                    return;
                }
                Boolean kontrolle        = false;
                ArrayList unUsedUserList = (ArrayList)nameList.Clone();
                int arrayLengthOfUser    = userList.Count();// Convert.ToInt32(readArrayLengthOfUser);
                Boolean everythingToUse  = true;
                Random random            = new Random();
                string tempValueString;
                foreach (UserClass tUser in userList)
                {
                    int counter = 0;
                    kontrolle   = false;
                    while (!kontrolle)
                    {
                        if (counter > userList.Count-2)
                        {
                            //Suche ob etwas frei ist
                            everythingToUse = false;
                            foreach (string tUnusedUser in unUsedUserList)
                            {
                                everythingToUse = KontrolliereObVorhandenInBlackList(tUser, tUnusedUser);
                                if (everythingToUse)
                                {
                                    tUser.GezogenerWichtel = tUnusedUser;
                                    unUsedUserList.Remove(tUnusedUser);
                                    break;
                                }
                            }
                            if (!everythingToUse)
                            {
                                RandomVerlosung();
                                return;
                            }
                        }
                        else
                        {
                            tempValueString = nameList[random.Next(arrayLengthOfUser)].ToString(); //ToDo: möglicherweise fehler. Unbekannt wesshalb toString benötigt.
                            if (!KontrolliereObVorhandenInBlackList(tUser, tempValueString))
                            {
                                kontrolle = false;
                                counter++;
                            }
                            else
                            {
                                if (KontrollierObBereitsGezogen(unUsedUserList, tempValueString))
                                {
                                    kontrolle              = true;
                                    tUser.GezogenerWichtel = tempValueString;
                                    unUsedUserList.Remove(tempValueString);
                                    break;
                                }
                                else
                                {
                                    kontrolle = false;
                                    counter++;
                                }
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
            verknüpfteDaten.DataSource    = userList;
            dataGridView1.DataSource      = verknüpfteDaten;
            dataGridView1.AutoResizeColumns();
            dataGridView1.Refresh();
            
        }

        //-------------------------
        public Boolean KontrolleObKostelationBereitsVerwendet(UserClass aCurrentUser, string lastChoiseOfBLuser)
        {
            foreach (UserClass tUser in userList)                                       //jeder User
            {                                                                          
                int identischCounter = 0;                                              
                if (!aCurrentUser.UserName.Equals(tUser.UserName))                      //Nicht er selber
                {                                                                      
                    foreach (string tBLuserAcurrent in aCurrentUser.GetBlackList())     //jeder in der BL von CurrentUser
                    {                                                                  
                        foreach (string tBLuserTuser in tUser.GetBlackList())           //jeder in der BL von anderen User
                        {                                                              
                            if (tBLuserAcurrent.Equals(tBLuserTuser))                   //Kontrolle vorkommen
                            {                                                          
                                identischCounter++;                                     //Zähle identische
                            }                                                          
                            if (lastChoiseOfBLuser.Equals(tBLuserTuser))                //Auch zu kontrollieren die mögliche letzte wahl von Currentuser
                            {
                                identischCounter++;
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
            for (int n = userList.Count; n > 1; n--)
            {
                for (int i = 0; i < n-1; i++)
                {
                    if (userList[i].GetBlackList().Count < userList[i+1].GetBlackList().Count)
                    {
                        UserClass tSwapUser = userList[i];
                        userList[i] = userList[i+1];
                        userList[i + 1] = tSwapUser;
                    }
                }
            }
        }

        //===========Events===========================================
        private void Bt_CreateNewUser_Click(object sender, EventArgs e)
        {
            //Neues Objekt ersellen Mit Username
            
            if (string.IsNullOrEmpty(ed_UserName.Text))
            {
                return;
            }
            UserClass neuerUser = new UserClass(ed_UserName.Text);
            userList.   Add(neuerUser);
            nameList.   Add(neuerUser.UserName);
            ReloadAllWhiteLists();
            FillListBoxUser();
            ed_UserName.Text    = "";
            ed_UserName.Focus();
        }

        //-------------------------
        private void Lb_User_MouseClick(object sender, MouseEventArgs e)
        {
            UserClass tSelectedUserObject = GetCurrentUserObject(lb_User.Items[lb_User.SelectedIndex].ToString());
            FillListBoxBlackListFromUser  (tSelectedUserObject);
            RevisionWhiteList             (tSelectedUserObject);
            FillRemainingUserToLBwhiteList(tSelectedUserObject);
        }

        //-------------------------
        private void Bt_addUserToBlackList_Click(object sender, EventArgs e)
        {
            //Selektierter auf WhiteList zu BlackList verschieben
            UserClass tSelectedUserObject = GetCurrentUserObject(lb_User.Items[lb_User.SelectedIndex].ToString());
            UserClass tWhiteListUser = GetCurrentUserObject(lb_WhiteList.Items[lb_WhiteList.SelectedIndex].ToString());
            
            //Regelung Kontrolle
            if ( (tSelectedUserObject.GetBlackList().Count) >= (userList.Count-1) ) //Maximale wahl an BL Menge
            {
                MessageBox.Show("Leider sind können nicht mehr auf die BlackList von diesem Mitspieler gelegt werden.");
                return;
            }
            if ( (tWhiteListUser.GetbinInBlackListEingetragenVon().Count) >= (userList.Count-2) ) //Maximales Vorkommen auf BLs
            {
                MessageBox.Show("Leider ist der Mitspieler der auf die Blacklist geschoben werden soll, bereits zu viel auf einer BlackList vorhanden.");
                return;
            }
            if ((tSelectedUserObject.GetBlackList().Count) >= (userList.Count - 2)) //Letztes mal vor Max BL Wahl
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
            //Selektierter auf WhiteList zu BlackList verschieben
            UserClass tSelectedUserObject = GetCurrentUserObject(lb_User.Items[lb_User.SelectedIndex].ToString());
            UserClass tBlackListUser = GetCurrentUserObject(lb_BlackList.Items[lb_BlackList.SelectedIndex].ToString());
            MoveItemFromBlackToWhiteList  (tSelectedUserObject, tBlackListUser);
            //Neu Laden
            FillListBoxBlackListFromUser  (tSelectedUserObject);
            RevisionWhiteList             (tSelectedUserObject);
            FillRemainingUserToLBwhiteList(tSelectedUserObject);
        }

        //-------------------------
        private void Bt_verlosung_Click(object sender, EventArgs e)
        {
            SortUserListAnzahlBLAbsteigend();

            versucheBisAbbruchBeiVerlosung = 0;
            RandomVerlosung();
            FillGridView   ();
        }
    }
}
//======================================================
//===============END OF FILE============================
//======================================================
