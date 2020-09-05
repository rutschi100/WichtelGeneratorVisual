namespace WichtelGeneratorVisual
{
    partial class Frm_Main
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.bt_CreateNewUser = new System.Windows.Forms.Button();
            this.ed_UserName = new System.Windows.Forms.TextBox();
            this.lb_User = new System.Windows.Forms.ListBox();
            this.lbl_newUser = new System.Windows.Forms.Label();
            this.gb_NewUser = new System.Windows.Forms.GroupBox();
            this.gb_BlackList = new System.Windows.Forms.GroupBox();
            this.bt_addusertoWhiteList = new System.Windows.Forms.Button();
            this.bt_addUserToBlackList = new System.Windows.Forms.Button();
            this.lbl_BlackList = new System.Windows.Forms.Label();
            this.lb_BlackList = new System.Windows.Forms.ListBox();
            this.lb_WhiteList = new System.Windows.Forms.ListBox();
            this.lbl_WhiteListUser = new System.Windows.Forms.Label();
            this.lbl_ShowUser = new System.Windows.Forms.Label();
            this.gb_verlosung = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.bt_verlosung = new System.Windows.Forms.Button();
            this.lbl_Verlosung = new System.Windows.Forms.Label();
            this.gb_NewUser.SuspendLayout();
            this.gb_BlackList.SuspendLayout();
            this.gb_verlosung.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // bt_CreateNewUser
            // 
            this.bt_CreateNewUser.Location = new System.Drawing.Point(286, 16);
            this.bt_CreateNewUser.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bt_CreateNewUser.Name = "bt_CreateNewUser";
            this.bt_CreateNewUser.Size = new System.Drawing.Size(70, 19);
            this.bt_CreateNewUser.TabIndex = 0;
            this.bt_CreateNewUser.Text = "Hinzufügen";
            this.bt_CreateNewUser.UseVisualStyleBackColor = true;
            this.bt_CreateNewUser.Click += new System.EventHandler(this.Bt_CreateNewUser_Click);
            // 
            // ed_UserName
            // 
            this.ed_UserName.Location = new System.Drawing.Point(147, 17);
            this.ed_UserName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ed_UserName.Name = "ed_UserName";
            this.ed_UserName.Size = new System.Drawing.Size(135, 20);
            this.ed_UserName.TabIndex = 1;
            this.ed_UserName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ed_UserName_KeyDown);
            // 
            // lb_User
            // 
            this.lb_User.FormattingEnabled = true;
            this.lb_User.Location = new System.Drawing.Point(8, 42);
            this.lb_User.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lb_User.Name = "lb_User";
            this.lb_User.Size = new System.Drawing.Size(135, 303);
            this.lb_User.TabIndex = 2;
            this.lb_User.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Lb_User_MouseClick);
            // 
            // lbl_newUser
            // 
            this.lbl_newUser.AutoSize = true;
            this.lbl_newUser.Location = new System.Drawing.Point(6, 20);
            this.lbl_newUser.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_newUser.Name = "lbl_newUser";
            this.lbl_newUser.Size = new System.Drawing.Size(137, 13);
            this.lbl_newUser.TabIndex = 3;
            this.lbl_newUser.Text = "Neue Mitspieler Hinzufügen";
            // 
            // gb_NewUser
            // 
            this.gb_NewUser.Controls.Add(this.ed_UserName);
            this.gb_NewUser.Controls.Add(this.lbl_newUser);
            this.gb_NewUser.Controls.Add(this.bt_CreateNewUser);
            this.gb_NewUser.Location = new System.Drawing.Point(9, 10);
            this.gb_NewUser.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gb_NewUser.Name = "gb_NewUser";
            this.gb_NewUser.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gb_NewUser.Size = new System.Drawing.Size(518, 51);
            this.gb_NewUser.TabIndex = 4;
            this.gb_NewUser.TabStop = false;
            this.gb_NewUser.Text = "Neue Mitspieler";
            // 
            // gb_BlackList
            // 
            this.gb_BlackList.Controls.Add(this.bt_addusertoWhiteList);
            this.gb_BlackList.Controls.Add(this.bt_addUserToBlackList);
            this.gb_BlackList.Controls.Add(this.lbl_BlackList);
            this.gb_BlackList.Controls.Add(this.lb_BlackList);
            this.gb_BlackList.Controls.Add(this.lb_WhiteList);
            this.gb_BlackList.Controls.Add(this.lbl_WhiteListUser);
            this.gb_BlackList.Controls.Add(this.lbl_ShowUser);
            this.gb_BlackList.Controls.Add(this.lb_User);
            this.gb_BlackList.Location = new System.Drawing.Point(9, 61);
            this.gb_BlackList.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gb_BlackList.Name = "gb_BlackList";
            this.gb_BlackList.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gb_BlackList.Size = new System.Drawing.Size(518, 356);
            this.gb_BlackList.TabIndex = 6;
            this.gb_BlackList.TabStop = false;
            this.gb_BlackList.Text = "BlackList";
            // 
            // bt_addusertoWhiteList
            // 
            this.bt_addusertoWhiteList.Location = new System.Drawing.Point(332, 194);
            this.bt_addusertoWhiteList.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bt_addusertoWhiteList.Name = "bt_addusertoWhiteList";
            this.bt_addusertoWhiteList.Size = new System.Drawing.Size(32, 19);
            this.bt_addusertoWhiteList.TabIndex = 9;
            this.bt_addusertoWhiteList.Text = "<<<";
            this.bt_addusertoWhiteList.UseVisualStyleBackColor = true;
            this.bt_addusertoWhiteList.Click += new System.EventHandler(this.Bt_addusertoWhiteList_Click);
            // 
            // bt_addUserToBlackList
            // 
            this.bt_addUserToBlackList.Location = new System.Drawing.Point(332, 162);
            this.bt_addUserToBlackList.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bt_addUserToBlackList.Name = "bt_addUserToBlackList";
            this.bt_addUserToBlackList.Size = new System.Drawing.Size(32, 19);
            this.bt_addUserToBlackList.TabIndex = 8;
            this.bt_addUserToBlackList.Text = ">>>";
            this.bt_addUserToBlackList.UseVisualStyleBackColor = true;
            this.bt_addUserToBlackList.Click += new System.EventHandler(this.Bt_addUserToBlackList_Click);
            // 
            // lbl_BlackList
            // 
            this.lbl_BlackList.AutoSize = true;
            this.lbl_BlackList.Location = new System.Drawing.Point(370, 26);
            this.lbl_BlackList.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_BlackList.Name = "lbl_BlackList";
            this.lbl_BlackList.Size = new System.Drawing.Size(100, 13);
            this.lbl_BlackList.TabIndex = 7;
            this.lbl_BlackList.Text = "Black List Mitspieler";
            // 
            // lb_BlackList
            // 
            this.lb_BlackList.FormattingEnabled = true;
            this.lb_BlackList.Location = new System.Drawing.Point(372, 42);
            this.lb_BlackList.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lb_BlackList.Name = "lb_BlackList";
            this.lb_BlackList.Size = new System.Drawing.Size(135, 303);
            this.lb_BlackList.TabIndex = 6;
            // 
            // lb_WhiteList
            // 
            this.lb_WhiteList.FormattingEnabled = true;
            this.lb_WhiteList.Location = new System.Drawing.Point(189, 42);
            this.lb_WhiteList.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lb_WhiteList.Name = "lb_WhiteList";
            this.lb_WhiteList.Size = new System.Drawing.Size(135, 303);
            this.lb_WhiteList.TabIndex = 5;
            // 
            // lbl_WhiteListUser
            // 
            this.lbl_WhiteListUser.AutoSize = true;
            this.lbl_WhiteListUser.Location = new System.Drawing.Point(187, 26);
            this.lbl_WhiteListUser.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_WhiteListUser.Name = "lbl_WhiteListUser";
            this.lbl_WhiteListUser.Size = new System.Drawing.Size(101, 13);
            this.lbl_WhiteListUser.TabIndex = 4;
            this.lbl_WhiteListUser.Text = "White List Mitspieler";
            // 
            // lbl_ShowUser
            // 
            this.lbl_ShowUser.AutoSize = true;
            this.lbl_ShowUser.Location = new System.Drawing.Point(6, 26);
            this.lbl_ShowUser.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_ShowUser.Name = "lbl_ShowUser";
            this.lbl_ShowUser.Size = new System.Drawing.Size(102, 13);
            this.lbl_ShowUser.TabIndex = 3;
            this.lbl_ShowUser.Text = "Wähle ein Mitspieler";
            // 
            // gb_verlosung
            // 
            this.gb_verlosung.Controls.Add(this.dataGridView1);
            this.gb_verlosung.Controls.Add(this.bt_verlosung);
            this.gb_verlosung.Controls.Add(this.lbl_Verlosung);
            this.gb_verlosung.Location = new System.Drawing.Point(9, 422);
            this.gb_verlosung.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gb_verlosung.Name = "gb_verlosung";
            this.gb_verlosung.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gb_verlosung.Size = new System.Drawing.Size(518, 262);
            this.gb_verlosung.TabIndex = 7;
            this.gb_verlosung.TabStop = false;
            this.gb_verlosung.Text = "Verlosung";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(8, 49);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(498, 200);
            this.dataGridView1.TabIndex = 3;
            // 
            // bt_verlosung
            // 
            this.bt_verlosung.Location = new System.Drawing.Point(113, 21);
            this.bt_verlosung.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bt_verlosung.Name = "bt_verlosung";
            this.bt_verlosung.Size = new System.Drawing.Size(56, 19);
            this.bt_verlosung.TabIndex = 1;
            this.bt_verlosung.Text = "Ziehen";
            this.bt_verlosung.UseVisualStyleBackColor = true;
            this.bt_verlosung.Click += new System.EventHandler(this.Bt_verlosung_Click);
            // 
            // lbl_Verlosung
            // 
            this.lbl_Verlosung.AutoSize = true;
            this.lbl_Verlosung.Location = new System.Drawing.Point(6, 26);
            this.lbl_Verlosung.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_Verlosung.Name = "lbl_Verlosung";
            this.lbl_Verlosung.Size = new System.Drawing.Size(102, 13);
            this.lbl_Verlosung.TabIndex = 0;
            this.lbl_Verlosung.Text = "Starte die Verlosung";
            // 
            // Frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 690);
            this.Controls.Add(this.gb_verlosung);
            this.Controls.Add(this.gb_BlackList);
            this.Controls.Add(this.gb_NewUser);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Frm_Main";
            this.Text = "Wichtel Generator";
            this.gb_NewUser.ResumeLayout(false);
            this.gb_NewUser.PerformLayout();
            this.gb_BlackList.ResumeLayout(false);
            this.gb_BlackList.PerformLayout();
            this.gb_verlosung.ResumeLayout(false);
            this.gb_verlosung.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bt_CreateNewUser;
        private System.Windows.Forms.TextBox ed_UserName;
        private System.Windows.Forms.ListBox lb_User;
        private System.Windows.Forms.Label lbl_newUser;
        private System.Windows.Forms.GroupBox gb_NewUser;
        private System.Windows.Forms.GroupBox gb_BlackList;
        private System.Windows.Forms.ListBox lb_WhiteList;
        private System.Windows.Forms.Label lbl_WhiteListUser;
        private System.Windows.Forms.Label lbl_ShowUser;
        private System.Windows.Forms.Label lbl_BlackList;
        private System.Windows.Forms.ListBox lb_BlackList;
        private System.Windows.Forms.Button bt_addusertoWhiteList;
        private System.Windows.Forms.Button bt_addUserToBlackList;
        private System.Windows.Forms.GroupBox gb_verlosung;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button bt_verlosung;
        private System.Windows.Forms.Label lbl_Verlosung;
    }
}

