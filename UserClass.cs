using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WichtelGeneratorVisual
{
    class UserClass
    {
        //=============Konstruktoren=============
        public UserClass(string aUserName)
        {
            UserName = aUserName;
            fillBlackList(UserName);
        }

        //============Private Variablen==========
        private string userName;
        private ArrayList blackList = new ArrayList();

        //============Getter Setter=========================================
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public void fillBlackList(string aUserToLock)
        {
            this.blackList.Add(aUserToLock);
        }

        public ArrayList getBlackList()
        {
            return this.blackList;
        }
    }
}
