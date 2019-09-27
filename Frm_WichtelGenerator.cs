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
        private ArrayList userList = new ArrayList();
        private ArrayList nameList = new ArrayList();

        //======Initialisierung======================
        public Frm_Main()
        {
            InitializeComponent();
        }

		//===========Funktionen======================================
		public void fillListBoxUser()
        {
            lb_User.BeginUpdate();
			foreach( string tUser in nameList )
            {
                lb_User.Items.Add(tUser);
            }
            lb_User.EndUpdate();
        }

        //===========Events===========================================
        private void Bt_CreateNewUser_Click(object sender, EventArgs e)
        {
            //Neues Objekt ersellen Mit Username
            UserClass einUser = new UserClass(ed_UserName.Text);
            userList.Add( einUser );
            nameList.Add( einUser.UserName );
            fillListBoxUser();
        }
    }
}
