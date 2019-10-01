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


        //===========Events===========================================
        private void Bt_CreateNewUser_Click(object sender, EventArgs e)
        {
            //Neues Objekt ersellen Mit Username
            UserClass einUser = new UserClass(ed_UserName.Text);
            userList.Add(einUser);
            nameList.Add(einUser.UserName);
            //nameList.setItemToTheList(einUser.UserName);
            ReloadAllWhiteLists();
            FillListBoxUser();
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
    }
}
