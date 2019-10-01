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
        //ArrayList userList = new ArrayList();
        //UserNameList nameList = new UserNameList();
        ArrayList nameList = new ArrayList();
        int abbruchIntVerlosung;
        

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
            //ArrayList temp = nameList.getTheList();
            foreach (string tUser in nameList)
            {
                lb_User.Items.Add(tUser);
            }
            lb_User.EndUpdate();
        }

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

        public void FillListBoxBlackList(UserClass aCurrentUser)
        {
            ArrayList tBlackList = aCurrentUser.GetBlackList();
            lb_BlackList.Items.Clear();
            lb_BlackList.BeginUpdate();
            foreach(string temp in tBlackList)
            {
                lb_BlackList.Items.Add(temp);
            }
            lb_BlackList.EndUpdate();
        }

        public void RevisionWhiteList(UserClass aCurrentUser)
        {
            ArrayList tBlackList = aCurrentUser.GetBlackList();
            ArrayList tRemainingUser = aCurrentUser.GetWhiteList();
            foreach (string temp1 in tBlackList)
            {
                foreach (string temp2 in tRemainingUser)
                {
                    if (temp2.Equals(temp1))
                    {
                        aCurrentUser.RemoveItemFromWhiteList(temp2);
                        RevisionWhiteList(aCurrentUser);
                        break;
                    }
                }
            }
        }

        public void FillRemainingUserToLBwhiteList(UserClass aCurrentUser)
        {
            ArrayList tWhiteList = aCurrentUser.GetWhiteList();
            lb_WhiteList.Items.Clear();
            lb_WhiteList.BeginUpdate();
            foreach (string temp in tWhiteList)
            {
                lb_WhiteList.Items.Add(temp);
            }
            lb_WhiteList.EndUpdate();
        }
        
        public void ReloadAllWhiteLists()
        {
            foreach (UserClass temp in userList )
            {
                //aList = (ArrayList)userList.clone();
                //ArrayList copyTheList = nameList.getTheList();
                temp.SetWhiteList((ArrayList)nameList.Clone());
            }
        }

        public void MoveItemFromWhiteToBlackList(UserClass aUser, string aItem)
        {
            aUser.RemoveItemFromWhiteList(aItem);
            aUser.SetBlackList(aItem);
        }

        public void MoveItemFromBlackToWhiteList(UserClass aUser, string aItem)
        {
            aUser.RemoveItemInBlackList(aItem);
            aUser.SetWhiteListItem(aItem);
        }

        public void RandomVerlosung()
        {
            try
            {
                abbruchIntVerlosung++;
                if (abbruchIntVerlosung > 5)
                {
                    MessageBox.Show("Ein Fehler ist aufgetreten. Bitte versuche es nochmals");
                    return;
                }
                Boolean kontrolle = false;
                ArrayList unUsedUserList = (ArrayList)nameList.Clone();
                long readArrayLengthOfUser = userList.LongCount();
                int arrayLengthOfUser = Convert.ToInt32(readArrayLengthOfUser);
                Boolean everything = true;
                string tempValueString;
                Random random = new Random();
                foreach (UserClass aUser in userList)
                {
                    int counter = 0;
                    kontrolle = false;
                    while (!kontrolle)
                    {
                        if (counter > 5)
                        {
                            //Suche ob etwas frei ist
                            everything = false;
                            foreach (string temp in unUsedUserList)
                            {
                                everything = ZugKontrolleBlackList(aUser, temp);
                                if (everything)
                                {
                                    aUser.GezogenerWichtel = temp;
                                    unUsedUserList.Remove(temp);
                                    break;
                                }
                            }
                            if (!everything)
                            {
                                RandomVerlosung();
                                return;
                            }
                        }
                        else
                        {
                            tempValueString = nameList[random.Next(arrayLengthOfUser)].ToString(); //ToDo: möglicherweise fehler. Unbekannt wesshalb toString benötigt.
                            if (!ZugKontrolleBlackList(aUser, tempValueString))
                            {
                                kontrolle = false;
                                counter++;
                            }
                            else
                            {
                                if (ZugKontrolleBereitsGezogen(unUsedUserList, tempValueString))
                                {
                                    kontrolle = true;
                                    aUser.GezogenerWichtel = tempValueString;
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

        public Boolean ZugKontrolleBereitsGezogen(ArrayList aUnUsed, string aValue )
        {
            foreach(string temp in aUnUsed)
            {
                if (temp.Equals(aValue))
                {
                    return true;
                }
            }
            return false;
        }

        public Boolean ZugKontrolleBlackList(UserClass aUser, string aSearchingItem)
        {
            ArrayList tList = aUser.GetBlackList();
            foreach(string temp in tList)
            {
                if (temp.Equals(aSearchingItem))
                {
                    return false;
                }
            }
            return true;
        }

        public void FillGridView()
        {
            BindingSource source = new BindingSource();
            source.DataSource = userList;
            dataGridView1.DataSource = source;
            dataGridView1.AutoResizeColumns();
            dataGridView1.Refresh();
            
        }

        //===========Events===========================================
        private void Bt_CreateNewUser_Click(object sender, EventArgs e)
        {
            //Neues Objekt ersellen Mit Username
            
            if (string.IsNullOrEmpty(ed_UserName.Text))
            {
                return;
            }
            UserClass einUser = new UserClass(ed_UserName.Text);
            userList.Add(einUser);
            nameList.Add(einUser.UserName);
            //nameList.setItemToTheList(einUser.UserName);
            ReloadAllWhiteLists();
            FillListBoxUser();
            ed_UserName.Text = "";
            ed_UserName.Focus();
        }

        private void Lb_User_MouseClick(object sender, MouseEventArgs e)
        {
            // read aktuell ausgewählter
            UserClass tSelectedUserObject = GetCurrentUserObject(lb_User.Items[lb_User.SelectedIndex].ToString());
            //read blacklist und zeige an
            FillListBoxBlackList(tSelectedUserObject);
            // read blacklist und gleiche ab welche er noch nicht hatt.

            //ArrayList tempList = nameList.getTheList();
            //tSelectedUserObject.SetWhiteList(tempList);
            RevisionWhiteList(tSelectedUserObject);
            FillRemainingUserToLBwhiteList(tSelectedUserObject);
        }

        private void Bt_addUserToBlackList_Click(object sender, EventArgs e)
        {
            //Selektierter auf WhiteList zu BlackList verschieben
            UserClass tSelectedUserObject = GetCurrentUserObject(lb_User.Items[lb_User.SelectedIndex].ToString());
            MoveItemFromWhiteToBlackList(tSelectedUserObject, lb_WhiteList.Items[lb_WhiteList.SelectedIndex].ToString());
            //Neu Laden
            FillListBoxBlackList(tSelectedUserObject);
            RevisionWhiteList(tSelectedUserObject);
            FillRemainingUserToLBwhiteList(tSelectedUserObject);
        }

        private void Bt_addusertoWhiteList_Click(object sender, EventArgs e)
        {
            //Selektierter auf WhiteList zu BlackList verschieben
            UserClass tSelectedUserObject = GetCurrentUserObject(lb_User.Items[lb_User.SelectedIndex].ToString());
            MoveItemFromBlackToWhiteList(tSelectedUserObject, lb_BlackList.Items[lb_BlackList.SelectedIndex].ToString());
            //Neu Laden
            FillListBoxBlackList(tSelectedUserObject);
            RevisionWhiteList(tSelectedUserObject);
            FillRemainingUserToLBwhiteList(tSelectedUserObject);
        }

        private void Bt_verlosung_Click(object sender, EventArgs e)
        {
            abbruchIntVerlosung = 0;
            RandomVerlosung();
            // Ergebnisse anzeigen in der Liste
            FillGridView();
        }
    }
}
