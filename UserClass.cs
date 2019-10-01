using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WichtelGeneratorVisual
{
    public class UserClass
    {
        //=============Konstruktoren=============
        public UserClass(string aUserName)
        {
            UserName = aUserName;
            FillBlackList(UserName);
        }

        //============Private Variablen==========
        private string userName;
        private ArrayList blackList = new ArrayList();
        private ArrayList whiteList = new ArrayList();

        //============Getter Setter=========================================
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public void FillBlackList(string aUserToLock)
        {
            this.blackList.Add(aUserToLock);
        }

        public ArrayList GetBlackList()
        {
            return this.blackList;
        }

        public void SetBlackList(string aBlackListUser)
        {
            blackList.Add(aBlackListUser);
        }

        public ArrayList GetWhiteList()
        {
            return this.whiteList;
        }

        public void SetWhiteList(ArrayList aWhiteListUser)
        {
            whiteList = aWhiteListUser;
        }

        public void RemoveItemFromWhiteList(string aItem)
        {
            this.whiteList.Remove(aItem);
        }
    }
}
